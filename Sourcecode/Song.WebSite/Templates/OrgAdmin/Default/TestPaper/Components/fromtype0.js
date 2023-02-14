//试卷按课程抽题的组件
Vue.component('fromtype0', {
    //total:总分
    props: ['types', 'testpaper', 'total'],
    data: function () {
        return {
            items: [],       //配置项
            error: '',       //错误信息

            init: 0,         //初始化
            loading: false
        }
    },
    watch: {
        'testpaper': {
            handler: function (nv, ov) {
                if (nv != null) this.init++;
                if (this.init >= 2)
                    this.items = this.parserXML();
            }, immediate: true
        },
        'types': {
            handler: function (nv, ov) {
                if (nv != null) this.init++;
                if (this.init >= 2)
                    this.items = this.parserXML();
            }, immediate: true
        },
        'total': function (nv, ov) {
            this.changePercent();
        }
    },
    computed: {
        //各题型合计占比
        sumPercent: function () {
            var sum = 0;
            for (let i = 0; i < this.items.length; i++) {
                sum += this.items[i].TPI_Percent;
            }
            return sum;
        },
        //各题型合计分数
        sumNumber: function () {
            var sum = 0;
            for (let i = 0; i < this.items.length; i++) {
                sum += this.items[i].TPI_Number;
            }
            return sum;
        },
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'TestPaper/Components/Styles/fromtype0.css']);
    },
    methods: {
        //解析试题抽题的设置项
        parserXML: function (tp) {
            if (!this.testpaper || this.testpaper.Tp_FromConfig == null
                || this.testpaper.Tp_FromConfig == '')
                return this.emptyItems();
            var arr = [];
            //创建文档对象
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(this.testpaper.Tp_FromConfig, "text/xml");
            var nodes = null;
            if (xmlDoc.lastChild.children.length > 0)
                nodes = xmlDoc.lastChild.children[0].lastChild.children;
            else
                return this.emptyItems();
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
            return arr;
        },
        //生成xml数据
        buildXml() {
            var count = 0;
            var xml = '<All><Items>';
            for (let i = 0; i < this.items.length; i++) {
                const item = this.items[i];
                count += item.TPI_Count;
                xml += '<TestPaperItem>';
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
            }
            xml += '</Items></All>';
            if (this.testpaper.Tp_FromType == 0) this.testpaper.Tp_Count = count;
            return xml;
        },
        //创建空的设置项
        emptyItems: function () {
            if (this.types == null || this.types.length < 1) return [];
            var arr = [];
            for (let i = 0; i < this.types.length; i++) {
                var obj = {
                    Ol_ID: 0,
                    Org_ID: 0,
                    Org_Name: "",
                    TPI_Count: 0,
                    TPI_ID: i,
                    TPI_Number: 0,
                    TPI_Percent: 0,
                    TPI_Type: i + 1,
                    Tp_UID: ""
                };
                arr.push(obj);
            }
            return arr;
        },
        //当分数占比变更时
        changePercent: function () {
            this.error = '';
            var total = this.total ? this.total : 0;
            var surplus = total;
            for (let i = 0; i < this.items.length; i++) {
                this.items[i].TPI_Number = Math.floor(total * this.items[i].TPI_Percent / 100);
                surplus -= this.items[i].TPI_Number;
            }
            if (surplus < 0) {
                this.error = '各题型分数合计后大于总分 ' + this.total;
                return false;
            }
            //有没有分完的分数
            if (this.sumPercent == 100 && surplus > 0) {
                var max_item = this.items[0];   //题量最多的项
                for (let i = 1; i < this.items.length; i++) {
                    max_item = max_item.TPI_Count > this.items[i].TPI_Count ? max_item : this.items[i];
                }
                max_item.TPI_Number += surplus;
                console.log(max_item);
            }
            //如果大于100%
            if (this.sumPercent > 100) {
                this.error = '各题型占比的合计不得大于100%';
                return false;
            }
            return true;
        },
        inputCheck: function () {

        },
        //检验输入
        check: function () {
            var ispass = this.changePercent();
            if (!ispass) return ispass;
            //检查当有试题时，占分比不得为空
            for (let i = 0; i < this.items.length; i++) {
                if (this.items[i].TPI_Count > 0 && this.items[i].TPI_Percent <= 0)
                    return false;
            }
            return ispass;
        }
    },
    template: `<div>
        <el-row :gutter="10" v-for="(m,i) in items" class="fromtype0">
            <el-col :span="12">
                <el-input v-model.number="m.TPI_Count" type="number" :min="1">
                    <div slot="prepend" class="ques_type">
                        <ques_type :type="i+1" :types="types" :showname="true"></ques_type>                    
                    </div>
                    <template slot="append">道试题
                        <ques_count :couid='testpaper.Cou_ID' :qtype='i+1' :olid='0'> </ques_count>
                    </template>
                </el-input>
            </el-col>
            <el-col :span="12" :class="{'is-null':m.TPI_Count>0 && m.TPI_Percent<=0}">
                <el-input v-model.number="m.TPI_Percent" type="number" :min="1" :max="100" @input="changePercent">
                    <template slot="prepend"> 占总分</template>
                    <template slot="append">%， 计 {{m.TPI_Number}} 分</template>
                </el-input>
            </el-col>                           
        </el-row>    
        <el-row class="fromtype0" :gutter="10"> 
                <el-tag type="info">各题型占比合计 {{sumPercent}} %</el-tag>
                <el-tag type="info">各题型分数合计 {{sumNumber}} 分</el-tag>            
        </el-row>   
        <el-row class="fromtype0 alert" v-if="error!=''">
            <alert>{{error}}</alert>
        </el-row>   
    </div> `
});