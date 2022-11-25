// excel导入数据的组件
Vue.component('upload-excel', {
    //template: 模板文件的地址
    //config: 配置文件的地址
    //title: 组件的标题  
    //size:限制的文件大小，单位Kb
    //apiurl:接收导入数据的接口地址
    //params:接收导入数据时的参数对象，例如{type:1}
    props: ['template', 'config', 'title', 'size', 'apiurl', 'params'],
    data: function () {
        return {
            step: 0,         //步数
            steps: ['上传Excel文件', '选择工作簿', '匹配字段', '导入完成'],
            templatePath: '/Utilities/DataTemplate/',
            fields: [],          //数据库对应字段

            filename: '',            //当前上传文件的的本地文件名
            book: { 'file': '', sheets: [] },      //工作簿信息
            sheet: {},          //工作簿

            finish: {},          //导入完成的数据

            loading: false
        }
    },
    watch: {
        //当步进数变化时
        'step': {
            handler: function (nv, ov) {
                this.$emit('step', nv);
            }, immediate: true
        }
    },
    computed: {
        //配置文件的路径
        'config_file': function () {
            return this.templatePath + 'Config/' + this.config;
        }
    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/upload-excel.css']);
        this.getconfig();
    },
    methods: {
        //获取字段匹配的配置信息
        getconfig: function () {
            var th = this;
            $api.cache('Platform/ExcelConfig', { 'file': th.config_file }).then(function (req) {
                if (req.data.success) {
                    th.fields = req.data.result;
                    console.log(th.fields);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        //上传excel
        upload: function (file) {
            var th = this;
            th.filename = file.name;
            th.loading = true;
            $api.post('Platform/ExcelUpload', { "file": file }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.book = req.data.result;
                    th.step = 1;
                } else {
                    th.book = {};
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        //处理工作簿，获取工作簿上的列名
        handleSheet: function (sheet, index) {
            var th = this;
            th.loading = true;
            $api.get('Platform/ExcelSheetColumn', { 'xlsFile': this.book.file, 'sheetIndex': index })
                .then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.sheet = req.data.result;
                        console.log(th.sheet);
                        th.step = 2;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
        },
        //匹配excel列名和数据库字段
        marry: function (column, field) {
            if (column == field) return true;
            if (column.indexOf(field) > -1) return true;
            if (field.indexOf(column) > -1) return true;
            return false;
        },
        //导入数据
        btnInputData: function () {
            var ctrs = $dom("#column_to_field select");
            //excel列与数据库字段的匹配信息
            var marry = [];
            ctrs.each(function () {
                var item = $dom(this);
                marry.push({
                    "column": item.attr("label"),
                    "field": item.val()
                });
            });
            console.log(marry);
            //事件，四个参数。
            //xls文件名（服务器端）,工作簿索引，配置文件名（绝对路径）,列与字段的匹配信息        
            var th = this;
            if (th.apiurl == null || th.apiurl == '') return;
            th.loading = true;
            var params = { 'xls': th.book.file, 'sheet': th.sheet.index, 'config': th.config_file, 'matching': marry };
            if (th.params != null) {
                for (var k in th.params)
                    params[k] = th.params[k];
            }          
            $api.get(th.apiurl, params).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.finish = req.data.result;
                    th.step = 3;
                    console.log(th.finish);
                    th.$emit('finish', th.finish);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        }
    },
    template: `<div class="upload_excel_area">   
        <el-steps :active="step" finish-status="success">
            <el-step v-for="item in steps" :title="item"></el-step>            
        </el-steps>        
        <div v-if="step==0" remark="上传excel文件">
            <div label="模板">
                <a :href="templatePath+template" target="_blank" class="template_load">点击下载“{{title}}”模板</a>

                <help>（数据格式请按照模板整理）</hlep>
            </div>
            <div label="上传数据">
                <loading v-if="loading">正在上传中....</loading>
                <upload-file v-else @change="upload" height="30" ext="xls,xlsx" :size="size">
                    <el-tooltip content="允许的文件类型：xls,xlsx" placement="right" effect="light">
                        <el-button type="primary" plain class="file_btn">
                            <icon>&#xe6a7</icon>点击上传文件
                        </el-button>
                    </el-tooltip>
                </upload-file>                   
            </div>
            <div label="说明">                
                1、需要导入的Excel数据请按照模板格式整理；<br />
                2、Excel文档支持Excel97、2003、2007、2010多种版本；
            </div>
        </div>
        <div v-if="step==1" remark="选择工作簿">
            <div label="操作对象"> 正在操作文档 《{{filename}}》</div>
            <div label="工作簿">
                <el-button type="primary" plain v-for="(item,i) in book.sheets" @click="handleSheet(item,i)"
                    :disabled="item.count<1" :loading="loading">
                    {{item.name}} : {{item.count}}条数据
                </el-button>
            </div>
            <div label="说明">
                1、当前Excel文档包含了{{book.sheets.length}}个工作簿； <br/>
                2、请选择要导入的工作簿（点击上方工作簿按钮进步入一步操作）；
            </div>
            <div class="btn">
                <el-button type="primary" plain :disabled="loading" @click="step=0"><icon>&#xe748</icon>上一步</el-button>
            </div>
        </div>
        <div v-if="step==2" remark="匹配字段">
            <div label="操作对象"> 文档 《{{filename}}》的工作簿“{{sheet.name}}”，共{{sheet.count}}条数据</div>
            <div label="匹配字段">
                <dl id="column_to_field">
                    <dd v-for="item in sheet.columns" :label="item.Name">
                        <select :label="item.Name" :disabled="loading" >
                            <option value=""></option>
                            <option v-for="f in fields" :value ="f.Field" :selected="marry(item.Name,f.Column)">{{f.Column}}</option>                            
                        </select>
                    </dd>
                </dl>
            </div>
            <div label="说明">
                1、左侧是Excel表中的数据字段（excel表首行）；右侧是数据库中的字段；<br/>
                2、系统进行了自动匹配，但不保证完全正确，请手工设置对应关系。
            </div>
            <div class="btn">
                <el-button type="primary" plain :disabled="loading" @click="step=1"><icon>&#xe748</icon>上一步</el-button>
                <el-button type="primary" plain :disabled="loading" @click="btnInputData"><icon>&#xe67e</icon>导入数据</el-button>
                <loading v-if="loading">正在导入，请稍候....</loading>
            </div>
        </div>
        <div v-if="step==3" remark="完成">
            <div label="操作对象"> 文档 《{{filename}}》的工作簿“{{sheet.name}}”，共{{sheet.count}}条数据</div>
            <div label="结果">
                成功导入{{finish.success}}条记录<template v-if="finish.error && finish.error>0">，<alert>失败{{finish.error}}条</alert></template>
            </div>
            <div class="btn">
                <el-button type="primary" plain @click="step=1"><icon>&#xe85e</icon>继续导入其它工作簿</el-button>
                <el-button type="primary" plain @click="step=0"><icon>&#xe6a7</icon>继续导入其它Excel数据</el-button>
            </div>
            <div label="错误数据" v-if="finish.datas && finish.datas.length" class="error_data">
                <table>
                    <template v-for="(item,i) in finish.datas">
                        <tr v-if="i==0">
                            <th>#</th>
                            <th v-for="(value, name) in item">{{name}}</th>
                        </tr>
                        <tr>
                            <td>{{i+1}}</td>
                            <td v-for="(value, name) in item">{{value}}</td>
                        </tr>
                    </template>                  
                </table>               
            </div>
        </div>
    </div>`
});
