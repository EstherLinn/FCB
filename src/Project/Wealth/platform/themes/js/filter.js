$(function () {
  // 展開收合
  $('[data-filter-collapse]').on('click', function (e) {
    e.preventDefault();
    $('body').toggleClass('is-filterOpen');
    if (getComputedStyle(document.documentElement).getPropertyValue('--layout') === 'mobile') {
      $('[data-filter]').toggleClass('is-collapse is-expand');
      return;
    }
    let isCollapse = $('[data-filter]').hasClass('is-collapse');
    $('[data-filter-hide="desktop"]').animate({
      height: isCollapse ? $('[data-filter-wrap="desktop"]').outerHeight() : 0
    }, 300, function () {
      $('[data-filter-hide="desktop"]').removeAttr('style');
      $('[data-filter]').toggleClass('is-collapse is-expand');
    });
  });

  // checkbox切換
  $('[data-filter-checkbox]').on('change.filter', function () {
    setTag();
    $(this).closest('[data-filter-group]').find('[data-filter-checkall]').prop('checked', !$(this).closest('[data-filter-group]').find('[data-filter-checkbox] input:checked').length > 0);
  });

  // 全部checkbox切換
  $('[data-filter-checkall]').on('change.checkAll', function (e) {
    $(this).prop('checked', true);
    $(this).closest('[data-filter-group]').find('[data-filter-checkbox] input').each((index, item) => {
      $(item).prop('checked', false);
    });
    $('[data-filter-checkbox]').trigger('change.filter');
  });
  let keyword = [];
  function canPushKeyword(value) {
    return keyword.indexOf(value) === -1 && !!value.trim();
  }
  // 關鍵字搜尋(點擊)
  $('[data-keyword-submit]').on('click.filter', function () {
    const value = $('[data-keyword-input]').val();
    if (canPushKeyword(value)) {
      keyword.push(value.trim());
      setTag();
    }
  });

  // 關鍵字搜尋(鍵盤enter)
  $('[data-keyword-input]').on('keypress.filter', function (e) {
    const value = $(this).val();
    if (e.which === 13 && canPushKeyword(value)) {
      keyword.push(value.trim());
      setTag();
    }
  });

  // 拉霸更改
  $('[data-slider-val]').on('change.sliderBar', function () {
    let value = $(this).val().split(',');
    // 有前綴詞
    const prefixText = $(this).data('slider-prefix');
    if (!!prefixText) {
      value = value.map(item => {
        if (item !== 'min' && item !== 'max') {
          return `${prefixText + item}`;
        } else {
          return item;
        }
      });
    }
    // 有後綴詞
    const suffixText = $(this).data('slider-suffix');
    if (!!suffixText) {
      value = value.map(item => {
        if (item !== 'min' && item !== 'max') {
          return `${item + suffixText}`;
        } else {
          return item;
        }
      });
    }

    // 設定呈現文字
    if (value[0] === 'min' && value[1] === 'max') {
      value = '';
    } else if (value[1] === 'min') {
      value = $(this).siblings('[data-slider-bar]').find('.noUi-value').eq(1).text() + '(不含)以下';
    } else if (value[0] === 'max') {
      value = $(this).siblings('[data-slider-bar]').find('.noUi-value').eq($(this).siblings('[data-slider-bar]').find('.noUi-value').length - 2).text() + '(不含)以上';
    } else if (value[0] === 'min') {
      value = value[1] + '以下';
    } else if (value[1] === 'max') {
      value = value[0] + '以上';
    } else if (value[0] === value[1]) {
      value = value[0];
    } else {
      value = `${value[0]} ~ ${value[1]}`;
    }
    $(this).data('filter-bar', value);
    setTag();
  });

  // radio切換
  $('[data-filter-radio]').on('change.filter', function () {
    setTag();
  });

  // input輸入
  $('[data-filter-input]').on('change.filter', function () {
    $(this).val($(this).val().trim());
    setTag();
  });

  // 重設所有條件
  $('[data-filter-reset]').on('click.reset', function (e) {
    e.preventDefault();
    keyword = [];
    $('[data-keyword-input]').val('');
    $('[data-filter-checkbox] input, [data-filter-radio] input').prop('checked', false).trigger('change');
    $('[data-filter-bar]').val('').trigger('input.reset');
    $('[data-filter-input]').val('').trigger('change');
    if ($('[data-filter-input][data-calendar="true"]').length > 0) {
      $('[data-filter-input][data-calendar="true"]').datepicker('update');
    }
    $('[data-filter-tags]').empty();
    $('[data-filter-block]').addClass('no-picked');
  });

  /**
   * 顯示已選取條件
   */
  function setTag() {
    let allValue = [];
    $('[data-filter-tags]').empty();
    $('[data-filter-group]').each((index, item) => {
      const isPart = $(item).find('[data-filter-part]').length > 0;
      allValue.push({
        title: $(item).find('[data-filter-title] span').text(),
        part: isPart,
        special: $(item).data('filter-group') === 'special',
        val: []
      });
      if (isPart) {
        $(`[data-part-id="${$(item).find('[data-filter-part]').data('filter-part')}"]`).each((partIndex, partItem) => {
          if (typeof findValue(partItem) === 'string') {
            allValue[index].val.push(findValue(partItem));
            return;
          }
          if (!!findValue(partItem)) {
            allValue[index].val = [...allValue[index].val, ...findValue(partItem)];
          }
        });
        return;
      }
      if (typeof findValue(item) === 'string') {
        allValue[index].val.push(findValue(item));
        return;
      }
      if (!!findValue(item)) {
        allValue[index].val = [...findValue(item)];
      }
    });
    // set keyword value
    allValue[0].val = keyword;
    $('[data-filter-block]').toggleClass('no-picked', allValue.filter(item => {
      return !item.special && item.val.length > 0;
    }).length <= 0);

    // 印出tag
    allValue.forEach((item, index) => {
      if (item.special) {
        return;
      }
      if (item.part) {
        let partValue = '';
        const partDivideVal = $('[data-filter-group]').eq(index).find('[data-part-divide]').data('part-divide');
        const sign = !!partDivideVal ? partDivideVal : '/';
        item.val.forEach((val, index) => {
          const divider = index === 0 ? '' : sign;
          partValue = partValue + divider + val;
        });
        let newTag = $(`<div class="l-pickGroup__item" data-filter-tag="${htmlEncode(item.title)}">
                    <div class="o-pickedTag">
                        <div class="o-pickedTag__text">${htmlEncode(item.title)} ${htmlEncode(partValue)}</div>
                        <a href="#" class="o-pickedTag__button" data-tag-close></a>
                    </div>
                </div>`);

        // 綁定點擊取消事件
        resetValue(newTag, index, partValue);
        $('[data-filter-tags]').append(newTag);
      } else {
        item.val.forEach(val => {
          let newTag = $(`<div class="l-pickGroup__item" data-filter-tag="${htmlEncode(item.title)}">
                        <div class="o-pickedTag">
                            <div class="o-pickedTag__text">${htmlEncode(item.title)} ${htmlEncode(val)}</div>
                            <a href="#" class="o-pickedTag__button" data-tag-close></a>
                        </div>
                    </div>`);

          // 綁定點擊取消事件
          resetValue(newTag, index, val);
          $('[data-filter-tags]').append(newTag);
        });
      }
    });
  }
  function findValue(filterGroup) {
    // set checkbox value
    if ($(filterGroup).find('[data-filter-checkbox] input:checked').length > 0) {
      let pushArray = [];
      $(filterGroup).find('[data-filter-checkbox] input:checked').each((checkIndex, checkItem) => {
        pushArray.push($(checkItem).closest('[data-filter-checkbox]').find('[data-filter-value]').data('filter-value'));
      });
      return pushArray;
    }

    // set input value
    if (!!$(filterGroup).find('[data-filter-input]').val()) {
      return $(filterGroup).find('[data-filter-input]').val();
    }

    // set slider bar value
    if (!!$(filterGroup).find('[data-filter-bar]').data('filter-bar')) {
      return $(filterGroup).find('[data-filter-bar]').data('filter-bar');
    }

    // set radio value
    if ($(filterGroup).find('[data-filter-radio] input:checked').length > 0) {
      return $(filterGroup).find('[data-filter-radio] input:checked').closest('[data-filter-radio]').find('[data-filter-value]').data('filter-value');
    }
  }

  /**
   * tag 點擊叉叉
   * @param tagEl newTag jq element
   * @param groupIndex [data-filter-group] index
   * @param resetItem [data-filter-value] value(text)
   */
  function resetValue(tagEl, groupIndex, resetItem) {
    tagEl.find('.o-pickedTag__button').on('click.tag', function (e) {
      e.preventDefault();
      // reset keyword
      if (groupIndex === 0) {
        keyword.splice(keyword.indexOf(resetItem), 1);
        setTag();
        // ------------- 示意資料載入出現loading畫面，請更換成正確程式碼 -------------
        // 開啟loading方式:
        window.loading('show');
        setTimeout(() => {
          // 載入完成需將loading關閉
          // 關閉loading方式:
          window.loading('hide');
        }, 300);
        // ------------- 示意資料載入出現loading畫面，請更換成正確程式碼 -------------

        return;
      }
      ;
      const $thisGroup = $('[data-filter-group]').eq(groupIndex);
      // reset checkbox
      $thisGroup.find(`[data-filter-value="${resetItem}"]`).closest('[data-filter-checkbox]').find('input').prop('checked', false).trigger('change');
      // reset sliderbar
      $thisGroup.find('[data-filter-bar]').val('').trigger('input.reset');
      // reset radio
      $thisGroup.find('[data-filter-radio] input:checked').prop('checked', false).trigger('change');
      // reset input
      $thisGroup.find('[data-filter-input]').val('').trigger('change');
      if ($thisGroup.find('[data-filter-input][data-calendar="true"]').length > 0) {
        $thisGroup.find('[data-filter-input][data-calendar="true"]').datepicker('update');
      }
    });
  }
  function htmlEncode(str) {
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
  }
});