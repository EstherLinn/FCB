// Avoid `console` errors in browsers that lack a console.
(function () {
  var method;
  var noop = function () {};
  var methods = ['assert', 'clear', 'count', 'debug', 'dir', 'dirxml', 'error', 'exception', 'group', 'groupCollapsed', 'groupEnd', 'info', 'log', 'markTimeline', 'profile', 'profileEnd', 'table', 'time', 'timeEnd', 'timeline', 'timelineEnd', 'timeStamp', 'trace', 'warn'];
  var length = methods.length;
  var console = window.console = window.console || {};
  while (length--) {
    method = methods[length];

    // Only stub undefined methods.
    if (!console[method]) {
      console[method] = noop;
    }
  }
})();

// Place any jQuery/helper plugins in here.

// loading
(function ($, undefined) {
  'use strict';

  var pluginName = 'loading';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    show: function () {
      this.$element.show();
    },
    hide: function () {
      this.$element.fadeOut();
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  window[pluginName] = methodOrOptions => $('.c-loading').loading(methodOrOptions);

  // 開啟loading方式:
  // $('.c-loading').loading('show');
  // window.loading('show');

  // 關閉loading方式:
  // $('.c-loading').loading('hide');
  // window.loading('hide');
})(jQuery);

// popover 彈跳式視窗
(function ($, window, undefined) {
  'use strict';

  var pluginName = 'popover';
  var defaults = {
    placement: 'bottom',
    middleware: [window.FloatingUIDOM.offset(), window.FloatingUIDOM.flip(), window.FloatingUIDOM.shift()]
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var $tooltip = $(this.$element.data('popover') || this.$element.attr('href'));
      if (!$tooltip.length) {
        return;
      }
      this.tooltip = $tooltip.get(0);
      this.update();
      this.$element.off('click.popover').on('click.popover', e => {
        e.preventDefault();
        this.toggle.call(this);
      });
      $(window).on('click.popover', {
        el: this.element,
        tooltip: this.tooltip
      }, e => {
        if (!$(e.target).closest(e.data.el).length && !$(e.target).closest(e.data.tooltip).length) {
          this.hide.call(this);
        }
      });
    },
    update: function () {
      window.FloatingUIDOM.autoUpdate(this.element, this.tooltip, () => {
        window.FloatingUIDOM.computePosition(this.element, this.tooltip, this.options).then(_ref => {
          let {
            x,
            y
          } = _ref;
          this.tooltip.style.setProperty('--left', `${x}px`);
          this.tooltip.style.setProperty('--top', `${y}px`);
        });
      });
    },
    toggle: function () {
      $(this.tooltip).hasClass('is-show') ? this.hide.call(this) : this.show.call(this);
    },
    show: function () {
      this.update();
      $(this.tooltip).addClass('is-show');
    },
    hide: function () {
      $(this.tooltip).removeClass('is-show');
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    if (this.length && typeof window.FloatingUIDOM === 'undefined') {
      console.error('The third party "FloatingUI" plugin could not be loaded!\n%cUnable to initialize the "popover" plugin.', 'color: red;');
      return this;
    }
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-popover]').popover();
})(jQuery, window);

// megamenu
(function ($, document, window, undefined) {
  'use strict';

  var pluginName = 'megamenu';
  var defaults = {
    placement: 'bottom',
    middleware: [window.FloatingUIDOM.offset(), window.FloatingUIDOM.flip(), window.FloatingUIDOM.shift()]
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var $tooltip = this.$element.siblings('.c-menu');
      if (!$tooltip.length) {
        return;
      }
      this.tooltip = $tooltip.get(0);
      this.update();
      this.$element.closest('.c-menu__item').off('mouseenter mouseleave').hover(e => {
        $(window).trigger('click');
        $('body').addClass('is-menuDesktopOpen');
      }, e => {
        $('body').removeClass('is-menuDesktopOpen');
      });
      this.$element.off('click.popover').on('click.popover', e => {
        e.preventDefault();
        this.collapse.call(this);
      });
    },
    update: function () {
      window.FloatingUIDOM.autoUpdate(this.element, this.tooltip, () => {
        window.FloatingUIDOM.computePosition(this.element, this.tooltip, this.options).then(_ref2 => {
          let {
            x,
            y
          } = _ref2;
          this.tooltip.style.setProperty('--left', `${x}px`);
          this.tooltip.style.setProperty('--top', `${y}px`);
        });
      });
    },
    collapse: function () {
      var layout = getComputedStyle(document.documentElement).getPropertyValue('--layout');
      if (layout == 'desktop') {
        return;
      }
      var $child = this.$element.siblings('.c-menu');
      var height = $child.children(':visible').get().reduce(function (prev, current) {
        return prev + current.offsetHeight;
      }, 0);
      $child.get(0).style.setProperty('--height', height + 'px');
      var $item = this.$element.closest('.c-menu__item--lv1');
      $('.c-menu__item--lv1').not($item).removeClass('is-open');
      $item.toggleClass('is-open');
      if ($item.hasClass('is-open')) {
        setTimeout(function () {
          var $container = $('.l-header__menu');
          $container.animate({
            scrollTop: $item.offset().top + $container.scrollTop() - $container.offset().top
          }, 300);
        }, 300);
      }
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    if (this.length && typeof window.FloatingUIDOM === 'undefined') {
      console.error('The third party "FloatingUI" plugin could not be loaded!\n%cUnable to initialize the "megamenu" plugin.', 'color: red;');
      return this;
    }
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-megamenu="true"]').megamenu();
  $('body').off('click.burger').on('click.burger', '[data-burger]', function (e) {
    e.preventDefault();
    $(this).toggleClass('is-open');
    $('body').toggleClass('is-menuMobileOpen', $(this).hasClass('is-open'));
  });
})(jQuery, document, window);

// tab
(function ($, window, undefined) {
  'use strict';

  var pluginName = 'tab';
  var defaults = {
    docking: true,
    scroll: true
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var that = this;

      // tab switch 功能
      this.$element.find('.c-tab__switch').off('click').on('click', function (e) {
        e.preventDefault();
        $(this).closest('.c-tab__collapse').toggleClass('is-open');
      });
      var align = this.$element.find('[data-tab-align]').data('tab-align');
      this.$element.find('[data-tab-target]').off('switch').on('switch', function (e) {
        var target = $(this).data('tabTarget');
        var $target = that.$element.find('> [data-tab-panel-id="' + target + '"]');
        if (target && $target.length) {
          e.preventDefault();
          that.$element.find('> .c-tab__header [data-tab-target], > [data-tab-panel-id]').removeClass('is-active');
          that.$element.find('> .c-tab__header [data-tab-target="' + target + '"]').addClass('is-active');
          $target.addClass('is-active');
          that.scrollAlign.call(that, align);
        }
        that.$element.find('.c-tab__collapse').removeClass('is-open');
      }).off('click').on('click', function (e) {
        e.preventDefault();
        $(this).trigger('switch');
        if (that.options.scroll) {
          setTimeout(function () {
            $('html, body').animate({
              scrollTop: that.$element.offset().top - $('.l-header').outerHeight(true)
            }, 300, function () {
              try {
                window.AOS.refresh();
              } catch (ex) {}
            });
          }, 300);
        }
      });
      this.$element.find('> .c-tab__header > .c-tab__navs > [data-tab-target].is-active').trigger('switch');

      // tab docking 功能
      if (this.options.docking && $.fn.scrollToFixed) {
        this.$element.find('.c-tab__header:first').scrollToFixed({
          limit: function () {
            var $container = $(this).closest('[data-tab="true"]');
            var limit = $container.offset().top + $container.innerHeight() - $(this).outerHeight(true);
            return limit;
          },
          fixed: function () {
            $(this).addClass('is-docking');
          },
          unfixed: function () {
            $(this).removeClass('is-docking');
          },
          preAbsolute: function () {
            $(this).addClass('is-freeze');
          },
          postAbsolute: function () {
            $(this).removeClass('is-freeze');
          },
          zIndex: 5
        });
      }

      // 頁面預設開啟其他頁籤
      const url = location.href;
      if (url.indexOf('?') !== -1 && url.indexOf('tab') !== -1) {
        var tabParam = url.split('?')[1].split('&').find(item => item.indexOf('tab') === 0);
        if (!!tabParam && tabParam.split('=')[0] === 'tab') {
          $(`[data-tab-target="${tabParam.split('=')[1]}"]`).trigger('switch');
        }
      }
      this.$element.attr('data-tab', true);
    },
    scrollAlign: function (align) {
      var $li = this.$element.find('.c-tab__item.is-active').closest('li');
      var $ul = this.$element.find('.c-tab__navs');
      if (align === 'left') {
        var pos = $li.position().left + $ul.scrollLeft();
      } else {
        var pos = $li.position().left + $li.width() / 2 + $ul.scrollLeft() - $ul.width() / 2;
      }
      $ul.animate({
        scrollLeft: pos
      }, 300);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-tab="true"]').tab();
})(jQuery, window);

// 搜尋框清除按鈕
(function ($, undefined) {
  'use strict';

  var pluginName = 'clear';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var $textbox = this.$element.find('[data-clear="textbox"]'),
        $button = this.$element.find('[data-clear="button"]');
      $button.off('click.clear').on('click.clear', function () {
        $textbox.val('').blur();
      });
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-clear="true"]').clear();
})(jQuery);

