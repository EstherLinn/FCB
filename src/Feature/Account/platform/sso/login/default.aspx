<%@ Page Language="C#" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server" enableviewstate="false">
    <title>sso login</title>
    <script runat="server">

        protected override void OnLoad(EventArgs e)
        {
            var context = HttpContext.Current;
            var headers = context.Request.Headers;
            foreach (string field in headers)
            {
                this.Response.Write("<p>" + field + "：" + headers[field] + "</p>");
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server" enableviewstate="false">
    </form>
</body>
</html>