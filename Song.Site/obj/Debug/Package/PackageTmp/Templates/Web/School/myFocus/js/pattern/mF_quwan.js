myFocus.extend({//*********************趣玩风格******************
	mF_quwan:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','num']);
		var pic=F.$li('pic',box),txt=F.$li('txt',box),num=F.$li('num',box),n=txt.length;
		//CSS
		var numH=num[0].offsetHeight;
		for(var i=0;i<n;i++){
			box.style.height=par.height+numH+1+'px';
			txt[i].style.cssText='bottom:'+(numH+1)+'px;';
		}
		//PLAY
		eval(F.switchMF(function(){
			pic[index].style.display='none';
			txt[index].style.display='none';
			num[index].className='';
		},function(){
			F.fadeIn(pic[next]);
			txt[next].style.display='block';
			num[next].className='current';
		}));
		eval(F.bind('num','par.trigger',par.delay));
	}
});