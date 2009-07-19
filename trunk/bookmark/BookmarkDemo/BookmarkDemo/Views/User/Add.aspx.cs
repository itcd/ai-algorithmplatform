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
using Bookmark.Entity;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data.Linq;
using System.Linq;
using System.Transactions;

public partial class Views_User_Add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        panLogin.Visible = !BookmarkContext.IsAuthenticated();

        if (!this.IsPostBack)
        {
            labUserID.Text = UserID;
            //收藏信息
            txtTitle.Text = Request["title"];
            txtTag.Text = Request["tag"];
            txtHref.Text = Request["url"];
            txtReferSite.Text = Request["from"];
        }
    }

    /// <summary>
    /// 获取当前用户名
    /// </summary>
    public string UserID
    {
        get { return BookmarkContext.GetUsername(); }
    }

    private BookmarkUser _BookmarkUser;
    /// <summary>
    /// 获取当前用户对象
    /// </summary>
    public BookmarkUser BookmarkUser
    {
        get { return _BookmarkUser == null ? new BookmarkUser(UserID) : _BookmarkUser; }
    }

    /// <summary>
    /// 弹出Javascript提示框
    /// </summary>
    /// <param name="msg"></param>
    public void Alert(string msg)
    {
        (Master as Views_Template_Pop).Alert(msg);
    }

    /// <summary>
    /// 添加收藏
    /// </summary>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!BookmarkContext.IsAuthenticated())
        {
            if (txtUsername.Text != "latermoon" || txtPassword.Text != "1234")
            {
                Alert("密码错误");
                return;
            }
            else
            {
                //在这里登陆
            }
        }
        //在这里添加收藏

        List<Hashtable> fieldsList = new List<Hashtable>();

        //获取一条数据库空行
        Hashtable fields = FavorField.EmptyRow();
        fields[FavorField.Username] = txtUsername.Text;
        fields[FavorField.Title] = txtTitle.Text.Length > 250 ? txtTitle.Text.Substring(0, 250) : txtTitle.Text;
        fields[FavorField.Href] = txtHref.Text;
        fields[FavorField.Tag] = txtTag.Text;
        fieldsList.Add(fields);

        BookmarkFavors Favors = new BookmarkFavors(new BookmarkUser(txtUsername.Text));

        Favors.Add(fields);

        //批量添加
        //long[] idList = Favors.Add(fieldsList.ToArray());
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        FavorsTableDataContext db = new FavorsTableDataContext();

        var query = from site in db.AppraiseOfWebSite where site.Href == txtHref.Text select site;

        foreach(var site in query)
        {
            txtTag.Text = site.FavorLevel.ToString();
            break;
        }
    }
}
