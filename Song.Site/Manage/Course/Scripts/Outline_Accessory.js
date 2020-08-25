$(function () {
    //刷新缓存
    var $notify = window.ELEMENT.Notification;

    $api.put('Outline/Accessory', { 'uid': $().getPara("uid") }).then(function (req) {
        if (req.data.success) {
            var result = req.data.result;
            $notify({ message: '附件缓存刷新成功', type: 'success',position: 'bottom-left' });
        } else {
            throw req.data.message;
        }
    }).catch(function (err) {
        // alert(err);
    });
});

