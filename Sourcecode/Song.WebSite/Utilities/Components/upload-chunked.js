// 大文件分片上传
//事件
//start:开始上传，参数为本地文件对象
//success:上传完成，参数为服务器端文件对象
Vue.component('upload-chunked', {
    //pathkey: 上传到服务器端的路径，来自web.config中upload中的key值
    //ext: 限定的扩展名
    //chunk: 分块的大小，单位为mb 
    //thread:最大并发数
    //record:本地记录上传信息，用于断点续传
    props: ['pathkey', 'ext', 'chunk', 'thread', 'title'],
    data: function () {
        return {
            percentage: 0,      //完成的进度
            complete: 0,
            total: 0,
            filename: '',       //当前上传的文件名
            record: [],      //上传的记录信息，用于断点续传            
            loading: false
        }
    },
    watch: {
    },
    computed: {

    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/upload-chunked.css']);
    },
    methods: {
        //上传文件
        upload: function (file) {
            //触发开始上传的事件
            this.$emit('start', file);   
            var th = this;
            th.filename = file.name;
            //md5的文件信息
            var uid = $api.md5(file.name + file.size);
            //分片数组
            var chunkarr = th.filechunked(file, th.chunk, uid);
            //线程数组
            var threadarr = th.threadgroup(chunkarr, th.thread);
            //开始上传
            th.percentage = 0;
            th.complete = 0;
            th.loading = true;
            th.record = th.record_get(uid);

            th.chunkedup(threadarr, 0);
        },
        //文件分块
        //file:要上传的文件；chunk:分块大小（单位mb)；uid：md5加密信息
        filechunked(file, chunk, uid) {
            var chunkedArr = [];      //分片后的数组
            chunk = chunk == null ? 1 : chunk;
            const chunkSize = chunk * 1024 * 1024; // 分片文件大小
            this.total = Math.ceil(file.size / chunkSize); // 总片数
            for (var i = 0; i < this.total; i++) {
                //计算每一片的起始与结束位置
                var start = i * chunkSize, end = Math.min(file.size, start + chunkSize);
                chunkedArr.push({
                    "file": file.slice(start, end),
                    "pathkey": this.pathkey,
                    "filename": file.name,
                    "total": this.total,
                    "index": i + 1,
                    "uid": uid
                });
            }
            return chunkedArr;
        },
        //分线程组
        threadgroup: function (chunkarr, threadcount) {
            var arr = [];
            const total = Math.ceil(chunkarr.length / threadcount);
            for (var i = 0; i < total; i++) {
                var item = { "success": false, "list": [] };
                for (var j = 0; j < threadcount; j++) {
                    if ((i * threadcount + j) > chunkarr.length - 1) continue;
                    item.list.push(chunkarr[i * threadcount + j]);
                }
                arr.push(item);
            }
            return arr;
        },
        //上传分片文件
        chunkedup: function (arr, index) {
            var items = arr[index];
            var th = this;
            for (let i = 0; i < items.list.length; i++) {
                var exist = th.record_exist(items.list[i].index);
                if (exist) {
                    th.result_handler(items.list[i]);
                    items.list[i]['complete'] = true;
                    var success = th.isthread_complete(items);
                    if (success) {
                        if (index < arr.length - 1) {
                            th.chunkedup(arr, ++index);
                        }
                    }
                    continue;
                }
                $api.post('Platform/ChunkedUpload', items.list[i]).then(function (req) {
                    if (req == null) return;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.result_handler(result);
                        th.record_set(result.uid, result.index);

                        for (let i = 0; i < items.list.length; i++) {
                            if (items.list[i].index == result.index) {
                                items.list[i]['complete'] = true;
                            }
                        }
                        var success = th.isthread_complete(items);
                        if (success) {
                            items.success = true;
                            if (index < arr.length - 1) {
                                th.chunkedup(arr, ++index);
                            }
                            else {
                                th.loading = false;
                                console.log("上传完成");
                                //删除记录的切片索引
                                th.record_set(result.uid, null);
                                //上传完成的事件
                                th.$emit('success', result);                              
                            }
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        },
        //上传后的结果处理
        result_handler: function (result) {
            var th = this;
            th.complete++;
            th.percentage = Math.floor(th.complete / th.total * 10000) / 100;
        },
        //当前线程组是否完成
        isthread_complete: function (items) {
            var success = true;
            for (let i = 0; i < items.list.length; i++) {
                if (!items.list[i]['complete']) success = false;
            }
            if (success) items.success = true;
            return success;
        },

        record_get: function (uid) {
            var arr = $api.storage('a' + uid);
            return arr == null ? [] : arr;
        },
        record_set: function (uid, item) {
            if (item == null) {
                this.record = [];
                $api.storage('a' + uid, null);
                return;
            }
            this.record.push(item);
            $api.storage('a' + uid, this.record);
        },
        record_exist: function (index) {
            var r = this.record.findIndex((item) => {
                return item == index;
            })
            return r >= 0;
        }
    },
    template: `<div class="upload_chunked_area">            
            <upload-file @change="upload" height="30" :ext="ext" v-if="!loading">
                <el-tooltip :content="'允许的文件类型：'+ext" placement="right" effect="light">
                    <el-button type="primary" plain :disabled="loading">
                        <icon style="font-size:16px">&#xe761</icon>
                        <template v-if="title=='' || title==null">上传文件</template>
                        <template v-else>{{title}}</tempalte>
                    </el-button>
                </el-tooltip>
               
            </upload-file>   
            <div v-else>              
                <el-progress :text-inside="true" :stroke-width="24" :percentage="percentage" status="success">
            
                </el-progress>    
                <div class="filename"> {{filename}}</div>
            </div>  
    </div>`
});
