//寮瑰嚭鎻愮ず绫绘秷鎭?
window.Prompt=function(msg,width,height){
    var show=new Message(msg,"prompt",width,height);
    show.Title="淇℃伅鎻愮ず";
    show.Show();
}
//寮瑰嚭鎻愮ず绫绘秷鎭?
window.Alert=function(msg,width,height){
    var show=new Message(msg,"alert",width,height);
    show.Title="璀﹀憡锛?;
    show.Show();
};
//寮瑰嚭璀﹀憡绫绘秷鎭?
window.Warning=function(msg,width,height){
    var show=new Message(msg,"warning",width,height);
    show.Title="搴旂敤绋嬪簭閿欒锛?;
    show.Show();
}
//寮瑰嚭鎺堟潈鐩稿叧淇℃伅
window.License=function(msg,width,height){
    var show=new Message(msg,"license",width,height);
    show.Title="杞欢鎺堟潈";
    show.Show();
}

//璇存槑锛氱敤浜庡脊鍑烘秷鎭彁绀?
//msg:瑕佹樉绀虹殑娑堟伅
//type:娑堟伅绫诲瀷
//width:寮瑰嚭娑堟伅绐椾綋鐨勫搴?
//height:寮瑰嚭娑堟伅绐椾綋鐨勯珮搴?
function Message(msg,type,width,height){
    this.Msg=msg;
    this.Width= (width && typeof(width)!="undefined" && width!=0) ? width:this.Width ;
    this.Height=(height && typeof(height)!="undefined" && height!=0) ? height:this.Height;
    this.Type=(type && typeof(type)!="undefined") ? type:this.Type;
}
//瑕佸彂甯冪殑娑堟伅鍐呭
Message.prototype.Msg="";
//瀹介珮
Message.prototype.Width=400;
Message.prototype.Height=200;
//寮瑰嚭娑堟伅鐨勭被鍨?
Message.prototype.Type="alert";
//娑堟伅涓婚
Message.prototype.Title="";
//浠ヤ笅鏄疢essage瀵硅薄鐨勬柟娉?
//寮瑰嚭娑堟伅
Message.prototype.Show=function(){
    $("body").append("<div id=\"MessageBox\"/>");
    var box=$("#MessageBox");
    //灞忓箷鐨勫楂?
    var hg=document.documentElement.clientHeight;
    var wd =document.documentElement.clientWidth;
    //璁剧疆绐楀彛鐨勪綅缃?
    box.css({"position":"absolute","z-index":"20001","border":"1px solid #999999",
        "background-color":"#fff","width":this.Width,"height":this.Height,
        top:(hg-this.Height)/2,left:(wd-this.Width)/2});
    //鐢熸垚绐楀彛
    this.Mask();
    //鐢熸垚鏍囬
    box.append("<div id=\"MessageBoxTitle\"></div>");
    var titbox=$("#MessageBoxTitle");
    titbox.append("<div id=\"MessageBoxTitleTxt\">"+this.Title+"</div>");
    titbox.append("<div id=\"MessageBoxTitleClose\" class='MessageClose'><img src=\"images/winClose.gif\"/></div>");
    titbox.css({"border-bottom-width": "1px","border-top-style": "none","border-right-style": "none",
        "border-bottom-style": "solid","border-left-style": "none","border-bottom-color": "#999999",
        "line-height": "25px","height": "25px"});
    if(this.Type=="prompt")titbox.css({"background-color":"#3399FF"});
    if(this.Type=="alert")titbox.css({"background-color":"#FF9900"});
    if(this.Type=="warning")titbox.css({"background-color":"#FF0000"});
    if(this.Type=="license")titbox.css({"background-color":"#666666"});
    $("#MessageBoxTitleTxt").css({"width":this.Width-22,"float":"left","text-indent":"10px"});
    $("#MessageBoxTitleClose").css({"height":"16px","width":"16px","margin-top":"4px","margin-right":"4px","float":"right","cursor":"pointer"});
    //鐢熸垚鍐呭鍖?
    box.append("<div id=\"MessageBoxContext\"></div>");
    var context=$("#MessageBoxContext");
    context.css({"height":this.Height-titbox.height()-30-20,"margin":"10px"});
    context.html(this.Msg);
    //鐢熸垚鎸夐挳鍖?
    box.append("<div id=\"MessageBoxFoot\"></div>");
    var foot=$("#MessageBoxFoot");
    foot.css({"height":30});
    foot.append("<div id=\"MessageBoxFootBtn\" class='MessageClose'>鍏抽棴</div>");
    $("#MessageBoxFootBtn").css({"border":"1px solid #999999","background-color":"#eee","width":80,"height":24,
        "margin-right":"4px","float":"right","line-height":"25px","text-align":"center","cursor":"pointer"});
    //璁剧疆鎷栧姩
    $("#MessageBox").easydrag();
    $("#MessageBox").setHandler("MessageBoxTitle");
    this.CreateEvent();
}
//鍒涘缓娑堟伅涓殑浜嬩欢锛屼富瑕佹槸鍏抽棴鎸夐挳
Message.prototype.CreateEvent=function(){
    $(".MessageClose").click(function(){
        $("div[id^=Message]").remove();
    });
}
//鐢熸垚閬僵灞?
Message.prototype.Mask=function(){
    $("body").append("<div id=\"MessageMask\"/>");
    var mask=$("#MessageMask");
    //灞忓箷鐨勫楂?
    var hg=document.documentElement.clientHeight;
    var wd =document.documentElement.clientWidth;
    mask.css({"position":"absolute","z-index":"20000",
        "width":wd,"height":hg,top:0,left:0});
    var alpha=60;
    mask.css({"background-color":"#fff","filter":"Alpha(Opacity="+alpha+")",
        "display":"block","-moz-opacity":alpha/100,"opacity":alpha/100});
    mask.fadeIn("slow");
}

