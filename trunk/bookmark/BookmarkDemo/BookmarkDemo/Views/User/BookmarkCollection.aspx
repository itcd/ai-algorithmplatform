<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookmarkCollection.aspx.cs" Inherits="Views_User_BookmarkCollection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        .style1
        {
            height: 262px;
        }
        .style2
        {
            width: 222px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="labUserID" runat="server" Text="Label"></asp:Label>
    
        <asp:Label ID="TagTest" runat="server" Text="Label"></asp:Label>
    
    </div>
    <table style="width: 100%; height: 298px;">
        <tr>
            <td class="style2" rowspan="2" valign="top" width="30%">
                <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" Height="500px" 
                    onselectedindexchanged="ListBox1_SelectedIndexChanged" Width="200px"></asp:ListBox>
            </td>
            <td class="style1" valign="top">
                <asp:GridView ID="gridFavors" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" Width="100%" AutoGenerateColumns="False" >
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="标题" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="500px"
                    ItemStyle-CssClass="linktd">
                    <ItemTemplate>
                        <a target="_blank" href="<%# DataBinder.Eval(Container.DataItem,"Href")%>">
                            <%# DataBinder.Eval(Container.DataItem,"Title")%></a>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="500px"></HeaderStyle>
                </asp:TemplateField>
                
                <asp:BoundField HeaderText="评价等级" DataField="FavorLevel" />
                <asp:BoundField DataField="VisitCount" HeaderText="访问频率" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
            </td>
        </tr>
        <tr>
            <td valign="baseline" dir="rtl">
                <asp:HyperLink ID="linkPrevious" runat="server">上一页</asp:HyperLink>
                <asp:Label ID="labPageCount" runat="server"></asp:Label>
                <asp:HyperLink ID="linkNext" runat="server">下一页</asp:HyperLink>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
