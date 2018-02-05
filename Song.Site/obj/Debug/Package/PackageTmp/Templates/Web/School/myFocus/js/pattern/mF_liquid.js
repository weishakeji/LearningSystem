myFocus.extend({//*********************液动风格******************
	mF_liquid:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['mod','txt','num']);F.$c('mod',box).innerHTML=F.$c('pic',box).innerHTML;
		var pic=F.$li('pic',box),mod=F.$li('mod',box),txt=F.$li('txt',box),num=F.$li('num',box),n=pic.length;
		//CSS
		var imod=(function(){for(var a=[],i=0;i<n;i++) a.push(F.$$('img',mod[i])[0]);return a;})();
		for(var i=0;i<n;i++){
			pic[i].style.cssText='width:0px;z-index:1;';
			imod[i].style.cssText='width:'+par.width*10+'px;height:'+par.height+'px;left:'+par.width+'px;';
		}
		//PLAY
		eval(F.switchMF(function(){
			F.stop(pic[index]).stop(imod[index]);
			pic[index].style.width=0+'px';
			imod[index].style.left=par.width+'px';
			txt[index].style.display='none';
			num[index].className = '';
		},function(){
			F.slide(imod[next],{left:0},100,'linear',function(){F.slide(pic[next],{width:par.width},700).slide(this,{left:-9*par.width},700)});
			txt[next].style.display='block';
			num[next].className = 'current';
		}));
		eval(F.bind('num','par.trigger',0));//延迟固定为0以兼容IE6
	}
});