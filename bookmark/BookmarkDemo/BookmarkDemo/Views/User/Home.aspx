<%@ Page Language="C#" MasterPageFile="~/Views/Template/Main.Master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Views_User_Home" Title="的收藏列表" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .linktd
        {
            padding: 4px 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="Server">
    <div class="center">
        <!--导航-->
        <div style="padding: 8px 0 8px 0; border-bottom: solid 1px #ccc; margin-bottom: 8px;">
            <span style="float: right">
                <asp:Label ID="labPageCount" runat="server" Text="0"></asp:Label>
                &nbsp;&nbsp;
                <asp:HyperLink ID="linkPrevious" runat="server">上一页</asp:HyperLink>
                &nbsp;&nbsp;
                <asp:HyperLink ID="linkNext" runat="server">下一页</asp:HyperLink>
            </span>
            <asp:Label ID="labUserID" runat="server" Text=""></asp:Label>的收藏列表
        </div>
        <!--GridView收藏列表-->
        <asp:GridView ID="gridFavors" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" Width="100%" AutoGenerateColumns="False" OnSelectedIndexChanged="gridFavors_SelectedIndexChanged">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="标题" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="500px"
                    ItemStyle-CssClass="linktd">
                    <ItemTemplate>
                        <a target="_blank" href="<%# DataBinder.Eval(Container.DataItem,"Href")%>">
                            <%# DataBinder.Eval(Container.DataItem,"Title")%></a>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="500px"></HeaderStyle>
                    <ItemStyle CssClass="linktd"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Tag" HeaderText="Tag" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-CssClass="linktd">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle CssClass="linktd"></ItemStyle>
                </asp:BoundField>
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
    </div>
</asp:Content>
