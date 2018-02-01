myFocus.extend({//*********************tbhuabao******************
	mF_tbhuabao:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['dot','txt','prev','next']);F.wrap([F.$c('pic',box)],'win');
		var pics=F.$c('pic',box),dot=F.$li('dot',box),txt=F.$li('txt',box),pre=F.$c('prev',box),nex=F.$c('next',box),n=txt.length;
		pics.innerHTML+=pics.innerHTML;//无缝复制
		for(var i=0;i<n;i++) dot[i].innerHTML='<a href="javascript:;">&bull;</a>';//小点
		pre.innerHTML='<a href="javascript:;">&#8249;</a>';nex.innerHTML='<a href="javascript:;">&#8250;</a>';//前后箭头
		//CSS
		var win=F.$c('win',box),pic=F.$li('pic',box),dots=F.$c('dot',box),dotH=32,arTop=par.height/2-32;
		box.style.height=par.height+dotH+'px';
		win.style.cssText='width:'+par.width+'px;height:'+par.height+'px;';
		pics.style.width=par.width*2*n+'px';
		for(var i=0;i<n;i++) txt[i].style.bottom=dotH+'px';
		for(var i=0;i<2*n;i++) pic[i].style.cssText='width:'+par.width+'px;height:'+par.height+'px;';//滑动固定其大小
		pre.style.cssText=nex.style.cssText='top:'+arTop+'px;';
		//PLAY
		eval(F.switchMF(function(){
			txt[index>=n?(index-n):index].style.display='none';
			dot[index>=n?(index-n):index].className = '';
		},function(){
			F.slide(pics,{left:-par.width*next});
			txt[next>=n?(next-n):next].style.display='block';
			dot[next>=n?(next-n):next].className = 'current';
		},'par.less'));
		eval(F.bind('dot','par.trigger',par.delay));
		eval(F.turn('pre','nex'));
	}
});
myFocus.set.params('mF_tbhuabao',{less:true});//支持无缝设置