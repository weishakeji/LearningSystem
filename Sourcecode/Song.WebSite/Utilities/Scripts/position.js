/*! 获取前端页面GPS信息 */

(function () {
    var posi = function () { }
    //获取GPS坐标的相关参数
    posi.prototype.options = {
        timeout: 1000,          //请求超时时间,单位ms；
        maximumAge: 1000,       //缓存最长时间，单位为毫秒；如果设为 0，表示不返回缓存值，据说在Chrome和IOS下被忽略
        enableHighAcuracy: true //是否获取更精确的位置
    };
    //经纬度坐标的变量值，当获取成功后作为临时值存储在这里
    posi.prototype.coords = {
        'longitude': 0,     //经度
        'latitude': 0       //纬度
    };
    //获取GPS
    posi.prototype.getRegion = function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(this.updataPosition,
                this.handleError,
            );
        }
    };
    //获取成功
    posi.prototype.updataPosition = function (position) {
        let lng = position.coords.longitude;//经度
        let lat = position.coords.latitude;//纬度
        let acu = position.coords.accuracy; //精准度
        /*
        console.log('经度:' + lng);
        console.log('纬度:' + lat);
        console.log('精度:' + acu);
        alert('经度:' + lng + ',纬度:' + lat);
        */
        //alert(JSON.stringify(position.coords));
        window.$posi.coords = position.coords;
    };
    //获取失败
    posi.prototype.handleError = function (error) {
        let msg = "";
        switch (error.code) {
            case error.PERMISSION_DENIED:
                msg = "用户拒绝对地理位置要求";
                break;
            case error.POSITION_UNAVAILABLE:
                msg = "信息是不可用的";
                break;
            case error.TIMEOUT:
                msg = "连接超时，请重试";
                break;
            case error.UNKNOWN_ERROR:
                msg = "出现未知错误";
                break;
        }
        console.error(msg);
        //alert('地理位置服务_错误提示：' + msg)
    }

    window.$posi = new posi();
    //获取GPS信息
    window.position_coords_intervalId = setInterval(function () {
        if (window.$posi.coords.longitude == 0 || window.$posi.coords.latitude == 0)
            window.$posi.getRegion();
        else
            clearInterval(window.position_coords_intervalId);
    }, 500);

    //如果是移动端，则当地理位置变更时，重新获取GPS信息
    if ($dom.ismobi() || $dom.ispad())
        window.navigator.geolocation.watchPosition($posi.updataPosition, $posi.handleError, $posi.options);

})();