// 開合
(function ($, window, undefined) {
  'use strict';

  var pluginName = 'collapse';
  var defaults = {
    // button: '.c-notice__actions',
    // content: '.c-notice__outer',
    button: '[data-collapse-button]',
    content: '[data-collapse-content]',
    cssClass: 'is-active'
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var that = this;
      this.$element.find(this.options.button).off('click.collapse').on('click.collapse', function (e) {
        e.preventDefault();
        that.toggle.call(that);
      });
      $(window).on('resize.collapse', function () {
        if (that.$element.hasClass(that.options.cssClass)) {
          that.update.call(that);
        }
      });
      this.update.call(this);
      this.$element.attr('data-collapse', true);
    },
    update: function () {
      var $content = this.$element.find(this.options.content);
      if ($content.length) {
        var totalHeight = this.$element.find(this.options.content).children().map(function (i, el) {
          return el.offsetHeight;
        }).toArray().reduce((a, b) => a + b, 0);
        $content.get(0).style.setProperty('--height', `${totalHeight}px`);
      }
    },
    open: function () {
      this.update.call(this);
      this.$element.addClass(this.options.cssClass);
    },
    close: function () {
      this.$element.removeClass(this.options.cssClass);
    },
    toggle: function () {
      this.$element.hasClass(this.options.cssClass) ? this.close.call(this) : this.open.call(this);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-collapse="true"]').collapse();
})(jQuery, window);

