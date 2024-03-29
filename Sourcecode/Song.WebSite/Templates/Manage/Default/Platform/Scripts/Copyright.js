$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            modify: false,     //状态，修改或查看
            datas: [],
            loading: false,
            issuper: false       //是否是超管登录
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.cache('Copyright/Datas').then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.datas = req.data.result;
                    th.rowdrop();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                alert(err);
                console.error(err);
            });
            $api.post('Admin/IsSuper').then(function (req) {
                if (req.data.success) {
                    th.issuper = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        watch: {},
        methods: {
            add: function () {
                this.datas.push({
                    "name": "", "remark": "",
                    "fixed": false, "type": "text", "text": ""
                });
            },
            delete: function (index) {
                this.datas.splice(index, 1);
            },
            //保存
            update: function () {
                if (this.loading) return;
                //判断标识是否为空
                for (var i = 0; i < this.datas.length; i++) {
                    var name = $api.trim(this.datas[i].name);
                    if (name == '') {
                        this.$alert('请在第' + (i + 1) + '个项目中输入标识', '标识为空');
                        return;
                    }
                    if (!(/^[a-zA-Z]+.$/.test(name))) {
                        this.$alert('第' + (i + 1) + '项的标识不合规，必须以字母开头', '标识不合规');
                        return;
                    }
                }
                //判断标识是否重复
                for (var i = 0; i < this.datas.length; i++) {
                    var curr = this.datas[i];
                    var isexist = false;
                    for (var j = 0; j < this.datas.length; j++) {
                        if (i == j) continue;
                        if (curr.name == this.datas[j].name) {
                            this.$alert('第' + (i + 1) + '项 与 第' + (j + 1) + '项，标识存在重复', '标识重复');
                            isexist = true;
                            break;
                        }
                    }
                    if (isexist) return;
                }
                var th = this;
                this.loading = true;
                $api.post('Copyright/Update', { 'json': this.datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        $api.put('Copyright/Datas');
                        $api.cache('Copyright/Datas:update');
                        $api.cache('Copyright/Info:update');
                        if (result) {
                            th.$message({
                                message: '操作成功！',
                                type: 'success'
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    vapp.loading = false;
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('.modifyarea')[0];
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onStart: function (evt) { },
                    onMove: function (evt, originalEvent) { },
                    onEnd: (e) => {
                        let arr = this.datas; // 获取表数据
                        this.datas = [];
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                        });
                    }
                });
            }
        },
        components: {
            'item_editor': {
                props: ['item', 'index'],
                data: function () {
                    return {
                        types: [{ value: 'text', label: '文本' },
                        { value: 'multi', label: '多行文本' },
                        { value: 'link', label: '链接' },
                        { value: 'image', label: '图片' },],
                        imgLimit: ['jpg', 'png', 'gif'],     //上传图片的类型限制
                        loading: false,
                        rules: {
                            name: [
                                { required: true, message: '标识名不得为空', trigger: 'blur' }
                            ]
                        }
                    }
                },
                methods: {
                    //图片上传
                    beforeAvatarUpload: function (file) {
                        return false;
                    },
                    //当图片更改时
                    imgChange: function (file, fileList) {
                        //后缀名限制
                        var suffix = file.name.indexOf('.') > -1 ? file.name.substring(file.name.lastIndexOf('.') + 1) : '';
                        if (this.imgLimit.indexOf(suffix.toLowerCase()) < 0) {
                            this.$message.error('上传图片只能是 ' + this.imgLimit.join('、') + ' 格式!');
                            return false;
                        }
                        if (file.size / 1024 / 1024 > 2) {
                            this.$message.error('上传图片大小不能超过 2MB!');
                            return false;
                        }
                        var th = this;
                        this.getBase64(file.raw).then(function (res) {
                            th.item.text = res;
                            th.loading = true;
                            window.setTimeout(function () {
                                th.loading = false;
                            }, 500);
                        });
                    },
                    getBase64: function (file) {
                        return new Promise(function (resolve, reject) {
                            let reader = new FileReader();
                            let imgResult = "";
                            reader.readAsDataURL(file);
                            reader.onload = function () {
                                imgResult = reader.result;
                            };
                            reader.onerror = function (error) {
                                reject(error);
                            };
                            reader.onloadend = function () {
                                resolve(imgResult);
                            };
                        });
                    },
                },
                template: `<template>
                <el-card class="box-card"  shadow="hover">                
                <div slot="header" class="cardheader draghandle">
                    <el-form ref="item" :model="item" :rules="rules" :inline="true">                       
                    <el-form-item :label="(index+1)+'. 说明'">
                    <el-input v-model="item.remark" style="width:100%;"></el-input>
                    </el-form-item>                
                    <el-form-item label="标识" prop="name">
                        <el-input v-model="item.name" :disabled="item.fixed" style="width:140px;"></el-input>
                    </el-form-item>                
                    <el-form-item label="" >
                    <el-select v-model="item.type" style="width:100px;">
                        <el-option v-for="t in types" :value="t.value" :key="t.value" :label="t.label"></el- option>
                    </el-select>
                    </el-form-item>                  
                    </el-form>
                    <template>
                    <el-popconfirm  title="确定删除吗？" :disabled="item.fixed"  @confirm="vapp.delete(index)">
                    <el-link class="delitem el-icon-delete" type="danger" v-if="!item.fixed" :disabled="item.fixed" 
                    :title="item.fixed ? '禁止删除' : ''" slot="reference"> 删除</el-link>    
                    </el-popconfirm>                      
                </div>
                <el-input v-model="item.text" v-if="item.type=='text'"><template slot="prepend">内容</template></el-input>\
                <el-input v-model="item.text" v-if="item.type=='multi'" type="textarea" :rows="4" autosize></el-input>\
                <el-input v-model="item.text" v-if="item.type=='link'"><template slot="prepend">地址</template></el-input>\
                <template v-if="item.type=='image'">
                    <el-upload class="avatar-uploader" v-loading="loading" :show-file-list="false"
                        :before-upload="beforeAvatarUpload" :on-change="imgChange">
                        <div class="el-icon-upload2 photo-txt">上传图片，限 {{imgLimit.join('、')}} 格式</div>
                        <img :src="item.text" class="avatar photo" id="BgImage" style="float: left; ">
                        <br />
                    </el-upload>
                    <el-link type="danger" @click="item.text=''" v-if="item.text!=''">清除图片
                    </el-link>
                </template>              
                </el-card> </template>`
            }
        }
    });

});