<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LINQTest.aspx.cs" Inherits="Views_Test_LINQTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        .style1
        {
            color: #FF6600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 990px; width: 877px">
    <asp:Button ID="Button1" runat="server" Text="批量更新" onclick="Button1_Click" />
        <br />
        <br />
        <asp:DataList ID="DataList1" runat="server" DataSourceID="UserFavorWebSite" 
            onselectedindexchanged="DataList1_SelectedIndexChanged" 
            style="text-align: center; margin-right: 92px;" Height="873px" Width="1173px" 
            BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
            CellPadding="2" ForeColor="Black">
            <FooterStyle BackColor="Tan" />
            <ItemTemplate>
                FavorID:
                <asp:Label ID="FavorIDLabel" runat="server" Text='<%# Eval("FavorID") %>' 
                    style="font-weight: 700; color: #003366" />
                &nbsp;&nbsp; <span class="style1">Title</span>: &nbsp;
                <asp:HyperLink ID="HyperLink1" runat="server" href = '<%# Eval("Href") %>'>&#39;<%# Eval("Title") %>&#39;</asp:HyperLink>
                &nbsp; Tag:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%>
                <asp:TextBox ID="TagTextBox" runat="server" Text='<%# Eval("Tag") %>' 
                    Height="16px" Width="187px"></asp:TextBox>
                &nbsp; FavorLevel:
                <asp:TextBox ID="FavorLevelTextBox" runat="server" 
                    Text='<%# Eval("FavorLevel") %>' Height="16px" Width="23px" 
                    ontextchanged="FavorLevelTextBox_TextChanged" />
                &nbsp;VisitCount:
                <asp:Label ID="FavorLevelLabel" runat="server" 
                    Text='<%# Eval("FavorLevel") %>' />
                &nbsp;&nbsp; Remark:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%>
                <asp:TextBox ID="RemarkTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("Remark") %>' Width="187px" 
                    ontextchanged="RemarkTextBox_TextChanged"></asp:TextBox>
                <br />
            </ItemTemplate>
            <AlternatingItemTemplate>
                FavorID:
                <asp:Label ID="FavorIDLabel" runat="server" 
                    Text='<%# Eval("FavorID") %>' style="font-weight: 700; color: #003366" />
                &nbsp;&nbsp;
                <span class="style1">Title</span>: &nbsp;
                <asp:HyperLink ID="HyperLink1" runat="server" href='<%# Eval("Href") %>'>&#39;<%# Eval("Title") %>&#39;</asp:HyperLink>
                &nbsp; Tag:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%>
                <asp:TextBox ID="TagTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("Tag") %>' Width="187px"></asp:TextBox>
                &nbsp; FavorLevel:
                <asp:TextBox ID="FavorLevelTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("FavorLevel") %>' Width="23px" 
                    ontextchanged="FavorLevelTextBox_TextChanged" />
                &nbsp;VisitCount:
                <asp:Label ID="FavorLevelLabel" runat="server" 
                    Text='<%# Eval("FavorLevel") %>' />
                &nbsp;&nbsp; Remark:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%>
                <asp:TextBox ID="RemarkTextBox" runat="server" Height="16px" 
                    ontextchanged="RemarkTextBox_TextChanged" Text='<%# Eval("Remark") %>' 
                    Width="187px"></asp:TextBox>
            </AlternatingItemTemplate>
            <AlternatingItemStyle BackColor="PaleGoldenrod" />
            <EditItemTemplate>
                FavorID:
                <asp:Label ID="FavorIDLabel" runat="server" Text='<%# Eval("FavorID") %>' />
                &nbsp;&nbsp; Title: &nbsp;
                <asp:HyperLink ID="HyperLink1" runat="server" href='<%# Eval("Href") %>'>&#39;<%# Eval("Title") %>&#39;</asp:HyperLink>
                &nbsp; Tag:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%>
                <asp:TextBox ID="TagTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("Tag") %>' Width="187px"></asp:TextBox>
                &nbsp; FavorLevel:
                <asp:TextBox ID="FavorLevelTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("FavorLevel") %>' Width="23px" />
                &nbsp;VisitCount:
                <asp:Label ID="VisitCountLabel" runat="server" 
                    Text='<%# Eval("VisitCount") %>' />
                &nbsp;&nbsp;
            </EditItemTemplate>
            <SelectedItemStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <HeaderTemplate>
                FavorID: &nbsp;&nbsp; Title: &nbsp; &nbsp; Tag:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%> &nbsp; 
                FavorLevel: &nbsp;VisitCount: &nbsp;&nbsp;
                <br />
            </HeaderTemplate>
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <SelectedItemTemplate>
                FavorID:
                <asp:Label ID="FavorIDLabel" runat="server" Text='<%# Eval("FavorID") %>' />
                &nbsp;&nbsp; Title: &nbsp;
                <asp:HyperLink ID="HyperLink1" runat="server" href='<%# Eval("Href") %>'>&#39;<%# Eval("Title") %>&#39;</asp:HyperLink>
                &nbsp; Tag:
                <%--<asp:Label ID="TagLabel" runat="server" Text='<%# Eval("Tag") %>' />--%>
                <asp:TextBox ID="TagTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("Tag") %>' Width="187px"></asp:TextBox>
                &nbsp; FavorLevel:
                <asp:TextBox ID="FavorLevelTextBox" runat="server" Height="16px" 
                    Text='<%# Eval("FavorLevel") %>' Width="23px" />
                &nbsp;VisitCount:
                <asp:Label ID="VisitCountLabel" runat="server" 
                    Text='<%# Eval("VisitCount") %>' />
                &nbsp;&nbsp;
            </SelectedItemTemplate>
        </asp:DataList>
        <br />
        <asp:LinqDataSource ID="UserFavorWebSite" runat="server" 
            ContextTypeName="FavorsTableDataContext" 
            Select="new (FavorID, Privacy, VisitCount, SubmitTime, LastVisit, FavorLevel, Title, Href, Tag, Remark)" 
            TableName="Favors" Where="Username == @Username">
            <WhereParameters>
                <asp:Parameter DefaultValue="tczyp" Name="Username" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
        <br />
        <br />
    </div>
    
    </form>
</body>
</html>