// 超過高度收合
(function ($, undefined) {
  'use strict';

  var pluginName = 'overflowHide';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      const $this = this;
      // set原始收合高
      this.$element.attr('data-overflow-height', this.$element.outerHeight());
      this.judge(this.$element);
      // resize all element重新判斷高度收合
      $(window).off('resize.overflow').on('resize.overflow', function () {
        $('[data-overflow-hide]').each(function () {
          $this.judge($(this));
        });
      });

      // 點擊 展開or收合
      $('[data-overflow-action]').off('click.overflow').on('click.overflow', function (e) {
        e.preventDefault();
        const id = $(this).attr('data-overflow-action');
        const $parent = $(`[data-overflow-hide="${id}"]`);
        let isCollapse = $parent.hasClass('is-collapse');
        $parent.toggleClass('is-collapse is-expand').animate({
          height: isCollapse ? $(`[data-overflow-content="${id}"]`).outerHeight() : $parent.attr('data-overflow-height')
        }, 300, function () {
          if (isCollapse) {
            $parent.css('height', 'auto');
          } else {
            $parent.removeAttr('style');
          }
        });
      });
    },
    /**
     * 判斷是否超過高度，需要收起來
     * @param $this element
     */
    judge: function ($this) {
      const id = $this.attr('data-overflow-hide');
      let originalH = $this.attr('data-overflow-height');
      let contentH = $(`[data-overflow-content="${id}"]`).outerHeight();
      $this.toggleClass('is-overflow', contentH > originalH);
      $this.toggleClass('not-overflow', !(contentH > originalH));
      if (!$this.hasClass('is-expand')) {
        $this.toggleClass('is-collapse', contentH > originalH);
      }
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-overflow-hide]').overflowHide();
})(jQuery);

