Page({
  data: {
    options:null  
  },  
  onLoad: function (options) {
    var that = this;
    //console.log(options)
    //console.log("mchid:" + options.mchid + "  paykey:" + options.paykey)
    this.setData({
      options: options
    })
    wx.showToast({
      title: '正在获取Openid',
      icon: 'loading',
      duration: 2000,
      mask: true
    })
    wx.login({
      //获取code  
      success: (res) => {
        wx.request({
          method: "GET",
          url: getApp().data.server+'/pay/weixin/miniProgramOpenid.aspx',
          data: {
            scode: res.code   // 使用wx.login得到的登陆凭证，用于换取openid  
          },
          header: {
            'content-type': 'application/json' // 默认值  
          },
          success: (d) => {             
            that.getorder(d.data, that.data.options)
          }
        })
        //console.log("code:" + res.code) //这里只是为了在微信小程序客户端好查看结果，找寻问题  
      }
    })    
  },
  //  
  //发起统一定单
  getorder: function (openid, options) {
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
      header: {
        'content-type': 'application/json' // 默认值  
      },
      success: (d) => {
        //that.getorder(d.data, that.data.options)
        console.log("json:" + d.data.result_code)
      }
    })
    return 4;
  }
})