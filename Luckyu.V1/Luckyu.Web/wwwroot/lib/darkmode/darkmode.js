(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
  typeof define === 'function' && define.amd ? define(factory) :
  (global = typeof globalThis !== 'undefined' ? globalThis : global || self, global.darkModeJs = factory());
})(this, (function () { 'use strict';

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties(Constructor, staticProps);
    return Constructor;
  }

  function _inherits(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf(subClass, superClass);
  }

  function _getPrototypeOf(o) {
    _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf(o);
  }

  function _setPrototypeOf(o, p) {
    _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf(o, p);
  }

  function _isNativeReflectConstruct() {
    if (typeof Reflect === "undefined" || !Reflect.construct) return false;
    if (Reflect.construct.sham) return false;
    if (typeof Proxy === "function") return true;

    try {
      Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {}));
      return true;
    } catch (e) {
      return false;
    }
  }

  function _assertThisInitialized(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _possibleConstructorReturn(self, call) {
    if (call && (typeof call === "object" || typeof call === "function")) {
      return call;
    } else if (call !== void 0) {
      throw new TypeError("Derived constructors may only return object or undefined");
    }

    return _assertThisInitialized(self);
  }

  function _createSuper(Derived) {
    var hasNativeReflectConstruct = _isNativeReflectConstruct();

    return function _createSuperInternal() {
      var Super = _getPrototypeOf(Derived),
          result;

      if (hasNativeReflectConstruct) {
        var NewTarget = _getPrototypeOf(this).constructor;

        result = Reflect.construct(Super, arguments, NewTarget);
      } else {
        result = Super.apply(this, arguments);
      }

      return _possibleConstructorReturn(this, result);
    };
  }

  var IS_BROWSER = typeof window !== 'undefined';
  var addBox = function addBox(name, fatherEle, rest) {
    var box = document.createElement('div');
    box.innerHTML = name;
    Object.keys(rest).forEach(function (key) {
      if (key === 'class') {
        box.classList.add(rest[key]);
      } else {
        box[key] = rest[key];
      }
    });

    if (fatherEle) {
      fatherEle.appendChild(box);
    } else {
      document.body.appendChild(box);
    }

    return box;
  };
  var addBtn = function addBtn(name, fatherEle, rest) {
    var box = document.createElement('button');
    box.innerHTML = name; // const child = document.createElement('b')

    Object.keys(rest).forEach(function (key) {
      if (key === 'class') {
        box.classList.add(rest[key]);
      } else {
        box[key] = rest[key];
      }
    }); // box.appendChild(child)

    if (fatherEle) {
      fatherEle.appendChild(box);
    } else {
      document.body.appendChild(box);
    }

    return box;
  };
  var getId = function getId(id) {
    return document.getElementById(id);
  };
  var $ = function $(className) {
    return document.querySelector(className);
  };
  var addStyle = function addStyle(css) {
    var linkElement = document.createElement('link');
    linkElement.setAttribute('rel', 'stylesheet');
    linkElement.setAttribute('type', 'text/css');
    linkElement.setAttribute('href', 'data:text/css;charset=UTF-8,' + encodeURIComponent(css));
    document.head.appendChild(linkElement);
  };
  var isAllowDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
  var getLocal = function getLocal(name) {
    return window.localStorage.getItem(name);
  };
  var setLocal = function setLocal(name, value) {
    return window.localStorage.setItem(name, value);
  };
  var insertTop = function insertTop(ele) {
    return document.body.insertBefore(ele, document.body.firstChild);
  };

  var utils = /*#__PURE__*/Object.freeze({
    __proto__: null,
    IS_BROWSER: IS_BROWSER,
    addBox: addBox,
    addBtn: addBtn,
    getId: getId,
    $: $,
    addStyle: addStyle,
    isAllowDarkMode: isAllowDarkMode,
    getLocal: getLocal,
    setLocal: setLocal,
    insertTop: insertTop
  });

  var defaultOptions = {
    label: '',
    saveInCookies: true,
    autoMatchOsTheme: true
  };
  var styleOptions = {
    bottom: '32px',
    right: '32px',
    left: 'unset',
    time: '0.3s',
    mixColor: '#fff',
    backgroundColor: '#fff',
    buttonColorDark: '#100f2c',
    buttonColorLight: '#fff'
  };
  var cssLayer = function cssLayer(options) {
    return "\n    .darkmode--activated img {\n      filter: hue-rotate(180deg);\n    }\n    .darkmode--activated .no-mode {\n      filter: hue-rotate(180deg);\n    }\n    .darkmode-layer {\n      position: fixed;\n      pointer-events: none;\n      background: ".concat(options.mixColor, ";\n      transition: all ").concat(options.time, " ease;\n      mix-blend-mode: difference;\n      z-index: 10000;\n    }\n\n    .darkmode-layer--simple {\n      width: 100%;\n      height: 100%;\n      top: 0;\n      left: 0;\n      transform: scale(1);\n    }\n\n    .darkmode-layer--expanded {\n      transform: scale(100);\n      border-radius: 0;\n    }\n\n    .darkmode-layer--no-transition {\n      transition: none;\n    }\n\n    .darkmode-background {\n      background: ").concat(options.backgroundColor, ";\n      position: fixed;\n      pointer-events: none;\n      z-index: -10;\n      width: 100%;\n      height: 100%;\n      top: 0;\n      left: 0;\n    }\n\n    img, .darkmode-ignore {\n      isolation: isolate;\n      display: inline-block;\n    }\n\n    @media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {\n      .darkmode-toggle {display: none !important}\n    }\n\n    @supports (-ms-ime-align:auto), (-ms-accelerator:true) {\n      .darkmode-toggle {display: none !important}\n    }\n  ");
  };
  var cssBtn = function cssBtn(options) {
    return "\n  .darkmode-layer--button {\n    width: 2.9rem;\n    height: 2.9rem;\n    border-radius: 50%;\n    right: ".concat(options.right, ";\n    bottom: ").concat(options.bottom, ";\n    left: ").concat(options.left, ";\n  }\n\n  .darkmode-toggle {\n    background: ").concat(options.buttonColorDark, ";\n    width: 3rem;\n    height: 3rem;\n    position: fixed;\n    border-radius: 50%;\n    border:none;\n    right: ").concat(options.right, ";\n    bottom: ").concat(options.bottom, ";\n    left: ").concat(options.left, ";\n    cursor: pointer;\n    transition: all 0.5s ease;\n    display: flex;\n    justify-content: center;\n    align-items: center;\n    margin: 0;\n    padding: 0;\n    z-index: 10000;\n  }\n\n  .darkmode-toggle--white {\n    background: ").concat(options.buttonColorLight, ";\n  }\n\n  .darkmode-toggle--inactive {\n    display: none;\n  }\n\n\n");
  };

  var Widget = /*#__PURE__*/function () {
    function Widget(params) {
      _classCallCheck(this, Widget);

      if (!IS_BROWSER) {
        return;
      }

      var options = Object.assign({}, defaultOptions, styleOptions, params);
      this.options = options;
      var button = addBtn(options.label, null, {
        "class": 'darkmode-toggle--inactive'
      });
      insertTop(button);
      addStyle(cssBtn(options));
      this.button = button;
      this.saveInCookies = options.saveInCookies;
      this.time = options.time; // 手动开启
      // this.showWidget()

      this.init();
    }

    _createClass(Widget, [{
      key: "defaultTheme",
      value: function defaultTheme() {
        var darkmodeActivated = getLocal('darkmode') === 'true';
        var preferedThemeOs = this.options.autoMatchOsTheme && isAllowDarkMode;
        var darkmodeNeverActivatedByAction = getLocal('darkmode') === null;

        if (darkmodeActivated === true && this.options.saveInCookies || darkmodeNeverActivatedByAction && preferedThemeOs) {
          this.button.classList.toggle('darkmode-toggle--white');
          document.body.classList.toggle('darkmode--activated');
          this.setDarkTheme();
        }
      }
    }, {
      key: "openDark",
      value: function openDark() {
        var _this = this;

        var isDarkmode = this.isActivated();
        this.button.setAttribute('disabled', true);
        setTimeout(function () {
          _this.button.removeAttribute('disabled');
        }, 1);
        this.button.classList.toggle('darkmode-toggle--white');
        document.body.classList.toggle('darkmode--activated');
        setLocal('darkmode', !isDarkmode);
        this.setDarkTheme();
      }
    }, {
      key: "closeDark",
      value: function closeDark() {
        var _this2 = this;

        var isDarkmode = this.isActivated();
        var time = parseFloat(this.time) * 1000;
        this.button.setAttribute('disabled', true);
        setTimeout(function () {
          _this2.button.removeAttribute('disabled');
        }, time);
        this.button.classList.toggle('darkmode-toggle--white');
        document.body.classList.toggle('darkmode--activated');
        setLocal('darkmode', !isDarkmode);
        this.setLightTheme();
      }
    }, {
      key: "showWidget",
      value: function showWidget() {
        var _this3 = this;

        if (!IS_BROWSER) {
          return;
        }

        var button = this.button;
        button.classList.add('darkmode-toggle');
        button.classList.remove('darkmode-toggle--inactive');
        button.setAttribute('aria-label', 'Activate dark mode');
        button.setAttribute('aria-checked', 'false');
        button.setAttribute('role', 'checkbox');
        button.addEventListener('click', function () {
          var isDarkmode = _this3.isActivated();

          if (!isDarkmode) {
            _this3.openDark();
          } else {
            _this3.closeDark();
          }
        });
      }
    }, {
      key: "isActivated",
      value: function isActivated() {
        return document.body.classList.contains('darkmode--activated');
      } // toggle() {
      //   const layer = this.layer;
      //   const isDarkmode = this.isActivated();
      //   const button = this.button;
      //   layer.classList.toggle('darkmode-layer--simple');
      //   document.body.classList.toggle('darkmode--activated');
      //   window.localStorage.setItem('darkmode', !isDarkmode);
      //   button.setAttribute('aria-label', 'De-activate dark mode');
      //   button.setAttribute('aria-checked', 'true');
      // }
      // public

    }, {
      key: "init",
      value: function init() {} // public

    }, {
      key: "setDarkTheme",
      value: function setDarkTheme() {} // public

    }, {
      key: "setLightTheme",
      value: function setLightTheme() {}
    }]);

    return Widget;
  }();

  var DarkMode = /*#__PURE__*/function (_Widget) {
    _inherits(DarkMode, _Widget);

    var _super = _createSuper(DarkMode);

    function DarkMode() {
      _classCallCheck(this, DarkMode);

      return _super.apply(this, arguments);
    }

    _createClass(DarkMode, [{
      key: "init",
      value: function init() {
        var layer = addBox(null, null, {
          "class": 'darkmode-layer'
        });
        var background = addBox(null, null, {
          "class": 'darkmode-background'
        });
        insertTop(layer);
        insertTop(background);
        addStyle(cssLayer(this.options));
        this.layer = layer;
        this.time = this.options.time;
      } // public

    }, {
      key: "setDarkTheme",
      value: function setDarkTheme() {
        var _this = this;

        console.log('暗黑模式', this.layer);
        this.layer.classList.add('darkmode-layer--expanded');
        setTimeout(function () {
          _this.layer.classList.add('darkmode-layer--simple');

          _this.layer.classList.add('darkmode-layer--no-transition');
        }, this.time);
      } // public

    }, {
      key: "setLightTheme",
      value: function setLightTheme() {
        var _this2 = this;

        console.log('高亮模式');
        this.layer.classList.remove('darkmode-layer--simple');
        setTimeout(function () {
          _this2.layer.classList.remove('darkmode-layer--no-transition');

          _this2.layer.classList.remove('darkmode-layer--expanded');
        }, 1);
      }
    }]);

    return DarkMode;
  }(Widget);

  //   --color: #0097fc;
  //   --color-accent: #0097fc4f;
  //   --color-bg: #333;
  //   --color-bg-secondary: #555;
  //   --color-link: #0097fc;
  //   --color-secondary: #e20de9;
  //   --color-secondary-accent: #e20de94f;
  //   --color-shadow: #bbbbbb20;
  //   --color-table: #0097fc;
  //   --color-text: #f7f7f7;
  //   --color-text-secondary: #aaa;
  // }

  var DarkModeRoot = /*#__PURE__*/function (_Widget) {
    _inherits(DarkModeRoot, _Widget);

    var _super = _createSuper(DarkModeRoot);

    function DarkModeRoot() {
      _classCallCheck(this, DarkModeRoot);

      return _super.apply(this, arguments);
    }

    _createClass(DarkModeRoot, [{
      key: "init",
      value: function init() {
        this.setRoot();
      }
    }, {
      key: "setRoot",
      value: function setRoot(rootConfig, lRootConfig) {
        this.rootConfig = rootConfig !== null && rootConfig !== void 0 ? rootConfig : {
          '--color': '#0097fc',
          '--color-accent': '#0097fc4f',
          '--color-bg': '#333',
          '--color-bg-secondary': '#555',
          '--color-link': '#0097fc',
          '--color-secondary': '#e20de9',
          '--color-secondary-accent': '#e20de94f',
          '--color-shadow': '#bbbbbb20',
          '--color-table': '#0097fc',
          '--color-text': '#f7f7f7',
          '--color-text-secondary': '#aaa'
        };
        this.lRootConfig = lRootConfig !== null && lRootConfig !== void 0 ? lRootConfig : {
          '--color': '#118bee',
          '--color-accent': '#118bee15',
          '--color-bg': '#fff',
          '--color-bg-secondary': '#e9e9e9',
          '--color-link': '#118bee',
          '--color-secondary': '#920de9',
          '--color-secondary-accent': '#920de90b',
          '--color-shadow': '#f4f4f4',
          '--color-table': '#118bee',
          '--color-text': '#000',
          '--color-text-secondary': '#999'
        };
      } // public

    }, {
      key: "setDarkTheme",
      value: function setDarkTheme() {
        var _this = this;

        console.log('暗黑模式');
        Object.keys(this.rootConfig).forEach(function (key) {
          // document.documentElement.style.setProperty(key, this.rootConfig[key])
          $(':root').style.setProperty(key, _this.rootConfig[key]);
        });
      } // public

    }, {
      key: "setLightTheme",
      value: function setLightTheme() {
        var _this2 = this;

        console.log('高亮模式');
        Object.keys(this.lRootConfig).forEach(function (key) {
          // document.documentElement.style.setProperty(key, this.lRootConfig[key])
          $(':root').style.setProperty(key, _this2.lRootConfig[key]);
        });
      }
    }]);

    return DarkModeRoot;
  }(Widget);

  var index = {
    widget: Widget,
    darkMode: DarkMode,
    darkRoot: DarkModeRoot,
    utils: utils
  };

  return index;

}));
