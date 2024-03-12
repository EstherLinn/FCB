(function (undefined) {
  'use strict';

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
    }
  };
  var sliders = document.querySelectorAll('[data-swiper="true"]');
  sliders.forEach(slider => {
    let options = slider.dataset.swiper ? JSON.parse(slider.dataset.swiper) : {};
    slider.options = Object.assign({}, defaults, options);
    var swiper = new Swiper(slider, slider.options);
    if (typeof slider.options.autoplay !== 'undefined' && slider.options.autoplay !== false) {
      slider.addEventListener('mouseenter', function (e) {
        swiper.autoplay.stop();
      });
      slider.addEventListener('mouseleave', function (e) {
        swiper.autoplay.start();
      });
    }
  });
})();