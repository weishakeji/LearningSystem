myFocus.extend({//*********************kiki******************
	mF_kiki:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','btn','prev','next']);
		var pic=F.$li('pic',box),txt=F.$li('txt',box),btn=F.$li('btn',box),pre=F.$c('prev',box),nex=F.$c('next',box),n=txt.length;
		F.wrap([pre,nex],'con'),pre.innerHTML='<a href="javascript:;">PREV</a> |',nex.innerHTML='<a href="javascript:;">NEXT</a>';
		//CSS
		var d1=par.width/2,d2=par.height/2,txtH=isNaN(par.txtHeight/1)?30:par.txtHeight;//设置默认的文字高度;
		box.style.height=par.height+txtH+'px';
		//PLAY
		var s1,s2=1;//方向选择
		switch(par.turn){
			case 'left':s1=1,s2=1;break;
			case 'right':s1=1,s2=-1;break;
			case 'up':s1=2,s2=1;break;
			case 'down':s1=2,s2=-1;break;
		}
		eval(F.switchMF(function(){},function(){
			btn[prev].className='',btn[next].className='current';
			var tt=s1==1?1:(s1==2?2:Math.round(1+(Math.random()*(2-1)))),dis,d,p_s1={},p_s2={},p_e={};
			dis=tt==1?d1:d2,d=s2*dis,p_s1[tt==1?'left':'top']=d,p_s2[tt==1?'left':'top']=-d,p_e[tt==1?'left':'top']=0;
			if(!first){F.stop(pic[prev]),pic[prev].style.cssText='left:0;top:0;z-index:3';}
			if(!first){F.stop(pic[next]),pic[next].style.cssText='left:0;top:0;z-index:2';}
			F.slide(pic[prev],p_s2,300,'linear',function(){txt[prev].style.display='none',this.style.zIndex=1,F.slide(this,p_e,800,'easeOut',function(){this.style.zIndex='';});});
			F.slide(pic[next],p_s1,300,'linear',function(){txt[next].style.display='block',this.style.zIndex=3,F.slide(this,p_e,800,'easeOut');});
		}));
		eval(F.bind('btn','par.trigger',0));
		eval(F.turn('pre','nex'));
	}
});
myFocus.set.params('mF_kiki',{turn:'random'})//翻牌方向,可选：'left'(左)|'right'(右)|'up'(上)|'down'(下)|'random'(单向随机)