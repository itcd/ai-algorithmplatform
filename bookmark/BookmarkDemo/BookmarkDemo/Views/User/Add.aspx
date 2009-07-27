<%@ Page Language="C#" MasterPageFile="~/Views/Template/Pop.Master" AutoEventWireup="true" CodeFile="Add.aspx.cs" Inherits="Views_User_Add" Title="添加收藏" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .tableAdd{ width:100%;
            height: 100px;
        }
        .tableAdd .tdleft{ padding:5px 8px; width:150px}
        .input{ padding:0.4em; border:solid 1px #ccc}
        .style1
        {
            width: 410px;
        }
        .style2
        {
            width: 279px;
        }
    </style>
    
    <!--使用jQuery库-->
    
    <script type="text/javascript">
        
     function addTag(tag){
  //获取要填充的文本框
  fm = document.getElementById("txtTag");
  //将tag增加到哪个文本框
  fm.value += tag;
  fm.value += ";";
        }
        
            function addTitle(title){
  //获取要填充的文本框
  fm = document.getElementById("txtTitle");
  //将tag增加到哪个文本框
  fm.value += title;
  fm.value += ";";
        }
        
            function addRemark(remark){
  //获取要填充的文本框
  fm = document.getElementById("txtRemark");
  //将tag增加到哪个文本框
  fm.value += remark;
  fm.value += ";";
        }
   
        
    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" Runat="Server">
    
<div class="center">
    <!--导航-->
    <div style="padding:8px 0 8px 0; border-bottom:solid 1px #ccc; margin-bottom:8px;">
        <span style="float:right">
            <%=DateTime.Now.ToString("yyyy-MM-dd") %>
        </span>
        添加到<asp:Label ID="labUserID" runat="server" Text=""></asp:Label>的收藏列表
    </div>
    <!--登录-->
    <asp:Panel ID="panLogin" runat="server" Height="132px">
        <table cellpadding="0" cellspacing="0" class="tableAdd">
        <tr>
            <td class="tdleft">
                用户名：</td>
            <td class="style2">
                <asp:TextBox ID="txtUsername" runat="server" Width="195px" CssClass="input"></asp:TextBox>
            </td>
            <td rowspan="2">
                <asp:Image ID="WebSiteImage" runat="server" Height="107px" Width="201px" />
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                密码：</td>
            <td class="style2">
                <asp:TextBox ID="txtPassword" runat="server" Width="193px" CssClass="input" 
                    TextMode="Password" ></asp:TextBox>
            </td>
        </tr>
        </table>
    </asp:Panel>
    <!--填写信息-->
    <div>
    
        <table cellpadding="0" cellspacing="0" class="tableAdd">
        <tr>
            <td class="tdleft">
                网址(URL)：</td>
            <td class="style1">
                <asp:TextBox ID="txtHref" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
                </td>
            <td>
            
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="检测" />
            
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                网址标题：</td>
            <td class="style1">
                <%--<asp:TextBox ID="txtTitle" runat="server" Width="401px" CssClass="input"></asp:TextBox>--%>
                <input type = "text" id = "txtTitle" name = "txtTitle" class =" input" style="width:401px;"/>
            </td>
            <td>
            <asp:Panel ID="Panel2" runat="server" Height="27px">
            <%
                if (this.GetUrlInfo)
                {
                    foreach (var title in TitleList)
                    {
            %>
            
                <a href="javascript:addTitle('<%=title %>')"><%=title%></a>  
            <%
                    }
                }
            %>
            </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                标签(TAG)：</td>
            <td class="style1">
            
                <%--<asp:TextBox ID="txtTag" runat="server" Width="401px" CssClass="input"></asp:TextBox>--%>
                <input type = "text" id = "txtTag" name = "txtTag" class =" input" style="width:401px;"/>
            </td>
            <td>
            <asp:Panel ID="TitlePanel" runat="server" Height="27px">
            <%
                if (this.GetUrlInfo)
                {            
                    foreach(var tag in TagList)
                    {
            %>
            
                <a href="javascript:addTag('<%=tag %>')"><%=tag %></a>  
            <%
                    }
                }
            %>
            </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                个人备注：</td>
            <td class="style1">
                <%--<asp:TextBox ID="txtReferSite" runat="server" Width="401px" CssClass="input"></asp:TextBox>--%>
                <input type = "text" id = "txtRemark" name = "txtRemark" class =" input" style="width:401px;"/>
            </td>
            <td>
            <asp:Panel ID="Panel1" runat="server" Height="27px">
            <%
                if (this.GetUrlInfo)
                {
                    foreach (var remark in RemarkList)
                    {
            %>
            
                <a href="javascript:addRemark('<%=remark %>')"><%=remark%></a>  
            <%
                    }
                }
            %>
            </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                评价等级：</td>
            <td class="style1">
            <asp:TextBox ID="txtFavorLevel" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                &nbsp;</td>
            <td class="style1">
                <asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click" Text="添加收藏" />
            </td>
        </tr>
    </table>
    </div>
</div>
</asp:Content>
