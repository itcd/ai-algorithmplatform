using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using Bookmark.Model;
using Latermoon.Plugin.Bookmark;

public partial class Views_Test_ImportFavorites : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!BookmarkContext.IsAuthenticated())
        {
            FormsAuthentication.RedirectToLoginPage("sa=1234");
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fileFavorites.HasFile)
        {
            //创建收藏夹备份解释器
            BookmarkDocument doc = new BookmarkDocument();
            doc.Load(fileFavorites.FileContent);
            //获取当前用户
            BookmarkUser user = BookmarkContext.GetCurrentUser();
            //导入用户收藏夹
            user.Import(doc);
        }
    }
}
