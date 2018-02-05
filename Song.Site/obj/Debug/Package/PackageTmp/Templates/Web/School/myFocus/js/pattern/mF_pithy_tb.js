myFocus.extend({//*********************pithy******************
	mF_pithy_tb:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['thumb','txt','prev','next']);
		var pics=F.$c('pic',box),thus=F.$c('thumb',box),thu=F.$li('thumb',box),txt=F.$li('txt',box),pre=F.$c('prev',box),nex=F.$c('next',box),n=txt.length;
		pics.innerHTML+=pics.innerHTML;//无缝复制
		//CSS
		var pic=F.$li('pic',box),sw=par.width/4,sh=Math.floor(par.height/4);
		box.style.width=5*sw+4+'px';
		pics.style.height=2*par.height*n + 'px';
		thus.style.cssText='width:'+sw+'px;height:'+sh*n+'px;';
		for(var i=0;i<n;i++){
			thu[i].style.cssText='width:'+(sw-17)+'px;height:'+(sh-10)+'px';//减去总padding
			F.$$('span',thu[i])[0].style.cssText='width:'+(sw-7-1)+'px;height:'+(sh-2)+'px';//减去1px边框和pading
		}
		for(var i=0;i<2*n;i++) pic[i].style.height=par.height+'px';//消除间隙
		pre.style.right=sw+40+'px';nex.style.right=sw+16+'px';//相差nex的宽=24
		//PLAY
		eval(F.switchMF(function(){
			txt[index>=n?(index-n):index].style.display='none';
			thu[index>=n?(index-n):index].className = '';
		},function(){
			F.slide(pics,{top:-par.height*next},600);
			txt[next>=n?(next-n):next].style.display='block';
			thu[next>=n?(next-n):next].className = 'current';
			eval(F.scroll('thus','"top"','sh',4));
		},'par.less','"top"'));
		eval(F.bind('thu','"click"'));//让其只支持click点击
		eval(F.turn('pre','nex'));
	}
});
myFocus.set.params('mF_pithy_tb',{less:true});