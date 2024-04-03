(function ($, undefined) {
  'use strict';

  $.magnificPopup.instance.history = [];
  $.magnificPopup.closeAll = function () {
    if (this.instance && this.instance.history) {
      this.instance.history.length = 0;
    }
    return this.instance && this.instance.close();
  };
  $.extend($.magnificPopup.defaults, {
    fixedContentPos: true,
    fixedBgPos: true,
    removalDelay: 0,
    // showCloseBtn: false,
    // closeOnBgClick: false,
    // closeBtnInside: false,
    // closeMarkup: '<button title="%title%" type="button" class="c-modal__close"></button>',
    callbacks: {
      afterChange: function () {
        var src = this.items[this.index].src;
        if (this.history[this.history.length - 1] !== src) {
          this.history.push(src);
        }
      },
      afterClose: function () {
        this.history.pop();
        if (this.history.length > 0) {
          $.magnificPopup.open({
            items: {
              src: this.history[this.history.length - 1]
            }
          });
        }
      }
    }
  });
  $(function () {
    $('[data-popup]').magnificPopup();
    $('[data-youtube]').magnificPopup({
      disableOn: 700,
      type: 'iframe',
      mainClass: 'mfp-fade',
      removalDelay: 160,
      preloader: false,
      fixedContentPos: false
    });
    $('body').off('click.mfpClose').on('click.mfpClose', '[data-popup-close]', function (e) {
      e.preventDefault();
      $.magnificPopup.close();
    }).off('click.mfpCloseAll').on('click.mfpCloseAll', '[data-popup-close-all="true"]', function (e) {
      e.preventDefault();
      $.magnificPopup.closeAll();
    });
  });
})(jQuery);