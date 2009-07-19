using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Views_Test_LINQTest : System.Web.UI.Page
{
    FavorsTableDataContext db = new FavorsTableDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        db.SubmitChanges();

        //var FavorList = db.GetTable<Favors>();

        //var query = from favor in FavorList where favor.Username == "tczyp" select favor.Href;

        //foreach (var favor in query)
        //{
        //    Response.Write(favor + "<BR/>");
        //}
    }
    protected void DataList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void FavorLevelTextBox_TextChanged(object sender, EventArgs e)
    {
        int favorID =Int32.Parse(((Label)(((DataListItem)(((TextBox)sender).Parent)).Controls[1])).Text);

        var siteToUpdate = from site in db.Favors where site.FavorID == favorID select site;

        foreach (var site in siteToUpdate)
        {
            site.FavorLevel = Int16.Parse(((TextBox)sender).Text);
        }
    }
    protected void RemarkTextBox_TextChanged(object sender, EventArgs e)
    {
        int favorID = Int32.Parse(((Label)(((DataListItem)(((TextBox)sender).Parent)).Controls[1])).Text);

        var siteToUpdate = from site in db.Favors where site.FavorID == favorID select site;

        foreach (var site in siteToUpdate)
        {
            site.Remark = ((TextBox)sender).Text;
        }
    }
}
