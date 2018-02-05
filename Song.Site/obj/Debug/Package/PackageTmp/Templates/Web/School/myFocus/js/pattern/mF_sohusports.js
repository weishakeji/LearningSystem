myFocus.extend({//*********************搜狐体育******************
	mF_sohusports:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','num']);
		var pic=F.$li('pic',box),txt=F.$li('txt',box),num=F.$li('num',box),n=txt.length;
		//CSS
		for(var i=0;i<n;i++){pic[i].style.display=txt[i].style.display='none';}
		//PLAY
		eval(F.switchMF(function(){
			pic[index].style.display='none';
			txt[index].style.display='none';
			num[index].className='';
		},function(){
			F.fadeIn(pic[next]);
			txt[next].style.display='';
			num[next].className='current';
		}))
		eval(F.bind('num','par.trigger',par.delay));
	}
});