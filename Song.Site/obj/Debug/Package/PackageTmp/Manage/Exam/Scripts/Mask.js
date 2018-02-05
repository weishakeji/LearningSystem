//生成遮罩层
function Mask(){};
Mask.Open=function()
{
    var mask=$("#screenMask");
    //屏幕的宽高
    var hg=document.documentElement.clientHeight;
    var wd =document.documentElement.clientWidth;
    //设置遮罩的宽高
    mask.width(wd).height(hg);
    mask.css("top",0).css("left",0);
    //mask.fadeIn("slow");
    mask.show();
}
Mask.Loading=function() {
    Mask.Open();
    var loading=$("#loading");
    var hg=document.documentElement.clientHeight;
    var wd =document.documentElement.clientWidth;
    loading.css("top",(hg-loading.height())/2);
    loading.css("left",(wd-loading.width())/2);
    loading.show();
}
Mask.LoadingClose=function(){
    $("#screenMask").hide();
    $("#loading").hide();
}
Mask.Submit=function() {
    Mask.Open();
    var loading=$("#submitBox");
    var hg=document.documentElement.clientHeight;
    var wd =document.documentElement.clientWidth;
    loading.css("top",(hg-loading.height())/2);
    loading.css("left",(wd-loading.width())/2);
    loading.show();
}
Mask.SubmitClose=function(){
    $("#submitBox").hide();
    alert("答题信息已经提交！");
    $("#screenMask").fadeOut();
    new parent.PageBox().Close();
}
Mask.InResult=function() {
    $("#inResultLoading").show();
}
Mask.InResultClose=function(){
    $("#inResultLoading").hide();
}