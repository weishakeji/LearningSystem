myFocus.extend({//*********************太平洋电脑网风格******************
	mF_pconline:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','num']);
		var pic=F.$li('pic',box),txt=F.$li('txt',box),num=F.$li('num',box),n=txt.length;
		//CSS
		var txtH=isNaN(par.txtHeight/1)?28:par.txtHeight;//设置默认的文字高度
		box.style.height=par.height+txtH+'px';
		//PLAY
		eval(F.switchMF(function(){
			pic[index].style.display='none';
			txt[index].style.display='none';
			num[index].className='';
		},function(){
			F.fadeIn(pic[next],par.duration);
			txt[next].style.display='block';
			num[next].className='current';
		}));
		eval(F.bind('num','par.trigger',par.delay));
	}
});
myFocus.set.params('mF_pconline',{duration:300});