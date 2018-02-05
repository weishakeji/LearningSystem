myFocus.extend({//*********************luluJQ******************
	mF_luluJQ:function(par,F){
		var box=F.$(par.id),pics=F.$c('pic',box);
		for(var i=0,l=F.$$('a',pics);i<l.length;i++) l[i].innerHTML+='<span><b>'+F.$$('img',pics)[i].alt+'</b><i></i></span>';
		var pics=F.$c('pic',box),pic=F.$li('pic',box),txt=F.$$('span',pics),txtBg=F.$$('i',pics),n=pic.length;
		//CSS
		var pad=par.pad||32,txtH=isNaN(par.txtHeight/1)?34:par.txtHeight;//设置默认的文字高度;
		box.style.width=par.width+(n-1)*pad+'px';
		for(var i=0;i<n;i++){
			if(i>0) pic[i].style.left=par.width+(i-1)*pad+'px';
			if(i>0&&par.gray) F.alterSRC(pic[i],'-2');
			txt[i].style.cssText=txtBg[i].style.cssText='height:'+txtH+'px;line-height:'+txtH+'px;'
		}
		//PLAY
		eval(F.switchMF(function(){
			F.slide(txt[index],{top:0});
			if(par.gray) F.alterSRC(pic[index],'-2');
		},function(){
			for(var i=0;i<n;i++){
				if(i<=next) F.slide(pic[i],{left:i*pad});
				else F.slide(pic[i],{left:par.width+i*pad-pad});
			}
			F.slide(txt[next],{top:-txtH});
			if(par.gray) F.alterSRC(pic[next],'-2',true);
		}))
		eval(F.bind('pic','par.trigger',par.delay));
	}
});
myFocus.set.params('mF_luluJQ',{//可选个性参数
	pad:68,//图片留边宽度(像素)
	gray:false//非当前图片是否变灰,可选：true(是) | false(否),如果要变灰,请先准备图片灰色的副本并命名为："原来图片的名字-2"
});