// 拉霸
(function ($, undefined) {
  'use strict';

  var pluginName = 'sliderBar';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      const $this = this;
      noUiSlider.create(this.element, this.options);
      this.$element.find('.noUi-marker').remove();
      this.$element.find('.noUi-value').first().text('-');
      this.$element.find('.noUi-value').last().text('+');
      this.element.noUiSlider.on('set', function (values, handle, unencoded, tap, positions, noUiSlider) {
        const $parent = $this.$element.closest('.c-sliderBar');
        let value = unencoded;
        positions.forEach((item, index) => {
          if (item === 0) {
            value[index] = 'min';
          } else if (item === 100) {
            value[index] = 'max';
          }
        });
        $parent.find('[data-slider-val]').val(value).trigger('change');
      });
      this.element.noUiSlider.set();
      this.$element.closest('.c-sliderBar').find('[data-slider-val]').on('input.reset', function () {
        $this.element.noUiSlider.reset();
      });
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
})(jQuery);

// tooltip
(function ($, window, undefined) {
  'use strict';

  var pluginName = 'tooltip';
  var defaults = {
    placement: 'top',
    middleware: [window.FloatingUIDOM.offset(8), window.FloatingUIDOM.flip(), window.FloatingUIDOM.shift()]
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      if (!this.tooltip && !this.element.getAttribute('data-tooltip')) {
        return;
      }
      if (!this.tooltip) {
        var tooltip = document.createElement('div');
        tooltip.classList.add('o-tooltip');
        var content = document.createElement('div');
        content.classList.add('o-tooltip__content');
        content.innerHTML = this.element.getAttribute('data-tooltip');
        tooltip.append(content);
        var arrow = document.createElement('div');
        arrow.classList.add('o-tooltip__arrow');
        tooltip.append(arrow);
        this.element.after(tooltip);
        this.tooltip = tooltip;
      }
      [['mouseenter', this.show], ['mouseleave', this.hide],
      //['focus', this.show],
      ['blur', this.hide]].forEach(_ref3 => {
        let [event, listener] = _ref3;
        this.element.addEventListener(event, () => listener.call(this));
      });

      // [data-tooltip] 的值必需為要提示的文字訊息，為必要項，故不另外加上此 attribute.
      // this.$element.attr('data-tooltip', '');
    },

    update: function () {
      this.tooltip.getElementsByClassName('o-tooltip__content')[0].innerHTML = this.element.getAttribute('data-tooltip');
      const arrowElement = this.tooltip.getElementsByClassName('o-tooltip__arrow')[0];
      this.options.middleware.push(window.FloatingUIDOM.arrow({
        element: arrowElement
      }));
      window.FloatingUIDOM.autoUpdate(this.element, this.tooltip, () => {
        window.FloatingUIDOM.computePosition(this.element, this.tooltip, this.options).then(_ref4 => {
          let {
            x,
            y,
            placement,
            middlewareData
          } = _ref4;
          Object.assign(this.tooltip.style, {
            left: `${x}px`,
            top: `${y}px`
          });
          const {
            x: arrowX,
            y: arrowY
          } = middlewareData.arrow;
          const staticSide = {
            top: 'bottom',
            right: 'left',
            bottom: 'top',
            left: 'right'
          }[placement.split('-')[0]];
          const rotate = staticSide == 'top' ? 'rotate(180deg)' : 'rotate(0)';
          Object.assign(arrowElement.style, {
            left: arrowX != null ? `${arrowX}px` : '',
            top: arrowY != null ? `${arrowY}px` : '',
            right: '',
            bottom: '',
            [staticSide]: '-8px',
            transform: rotate
          });
        });
      });
    },
    show: function () {
      this.update();
      this.tooltip.classList.add('is-active');
    },
    hide: function () {
      this.tooltip.classList.remove('is-active');
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    if (this.length && typeof window.FloatingUIDOM === 'undefined') {
      console.error('The third party "FloatingUI" plugin could not be loaded!\n%cUnable to initialize the "tooltip" plugin.', 'color: red;');
      return this;
    }
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-tooltip]').tooltip();
})(jQuery, window);

