//在Vue中实现各种手式操作的事件
//采用HammerJS作为基础库，此处只构建Vue中的事件，其实还是HammerJS的事件
function vueTouch(el, type, binding) {
  this.el = el;
  this.type = type;
  this.binding = binding;
  var hammertime = new Hammer(this.el);
  hammertime.on(this.type, this.binding.value);
  hammertime.get('pinch').set({ enable: true });
};

//在指定的 DOM 区域内，一个手指轻拍或点击时触发该事件（类似 PC 端的 click），
//该事件最大点击时间为 250ms，如果超过 250ms 则按 press 事件处理
Vue.directive("tap", {
  bind: function (el, binding) {
    new vueTouch(el, "tap", binding);
  }
});
//相当于 PC 端的 Click 事件，不能包含任何的移动，最小按压时间为 500ms，
//常用于我们在手机上用的复制粘贴等功能
Vue.directive("press", {
  bind: function (el, binding) {
    new vueTouch(el, "press", binding);
  }
});

//
//一个手指快速的在触屏上滑动，即平时用到最多的滑动事件
Vue.directive("swipe", {
  bind: function (el, binding) {
    new vueTouch(el, "swipe", binding);
  }
});
//向左滑动, direction=2
Vue.directive("swipeleft", {
  bind: function (el, binding) {
    new vueTouch(el, "swipeleft", binding);
  }
});
//向右滑动 direction=4
Vue.directive("swiperight", {
  bind: function (el, binding) {
    new vueTouch(el, "swiperight", binding);
  }
});
//向上滑动，官方不建议使用上下滑动，我个人感觉也没有这个意义，可以使用浏览器默认滚动来实现
Vue.directive("swipeup", {
  bind: function (el, binding) {
    new vueTouch(el, "swipeup", binding);
  }
});
//向下滑动
Vue.directive("swipedown", {
  bind: function (el, binding) {
    new vueTouch(el, "swipedown", binding);
  }
});

//
//两个手指（默认为两个手指，多指触控需要单独设置）或多个手指相对（越来越近）移动或相向（越来越远）移动时事件
Vue.directive("pinch", {
  bind: function (el, binding) {
    new vueTouch(el, "pinch", binding);
  }
});
//多点触控时两手指越来越近
Vue.directive("pinchin", {
  bind: function (el, binding) {
    new vueTouch(el, "pinchin", binding);
  }
});
//多点触控时两手指越来越远
Vue.directive("pinchout", {
  bind: function (el, binding) {
    new vueTouch(el, "pinchout", binding);
  }
});
//多点触控开始
Vue.directive("pinchstart", {
  bind: function (el, binding) {
    new vueTouch(el, "pinchstart", binding);
  }
});
//多点触控过程
Vue.directive("pinchmove", {
  bind: function (el, binding) {
    new vueTouch(el, "pinchmove", binding);
  }
});
//多点触控取消
Vue.directive("pinchend", {
  bind: function (el, binding) {
    new vueTouch(el, "pinchend", binding);
  }
});

