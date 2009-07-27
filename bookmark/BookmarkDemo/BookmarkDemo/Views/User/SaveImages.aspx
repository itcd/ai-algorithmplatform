<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaveImages.aspx.cs" Inherits="Views_User_Default" AspCompat="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    
    <form id="form1" runat="server">
    <asp:Image ID="Image1" runat="server" />
    <div>
    <asp:button runat="server" text="Button" onclick="Unnamed1_Click" />
    </div><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <p>
    <asp:button runat="server" text="Button" onclick="button1_Click" ID="button1" />
    </p>
    </form>
</body>
</html>

