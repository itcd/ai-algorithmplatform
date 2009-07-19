<%@ Page Language="C#" MasterPageFile="~/Views/Template/Main.Master" AutoEventWireup="true" CodeFile="ImportFavorites.aspx.cs" Inherits="Views_Test_ImportFavorites" Title="导入IE收藏" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <p>
    导入IE收藏:</p>
<p>
    &nbsp;</p>
<p>
    选择文件:
</p>
<p>
    <asp:FileUpload ID="fileFavorites" runat="server" Width="357px" />
&nbsp;<asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click" 
        Text="上传" />
</p>
</asp:Content>

