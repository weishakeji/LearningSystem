//试卷按章节抽题的组件
Vue.component('fromtype1', {
    //total:总分
    props: ['types', 'testpaper', 'total'],
    data: function () {
        return {
            percents: [],     //各题型的分数占比
            items: [],       //章节中的各题型数量

            error: '',       //错误信息
            outlines: null,        //章节列表

            init: 0,         //初始化
            loading: false
        }
    },
    watch: {
        'testpaper': {
            handler: function (nv, ov) {
                if (nv != null && nv.Tp_Id > 0) {
                    this.init++;
                }
                if (this.init >= 2) this.parserPercXML(nv);
            }, immediate: true
        },
        'types': {
            handler: function (nv, ov) {
                if (nv != null) this.init++;
                if (this.init >= 2) this.parserPercXML(nv);
            }, immediate: true
        },
        'init': function () {

        },
        'total': function (nv, ov) {
            this.changePercent();
        }
    },
    computed: {
        //各题型合计占比
        sumPercent: function () {
            var sum = 0;
            for (let i = 0; i < this.percents.length; i++) {
                sum += this.percents[i].TPI_Percent;
            }
            return sum;
        },
        //各题型合计分数
        sumNumber: function () {
            var sum = 0;
            for (let i = 0; i < this.percents.length; i++) {
                sum += this.percents[i].TPI_Number;
            }
            return sum;
        },

    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'TestPaper/Components/Styles/fromtype1.css']);
    },
    methods: {
        //获取章节
        getOutline: function () {
            if (this.outlines != null) return;
            var th = this;
            let couid = th.testpaper.Cou_ID;
            if (couid <= 0) couid = $api.querystring('couid');
            if (couid <= 0 || couid == '') return;
            $api.cache('Outline/Tree', { 'couid': couid, 'isuse': true }).then(function (req) {
                if (req.data.success) {
                    th.outlines = req.data.result;
                    th.parseItemsXml();
                } else {
                    th.outlines = [];
                    th.parseItemsXml();
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() =>  {});
        },
        //解析试题抽题的设置项
        parserPercXML: function () {
            if (!this.testpaper || this.testpaper.Tp_FromConfig == null
                || this.testpaper.Tp_FromConfig == '') {
                this.percents = this.emptyPercItems();
            }
            //创建文档对象，开始解析xml
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(this.testpaper.Tp_FromConfig, "text/xml");
            var nodes = null;
            //解析按章节抽题时，各题型的占比
            if (xmlDoc.lastChild.children.length > 1) {
                nodes = xmlDoc.lastChild.children[1].firstChild.children;
                var arr = [];
                for (var i = 0; i < nodes.length; i++) {
                    const n = nodes[i];
                    var obj = {};
                    for (let j = 0; j < n.children.length; j++) {
                        const c = n.children[j];
                        var name = c.tagName;
                        var type = c.getAttribute('type');
                        var val = type.indexOf('Int') > -1 ? Number(c.textContent) : c.textContent;
                        obj[name] = val;
                    }
                    arr.push(obj);
                }
                //console.error(arr);
                this.percents = arr;
            }
            else {
                this.percents = this.emptyPercItems();
            }
            //获取章节信息，并生成章节相关配置项
            this.getOutline();
            return arr;
        },
        //解析章节配置项
        parseItemsXml: function () {
            //创建文档对象，开始解析xml
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(this.testpaper.Tp_FromConfig, "text/xml");
            var nodes = null;
            var steuparr = [];
            //解析按章节抽题时，各题型的占比
            if (xmlDoc.lastChild.children.length > 1) {
                nodes = xmlDoc.lastChild.children[1].lastChild.children;
                for (var i = 0; i < nodes.length; i++) {
                    const n = nodes[i];
                    var obj = {};
                    for (let j = 0; j < n.children.length; j++) {
                        const c = n.children[j];
                        var name = c.tagName;
                        var type = c.getAttribute('type');
                        var val = type.indexOf('Int') > -1 ? Number(c.textContent) : c.textContent;
                        obj[name] = val;
                    }
                    steuparr.push(obj);
                }
                this.items = steuparr;
            }
            /*
            //按章节生成配置信息
            for (let i = 0; i < this.outlines.length; i++) {
                const ol = this.outlines[i];
                this.items.push({'item':''});
            }
*/
        },
        //某个章节的试题类型的配置项
        //
        getOutlineItem: function (outline, type) {
            var olid = outline.Ol_ID;
            var obj = this.items.find(function (item) {
                return item.Ol_ID == olid && item.TPI_Type == type;
            });
            if (obj == null) {
                obj = this.emptyObj(type, olid, 0);
                this.items.push(obj);
            }
            return obj;
        },
        //创建空的设置项
        emptyPercItems: function () {
            if (this.types == null || this.types.length < 1) return [];
            var arr = [];
            for (let i = 0; i < this.types.length; i++) {
                arr.push(this.emptyObj(i + 1, 0, i));
            }
            return arr;
        },
        //空的配置项
        emptyObj: function (type, olid, index) {
            return {
                Ol_ID: olid,
                Org_ID: 0,
                Org_Name: "",
                TPI_Count: 0,
                TPI_ID: index,
                TPI_Number: 0,
                TPI_Percent: 0,
                TPI_Type: type,
                Tp_UID: ""
            };
        },
        //生成xml数据
        buildXml() {
            var xml = '<Outline><Percent>';
            for (let i = 0; i < this.percents.length; i++) {
                xml += build_item(this.percents[i]);
            }
            xml += '</Percent>';
            xml += '<Items>';
            //生成配置项，顺便计算试题量
            var count = 0;
            for (let i = 0; i < this.items.length; i++) {
                count += this.items[i].TPI_Count;
                xml += build_item(this.items[i]);
            }
            if (this.testpaper.Tp_FromType == 1) this.testpaper.Tp_Count = count;
            xml += '</Items></Outline>';
            function build_item(item) {
                let xml = '<TestPaperItem>';
                for (var t in item) {
                    var type = '';
                    switch ($api.getType(item[t])) {
                        case 'Number':
                            type = 'System.Int32';
                            break;
                        case 'String':
                            type = 'System.String';
                            break;
                    }
                    xml += '<' + t + ' type="' + type + '">';
                    if (type == 'System.Int32')
                        xml += item[t];
                    if (type == 'System.String')
                        xml += '<![CDATA[' + item[t] + ']]>';
                    xml += '</' + t + '>';
                }
                xml += '</TestPaperItem>';
                return xml;
            }
            return xml;
        },
        //当分数占比变更时
        changePercent: function () {
            this.error = '';
            var total = this.total ? this.total : 0;
            var surplus = total;
            for (let i = 0; i < this.percents.length; i++) {
                this.percents[i].TPI_Number = Math.floor(total * this.percents[i].TPI_Percent / 100);
                surplus -= this.percents[i].TPI_Number;
            }
            if (surplus < 0) {
                this.error = '各题型分数合计后大于总分 ' + this.total;
                return false;
            }
            //有没有分完的分数
            if (this.sumPercent == 100 && surplus > 0) {
                var max_item = this.percents[0];   //题量最多的项
                for (let i = 1; i < this.percents.length; i++) {
                    max_item = max_item.TPI_Count > this.percents[i].TPI_Count ? max_item : this.percents[i];
                }
                max_item.TPI_Number += surplus;
                console.log(max_item);
            }
            //如果大于100%
            if (this.sumPercent != 100) {
                this.error = '各题型占比的合计必须等于 100%';
                return false;
            }
            return true;
        },
        //检验输入
        check: function () {
            var ispass = this.changePercent();
            console.error(ispass);
            if (!ispass) return ispass;
            //检查当有试题时，占分比不得为空
            for (let i = 0; i < this.items.length; i++) {
                if (this.items[i].TPI_Count > 0 && this.items[i].TPI_Percent < 0)
                    return false;
            }
            if (this.sumPercent != 100) ispass = false;
            return ispass;
        },
    },
    template: `<div class="fromtype1">
        <el-row class="alert" v-if="error!=''">
            <alert>{{error}}</alert>
        </el-row>  
        <div>
        各题型占总分百分比： <el-tag type="info">各题型占比合计 {{sumPercent}} %</el-tag>
        </div>
        <div class="item_box">        
            <el-input v-model.number="m.TPI_Percent" v-for="(m,i) in percents" 
            type="number" :min="1" :max="100" @input="changePercent">
                <template slot="prepend">
                    <ques_type :type="i+1" :types="types" :showname="true"></ques_type>                    
                </template>
                <template slot="append">%</template>
            </el-input>              
        </div>   
        <alert v-if="outlines!=null && outlines.length<1">试卷所在课程没有章节信息</alert> 
        <div class="outline_box">
            <el-card class="box-card" v-for="(o,i) in outlines">
                <div slot="header" class="header">
                    {{i+1}}. {{o.Ol_Name}}             
                </div>
                <fromtype1_item  v-for="(t,j) in types" :item="getOutlineItem(o,j+1)" :types="types" 
                :couid="-1"></fromtype1_item>
            </el-card>
        </div>
    </div> `
});

Vue.component('fromtype1_item', {
    props: ['item', 'types', 'couid'],
    data: function () {
        return {

        }
    },
    template: `<el-input v-model.number="item.TPI_Count"
        type="number" :min="1" >
            <template slot="prepend">
                <ques_type :type="item.TPI_Type" :types="types" :showname="true"></ques_type>                    
            </template>
            <template slot="append">道
                <ques_count :couid='couid' :qtype='item.TPI_Type' :olid='item.Ol_ID'> </ques_count>
            </template>
        </el-input> `
});