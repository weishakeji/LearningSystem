<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <%
        string p = this.Request.Url.PathAndQuery;
        this.Response.Redirect(p.Replace("aspx","ashx"));
         %>
</body>
</html>
