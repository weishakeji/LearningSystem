//试题右侧的按钮组
Vue.component('quesbuttons', {
    //question:当前显示的试题，即滑动到这个试题
    props: ['question', 'account'],
    data: function () {
        return {
            //试题中的按钮，当used为true时，启用icon2图标
            buttons: [
                { id: 'error', name: '报错', icon1: '&#xe70e', icon2: '&#xe72c', used: false, evt: t => t.isShowError = true },
                { id: 'notes', name: '笔记', icon1: '&#xa02e', icon2: '&#xa02e', used: false, evt: t => t.isShowNote = true },
                { id: 'collect', name: '收藏', icon1: '&#xe747', icon2: '&#xe679', used: false, evt: this.addcollect }],
            //供选择的错误项
            errorItems: [
                '试题没有答案',
                '试题图片不显示',
                '试题或答案有错别字',
                '试题不属于本学科',
                '解题思路与题干不符',
                '解题思路与答案矛盾',
                '与其他试题有重复'],
            errorSelect: [],     //选中的错误项
            errorInfo: '',       //填写的错误内容
            //笔记内容
            note: '',
            //是否显示笔记编辑的面板
            isShowNote: false,
            //是否显示报错界面
            isShowError: false,
            //更新 
            loading: false
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                if (nv == null) return;
                //试题是否已经报错
                const btn = this.getbtn('error');
                if (btn != null) btn.used = nv.Qus_IsWrong;
                //获了试题的收藏与笔记状态
                this.collectState();
                this.noteState();
            }, immediate: true
        },
    },
    mounted: function () {

    },
    methods: {
        //获取按钮
        getbtn: function (id) {
            return this.buttons.find(btn => btn.id === id) || null;
        },
        //按钮点击事件
        btnevent: function (btn) {
            btn.evt(this, btn);
        },
        //收藏的状态
        collectState: function () {
            if ($api.isnull(this.account) || $api.isnull(this.question)) return;
            const btn = this.getbtn('collect');
            $api.get('Question/CollectExist', { acid: this.account.Ac_ID, qid: this.question.Qus_ID })
                .then(req => req.data.success && (btn.used = req.data.result))
                .catch(err => console.error(err));
        },
        //笔记的状态，及内容
        noteState: function () {
            if ($api.isnull(this.account) || $api.isnull(this.question)) return;
            var th = this;
            const btn = this.getbtn('notes');
            btn.used = false;
            th.note = '';
            $api.get('Question/NotesSingle', { acid: this.account.Ac_ID, qid: this.question.Qus_ID })
                .then(req => {
                    if (req.data.success) {
                        const result = req.data.result;
                        th.note = result ? result.Stn_Context : '';
                        btn.used = $api.trim(th.note) != '';
                    }
                }).catch(err => console.error(err));
        },
        //设置收藏
        addcollect: function (th, btn) {
            if ($api.isnull(this.account) || $api.isnull(this.question)) return;
            const query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID };
            th.loading = true;
            $api.post(btn.used ? 'Question/CollectDelete' : 'Question/CollectAdd', query).then(function (req) {
                if (req.data.success) {
                    th.$toast({ position: 'bottom', message: (btn.used ? '删除收藏成功' : '试题收藏成功') });
                    btn.used = !btn.used;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err)).finally(() => th.loading = false);
        },
        //更改笔记内容
        noteUpdate: function (note) {
            if ($api.isnull(this.account)) return;
            var th = this;
            th.loading = true;
            th.isShowNote = false;
            var query = { 'acid': this.account.Ac_ID, 'qid': this.question.Qus_ID, 'note': note };
            $api.post('Question/NotesModify', query).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.$notify({ type: 'success', message: '笔记编辑成功' });
                    var btn = th.getbtn('notes');
                    if (btn != null) btn.used = result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err)).finally(() => th.loading = false);
        },
        //提交错误信息
        errorUpdate: function () {
            var th = this;
            th.loading = true;
            th.isShowError = false;
            var error = th.errorSelect.join(',') + "；" + this.errorInfo;
            $api.get('Question/WrongModify', { 'qid': this.question.Qus_ID, 'error': error }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.$notify({ type: 'success', message: '提交成功' });
                    $api.put('Question/ForID', { 'id': th.question.Qus_ID }).then(function (req) {
                        if (req.data.success) {
                            th.question = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err)).finally(() => th.loading = false);
        }
    },
    template: `<buttons no-font-size>        
        <loading asterisk v-if="loading">...</loading>
        <btn v-for="btn in buttons" @click="btnevent(btn)" :class="{used:btn.used}" v-else>
            <i v-html="btn.icon1" v-if="!btn.used"></i>
            <i v-html="btn.icon2" v-else></i>
            {{btn.name}}
        </btn>
        <van-popup v-model="isShowNote" position="left" closeable class="quesNote" remark="笔记">
                <van-field v-model="note" rows="5" autosize  type="textarea"  maxlength="140"
                placeholder="请输入笔记内容" show-word-limit></van-field>
                <van-cell>
                    <van-button type="primary" @click="noteUpdate(note)">确 定</van-button>
                </van-cell>
      </van-popup>
      <van-popup v-model="isShowError" position="left" closeable class="quesError" remark="报错">
            <template v-if="!question.Qus_IsWrong">
            <van-checkbox-group v-model="errorSelect">
                <van-checkbox :name="err" v-for="err in errorItems" shape="square">{{err}}</van-checkbox>                   
            </van-checkbox-group>
            <van-cell value="其它问题"></van-cell>
            <van-field v-model="errorInfo" rows="2" autosize  type="textarea"  maxlength="140"
            placeholder="请输入需要报错的内容" show-word-limit></van-field>               
            <van-button type="primary" @click="errorUpdate()">提交信息</van-button>    
            </template>       
            <div v-else>
            <van-cell value="已经有学员反馈："></van-cell>
            <van-cell value="其它问题">{{question.Qus_WrongInfo}}</van-cell>
            </div>    
        </van-popup>
    </buttons> `
});