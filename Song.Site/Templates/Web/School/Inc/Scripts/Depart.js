$(function(){
    var cssfile=$(".cssfile:first").attr("src");
   /* $("<link>").attr({ rel: "stylesheet",
            type: "text/css",
            href: cssfile
        }).appendTo("head");
   */
        var styleTag = document.createElement("link");
        styleTag.setAttribute('type', 'text/css');
        styleTag.setAttribute('rel', 'stylesheet');
        styleTag.setAttribute('href', cssfile);
        $("head")[0].appendChild(styleTag);

    window.setTimeout(function(){
        DepartInitLoyout();
        CourseBoxShow();
        subjectHoverEvent();
    },200);

});
//选课按钮
function CourseEvent(){

}
//初始布局
function DepartInitLoyout(){
    //设置位置
    var btn=$(".courseBtn");
    //设置院系的行高
    var box=$(".departBox");
    var off=btn.offset();
    box.width(btn.width());
    box.css({top:off.top+btn.height(),left:off.left});
    var depname=box.find(".depName");
    var hg=box.height()/depname.size();
    depname.height(hg);
    depname.css("line-height",hg+"px");
    box.find(".depName i").height(hg);
    box.find(".depName i").css("line-height",hg+"px");
    //设置专业的显示块
    var subject=$(".subjectBox");
    $("#subjectArea").width(980-btn.width()).height(box.height);
    subject.width(980-btn.width()).height(box.height);
}
/*课程选择区域*/
function CourseBoxShow(){
    var btn=$(".courseBtn");
    var box=$(".departBox");
    //如果有供显示学院的区域，则直接显示，否则显示下拉
    if($("#mainShow").size()>0) {
        box.slideDown(1000);
    }else{
        _departHoverEvent();
    }
    //当鼠标滑过院系名称时
    $(".depName").hover(function(){
        $(".departBox .depName").removeClass("depNameOver");
        $(this).addClass("depNameOver");
        //
        var depid=$(this).attr("depid");
        $(".subjectBox").hide();
        var curr=$(".subjectBox[depid="+$(this).attr("depid")+"]");
        var btn=$(".courseBtn");
        var off=btn.offset();
        curr.css({top:off.top+btn.height(),left:off.left+btn.width()});
        curr.show();
        window.depHover=true;
    },function(){
        window.depHover=false;
        window.setTimeout(function(){
            if(!window.depHover){
                $(".departBox .depName").removeClass("depNameOver");
                $(".subjectBox").hide();
            }
        },1000);
    });
    $(".subjectBox").hover(function(){
        window.depHover=true;
    },function(){
        window.depHover=false;
        window.setTimeout(function(){
            if(!window.depHover){
                $(".departBox .depName").removeClass("depNameOver");
                $(".subjectBox").hide();
            }
        },1000);
    });
}
//鼠标滑过事件
function _departHoverEvent(){
    $(".courseBtn").hover(function(){
        $(".departBox").slideDown(1000);
        window.departHover=true;
    },function(){
        window.departHover=false;
        window.setTimeout(function(){
            if(!window.departHover){
                $(".departBox .depName").removeClass("depNameOver");
                $(".departBox").slideUp(1000);
                $(".subjectBox").hide();
            }
        },1000);
    });
    $("div[hover=depart]").hover(function(){
        window.departHover=true;
    },function(){
        window.departHover=false;
        window.setTimeout(function(){
            if(!window.departHover){
                $(".departBox .depName").removeClass("depNameOver");
                $(".departBox").slideUp(1000);
                $(".subjectBox").hide();
            }
        },1000);
    });
}
//专业的鼠标滑过事件
function subjectHoverEvent(){
    var sbjitem= $(".sbjItem");
    sbjitem.click(function(){
        window.location.href=$(this).attr("href");
    });
    sbjitem.hover(function(){
        sbjitem.removeClass("sbjItemOver");
        $(this).addClass("sbjItemOver");
    },function(){
        sbjitem.removeClass("sbjItemOver");
    });
}