// 狀態/互動式按鈕
(function ($, window, undefined) {
  'use strict';

  var pluginName = 'interactive';
  var defaults = {
    cssClass: 'is-active',
    msg: '',
    iaToast: true,
    iaTooltip: true,
    swal: {
      iconHtml: '<i class="o-icon o-icon--tickSolid o-icon--wt o-icon--xl o-icon--nonhover"></i>',
      customClass: {
        container: 'c-swal',
        popup: 'c-swal__popup',
        icon: 'c-swal__icon',
        title: 'c-swal__title',
        timerProgressBar: 'c-swal__timerProgressBar'
      },
      position: 'center',
      showConfirmButton: false,
      backdrop: false,
      timerProgressBar: true,
      returnFocus: false,
      timer: 1500
    }
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      let msgs = this.options.msg.split('|');
      this.options.msgIn = msgs[0] || '';
      this.options.msgOut = msgs[1] || '';
      let that = this;
      this.$element.off('update.tooltip').on('update.tooltip', function (e) {
        if (!that.options.iaTooltip || !that.options.msg) {
          return;
        }
        let tooltip = $(this).hasClass(that.options.cssClass) ? that.options.msgOut : that.options.msgIn;
        if (tooltip) {
          this.setAttribute('data-tooltip', tooltip);
        }
        if ($(this).tooltip) {
          $(this).tooltip('update');
        }
      }).off('click.interactive').on('click.interactive', function (e) {
        e.preventDefault();
        $(this).toggleClass(that.options.cssClass).trigger('update.tooltip');
        if (that.options.iaToast && that.options.msg) {
          if (typeof window.Swal === 'undefined') {
            console.info('The third party "Swal" plugin could not be loaded!\n%cUnable to run the "toast" alert function.', 'color: orange;');
            return;
          }
          let message = $(this).hasClass(that.options.cssClass) ? that.options.msgIn : that.options.msgOut;
          if (message) {
            let toast = window.Swal.mixin(that.options.swal);
            toast.fire({
              titleText: `已${message}`
            });
          }
        }
      }).trigger('update.tooltip').tooltip();
      this.$element.attr('data-ia', true);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-ia="true"]').interactive();
})(jQuery, window);

