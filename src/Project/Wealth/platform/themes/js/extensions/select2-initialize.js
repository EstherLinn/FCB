(function ($, undefined) {
  'use strict';

  if (!$.fn.select2) {
    console.error('The jQuery select2 plugin could not be loaded!');
    return false;
  }
  $.fn.select2.defaults.set('theme', 'fcb');
  $.fn.select2.defaults.set('minimumResultsForSearch', 'Infinity');

  //$(function () {
  //    $('[data-selectbox="true"]').select2({
  //        //theme: 'fcb',
  //        //selectionCssClass: 'select2-selection--shadow',
  //        //width: '100%',
  //        //dropdownAutoWidth: true,
  //        //minimumResultsForSearch: Infinity
  //    });
  //});
})(jQuery);

// 標準下拉
(function ($, undefined) {
  'use strict';

  if (!$.fn.select2) {
    console.error('The jQuery select2 plugin could not be loaded!\n%cUnable to initialize the "selectbox" plugin.', 'color: red;');
    return;
  }
  var pluginName = 'selectbox';
  var defaults = {
    // theme: 'fcb',
    // minimumResultsForSearch: 'Infinity'
    dropdownCssClass: 'select2-dropdown--selectbox'
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
      this.$element.select2(this.options);
      this.$element.attr('data-selectbox', 'true').removeClass('select2-hidden-accessible');
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
  $('[data-selectbox="true"]').selectbox();
})(jQuery);

// 分頁筆數下拉
(function ($, undefined) {
  'use strict';

  if (!$.fn.select2) {
    console.error('The jQuery select2 plugin could not be loaded!\n%cUnable to initialize the "sizebox" plugin.', 'color: red;');
    return;
  }
  var pluginName = 'sizebox';
  var defaults = {
    width: 'auto',
    selectionCssClass: 'select2-selection--pagesize',
    dropdownCssClass: 'select2-dropdown--pagesize select2-dropdown--autoWidth',
    dropdownAutoWidth: true,
    templateResult: function (state, li) {
      if (state.element && state.element.hasAttribute('data-sizebox-option')) {
        var $input = $(`<input type="number" value="${state.id}" class="select2-results__pager">`).off('click.pagesize').on('click.pagesize', function (e) {
          e.stopPropagation();
          e.preventDefault();
        }).off('entraKey.pagesize').on('entraKey.pagesize', function () {
          var size = parseInt($(this).val());
          if (isNaN(size) || size <= 0) {
            $(this).val('').focus();
            return;
          }
          var $select = $(state.element.parentNode);
          $select.find('[data-sizebox-option]').detach();
          var option = new Option(`${size}筆`, size, true, true);
          option.setAttribute('data-sizebox-option', true);
          $select.append(option).trigger('change').select2('close');
        }).off('keyup.pagesize').on('keyup.pagesize', function (e) {
          if (e.keyCode == 13) {
            $(this).trigger('entraKey.pagesize');
          }
        });
        var $btn = $('<a href="#" class="o-btn o-btn--primary o-btn--auto o-btn--thin">確認</a>').off('click.pagesize').on('click.pagesize', function (e) {
          e.preventDefault();
          $input.trigger('entraKey.pagesize');
        });
        var $html = $('<label class="select2-results__custom"></label>').append('<span class="u-nowrap">自訂</span>').append($input).append('<span>筆</span>').append($btn);
        li.classList.add('select2-results__option--custom');
        return $html;
      }
      return $(`<label class="select2-results__custom">顯示${state.text}</label>`);
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
      var that = this;
      this.$element.off('select2:selecting').on('select2:selecting', function (e) {
        var option = e.params.args.data.element;
        if (option.hasAttribute('data-sizebox-option')) {
          $(this).data('select2').results.$results.find('.select2-results__option--custom').addClass('select2-results__option--actived').siblings().removeClass('select2-results__option--selected');
          return false;
        }
      }).off('select2:closing').on('select2:closing', function (e) {
        // 移除自訂選項未輸入值的選項
        $(this).find('[value=""][data-sizebox-option]').detach();
      }).off('select2:opening').on('select2:opening', function (e) {
        var $this = $(this);
        // 若無自訂頁數，動態新增一筆空的自訂頁數選項
        if (!$this.find('[data-sizebox-option]').length) {
          $this.append('<option value="" data-sizebox-option="true"></option>');
        }
      }).off('select2:open').on('select2:open', function (e) {
        var $this = $(this);
        setTimeout(() => {
          var $li = $this.data('select2').results.$results.find('.select2-results__option--custom');
          if ($li.hasClass('select2-results__option--selected')) {
            $li.addClass('select2-results__option--actived');
            $li.removeClass('select2-results__option--selected');
          }
        }, 10);
      }).select2(that.options);
      this.$element.attr('data-sizebox', 'true').removeClass('select2-hidden-accessible');
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
  $('[data-sizebox="true"]').sizebox();
})(jQuery);

// 排序下拉
(function ($, undefined) {
  'use strict';

  if (!$.fn.select2) {
    console.error('The jQuery select2 plugin could not be loaded!\n%cUnable to initialize the "selectbox" plugin.', 'color: red;');
    return;
  }
  var pluginName = 'sortingbox';
  var defaults = {
    width: 'auto',
    //placeholder: '排序',
    dropdownAutoWidth: true,
    dropdownCssClass: 'select2-dropdown--sortingbox',
    selectionCssClass: 'select2-selection--sortingbox'
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
      this.$element.off('select2:open.sorting').on('select2:open.sorting', function (e) {
        // 下拉選項齊右，避免超出畫面
        var $dropdown = $('.' + that.options.dropdownCssClass).closest('.select2-container');
        $dropdown.addClass('u-transparent');
        setTimeout(() => {
          var shift = $dropdown.width() - $(e.currentTarget.nextElementSibling).width();
          $dropdown.css('margin-left', -shift + 'px').removeClass('u-transparent');
        }, 50);
      }).select2(this.options);
      this.$element.attr('data-sortingbox', 'true').removeClass('select2-hidden-accessible');
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
  $('[data-sortingbox="true"]').sortingbox();
})(jQuery);