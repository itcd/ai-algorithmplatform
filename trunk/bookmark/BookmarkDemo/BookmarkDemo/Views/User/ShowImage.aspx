<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowImage.aspx.cs" Inherits="Views_User_ShowImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:DataList ID="DataList1" runat="server">
        <ItemTemplate>
            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImageUrl") %>'  Height="300" Width="300" />
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("Href") %>' 
                Text='<%# Eval("Titles") %>'></asp:HyperLink>
        </ItemTemplate>
    </asp:DataList>
   
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <asp:Label ID="Label3" runat="server" Text="/"></asp:Label>
        <asp:Label ID="Label2" runat="server"></asp:Label>
        <asp:Button ID="prevButton" runat="server" Text="上一页" 
            onclick="prevButton_Click" />
        <asp:Button ID="nextButton" runat="server" onclick="nextButton_Click" 
            Text="下一页" />
    </asp:Panel>
    </form>
</body>
</html>