// 輪播
(function ($, window, undefined) {
  'use strict';

  var pluginName = 'carousel';
  var defaults = {
    lazy: true,
    loop: true,
    autoplay: {
      delay: 6000
    },
    speed: 1000,
    pagination: {
      el: '.swiper-pagination',
      clickable: true
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev'
    },
    on: {
      lock: function (swiper) {
        $(swiper.el).addClass('is-paginationless');
      },
      unlock: function (swiper) {
        $(swiper.el).removeClass('is-paginationless');
      }
    }
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    let settings = this.element.dataset.swiper ? JSON.parse(this.element.dataset.swiper) : {};
    this.options = $.extend(true, {}, defaults, options, settings, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var swiper = new Swiper(this.element, this.options);
      if (typeof this.options.autoplay !== 'undefined' && this.options.autoplay !== false) {
        this.element.addEventListener('mouseenter', function (e) {
          swiper.autoplay.stop();
        });
        this.element.addEventListener('mouseleave', function (e) {
          swiper.autoplay.start();
        });
      }
      this.$element.attr('data-carousel', true);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    if (this.length && typeof window.Swiper === 'undefined') {
      console.error('The third party "Swiper" plugin could not be loaded!\n%cUnable to initialize the "carousel" plugin.', 'color: red;');
      return this;
    }
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-carousel="true"]').carousel();
})(jQuery, window);

// 分頁
(function ($, undefined) {
  'use strict';

  var pluginName = 'pagination';
  var defaults = {
    prev: '.o-pagination__prev',
    next: '.o-pagination__next',
    input: '.o-pagination__no',
    total: 1,
    onchange: function (page) {}
    // onchange: function (page) { console.log(page); }
  };

  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.original = 1;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var that = this;
      this.$element.find(this.options.prev).off('click.pagination').on('click.pagination', function (e) {
        e.preventDefault();
        var page = parseInt(that.$element.find(that.options.input).val());
        if (page > 1) {
          that.goto.call(that, page - 1);
        }
      });
      this.$element.find(this.options.next).off('click.pagination').on('click.pagination', function (e) {
        e.preventDefault();
        var page = parseInt(that.$element.find(that.options.input).val());
        if (page < $(this).closest('[data-total]').attr('data-total')) {
          that.goto.call(that, page + 1);
        }
      });
      this.$element.find(this.options.input).off('keypress.pagination').on('keypress.pagination', function (e) {
        if (e.which === 13) {
          var page = parseInt($(this).val());
          if (!isNaN(page) && page >= 1 && page <= $(this).closest('[data-total]').attr('data-total')) {
            that.goto.call(that, page);
          } else {
            $(this).val(that.original);
          }
        }
      }).off('blur.pagination').on('blur.pagination', function () {
        $(this).val(that.original);
      });
      this.update.call(this, 1);
      this.$element.attr('data-pagination', 'true');
    },
    goto: function (page) {
      this.update.call(this, page);
      this.original = page;
      this.options.onchange(page);
      this.$element.find(this.options.input).trigger('update.pagination');
    },
    update: function (page) {
      this.$element.find(this.options.prev).toggleClass('is-disabled', page === 1);
      this.$element.find(this.options.next).toggleClass('is-disabled', page === parseInt(this.$element.attr('data-total')));
      this.$element.find(this.options.input).val(page);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-pagination="true"]').pagination();
})(jQuery);

// Scroll Top
(function ($, undefined) {
  'use strict';

  var pluginName = 'scrollPosition';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    scroll: function () {
      var top = this.$element.offset().top;
      // 扣除 header docking 高度
      if (!$('html').hasClass('is-embed')) {
        top -= $('.l-header').outerHeight(true) || 0;
      }

      // 移除 tab docking 狀態
      this.$element.closest('[data-tab]:not([data-docking="false"])').find('.c-tab__header').removeClass('scroll-to-fixed-fixed is-docking');
      $('html, body').animate({
        scrollTop: top
      }, 300);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
})(jQuery);

// mergeTable
(function ($, undefined) {
  'use strict';

  var pluginName = 'mergeTable';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      let tables = this.$element.find('table'),
        $leftTable = tables.eq(0),
        $rightTable = tables.eq(1);
      $rightTable.toggleClass('c-table--reversed@lt', $leftTable.find('tbody tr').length % 2 == 1);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-merge-table]').mergeTable();
})(jQuery);

// 基金評等
(function ($, undefined) {
  'use strict';

  var pluginName = 'level';
  var defaults = {};
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      $('[data-level-item]').on('change.level', function (e) {
        if (e.target.checked) {
          $(this).siblings('[data-level-item]').removeClass('o-starLevel__item--active');
          $(this).addClass('o-starLevel__item--active').prevAll().addClass('o-starLevel__item--active');
        } else {
          $(this).removeClass('o-starLevel__item--active');
          $(this).siblings('[data-level-item]').find('input').each((i, el) => {
            if (!el.checked) {
              $(el).closest('[data-level-item]').removeClass('o-starLevel__item--active');
            }
          });
        }
      });
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-level]').level();
})(jQuery);

