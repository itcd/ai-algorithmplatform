<%@ Page Language="C#" MasterPageFile="~/Views/Template/Main.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Views_Web_Login" Title="登录测试 - Bookmark" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
<div class="center">


    <p>
    登录</p>
<p>
    &nbsp;</p>
<p>
    用户名:</p>
<p>
    <asp:TextBox ID="txtUsername" runat="server">latermoon</asp:TextBox>
</p>
<p>
    密码:</p>
<p>
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password">1234</asp:TextBox>
&nbsp;输入1234</p>
<p>
    &nbsp;</p>
<p>
    <asp:Button ID="btnLogin" runat="server" onclick="btnLogin_Click" Text="登录" />
</p>
</div>
</asp:Content>

