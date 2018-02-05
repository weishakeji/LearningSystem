myFocus.extend({//*********************51xflash******************
	mF_51xflash:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','play']);F.wrap([F.$c('pic',box)],'cont')
		var cont=F.$c('cont',box),pics=F.$c('pic',cont),pic=F.$li('pic',cont),txt=F.$li('txt',box),btn=F.$c('play',box);
		var n=pic.length;//运行时相关参数
		//CSS
		var pad=4,w=(par.width/3),h=(par.height-pad*2)/3,disX=w+pad,disY=h+pad,txtH=isNaN(par.txtHeight/1)?34:par.txtHeight;
		box.style.cssText='width:'+(par.width+disX)+'px;height:'+(par.height+txtH+(txtH==0?0:pad))+'px;';//焦点图盒子
		cont.style.cssText='width:'+(par.width+disX)+'px;height:'+par.height+'px;';//图片盒子
		for(var i=0;i<n;i++){
			txt[i].style.display='none';
			pic[i].style.cssText='width:'+w+'px;height:'+h+'px;top:'+disY*(i-1)+'px;';
		}
		//PLAY
		eval(F.switchMF(function(){
			txt[index].style.display='none';
		},function(){
			pic[prev].style.zIndex=2,pic[next].style.zIndex=1;
			F.slide(pic[prev],{right:0,top:parseInt(pic[next].style.top),width:w,height:h},400,'easeOut',function(){this.style.zIndex=''});
			F.slide(pic[next],{right:disX,top:0,width:par.width,height:par.height},400);
			txt[next].style.display='';
		}))
		eval(F.bind('pic','"click"'));
		eval(F.toggle('btn','play','stop'));
	}
});