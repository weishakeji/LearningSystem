Page({
  data: {
    options:null,
    title:'正在调取支付模块'
  },  
  onLoad: function (options) {
    var that = this;
    console.log(options);
    console.log("mchid:" + options.mchid + "  paykey:" + options.paykey);
    this.setData({
      options: options
    });
    wx.showToast({
      title: '拉取支付界面',
      icon: 'loading',
      duration: 2000,
      mask: true
    });
    wx.login({
      //获取code  
      success: (res) => {
        wx.request({
          url: getApp().data.server+'/pay/weixin/miniProgramOpenid.aspx',
          data: {
            scode: res.code   // 使用wx.login得到的登陆凭证，用于换取openid  
          },
          method: 'POST',
          header: {
            'content-type': 'application/x-www-form-urlencoded'
          },
          success: (d) => {    
            console.log(d);
            //d.data应该是openid，如果返回失败则为空         
            that.getorder(d.data, that.data.options)
          },
          fail: function (d) {
            console.log('submit fail');
            console.log(d);
          }, 
          complete: function (d) {
            console.log('submit complete');
          } 
        })
        console.log("code:" + res.code) //这里只是为了在微信小程序客户端好查看结果，找寻问题  
      }
    });
  },
  //  
  //发起统一定单
  getorder: function (openid, options) {
    var that = this;
    console.log("openid:" + openid);
    console.log("options:" + options.appid);
    console.log("自定义测试成功:" + options.appid);
    wx.request({
      method: "GET",
      url: getApp().data.server + '/pay/weixin/miniProgramCall.aspx',
      data: {
        openid: openid, appid: options.appid, secret: options.secret,
        mchid: options.mchid, paykey: options.paykey, total_fee: options.total_fee,
        serial: options.serial, orgid: options.org, notify_url: options.notify_url
      },      
      success: (d) => {
        //that.getorder(d.data, that.data.options)
        //console.log("json:" + d.data.package)
        that.pay(d.data);
      }
    });
    //return 4;
  },
  //调取支付功能
  pay:function(data){
    console.log(data);
    var that = this;
    wx.requestPayment({
      timeStamp: data.timeStamp,
      nonceStr: data.nonceStr,
      package: data.package,
      signType: 'MD5',
      paySign: data.paySign,
      success: function (res) {
        // 支付成功  
        that.navback()
      },
      fail: function (res) {
        if (res.errMsg === "requestPayment:fail cancel") {
          // 用户取消支付  
          that.navback()
          return
        }
        if (res.errMsg === "requestPayment:fail") {
          console.log(res.err_desc) // 错误信息  
          that.navback()
          return
        }
      }
    }) ;
  },
  navback: () => {
    setTimeout(function () {
      wx.navigateBack({ delta: 1 })
    }, 100)
  }
});