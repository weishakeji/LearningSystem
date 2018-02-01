myFocus.extend({//*********************qqyue******************
	mF_sd_qqyue:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt']);
		var pics=F.$c('pic',box),pic=F.$li('pic',box),txts=F.$c('txt',box),txt=F.$li('txt',box),ps=F.$c('par',box),p=F.$li('par',box),n=txt.length;
		//CSS
		txts.style.width=par.width*5/11+'px';
		box.style.width=par.width*16/11+1+'px';
		ps.style.width=par.width-12*2+'px';
		for(var i=0;i<n;i++){
			txt[i].style.cssText='width:'+(par.width*5/11+2)+'px;height:'+(par.height-n+4)/n+'px;';
		}
		//PLAY
		eval(F.switchMF(function(){
			pic[index].style.display=p[index].style.display='none';
			txt[index].className='';
		},function(){
			F.fadeIn(pic[next]);
			txt[next].className='current';
			p[next].style.display='block';
		}));
		eval(F.bind('txt','par.trigger',par.delay));
	}
});