using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Bookmark.Model;

/// <summary>
/// 当前上下文
/// </summary>
public class BookmarkContext
{
    /// <summary>
    /// 是否已验证(登录)
    /// </summary>
    /// <returns></returns>
    public static bool IsAuthenticated()
    {
        return HttpContext.Current.User.Identity.IsAuthenticated;
    }

    /// <summary>
    /// 获取登录用户名
    /// </summary>
    /// <returns></returns>
    public static string GetUsername()
    {
        return HttpContext.Current.User.Identity.Name;
    }

    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <returns></returns>
    public static BookmarkUser GetCurrentUser()
    {
        return IsAuthenticated() ? new BookmarkUser(GetUsername()) : null;
    }
}
