//index.js
//获取应用实例
const app = getApp();
Page({
  data: {
    url: 'https://exam.weisha100.cn',
    userInfo: {},
    hasUserInfo: false,
    canIUse: wx.canIUse('button.open-type.getUserInfo')
  },
  //事件处理函数
  bindViewTap: function() {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  onLoad: function () {
    if (app.globalData.userInfo) {
      this.setData({
        userInfo: app.globalData.userInfo,
        hasUserInfo: true
      })
    } else if (this.data.canIUse){
      // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
      // 所以此处加入 callback 以防止这种情况
      app.userInfoReadyCallback = res => {
        this.setData({
          userInfo: res.userInfo,
          hasUserInfo: true
        })
      }
    } else {
      // 在没有 open-type=getUserInfo 版本的兼容处理
      wx.getUserInfo({
        success: res => {
          app.globalData.userInfo = res.userInfo
          this.setData({
            userInfo: res.userInfo,
            hasUserInfo: true
          })
        }
      })
    }
    wx.login({
      //获取code  
      success: (res) => {   
        wx.request({
          method: "GET",
          url: 'https://exam.weisha100.cn/pay/weixin/miniProgramPay.aspx', 
          data: {
            scode: res.code   // 使用wx.login得到的登陆凭证，用于换取openid  
          },
          header: {
            'content-type': 'application/json', // 默认值  
            'Accept': 'application/json'
          },
          success: (d) => {
            console.log(d.data)
            if (d.data != '0') {
              this.setData({
                sopenid: d.data,
                //url: 'https://xxxx.xxx.xxx/?openid=' + res.data
              });
            }
            console.log("openid:"+d.sopenid)
          }
        })
        console.log("code:"+res.code) //这里只是为了在微信小程序客户端好查看结果，找寻问题  
      }
    }); 
  },
  getUserInfo: function(e) {
    console.log(e)
    app.globalData.userInfo = e.detail.userInfo
    this.setData({
      userInfo: e.detail.userInfo,
      hasUserInfo: true
    })
  }
})
