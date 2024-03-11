const chartColors = ['#238C6C', '#D0A66C', '#F58220', '#A4844E', '#948BF2', '#FFAD04', '#FF77C0', '#C16FE8', '#DD7676', '#ABA1E4', '#86BDDD', '#60A6E2', '#3AAB68', '#D0AD72', '#FDAA5E', '#63C98D', '#665DCC', '#E7465A'];
function apiRequestGet(url, params) {
  return new Promise((resolve, reject) => {
    $.ajax({
      url,
      type: 'GET',
      dataType: 'json',
      data: params,
      success: function (data) {
        resolve(data);
      },
      error: function (xhr, status, error) {
        reject(error);
      }
    });
  });
}
function apiRequestPost(url, params) {
  return new Promise((resolve, reject) => {
    $.ajax({
      url,
      type: 'POST',
      dataType: 'json',
      contentType: 'application/json',
      data: JSON.stringify(params),
      success: function (data) {
        resolve(data);
      },
      error: function (xhr, status, error) {
        reject(error);
      }
    });
  });
}
(function ($, undefined) {
  'use strict';

  Highcharts.theme = {
    colors: chartColors
  };
  Highcharts.setOptions(Highcharts.theme);
})();