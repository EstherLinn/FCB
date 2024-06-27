<%@ page language="C#" async="true" autoeventwireup="true" codebehind="~/SingleSignOn/PageCodeBehind/LoginPage.cs" inherits="Feature.Wealth.Account.SingleSignOn.PageCodeBehind.LoginPage" %>

<!DOCTYPE html>
<html>
<head>
    <title>sso login
    </title>
    <link rel="shortcut icon" href="/sitecore/images/favicon.ico">
    <meta name="robots" content="noindex, nofollow">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <link type="text/css" rel="stylesheet" href="/layouts/offline_fonts.css">
    <link href="/sitecore/login/Login.css" rel="stylesheet">
    <style>
      .login-outer {
          background: url('/sitecore/login/drop_wallpaper.jpg') no-repeat center center fixed;
          background-size: cover;
          font-size: 1.3rem;
          color: seashell;
          text-align: center;
          margin-top: 10px;
      }
      .login-main-wrap {
          display: table-caption;
          vertical-align: middle;
          padding-bottom: 20px;
      }

      html {
          overflow: -moz-hidden-unscrollable;
          height: 100%;
      }

      body::-webkit-scrollbar {
          display: none;
      }

      body {
         -ms-overflow-style: none;
         height: 100%;
	     width: calc(100vw + 18px);
	     overflow: auto;
      }
    </style>
</head>
<body class="login-outer">
    <div class="login-main-wrap">
        <div class="login-box">
            <div class="logo-wrap">
                <img src="/sitecore/login/logo_new.png" alt="Sitecore logo">
            </div>
        </div>
    </div>

</body>
</html>
