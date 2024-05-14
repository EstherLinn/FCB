<%@ Page Language="C#" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Text" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server" enableviewstate="false">
    <title>sso login</title>
<script runat="server">

    protected override void OnLoad(EventArgs e)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        var headers = context.Request.Headers;        
        foreach (string field in headers)
        {
            Response.Write("<p>" + field + "：" + headers[field] + "</p>");
        }
    }

</script>
</head>
<body>
    <form id="form1" runat="server" enableviewstate="false">
    </form>
</body>
</html>
