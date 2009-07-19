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
using Bookmark.Model;
using Bookmark;
using System.Collections.Generic;

public partial class Views_User_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次页面响应        
        if (!this.IsPostBack)
        {
            //绑定数据
            gridFavors.DataSource = FavorList;
            gridFavors.DataBind();
            //页面初始化
            this.Title = string.Format("{0}的收藏列表 - bookmark", UserID);
            labUserID.Text = UserID;
            labPageCount.Text = string.Format("共{0}页, 当前第{1}页", PageCount, PageIndex + 1);
            linkPrevious.Visible = PageIndex > 0;
            if (linkPrevious.Visible)
            {
                linkPrevious.NavigateUrl = string.Format("~/{0}/Home.aspx?page={1}", UserID, PageIndex - 1);
            }
            linkNext.Visible = PageIndex < PageCount - 1;
            if (linkNext.Visible)
            {
                linkNext.NavigateUrl = string.Format("~/{0}/Home.aspx?page={1}", UserID, PageIndex + 1);
            }
        }
    }

    /// <summary>
    /// 获取当前用户名
    /// </summary>
    public string UserID
    {
        get { return Request["userid"]; }
    }

    private BookmarkUser _BookmarkUser;
    /// <summary>
    /// 获取当前用户对象
    /// </summary>
    public BookmarkUser BookmarkUser
    {
        //get { return _BookmarkUser == null ? new BookmarkUser(UserID) : _BookmarkUser; }
        get { return _BookmarkUser == null ? new BookmarkUser("latermoon") : _BookmarkUser; }
    }

    /// <summary>
    /// 获取当前访问权限
    /// </summary>
    public Privacy Privacy
    {
        get
        {
            // 已登录且用户名与页面所有者一致
            if (BookmarkContext.IsAuthenticated() && BookmarkContext.GetUsername() == UserID)
            {
                return Privacy.PRIVATE;
            }
            else
            {
                return Privacy.PUBLIC;
            }
        }
    }

    /// <summary>
    /// 获取当前排序方式
    /// 默认降序
    /// </summary>
    public OrderBy OrderBy
    {
        get
        {
            string order = Request["orderby"];
            if (string.IsNullOrEmpty(order) || order == "desc")
            {
                return OrderBy.DESC;
            }
            else
            {
                return OrderBy.ASC;
            }
        }
    }

    private DataTable _FavorList;
    /// <summary>
    /// 获取当前收藏列表
    /// </summary>
    public DataTable FavorList
    {
        get
        {
            if (_FavorList == null)
            {
                _FavorList = BookmarkUser.Favors.GetList(Privacy, PageIndex, CountPerPage, OrderBy);
            }
            return _FavorList;
        }
    }

    private int _FavorCount = -1;
    /// <summary>
    /// 获取当前列表收藏总数
    /// </summary>
    public int FavorCount
    {
        get
        {
            if (_FavorCount == -1)
            {
                _FavorCount = BookmarkUser.Favors.GetCount(Privacy);
            }
            return _FavorCount;
        }
    }

    private int _PageIndex = -1;
    /// <summary>
    /// 获取当前页码
    /// 索引从0开始
    /// </summary>
    public int PageIndex
    {
        get
        {
            if (_PageIndex != -1)
            {
                return _PageIndex;
            }
            if (!string.IsNullOrEmpty(Request["page"]))
            {
                try
                {
                    return Convert.ToInt32(Request["page"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 获取当前每页要显示的收藏数
    /// </summary>
    public int CountPerPage
    {
        get { return 8; }
    }

    /// <summary>
    /// 获取当前页数
    /// </summary>
    public int PageCount
    {
        get
        {
            return FavorCount / CountPerPage + (FavorCount % CountPerPage > 0 ? 1 : 0);
        }
    }
    protected void gridFavors_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
