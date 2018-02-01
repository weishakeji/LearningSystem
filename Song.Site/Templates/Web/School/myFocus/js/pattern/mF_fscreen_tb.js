myFocus.extend({//*********************fscreen******************
	mF_fscreen_tb:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['thu-bg','thumb','txt','prev','next']);F.wrap([F.$c('thumb',box)],'win');
		var pics=F.$c('pic',box),thuBg=F.$c('thu-bg',box),win=F.$c('win',box),thus=F.$c('thumb',box),thu=F.$li('thumb',box),txt=F.$li('txt',box),pre=F.$c('prev',box),nex=F.$c('next',box),n=txt.length;
		pre.innerHTML='<a href="javascript:;">&#8249;</a>';nex.innerHTML='<a href="javascript:;">&#8250;</a>';//前后箭头
		//CSS
		var pic=F.$li('pic',box),winW=par.width-20,sw=Math.floor(winW/4),sh=par.height/par.width*(sw-36)+16;//参考*按比例求高
		thuBg.style.height=sh+'px';
		win.style.cssText='width:'+winW+'px;height:'+sh+'px;';
		thus.style.width=sw*n+'px';
		for(var i=0;i<n;i++){
			thu[i].style.cssText=F.$$('span',thu[i])[0].style.cssText='width:'+(sw-36)+'px;height:'+(sh-16)+'px';//减去总padding(*)
			txt[i].style.left=-par.width+'px';
		}
		pre.style.cssText=nex.style.cssText='height:'+(sh-16)+'px;line-height:'+(sh-20)+'px;';//-padding
		//PLAY
		eval(F.switchMF(function(){
			for(var i=0;i<n;i++){
				F.stop(txt[i]);
				txt[i].style.left=-par.width+'px';
			}
			pic[index].style.display='none';
			thu[index].className = '';
		},function(){
			F.fadeIn(pic[next],300,function(){F.slide(txt[next],{left:0})});
			thu[next].className = 'current';
			eval(F.scroll('thus','"left"','sw',4));
		}));
		eval(F.bind('thu','"click"'));//让其只支持click点击
		eval(F.turn('pre','nex'));
	}
});