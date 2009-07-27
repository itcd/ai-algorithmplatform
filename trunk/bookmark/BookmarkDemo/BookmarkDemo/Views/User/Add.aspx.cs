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
            //txtTitle.Text = Request["title"];
            //txtTag.Text = Request["tag"];
            //txtHref.Text = Request["url"];
            //txtReferSite.Text = Request["from"];
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
        fields[FavorField.Username] =  txtUsername.Text;
        string txtTitle = Request["txtTitle"];
        fields[FavorField.Title] = txtTitle.Length > 250 ? txtTitle.Substring(0, 250) : txtTitle;
        fields[FavorField.Href] = txtHref.Text;
        fields[FavorField.Tag] = Request["txtTag"];
        fields["Remark"] = Request["txtRemark"];
        fields[FavorField.FavorLevel] = Request["txtFavorLevel"];
        fieldsList.Add(fields);

        BookmarkFavors Favors = new BookmarkFavors(new BookmarkUser(txtUsername.Text));

        Favors.Add(fields);

        //批量添加
        //long[] idList = Favors.Add(fieldsList.ToArray());
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        FavorsTableDataContext db = new FavorsTableDataContext();

        string href = txtHref.Text.Trim().ToLowerInvariant();

        var query = from site in db.AppraiseOfWebSite where site.Href == href select site;

        AppraiseOfWebSite candidateRow = null;

        foreach(var row in query)
        {
            candidateRow = row;
            //候选选项表中存在当前网址的记录
            GetUrlInfo = true;
            break;
        }

        if (candidateRow != null)
        {
            TagList = candidateRow.Tags.Split(';');
            TitleList = candidateRow.Titles.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            RemarkList = candidateRow.Remarks.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            txtFavorLevel.Text = candidateRow.FavorLevel.ToString();
            WebSiteImage.ImageUrl = "~/Content/images/WebSiteImages/" + candidateRow.ImageUrl.Trim() + ".jpeg";
        }
    }

    bool getUrlInfo = false;

    public bool GetUrlInfo
    {
        get { return getUrlInfo; }
        set { getUrlInfo = value; }
    }

    public string[] TagList;

    public string[] TitleList;

    public string[] RemarkList;
}
