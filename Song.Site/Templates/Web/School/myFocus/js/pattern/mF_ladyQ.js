myFocus.extend({//*********************ladyQ******************
	mF_ladyQ:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','num','cout']);F.wrap([F.$c('num',box)],'num_box');
		var pic=F.$li('pic',box),txt=F.$li('txt',box),num=F.$li('num',box),cout=F.$c('cout',box),n=txt.length,over=false,start=true;
		//CSS
		var numH=28,coutW=par.width-23*n-6;
		box.style.height=par.height+numH+'px';
		cout.style.cssText='top:'+(par.height+4)+'px;width:'+coutW+'px';
		for(var i=0;i<n;i++) txt[i].style.bottom=numH-1+'px',pic[i].style.display='none';
		//PLAY
		eval(F.switchMF(function(){
			pic[index].style.zIndex=1;
			if(!start) F.slide(pic[index],{opacity:0},600,'easeOut',function(){this.index=''});
			txt[index].style.display='none';
			num[index].className='';
			F.stop(cout),cout.style.width=coutW+'px';
			if(!over) F.slide(cout,{width:0},_t,'linear');
		},function(){
			F.slide(pic[next],{opacity:1},600,'easeInOut');
			txt[next].style.display='block';
			num[next].className='current',start=false;
		}));
		eval(F.bind('num','par.trigger',par.delay));
		F.addEvent(box,'mouseover',function(){F.stop(cout),cout.style.width=coutW+'px',over=true;});
		F.addEvent(box,'mouseout',function(){F.slide(cout,{width:0},_t,'linear'),over=false;});
	}
});