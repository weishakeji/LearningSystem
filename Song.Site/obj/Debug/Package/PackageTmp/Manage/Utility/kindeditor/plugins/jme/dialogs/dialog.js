var KindEditor = parent.KindEditor || {};
document.write(
    "<link href=\"" + KindEditor.basePath + "plugins/jme/mathquill-0.9.1/mathquill.css\" rel=\"stylesheet\" type=\"text/css\" />" +
    "<script type=\"text/javascript\" src=\"" + KindEditor.basePath + "plugins/jme/mathquill-0.9.1/jquery.js\"></script>" +
    "<script type=\"text/javascript\" src=\"" + KindEditor.basePath + "plugins/jme/mathquill-0.9.1/mathquill.min.js\"></script>");
    var jmeMath = [
        [
            "{/}frac{}{}","^{}/_{}","x^{}","x_{}","x^{}_{}","{/}bar{}","{/}sqrt{}","{/}nthroot{}{}",
            "{/}sum^{}_{n=}","{/}sum","{/}log_{}","{/}ln","{/}int_{}^{}","{/}oint_{}^{}"
        ],
        [
            "{/}alpha","{/}beta","{/}gamma","{/}delta","{/}varepsilon","{/}varphi","{/}lambda","{/}mu",
            "{/}rho","{/}sigma","{/}omega","{/}Gamma","{/}Delta","{/}Theta","{/}Lambda","{/}Xi",
            "{/}Pi","{/}Sigma","{/}Upsilon","{/}Phi","{/}Psi","{/}Omega"
        ],
        [
            "+","-","{/}pm","{/}times","{/}ast","{/}div","/","{/}bigtriangleup",
            "=","{/}ne","{/}approx",">","<","{/}ge","{/}le","{/}infty",
            "{/}cap","{/}cup","{/}because","{/}therefore","{/}subset","{/}supset","{/}subseteq","{/}supseteq",
            "{/}nsubseteq","{/}nsupseteq","{/}in","{/}ni","{/}notin","{/}mapsto","{/}leftarrow","{/}rightarrow",
            "{/}Leftarrow","{/}Rightarrow","{/}leftrightarrow","{/}Leftrightarrow"
        ]
    ];
    function mathHtml(obj){
        var cols = 8;//一行放几个
        var slidLen = 34;//每个图标的宽或高
        var html="<div class='mathIcon'>";
        for(var i = 0 ; i < obj.count ; i ++){
            html += "<li onclick=\"insert('" + jmeMath[obj.groupid][i] 
			+ "')\" style=\"background-position:-" 
			+ (obj.x + Math.floor(i%8)*slidLen) 
			+ "px -" + (obj.y + Math.floor(i/8)*slidLen) + "px;\"></li>";
        }
        html += "</div>";
        if(obj.count > cols * 2){
            html += "<div class='more' mrows='" + Math.floor((obj.count + cols - 1) / cols) + "' isOpen='0'>更多</div>"
        }
        return html;
    }
     
    function insert(q){
        $("#jme-math").focus().mathquill("write", q.replace("{/}","\\"));
    }
setTimeout(function(){
    $(document).ready(function(){
        //隐藏内容div
        $(".tabContent div.mathBox").hide();
        //菜单点击事件
        $(".tabTitle li").click(function(){
            $(".tabContent div.mathBox").hide();
            var n = 0;
            var obj = this;
            $(".tabTitle li").each(function(i,o){
                if(obj == o){
                    n = i;
                }
            });
            $(".tabTitle li").removeClass("current");
            $(obj).addClass("current");
            $(".tabContent div.mathBox:eq(" + n + ")").show();
        });
        //缺省显示第一个
        $(".tabTitle li:eq(0)").click();
        //公式定义
        $(".tabContent div.mathBox:eq(0)").html(mathHtml({
                groupid:0,
                x:0,
                y:272,
                count:14
            }));
        $(".tabContent div.mathBox:eq(2)").html(mathHtml({
                groupid:2,
                x:0,
                y:0,
                count:36
            }));
        $(".tabContent div.mathBox:eq(1)").html(mathHtml({
                groupid:1,
                x:0,
                y:170,
                count:22
            }));
        //常用公式,更多按钮绑定的事件
        $(".tabContent div.mathBox div.more").click(function(){
                var rowHei = 40;
                var mi = $(this).parent().find(".mathIcon");
                if($(this).attr("isOpen") == '0'){
                    mi.animate({"height":(rowHei * Number($(this).attr("mrows")))+"px"});
                    $(this).html("↑ 收起");
                    $(this).attr("isOpen",'1');
                }else{
                    mi.animate({"height":(rowHei * 2)+"px"});
                    $(this).html("更多");
                    $(this).attr("isOpen",'0');
                }
                 
            });
        //公式编辑框
        $("#jme-math").html("").css("font-size", '30px').mathquill('editable').mathquill('write', "");
        //验证版本信息
        if(KindEditor.versionCode){
            $.ajax({
           url:"http://www.jmeditor.com/jme/v.php",
           type:"post", 
           dataType:"html",
           async:true,
           data: {
            versionCode:KindEditor.versionCode
           },
           success:function(data){
            $(".ad").html(data);
             },
             error:function(){
                //alert("err");
             }
        });
      }
    });
 
},1000);