$(
	function()
	{
		setInputButton();
	}
);

function setInputButton()
{
	var btn=$("input[id$=btnInput]");
	btn.click(
		function()
		{
			OpenWin('Register_Input.aspx','数据导入',800,600);
			return false;
		}
	);
}