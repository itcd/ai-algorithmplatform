<%@ Page Language="C#" MasterPageFile="~/Views/Template/Pop.Master" AutoEventWireup="true" CodeFile="Add.aspx.cs" Inherits="Views_User_Add" Title="添加收藏" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .tableAdd{ width:100%}
        .tableAdd .tdleft{ padding:5px 8px; width:150px}
        .input{ padding:0.4em; border:solid 1px #ccc}
    </style>
    
    <!--使用jQuery库-->
    <script src="../../Content/js/jquery-1.3.2.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        //使用Ajax更新FavorLevel
        function updateFavorLevel(input, favorId){
            //找到对应的span用于显示状态
            var span = $("#label_"+favorId);
            //旧的FavorLevel
            var originallevel = $(input).attr("originallevel");
            //新FavorLevel
            var level = $(input).val();
            if(level.length==0){
                span.html("请输入数值").show();
                //1秒后隐藏
                window.setTimeout(function(){span.hide();}, 1000);
                return;
            }
            //如果相等则不作修改
            if(originallevel == level){
                span.html("没有修改").show();
                //1秒后隐藏
                window.setTimeout(function(){span.hide();}, 1000);
                return;
            }
            //在input上保存最近的FavorLevel值
            $(input).attr("originallevel", level);
            span.html("正在修改...").show();
            window.setTimeout(function(){span.hide();}, 1000);
            //向服务器发送后台请求，并获取服务端生成的JSON
            $.getJSON("/BookmarkDemo/Service/Favor/AddWebSiteHandler.ashx",
                {"favorid":favorId, "level":level},
                function(json){
                    span.html(json["msg"]).show();
                    window.setTimeout(function(){span.hide();}, 1000);
                });
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
    <asp:Panel ID="panLogin" runat="server">
        <table cellpadding="0" cellspacing="0" class="tableAdd">
        <tr>
            <td class="tdleft">
                用户名：</td>
            <td>
                <asp:TextBox ID="txtUsername" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                密码：</td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" Width="401px" CssClass="input" 
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
                网址标题：</td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                标签(TAG)：</td>
            <td>
            
                <asp:TextBox ID="txtTag" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                URL：</td>
            <td>
                <asp:TextBox ID="txtHref" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="检测" />
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                来源页面：</td>
            <td>
                <asp:TextBox ID="txtReferSite" runat="server" Width="401px" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="tdleft">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click" Text="添加收藏" />
            </td>
        </tr>
    </table>
    </div>
</div>
</asp:Content>
