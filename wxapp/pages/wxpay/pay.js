Page({
  data: {
    tt: '测试A'   
  },
  onLoad: function () {
    wx.showToast({
      title: '成功',
      icon: 'succes',
      duration: 1000,
      mask: true
    })
  }
})