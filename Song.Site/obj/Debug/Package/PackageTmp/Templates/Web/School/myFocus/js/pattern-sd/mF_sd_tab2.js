myFocus.extend({//*********************tab2******************
	mF_sd_tab2:function(par,F){
		var box=F.$(par.id),dl=F.$$_('dl',box),dt=[],dd=F.$$_('dd',dl[0])[0],inlist=[];
		var n=dl.length,type=par.type||'slide';//运行时相关参数
		for(var i=0;i<n;i++){
			dt.push(F.$$_('dt',dl[i])[0]);
			inlist.push('<li class=in-li>'+F.$$_('dd',dl[i])[0].innerHTML+'</li>');
		}
		dd.innerHTML=inlist.join(''),F.wrapIn(dd,'in-ul');
		//CSS
		var ul=F.$c('in-ul',box),li=F.$li('in-ul',box);
		var w=dd.offsetWidth,h=F.style(dd,'height'),tH=F.style(dt[0],'height');
		box.style.cssText='width:'+w+'px;height:'+(h+tH+3)+'px;';//加边框(上中下各1px)
		ul.style.width=w*n+'px';
		for(var i=0;i<n;i++){
			F.$$_('dd',dl[i])[0].style.display='none';
			li[i].style.width=w+'px';
			if(type=='none') li[i].style.cssText='position:absolute;display:none;';
		}
		dd.style.display='block';
		//PLAY
		eval(F.switchMF(function(){
			dl[index].className='';
			if(type=='none') li[index].style.display='none';
		},function(){
			dl[next].className='current';
			if(type=='slide') F.slide(ul,{left:-next*w},300,'easeInOut');
			if(type=='none') li[next].style.display='block';
		}));
		eval(F.bind('dt','par.trigger',par.delay));
	}
});