$(function(){
	 _init();
});

function _init()
{
	var btn=$("#btnViewmap");
	btn.click(function(){
			var addr=$("input[id$=tbAddress]");
			getPosi(addr.val());
	});
	$("input[id$=tbAddress]").keyup(function(){
		$("input[id$='tbLng']").val($(this).val());
		getPosi($(this).val());
	});
}
//通过地址获取地理信息
function getPosi(addr)
{
	var mapUrl="../soap/Position.ashx?address={0}";
	mapUrl=mapUrl.replace("{0}",encodeURIComponent(addr));
	//alert(mapUrl);
	$.ajax({ 
           type: "get", 
           url: mapUrl, 
           dataType: "json", 
           success: function (data) { 
		  		var lng=data.lng;
				var lat=data.lat;
				var zoom=map.getZoom();
		   		map.centerAndZoom(new BMap.Point(lng, lat), zoom);
				map.clearOverlays();
				var point = new BMap.Point(lng,lat);
                var marker = new BMap.Marker(point);  // 创建标注
				map.addOverlay(marker);	
				//填充经纬度
				$("input[id$='tbLng']").val(lng);
				$("input[id$='tbLat']").val(lat);
            }, 
            error: function (XMLHttpRequest, textStatus, errorThrown) { 
                 //alert("错误代码："+textStatus); 				 
            } 
    });
}