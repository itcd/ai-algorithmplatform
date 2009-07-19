<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AjaxTest.aspx.cs" Inherits="Views_Test_AjaxTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>测试Ajax</title>
    <style type="text/css">
    .title{ color:Green;}
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
            $.getJSON("/BookmarkDemo/Service/Favor/FavorLevelUpdateHandler.ashx",
                {"favorid":favorId, "level":level},
                function(json){
                    span.html(json["msg"]).show();
                    window.setTimeout(function(){span.hide();}, 1000);
                });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>使用Ajax方法可以不刷新和点击更新页面，修改后台数据</div>
    <div>
            <asp:DataList ID="DataList1" runat="server" DataSourceID="UserFavorWebSite">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" href = '<%# Eval("Href") %>'><%# Eval("Title") %></asp:HyperLink>
                    <br />
                    FavorLevel:
                    <!--这里使用html元素的onblur(失去焦点时触发)调用Ajax请求-->
                    <input type="text" originallevel='<%# Eval("FavorLevel") %>' value='<%# Eval("FavorLevel") %>' onblur="updateFavorLevel(this, <%# Eval("FavorID") %>)" />
                    <!--下面的span用于显示状态-->
                    <span id="label_<%# Eval("FavorID") %>">..</span>
                    <div style="border-bottom:solid 1px #ccc; margin-top:4px" ></div>
                </ItemTemplate>
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
    </div>
    </form>
</body>
</html>
