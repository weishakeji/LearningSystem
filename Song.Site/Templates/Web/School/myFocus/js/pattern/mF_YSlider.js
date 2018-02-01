myFocus.extend({//*********************YSlide--翻页效果******************
	mF_YSlider:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['rePic','txt','num']);F.$c('rePic',box).innerHTML=F.$c('pic',box).innerHTML;//复制
		var pic=F.$li('pic',box),rePic=F.$li('rePic',box),txt=F.$li('txt',box),num=F.$li('num',box),n=pic.length;
		//PLAY
		var s=par.direct=='one'?1:0,d1=par.width,d2=par.height;
		eval(F.switchMF(function(){
			var r=s?1:Math.round(1+(Math.random()*(2-1))),dis,d,p={};
			dis=r==1?d1:d2,d=Math.round(Math.random()+s)?dis:-dis,p[r==1?'left':'top']=d;
			pic[index].style.display=txt[index].style.display='none';
			rePic[index].style.cssText='left:0;top:0;display:block;filter:alpha(opacity=100);opacity:1;';
			F.slide(rePic[index],p,500,'swing').fadeOut(rePic[index],500);
			num[index].className='';
		},function(){
			pic[next].style.display=txt[next].style.display='block';
			num[next].className='current';
		}))
		eval(F.bind('num','par.trigger',par.delay));
	}
});
myFocus.set.params('mF_YSlider',{direct:'random'});//切出方向,可选：'random'(随机) | 'one'(单向/向右)