myFocus.extend({//*********************奇艺******************
	mF_qiyi:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt','num']);F.wrap([F.$c('pic',box),F.$c('txt',box)],'swt');
		var swt=F.$c('swt',box),pic=F.$li('pic',swt),txt=F.$li('txt',swt),num=F.$li('num',box),n=txt.length;
		//CSS
		var txtH=isNaN(par.txtHeight/1)?34:par.txtHeight;//设置默认的文字高度
		swt.style.width=par.width*n+'px';
		for(var i=0;i<n;i++) pic[i].style.width=par.width+'px';
		//PLAY
		eval(F.switchMF(function(){
			num[index].className='';
		},function(){
			txt[next].style.top=0+'px';//复位
			F.slide(swt,{left:-par.width*next},800,'easeOut',function(){F.slide(txt[next],{top:-txtH})});
			num[next].className='current';
		}))
		eval(F.bind('num','par.trigger',par.delay));
	}
});