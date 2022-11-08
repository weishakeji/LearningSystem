/**
 * Copyright (c) Tiny Technologies, Inc. All rights reserved.
 * Licensed under the LGPL or a commercial license.
 * For LGPL see License.txt in the project root for license information.
 * For commercial licenses see https://www.tiny.cloud/
 *
 * Version: 5.2.0 (2020-02-13)
 */
(function () {
    'use strict';

    var Cell = function (initial) {
      var value = initial;
      var get = function () {
        return value;
      };
      var set = function (v) {
        value = v;
      };
      var clone = function () {
        return Cell(get());
      };
      return {
        get: get,
        set: set,
        clone: clone
      };
    };

    var global = tinymce.util.Tools.resolve('tinymce.PluginManager');

    var get = function (customTabs) {
      var addTab = function (spec) {
        var currentCustomTabs = customTabs.get();
        currentCustomTabs[spec.name] = spec;
        customTabs.set(currentCustomTabs);
      };
      return { addTab: addTab };
    };

    var register = function (editor, dialogOpener) {
      editor.addCommand('mceHelp', dialogOpener);
    };
    var Commands = { register: register };

    var register$1 = function (editor, dialogOpener) {
      editor.ui.registry.addButton('help', {
        icon: 'help',
        tooltip: 'Help',
        onAction: dialogOpener
      });
      editor.ui.registry.addMenuItem('help', {
        text: 'Help',
        icon: 'help',
        shortcut: 'Alt+0',
        onAction: dialogOpener
      });
    };
    var Buttons = { register: register$1 };

    var __assign = function () {
      __assign = Object.assign || function __assign(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
          s = arguments[i];
          for (var p in s)
            if (Object.prototype.hasOwnProperty.call(s, p))
              t[p] = s[p];
        }
        return t;
      };
      return __assign.apply(this, arguments);
    };

    var noop = function () {
    };
    var constant = function (value) {
      return function () {
        return value;
      };
    };
    function curry(fn) {
      var initialArgs = [];
      for (var _i = 1; _i < arguments.length; _i++) {
        initialArgs[_i - 1] = arguments[_i];
      }
      return function () {
        var restArgs = [];
        for (var _i = 0; _i < arguments.length; _i++) {
          restArgs[_i] = arguments[_i];
        }
        var all = initialArgs.concat(restArgs);
        return fn.apply(null, all);
      };
    }
    var not = function (f) {
      return function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
          args[_i] = arguments[_i];
        }
        return !f.apply(null, args);
      };
    };
    var never = constant(false);
    var always = constant(true);

    var none = function () {
      return NONE;
    };
    var NONE = function () {
      var eq = function (o) {
        return o.isNone();
      };
      var call = function (thunk) {
        return thunk();
      };
      var id = function (n) {
        return n;
      };
      var me = {
        fold: function (n, s) {
          return n();
        },
        is: never,
        isSome: never,
        isNone: always,
        getOr: id,
        getOrThunk: call,
        getOrDie: function (msg) {
          throw new Error(msg || 'error: getOrDie called on none.');
        },
        getOrNull: constant(null),
        getOrUndefined: constant(undefined),
        or: id,
        orThunk: call,
        map: none,
        each: noop,
        bind: none,
        exists: never,
        forall: always,
        filter: none,
        equals: eq,
        equals_: eq,
        toArray: function () {
          return [];
        },
        toString: constant('none()')
      };
      if (Object.freeze) {
        Object.freeze(me);
      }
      return me;
    }();
    var some = function (a) {
      var constant_a = constant(a);
      var self = function () {
        return me;
      };
      var bind = function (f) {
        return f(a);
      };
      var me = {
        fold: function (n, s) {
          return s(a);
        },
        is: function (v) {
          return a === v;
        },
        isSome: always,
        isNone: never,
        getOr: constant_a,
        getOrThunk: constant_a,
        getOrDie: constant_a,
        getOrNull: constant_a,
        getOrUndefined: constant_a,
        or: self,
        orThunk: self,
        map: function (f) {
          return some(f(a));
        },
        each: function (f) {
          f(a);
        },
        bind: bind,
        exists: bind,
        forall: bind,
        filter: function (f) {
          return f(a) ? me : NONE;
        },
        toArray: function () {
          return [a];
        },
        toString: function () {
          return 'some(' + a + ')';
        },
        equals: function (o) {
          return o.is(a);
        },
        equals_: function (o, elementEq) {
          return o.fold(never, function (b) {
            return elementEq(a, b);
          });
        }
      };
      return me;
    };
    var from = function (value) {
      return value === null || value === undefined ? NONE : some(value);
    };
    var Option = {
      some: some,
      none: none,
      from: from
    };

    var typeOf = function (x) {
      if (x === null) {
        return 'null';
      }
      var t = typeof x;
      if (t === 'object' && (Array.prototype.isPrototypeOf(x) || x.constructor && x.constructor.name === 'Array')) {
        return 'array';
      }
      if (t === 'object' && (String.prototype.isPrototypeOf(x) || x.constructor && x.constructor.name === 'String')) {
        return 'string';
      }
      return t;
    };
    var isType = function (type) {
      return function (value) {
        return typeOf(value) === type;
      };
    };
    var isFunction = isType('function');

    var nativeSlice = Array.prototype.slice;
    var nativeIndexOf = Array.prototype.indexOf;
    var rawIndexOf = function (ts, t) {
      return nativeIndexOf.call(ts, t);
    };
    var indexOf = function (xs, x) {
      var r = rawIndexOf(xs, x);
      return r === -1 ? Option.none() : Option.some(r);
    };
    var contains = function (xs, x) {
      return rawIndexOf(xs, x) > -1;
    };
    var map = function (xs, f) {
      var len = xs.length;
      var r = new Array(len);
      for (var i = 0; i < len; i++) {
        var x = xs[i];
        r[i] = f(x, i);
      }
      return r;
    };
    var filter = function (xs, pred) {
      var r = [];
      for (var i = 0, len = xs.length; i < len; i++) {
        var x = xs[i];
        if (pred(x, i)) {
          r.push(x);
        }
      }
      return r;
    };
    var find = function (xs, pred) {
      for (var i = 0, len = xs.length; i < len; i++) {
        var x = xs[i];
        if (pred(x, i)) {
          return Option.some(x);
        }
      }
      return Option.none();
    };
    var from$1 = isFunction(Array.from) ? Array.from : function (x) {
      return nativeSlice.call(x);
    };

    var keys = Object.keys;
    var hasOwnProperty = Object.hasOwnProperty;
    var get$1 = function (obj, key) {
      return has(obj, key) ? Option.from(obj[key]) : Option.none();
    };
    var has = function (obj, key) {
      return hasOwnProperty.call(obj, key);
    };

    var cat = function (arr) {
      var r = [];
      var push = function (x) {
        r.push(x);
      };
      for (var i = 0; i < arr.length; i++) {
        arr[i].each(push);
      }
      return r;
    };

    var getHelpTabs = function (editor) {
      return Option.from(editor.getParam('help_tabs'));
    };

    var shortcuts = [
      {
        shortcuts: ['Meta + B'],
        action: 'Bold'
      },
      {
        shortcuts: ['Meta + I'],
        action: 'Italic'
      },
      {
        shortcuts: ['Meta + U'],
        action: 'Underline'
      },
      {
        shortcuts: ['Meta + A'],
        action: 'Select all'
      },
      {
        shortcuts: [
          'Meta + Y',
          'Meta + Shift + Z'
        ],
        action: 'Redo'
      },
      {
        shortcuts: ['Meta + Z'],
        action: 'Undo'
      },
      {
        shortcuts: ['Access + 1'],
        action: 'Header 1'
      },
      {
        shortcuts: ['Access + 2'],
        action: 'Header 2'
      },
      {
        shortcuts: ['Access + 3'],
        action: 'Header 3'
      },
      {
        shortcuts: ['Access + 4'],
        action: 'Header 4'
      },
      {
        shortcuts: ['Access + 5'],
        action: 'Header 5'
      },
      {
        shortcuts: ['Access + 6'],
        action: 'Header 6'
      },
      {
        shortcuts: ['Access + 7'],
        action: 'Paragraph'
      },
      {
        shortcuts: ['Access + 8'],
        action: 'Div'
      },
      {
        shortcuts: ['Access + 9'],
        action: 'Address'
      },
      {
        shortcuts: ['Alt + 0'],
        action: 'Open help dialog'
      },
      {
        shortcuts: ['Alt + F9'],
        action: 'Focus to menubar'
      },
      {
        shortcuts: ['Alt + F10'],
        action: 'Focus to toolbar'
      },
      {
        shortcuts: ['Alt + F11'],
        action: 'Focus to element path'
      },
      {
        shortcuts: ['Ctrl + F9'],
        action: 'Focus to contextual toolbar'
      },
      {
        shortcuts: ['Shift + Enter'],
        action: 'Open popup menu for split buttons'
      },
      {
        shortcuts: ['Meta + K'],
        action: 'Insert link'
      },
      // {
      //   shortcuts: ['Meta + S'],
      //   action: 'Save (if save plugin activated)'
      // },
      {
        shortcuts: ['Meta + F'],
        action: 'Find'
      },
      {
        shortcuts: ['Meta + Shift + F'],
        action: 'Switch to or from fullscreen mode'
      }
    ];
    var KeyboardShortcuts = { shortcuts: shortcuts };

    var global$1 = tinymce.util.Tools.resolve('tinymce.Env');

    var convertText = function (source) {
      var mac = {
        alt: '&#x2325;',
        ctrl: '&#x2303;',
        shift: '&#x21E7;',
        meta: '&#x2318;',
        access: '&#x2303;&#x2325;'
      };
      var other = {
        meta: 'Ctrl ',
        access: 'Shift + Alt '
      };
      var replace = global$1.mac ? mac : other;
      var shortcut = source.split('+');
      var updated = map(shortcut, function (segment) {
        var search = segment.toLowerCase().trim();
        return has(replace, search) ? replace[search] : segment;
      });
      return global$1.mac ? updated.join('').replace(/\s/, '') : updated.join('+');
    };
    var ConvertShortcut = { convertText: convertText };

    var tab = function () {
      var shortcutList = map(KeyboardShortcuts.shortcuts, function (shortcut) {
        var shortcutText = map(shortcut.shortcuts, ConvertShortcut.convertText).join(' or ');
        return [
          shortcut.action,
          shortcutText
        ];
      });
      var tablePanel = {
        type: 'table',
        header: [
          'Action',
          'Shortcut'
        ],
        cells: shortcutList
      };
      return {
        name: 'shortcuts',
        title: 'Handy Shortcuts',
        items: [tablePanel]
      };
    };
    var KeyboardShortcutsTab = { tab: tab };

    var supplant = function (str, obj) {
      var isStringOrNumber = function (a) {
        var t = typeof a;
        return t === 'string' || t === 'number';
      };
      return str.replace(/\$\{([^{}]*)\}/g, function (fullMatch, key) {
        var value = obj[key];
        return isStringOrNumber(value) ? value.toString() : fullMatch;
      });
    };

    var global$2 = tinymce.util.Tools.resolve('tinymce.util.I18n');

    var urls = [
      {
        key: 'advlist',
        name: 'Advanced List'
      },
      {
        key: 'anchor',
        name: 'Anchor'
      },
      {
        key: 'autolink',
        name: 'Autolink'
      },
      {
        key: 'autoresize',
        name: 'Autoresize'
      },
      {
        key: 'autosave',
        name: 'Autosave'
      },
      {
        key: 'bbcode',
        name: 'BBCode'
      },
      {
        key: 'charmap',
        name: 'Character Map'
      },
      {
        key: 'code',
        name: 'Code'
      },
      {
        key: 'codesample',
        name: 'Code Sample'
      },
      {
        key: 'colorpicker',
        name: 'Color Picker'
      },
      {
        key: 'directionality',
        name: 'Directionality'
      },
      {
        key: 'emoticons',
        name: 'Emoticons'
      },
      {
        key: 'fullpage',
        name: 'Full Page'
      },
      {
        key: 'fullscreen',
        name: 'Full Screen'
      },
      {
        key: 'help',
        name: 'Help'
      },
      {
        key: 'hr',
        name: 'Horizontal Rule'
      },
      {
        key: 'image',
        name: 'Image'
      },
      {
        key: 'imagetools',
        name: 'Image Tools'
      },
      {
        key: 'importcss',
        name: 'Import CSS'
      },
      {
        key: 'insertdatetime',
        name: 'Insert Date/Time'
      },
      {
        key: 'legacyoutput',
        name: 'Legacy Output'
      },
      {
        key: 'link',
        name: 'Link'
      },
      {
        key: 'lists',
        name: 'Lists'
      },
      {
        key: 'media',
        name: 'Media'
      },
      {
        key: 'nonbreaking',
        name: 'Nonbreaking'
      },
      {
        key: 'noneditable',
        name: 'Noneditable'
      },
      {
        key: 'pagebreak',
        name: 'Page Break'
      },
      {
        key: 'paste',
        name: 'Paste'
      },
      {
        key: 'preview',
        name: 'Preview'
      },
      {
        key: 'print',
        name: 'Print'
      },
      {
        key: 'save',
        name: 'Save'
      },
      {
        key: 'searchreplace',
        name: 'Search and Replace'
      },
      {
        key: 'spellchecker',
        name: 'Spell Checker'
      },
      {
        key: 'tabfocus',
        name: 'Tab Focus'
      },
      {
        key: 'table',
        name: 'Table'
      },
      {
        key: 'template',
        name: 'Template'
      },
      {
        key: 'textcolor',
        name: 'Text Color'
      },
      {
        key: 'textpattern',
        name: 'Text Pattern'
      },
      {
        key: 'toc',
        name: 'Table of Contents'
      },
      {
        key: 'visualblocks',
        name: 'Visual Blocks'
      },
      {
        key: 'visualchars',
        name: 'Visual Characters'
      },
      {
        key: 'wordcount',
        name: 'Word Count'
      },
      {
        key: 'advcode',
        name: 'Advanced Code Editor*'
      },
      {
        key: 'formatpainter',
        name: 'Format Painter*'
      },
      {
        key: 'powerpaste',
        name: 'PowerPaste*'
      },
      {
        key: 'tinydrive',
        name: 'Tiny Drive*'
      },
      {
        key: 'tinymcespellchecker',
        name: 'Spell Checker Pro*'
      },
      {
        key: 'a11ychecker',
        name: 'Accessibility Checker*'
      },
      {
        key: 'linkchecker',
        name: 'Link Checker*'
      },
      {
        key: 'mentions',
        name: 'Mentions*'
      },
      {
        key: 'mediaembed',
        name: 'Enhanced Media Embed*'
      },
      {
        key: 'checklist',
        name: 'Checklist*'
      },
      {
        key: 'casechange',
        name: 'Case Change*'
      },
      {
        key: 'permanentpen',
        name: 'Permanent Pen*'
      },
      {
        key: 'pageembed',
        name: 'Page Embed*'
      },
      {
        key: 'tinycomments',
        name: 'Tiny Comments*'
      },
      {
        key: 'advtable',
        name: 'Advanced Tables*'
      },
      {
        key: 'autocorrect',
        name: 'Autocorrect*'
      }
    ];
    var PluginUrls = { urls: urls };

    var tab$1 = function (editor) {
      var availablePlugins = function () {
        var premiumPlugins = [
          'Accessibility Checker',
          'Advanced Code Editor',
          'Advanced Tables',
          'Case Change',
          'Checklist',
          'Tiny Comments',
          'Tiny Drive',
          'Enhanced Media Embed',
          'Format Painter',
          'Link Checker',
          'Mentions',
          'MoxieManager',
          'Page Embed',
          'Permanent Pen',
          'PowerPaste',
          'Spell Checker Pro'
        ];
        var premiumPluginList = map(premiumPlugins, function (plugin) {
          return '<li>' + global$2.translate(plugin) + '</li>';
        }).join('');
        return '<div data-mce-tabstop="1" tabindex="-1">' + '<p><b>' + global$2.translate('Premium plugins:') + '</b></p>' + '<ul>' + premiumPluginList + '<li class="tox-help__more-link" "><a href="https://www.tiny.cloud/pricing/?utm_campaign=editor_referral&utm_medium=help_dialog&utm_source=tinymce" target="_blank">' + global$2.translate('Learn more...') + '</a></li>' + '</ul>' + '</div>';
      };
      var makeLink = curry(supplant, '<a href="${url}" target="_blank" rel="noopener">${name}</a>');
      var maybeUrlize = function (editor, key) {
        return find(PluginUrls.urls, function (x) {
          return x.key === key;
        }).fold(function () {
          var getMetadata = editor.plugins[key].getMetadata;
          return typeof getMetadata === 'function' ? makeLink(getMetadata()) : key;
        }, function (x) {
          return makeLink({
            name: x.name,
            url: 'https://www.tiny.cloud/docs/plugins/' + x.key
          });
        });
      };
      var getPluginKeys = function (editor) {
        var keys$1 = keys(editor.plugins);
        return editor.settings.forced_plugins === undefined ? keys$1 : filter(keys$1, not(curry(contains, editor.settings.forced_plugins)));
      };
      var pluginLister = function (editor) {
        var pluginKeys = getPluginKeys(editor);
        var pluginLis = map(pluginKeys, function (key) {
          return '<li>' + maybeUrlize(editor, key) + '</li>';
        });
        var count = pluginLis.length;
        var pluginsString = pluginLis.join('');
        var html = '<p><b>' + global$2.translate([
          'Plugins installed ({0}):',
          count
        ]) + '</b></p>' + '<ul>' + pluginsString + '</ul>';
        return html;
      };
      var installedPlugins = function (editor) {
        if (editor == null) {
          return '';
        }
        return '<div data-mce-tabstop="1" tabindex="-1">' + pluginLister(editor) + '</div>';
      };
      var htmlPanel = {
        type: 'htmlpanel',
        presets: 'document',
        html: [
          installedPlugins(editor),
          availablePlugins()
        ].join('')
      };
      return {
        name: 'plugins',
        title: 'Plugins',
        items: [htmlPanel]
      };
    };
    var PluginsTab = { tab: tab$1 };

    var global$3 = tinymce.util.Tools.resolve('tinymce.EditorManager');

    var tab$2 = function () {
      var getVersion = function (major, minor) {
        return major.indexOf('@') === 0 ? 'X.X.X' : major + '.' + minor;
      };
      var version = getVersion(global$3.majorVersion, global$3.minorVersion);
      var changeLogLink = '<a href="https://www.tinymce.com/docs/changelog/?utm_campaign=editor_referral&utm_medium=help_dialog&utm_source=tinymce" target="_blank">TinyMCE ' + version + '</a>';
      var htmlPanel = {
        type: 'htmlpanel',
        html: '<p>' + global$2.translate([
          'You are using {0}',
          changeLogLink
        ]) + '</p>',
        presets: 'document'
      };
      return {
        name: 'versions',
        title: 'Version',
        items: [htmlPanel]
      };
    };
    var VersionTab = { tab: tab$2 };

   
    var tab$3 = function () {
      let description = tinymce.OperationManualHtml || '<p><h2>操作手册</h2></p>';
      // console.log("1212")
      var body = {
        type: 'htmlpanel',
        presets: 'document',
        html: '<div id="docBoxID">'+description+'</div>'
      };
      return {
        name: 'keyboardnav',
        title: '操作手册',
        items: [body]
      };
    };
    var KeyboardNavTab = { tab: tab$3 };

    var tab$4 = function () {
      let description = tinymce.CommonProblemHtml || '<div><h4><strong>问：为什么从word中直接粘贴带图文章到编辑器中图片会丢失？</strong></h4><p><em>答： 浏览器因为安全的原因是不允许直接访问本地文件的，所以需要手动对单个图进行复制（Ctrl+C） 粘贴（Ctrl+V） 。</em></p><p>&nbsp;</p></div>';
      // console.log("1212")
      let descriptionDomText = description
      // description = '<iframe src="'+iframe1+'" frameborder="0"></iframe>'
      var body = {
        type: 'htmlpanel',
        presets: 'document',
        html: '<div id="docBoxID" class="docBox">'+description+'</div>'
      };
      return {
        name: 'commonProblemTab',
        title: '疑问解答',
        items: [body]
      };
    };
    var commonProblemTab = { tab: tab$4 };

    var tab$5 = function () {
      let description = '<iframe id="feedBackID" src="'+tinymce.feedBackIframeUrl+'" frameborder="0"  onload="this.height=this.contentWindow.document.body.scrollHeight" allowtransparency="true" scrolling="no" style="width:100%!important;" ></iframe>'
      var body = {
        type: 'htmlpanel',
        presets: 'document',
        html: description,
      };
      return {
        name: 'feedback',
        title: '反馈问题',
        items: [body]
      };
    };
    var feedbackTab = { tab: tab$5 };

    var tab$6 = function () {
        let description = tinymce.functionHtml || '<p><h2>功能介绍</h2></p>';
      var body = {
        type: 'htmlpanel',
        presets: 'document',
        html: '<div id="docBoxID" class="docBox">'+description+'</div>',
      };
      return {
        name: 'functionTab',
        title: '功能介绍',
        items: [body]
      };
    };
    var functionTab = { tab: tab$6 };

    var parseHelpTabsSetting = function (tabsFromSettings, tabs) {
      var newTabs = {};
      var names = map(tabsFromSettings, function (t) {
        if (typeof t === 'string') {
          if (has(tabs, t)) {
            newTabs[t] = tabs[t];
          }
          return t;
        } else {
          newTabs[t.name] = t;
          return t.name;
        }
      });
      return {
        tabs: newTabs,
        names: names
      };
    };
    var getNamesFromTabs = function (tabs) {
      var names = keys(tabs);
      var versionsIdx = indexOf(names, 'versions');
      versionsIdx.each(function (idx) {
        names.splice(idx, 1);
        names.push('versions');
      });
      return {
        tabs: tabs,
        names: names
      };
    };
    var parseCustomTabs = function (editor, customTabs) {
      var _a;
      var shortcuts = KeyboardShortcutsTab.tab();
      var nav = KeyboardNavTab.tab();
      var cPTab = commonProblemTab.tab();
      var feedTab = feedbackTab.tab();
      var funcTab = functionTab.tab();
      // var plugins = PluginsTab.tab(editor);
      // var versions = VersionTab.tab();
      var tabs = __assign((_a = {}, _a[shortcuts.name] = shortcuts,_a[funcTab.name] = funcTab, _a[nav.name] = nav, _a[cPTab.name] = cPTab,_a[feedTab.name] = feedTab ,_a), customTabs.get());
      return getHelpTabs(editor).fold(function () {
        return getNamesFromTabs(tabs);
      }, function (tabsFromSettings) {
        return parseHelpTabsSetting(tabsFromSettings, tabs);
      });
    };
    var init = function (editor, customTabs) {
      return function () {
        var _a = parseCustomTabs(editor, customTabs), tabs = _a.tabs, names = _a.names;
        var foundTabs = map(names, function (name) {
          return get$1(tabs, name);
        });
        var dialogTabs = cat(foundTabs);
        var body = {
          type: 'tabpanel',
          tabs: dialogTabs
        };
        editor.windowManager.open({
          title: 'Help',
          size: 'medium',
          body: body,
          buttons: [{
              type: 'cancel',
              name: 'close',
              text: 'Close',
              primary: true
            }],
          initialData: {}
        });
      };
    };

    function Plugin () {
      global.add('help', function (editor) {
        var customTabs = Cell({});
        var api = get(customTabs);
        var dialogOpener = init(editor, customTabs);
        Buttons.register(editor, dialogOpener);
        Commands.register(editor, dialogOpener);
        editor.shortcuts.add('Alt+0', 'Open help dialog', 'mceHelp');
        return api;
      });
    }

    Plugin();

}());
