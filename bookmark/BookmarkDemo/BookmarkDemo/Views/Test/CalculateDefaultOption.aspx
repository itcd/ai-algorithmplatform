<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalculateDefaultOption.aspx.cs" Inherits="Views_Test_CalculateDefaultOption" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        把Favors表里面的所有大写网址变成小写<asp:Button ID="Button3" runat="server" 
            onclick="Button3_Click" Text="开始" />
    
        <br />
        把AppraiseOfWebSIte表的SiteID赋值<asp:Button ID="Button4" runat="server" 
            Text="开始" onclick="Button4_Click" />
    
        <br />
    
        生成默认选项数据库
    
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="开始" />
    
        <br />
        清空默认选项数据库 
        <asp:Button ID="Button2" runat="server" Height="23px" Text="开始" 
            Width="41px" onclick="Button2_Click" />
    
    </div>
    </form>
</body>
</html>
