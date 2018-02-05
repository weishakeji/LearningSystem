myFocus.extend({//*********************2010世博******************
	mF_expo2010:function(par,F){
		var box=F.$(par.id);//定义焦点图盒子
		F.addList(box,['txt-bg','txt','num']);//添加ul列表
		var pic=F.$li('pic',box),txt=F.$li('txt',box),num=F.$li('num',box),n=pic.length;//定义焦点图元素
		//CSS
		var H='default'?36:par.txtHeight+60;
		for(var i=0;i<n;i++){
			pic[i].style.display="none";
			txt[i].style.bottom=-H+'px';
		}
		//PLAY
		eval(F.switchMF(function(){
			F.fadeOut(pic[index]);
			num[index].className='';
		},function(){
			F.fadeIn(pic[next]);
			F.slide(txt[prev],{bottom:-H},200,'swing',function(){F.slide(txt[next],{bottom:0},200,'easeOut')});
			num[next].className='current';
		}))
		eval(F.bind('num','par.trigger',par.delay));//固定其延迟
	}
});