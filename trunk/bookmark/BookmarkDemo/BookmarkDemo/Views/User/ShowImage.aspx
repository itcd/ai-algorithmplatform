<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowImage.aspx.cs" Inherits="Views_User_ShowImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:DataList ID="DataList1" runat="server" DataKeyField="Href" 
        DataSourceID="SqlDataSource2">
        <ItemTemplate>
            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImageUrl") %>'  Height="300" Width="300" />
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("Href") %>' 
                Text='<%# Eval("Titles") %>'></asp:HyperLink>
        </ItemTemplate>
    </asp:DataList>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:bookmark1ConnectionString %>" 
        SelectCommand="SELECT [Href], [Titles], [ImageUrl] FROM [AppraiseOfWebSite]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    <asp:Button ID="Button1" runat="server" Text="Button" />
    </form>
</body>
</html>
