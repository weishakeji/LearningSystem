//鍒濆鍖栨柟娉?
$(
	function()
	{		
		var vtName=$(".vtName");	
		vtName.each(
			function()
			{
				var p=$(this).parent();
				$(this).css({width:p.width()+"px"});
			}
		);
	}  
 );
