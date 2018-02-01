myFocus.extend({
	mF_classicHB:function(par,F){//*********************经典怀旧系列二--海报风格******************
		var box=F.$(par.id);//定义焦点图盒子
		F.addList(box,['txt','num']);//添加ul列表
		var pic=F.$li('pic',box),txt=F.$li('txt',box),num=F.$li('num',box),n=pic.length;//定义焦点图元素
		//CSS
		var txtH=isNaN(par.txtHeight/1)?20:par.txtHeight;//设置默认的文字高度
		for(var i=0;i<n;i++){
			pic[i].style.cssText="display:none;top:-"+0.1*par.height+"px;left:-"+0.1*par.width+"px;width:"+1.2*par.width+"px;height:"+1.2*par.height+"px;"
			txt[i].style.top=-txtH+'px';
		}
		//PLAY
		eval(F.switchMF(function(){
			F.stop(pic[index]).stop(txt[index]);
			pic[index].style.cssText="display:none;top:-"+0.1*par.height+"px;left:-"+0.1*par.width+"px;width:"+1.2*par.width+"px;height:"+1.2*par.height+"px;"
			txt[index].style.top=-txtH+'px';
			F.slide(num[index],{width:16},200),num[index].className='';
		},function(){
			F.fadeIn(pic[next],300).slide(pic[next],{width:par.width,height:par.height,top:0,left:0},300)
			F.slide(txt[next],{top:0},300);
			F.slide(num[next],{width:26},200),num[next].className='current';
		}))
		eval(F.bind('num','par.trigger',par.delay));
	}
});