// 凍結欄位
(function ($, undefined) {
  'use strict';

  var pluginName = 'fixedColumns';
  var defaults = {
    fixedColumns: true,
    paging: false,
    searching: false,
    ordering: false,
    info: false,
    scrollX: true
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    // delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      var that = this;
      this.$element.css('width', '100%').DataTable(this.options);
      $(window).on('resize.fixedColumns', function (e) {
        const $scrollBody = that.$element.closest('.dataTables_scrollBody');
        if (!$scrollBody.length) {
          return;
        }
        const scrollBody = $scrollBody.get(0);
        let hasScrollableContent = scrollBody.scrollWidth > scrollBody.clientWidth;
        let isOverflowHidden = window.getComputedStyle(scrollBody).overflowX.indexOf('hidden') !== -1;
        let hasScrollbar = hasScrollableContent && !isOverflowHidden;
        $scrollBody.closest('.dataTables_scroll').toggleClass('has-scrollbar', hasScrollbar);
        if (hasScrollbar) {
          let headerHeight = $scrollBody.siblings('.dataTables_scrollHead').outerHeight() - 4;
          if (headerHeight < 0) {
            headerHeight = 0;
          }
          $scrollBody.closest('.dataTables_scroll').get(0).style.setProperty('--icon-offset-top', `${headerHeight}px`);
        }
      }).trigger('resize.fixedColumns');
      this.$element.closest('.dataTables_scrollBody').off('scroll.fixedColumns').on('scroll.fixedColumns', function (e) {
        const $this = $(this);
        if ($this.scrollLeft() > 50) {
          $this.closest('.dataTables_scroll').addClass('is-scrolled');
        }
      });
      this.$element.attr('data-fixed-columns', JSON.stringify(this.options.fixedColumns));
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    if (this.length && !$.fn.dataTable) {
      console.error('The DataTables plugin could not be loaded!\n%cUnable to initialize the "fixedColumn" plugin.', 'color: red;');
      return this;
    }
    return this.each(function () {
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-fixed-columns]').fixedColumns();
})(jQuery);

// 日期選擇器
(function ($, undefined) {
  'use strict';

  var pluginName = 'datepickerExtend';
  var defaults = {
    autoclose: true,
    language: 'zh-TW',
    format: 'yyyy/mm/dd',
    todayHighlight: true,
    startDate: '1911/01/01'
  };
  function Plugin(element, options) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend(true, {}, defaults, options, this.$element.data());
    delete this.options[pluginName];
    this._defaults = defaults;
    this._name = pluginName;
    this.init();
    this.$element.data('plugin_' + pluginName, this);
  }
  $.extend(Plugin.prototype, {
    init: function () {
      this.$element.datepicker(this.options);
    }
  });
  $.fn[pluginName] = function (methodOrOptions) {
    return this.each(function () {
      if (!jQuery().datepicker) {
        console.error('jQuery plugin "datepicker" has not been loaded.');
        return;
      }
      var plugin = $.data(this, 'plugin_' + pluginName) || new Plugin(this, methodOrOptions);
      if (typeof methodOrOptions === 'string' && plugin[methodOrOptions]) {
        plugin[methodOrOptions].apply(plugin, Array.prototype.slice.call(arguments, 1));
      }
    });
  };
  $('[data-calendar]').datepickerExtend();
})(jQuery);
(function ($, undefined) {
  'use strict';

  var pluginName = 'toast';
  var defaults = {
    iconHtml: '<i class="o-icon o-icon--tickSolid o-icon--wt o-icon--xl o-icon--nonhover"></i>',
    customClass: {
      container: 'c-swal',
      popup: 'c-swal__popup',
      icon: 'c-swal__icon',
      title: 'c-swal__title',
      timerProgressBar: 'c-swal__timerProgressBar'
    },
    position: 'center',
    showConfirmButton: false,
    backdrop: false,
    timerProgressBar: true,
    returnFocus: false,
    timer: 1500
  };
  window[pluginName] = (message, options) => {
    if (typeof window.Swal === 'undefined') {
      console.info('The third party "Swal" plugin could not be loaded!\n%cUnable to run the "toast" alert function.', 'color: orange;');
      return;
    }
    let settings = $.extend(true, {}, defaults, options);
    let toast = window.Swal.mixin(settings);
    toast.fire({
      titleText: message
    });
  };

  //window.toast('顯示訊息');
})(jQuery);