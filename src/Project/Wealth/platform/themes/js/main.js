// Global site event binding
(function ($, document, window, undefined) {
  // 頁面loading
  $(window).on('load', function () {
    window.loading('hide');
  });

  // viewport
  (function (targetWidth, document, window, undefined) {
    var deviceWidth = window.screen.width;
    var ratio = deviceWidth / targetWidth;
    var viewport = document.querySelector('meta[name="viewport"]');
    if (ratio < 1) {
      viewport.setAttribute('content', 'width=device-width, initial-scale=' + ratio + ', minimum-scale=' + ratio + ', maximum-scale=' + ratio + ', user-scalable=yes');
    }
  })(360, document, window);

  // set global css variable
  (function (document, window, undefined) {
    const setFillHeight = () => {
      // fixed ios css vh bug
      const vh = window.innerHeight * 0.01;
      document.documentElement.style.setProperty('--vh', `${vh}px`);

      // detect scrollbar width
      const scrollbar = window.innerWidth - document.documentElement.clientWidth;
      document.documentElement.style.setProperty('--scrollbar-width', `${scrollbar}px`);
    };
    window.addEventListener('resize', () => setFillHeight());
    setFillHeight();
  })(document, window);

  // scroll direction plugin initial
  if (typeof $.scrollDirection === 'undefined') {
    console.error('The third party "scrollDirection" plugin could not be loaded!\n%cThis website must have the "scrollDirection" plugin.', 'color: red;');
  }
  $.scrollDirection && $.scrollDirection.init();

  // Cookie彈窗關閉
  $('[data-cookie-close]').on('click.cookie', function () {
    $('[data-cookie]').fadeOut();
  });

  // go top button
  (function ($, window, undefined) {
    $(window).on('scrollDirection', function () {
      $('[data-gotop]').toggleClass('is-active', $.scrollDirection.isScrollAtMiddle);
    }).trigger('scrollDirection');
    $('[data-gotop]').off('click').on('click', function (e) {
      e.preventDefault();
      $('html, body').animate({
        scrollTop: 0
      }, 300);
    });
  })($, window);

  // header fadeout
  (function ($, window, undefined) {
    $(window).on('scrollDown.header', function () {
      var isFixed = $(this).scrollTop() > $('.l-header').outerHeight();
      $('body').toggleClass('is-headerFadeout', isFixed);
    }).on('scrollUp.header', function () {
      $('body').removeClass('is-headerFadeout');
    });
  })($, window);

  // sidebar
  (function ($, undefined) {
    $('[data-sidebar]').off('click').on('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
      var target = $(this).data('sidebar');
      $('[data-sidebar]').removeClass('is-active');
      $(this).addClass('is-active');
      $('[data-sidebar-id="' + target + '"]').addClass('is-active').siblings().removeClass('is-active');
      $('body').addClass('is-sidebarOpen');
    });
    $('.c-sidebar__panel').off('click').on('click', function (e) {
      e.stopPropagation();
    });
    $('body').off('click.sidebar').on('click.sidebar', function () {
      $('[data-sidebar], [data-sidebar-id]').removeClass('is-active');
      $('body').removeClass('is-sidebarOpen');
    });
    $('[data-sidebar-close]').on('click', function (e) {
      e.preventDefault();
      $('body').trigger('click.sidebar');
    });
  })($);

  // embed detect
  (function ($, window, undefined) {
    var url = new URL(window.location.href);
    $('html').toggleClass('is-embed', url.searchParams.has('embed'));
  })($, window);

  // 分享
  (function ($, undefined) {
    var url = window.location.href,
      encodeUrl = encodeURIComponent(url);
    $('[data-share]').each((i, el) => {
      var platformShareUrl = $(el).attr('href');
      $(el).attr('href', platformShareUrl + encodeUrl);
    });
    $('[data-copy-action]').on('click.copy', function (e) {
      e.preventDefault();
      var $toolTip = $(this).closest('[data-copy]').find('[data-copy-msg]');
      navigator.clipboard.writeText(url).then(() => {
        $toolTip.addClass('is-active');
        setTimeout(() => {
          $toolTip.removeClass('is-active');
        }, 1000);
      });
    });
  })($);
})(jQuery, document, window);