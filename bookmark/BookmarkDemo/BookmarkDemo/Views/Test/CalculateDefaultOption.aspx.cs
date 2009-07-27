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
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data.Linq;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

public partial class Views_Test_CalculateDefaultOption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public struct FavorsRow 
    {
        public FavorsRow(string href, Int16 favorLevel, Int16 queryFrequency)
        {
            Href = href;
            FavorLevel = favorLevel;
            QueryFrequency = queryFrequency;
        }

        public string Href;
        public Int16 FavorLevel;
        public Int16 QueryFrequency;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        FavorsTableDataContext db = new FavorsTableDataContext();

        var query = from row in db.Favors group row by row.Href;

        foreach (var rowGroup in query)
        {
            List<string> tags = new List<string>();
            int count = 0;
            int sumFavorLevel = 0;
            int sumQueryFrequency = 0;

            //对每一组拥有相同网址的行进行统计
            foreach (var row in rowGroup)
            {
                tags.AddRange(row.Tag.Split(new char[] { ';', ' ',',','&','，'}));

                sumFavorLevel += (int)row.FavorLevel;
                sumQueryFrequency += (int)row.VisitCount;
                count++;
            }

            //记录Tag（被标记越多的tag排序在越前面）
            var tagGroups = from tag in tags group tag by tag;
            var tagOrderGroups = from tagGroup in tagGroups orderby tagGroup.Count<string>() descending select tagGroup;

            var totalTag = "";
            var totalTagCount = "";

            foreach (var tagGroup in tagOrderGroups)
            {
                totalTag += tagGroup.First<string>();
                totalTag += ";";

                totalTagCount += tagGroup.Count<string>();
                totalTagCount += " ";
            }

            //记录Title（被使用越多的tag排序在越前面）
            var titlesString = "";
            var titleCountString = "";
            var titleGroups = from row in rowGroup group row.Title by row.Title;
            var titleOrderGroups = from titleGroup in titleGroups orderby titleGroup.Count<string>() descending select titleGroup;

            foreach (var titleGroup in titleOrderGroups)
            {
                titlesString += titleGroup.First<string>();
                titlesString += "##";
                
                titleCountString += titleGroup.Count<string>();
                titleCountString += " ";
            }            

            //简评，备注，备忘
            var remarksString = "";
            var remarkCountString = "";
            var remarkGroups = from row in rowGroup group row.Remark by row.Remark;
            var remarkOrderGroups = from remarkGroup in remarkGroups orderby remarkGroup.Count<string>() descending select remarkGroup;

            foreach (var remarkGroup in remarkOrderGroups)
            {
                remarksString += remarkGroup.First<string>();
                remarksString += "##";

                remarkCountString += remarkGroup.Count<string>();
                remarkCountString += " ";
            }


            var newRow = new AppraiseOfWebSite();
            newRow.Href = rowGroup.First<Favors>().Href;
            newRow.Titles = titlesString;
            newRow.TitlesCount = titleCountString;
            newRow.Tags = totalTag;
            newRow.TagsCount = totalTagCount;
            newRow.Remarks = remarksString;
            newRow.RemarksCount = remarkCountString;
            newRow.FavorLevel = (short)(sumFavorLevel / count + 0.5f);
            newRow.QueryFrequency = (short)(sumQueryFrequency / count + 0.5f);

            db.AppraiseOfWebSite.InsertOnSubmit(newRow);
        }

        db.SubmitChanges();

        //DataTable dataTable = defaultOption.GetList();

        //Dictionary<string, FavorsRow> dictionary = new Dictionary<string, FavorsRow>();

        //List<FavorsRow> Rows = new List<FavorsRow>();

        //string lastHref = null;
        //int count = 0;
        //int sumFavorLevel = 0;
        //int sumQueryFrequency = 0;

        //foreach (DataRow row in dataTable.Rows)
        //{
        //    string Href = ((string)row["Href"]).ToLowerInvariant().Trim();
        //    Int16 FavorLevel = (Int16)row["FavorLevel"];
        //    Int16 QueryFrequency = (Int16)row["VisitCount"];
                        
        //    ///以下代码为了方便测试
        //    Random r = new Random();
        //    FavorLevel = (short)r.Next(5);
        //    QueryFrequency = (short)r.Next(5);
        //    ///

        //    //如果是同一个网址
        //    if ((lastHref != null)&&(String.Compare(lastHref, Href) == 0))
        //    {
        //        sumFavorLevel += FavorLevel;
        //        sumQueryFrequency += QueryFrequency;
        //        count++;
        //    }
        //    //如果是一个新的网址
        //    else
        //    {
        //        //如果Rows不为空已经添加过网址
        //        if (lastHref != null)
        //        {
        //            Rows.Add(new FavorsRow(lastHref, (short)(sumFavorLevel / count + 0.5f), (short)(sumQueryFrequency / count + 0.5f)));
        //            sumFavorLevel = 0;
        //            sumQueryFrequency = 0;
        //        }

        //        lastHref = Href;
        //        count = 1;
        //        sumFavorLevel = FavorLevel;
        //        sumQueryFrequency = QueryFrequency;
        //    }            
        //}

        ////添加最后一个网址
        //Rows.Add(new FavorsRow(lastHref, (short)(sumFavorLevel / count + 0.5f), (short)(sumQueryFrequency / count + 0.5f)));
        
        //List<Hashtable> fieldsList = new List<Hashtable>();
        //foreach (FavorsRow row in Rows)
        //{
        //    //获取一条数据库空行
        //    Hashtable fields = new Hashtable();
        //    fields.Add("FavorLevel", row.FavorLevel);
        //    fields.Add("QueryFrequency", row.QueryFrequency);
        //    fields.Add("Href", row.Href);
        //    fieldsList.Add(fields);
        //}
        ////批量添加
        //defaultOption.Add(fieldsList.ToArray());
        ////long[] idList = Favors.Add(fieldsList.ToArray());
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        FavorsTableDataContext db = new FavorsTableDataContext();

        using (TransactionScope scope = new TransactionScope())
        {
            var webSiteToDelete = from site in db.AppraiseOfWebSite select site;

            foreach (var site in webSiteToDelete)
            {
                db.AppraiseOfWebSite.DeleteOnSubmit(site);
            }

            db.SubmitChanges();

            scope.Complete();
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        var db = new FavorsTableDataContext();

        var rowsToUpdate = from row in db.Favors select row;

        foreach (var row in rowsToUpdate)
        {
            row.Href = row.Href.ToLowerInvariant().Trim();
        }

        db.SubmitChanges();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        FavorsTableDataContext db = new FavorsTableDataContext();

        using (TransactionScope scope = new TransactionScope())
        {
            var webSiteToUpdate = from site in db.AppraiseOfWebSite select site;

            int index = 0;

            foreach (var site in webSiteToUpdate)
            {
                site.SiteID = index;
                index++;
            }

            db.SubmitChanges();

            scope.Complete();
        }
    }
}
