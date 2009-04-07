<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserJuniorLogin.aspx.cs"
    Inherits="MicroPayment.Web.Pages.UserInfo.UserJuniorLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                帐号：
            </td>
            <td align="left">
                <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                密码：
            </td>
            <td>
                <asp:TextBox ID="txtPass" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 40px" align="center">
                <asp:Button ID="btnLogin" runat="server" Text="登 陆" CssClass="btnLogin" OnClick="btnLogin_Click"
                    Width="86px" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lbMsg" runat="server"></asp:Label>
    </form>
</body>
</html>
