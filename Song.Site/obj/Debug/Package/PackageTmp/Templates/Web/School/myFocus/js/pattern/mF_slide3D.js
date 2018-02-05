myFocus__AGENT__.extend({//*********************mF_slide3D******************
	mF_slide3D:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['mask11','mask12','mask21','mask22','num','next']);
		var pics=F.$c('pic',box),num=F.$li('num',box),m11=F.$c('mask11',box),m12=F.$c('mask12',box),m21=F.$c('mask21',box),m22=F.$c('mask22',box),nex=F.$c('next',box),n=num.length;
		pics.innerHTML+=pics.innerHTML;
		nex.innerHTML='NEXT';
		//PLAY
		var pic=F.$li('pic',box),d=Math.ceil(par.height/6);
		eval(F.switchMF(function(){
			m11.style.cssText=m12.style.cssText='border-width:0px '+par.width/2+'px;';
			m21.style.cssText=m22.style.cssText='border-width:'+d+'px 0px;';
			num[index].className='';
		},function(){
			pic[n].innerHTML=pic[prev].innerHTML;
			pic[n+1].innerHTML=pic[next].innerHTML;
			F.$$('img',pic[n])[0].style.cssText='width:'+par.width+'px;height:'+par.height+'px';
			F.$$('img',pic[n+1])[0].style.cssText='width:0px;height:'+par.height+'px';
			F.slide(F.$$('img',pic[n])[0],{width:0});
			F.slide(F.$$('img',pic[n+1])[0],{width:par.width});
			F.slide(m11,{borderLeftWidth:0,borderRightWidth:0,borderTopWidth:d,borderBottomWidth:d});
			F.slide(m12,{borderLeftWidth:0,borderRightWidth:0,borderTopWidth:d,borderBottomWidth:d});
			F.slide(m21,{borderLeftWidth:par.width/2,borderRightWidth:par.width/2,borderTopWidth:0,borderBottomWidth:0});
			F.slide(m22,{borderLeftWidth:par.width/2,borderRightWidth:par.width/2,borderTopWidth:0,borderBottomWidth:0});
			num[next].className='current';
		}));
		eval(F.bind('num','par.trigger',par.delay));
		eval(F.turn('nex','nex'));
	}
});