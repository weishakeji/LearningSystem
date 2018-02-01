myFocus.extend({
	mF_liuzg:function(par,F){//*********************绚丽切片风格******************
		var box=F.$(par.id);
		F.addList(box,['txt','num']);//添加ul列表
		var pics=F.$c('pic',box),pic=F.$li('pic',box),n=pic.length;
		var c=par.height%par.chip?8:par.chip,h=par.height/c,html=[];
		for(var i=0;i<c;i++){
			html.push('<li><div>');
			for(var j=0;j<n;j++) html.push(pic[j].innerHTML);
			html.push('</div></li>');
		}
		pics.innerHTML=html.join('');
		//CSS
		var pic=F.$li('pic',box),txt=F.$li('txt',box),btn=F.$li('num',box),wrap=F.$$('div',pics);
		for(var i=0;i<c;i++){//初始化样式设置
			pic[i].style.cssText='width:'+par.width+'px;height:'+h+'px;';
			wrap[i].style.cssText='height:'+par.height*n+'px;top:'+-i*h+'px;';
			F.$$('a',pic[i])[0].style.height=par.height+'px';
		}
		//PLAY
		eval(F.switchMF(function(){
			txt[index].style.display='none';
			btn[index].className = '';
		},function(){
			var tt=par.type===4?Math.round(1+(Math.random()*(3-1))):par.type;//效果选择
			var dur=tt===1?1200:300;
			for(var i=0;i<c;i++){
				F.slide(wrap[i],{top:-next*c*h-i*h},tt===3?Math.round(300+(Math.random()*(1200-300))):dur);
				dur=tt===1?dur-150:dur+150;
			}
			txt[next].style.display='block';
			btn[next].className = 'current';
		}))
		eval(F.bind('btn','par.trigger',par.delay));
	}
});
myFocus.set.params('mF_liuzg',{//可选个性参数
	chip:8,//图片切片数量，能被焦点图的高整除才有效，默认为8片
	type:4////切片效果，可选：1(甩头) | 2(甩尾) | 3(凌乱) | 4(随机)
});