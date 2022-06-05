function vueTouch(el, type, binding) {
  this.el = el;
  this.type = type;
  this.binding = binding;
  var hammertime = new Hammer(this.el);
  hammertime.on(this.type, this.binding.value);
};
Vue.directive("tap", {
  bind : function(el, binding) {
      new vueTouch(el, "tap", binding);
  }
});
Vue.directive("swipe", {
  bind : function(el, binding) {
      new vueTouch(el, "swipe", binding);
  }
});
Vue.directive("swipeleft", {
  bind : function(el, binding) {
      new vueTouch(el, "swipeleft", binding);
  }
});
Vue.directive("swiperight", {
  bind : function(el, binding) {
      new vueTouch(el, "swiperight", binding);
  }
});
Vue.directive("press", {
  bind : function(el, binding) {
      new vueTouch(el, "press", binding);
  }
});
