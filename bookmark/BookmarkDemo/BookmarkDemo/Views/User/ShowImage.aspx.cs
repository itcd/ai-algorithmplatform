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
using System.Data.SqlClient;

public partial class Views_User_ShowImage : System.Web.UI.Page
{
    private DataTable dataTable;
    private int countPerPage = 10;
    private int pageIndex;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            countPerPage = 10;
            pageIndex = 1;
            dataTable = GetTable(countPerPage, pageIndex);
            DataList1.DataSource = dataTable;
            DataList1.DataBind();
            
            Label2.Text =(GetCount() / countPerPage + 1).ToString();
            Label1.Text = pageIndex.ToString();
            prevButton.Visible = false;
  
        }
    }

    

    protected DataTable GetTable(int countPerPage, int pageIndex)
    {
        DataTable dataTable = new DataTable();
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["bookmark"].ConnectionString.ToString();

        SqlCommand cmd = cnn.CreateCommand();
        string CommandText = string.Format("SELECT TOP {0} * FROM AppraiseOfWebSite WHERE  [Href] NOT IN ( SELECT TOP {1} [Href] FROM AppraiseOfWebSite ORDER BY Href) ORDER BY Href",
                                                countPerPage, countPerPage * pageIndex);
        cmd.CommandText = CommandText;
        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        cnn.Open();
        dataAdapter.Fill(dataTable);
        return dataTable;
    }

    protected int GetCount()
    {
        
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["bookmark"].ConnectionString.ToString();

        SqlCommand cmd = cnn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM AppraiseOfWebSite";
        cnn.Open();
        int count = (int)cmd.ExecuteScalar();
        cnn.Close();
        return count;
        
    }


    protected void nextButton_Click(object sender, EventArgs e)
    {
        pageIndex = Int32.Parse(Label1.Text.ToString());
        pageIndex++;
        Label1.Text = pageIndex.ToString();
        //dataTable.Clear();
        dataTable = GetTable(countPerPage, pageIndex);
        DataList1.DataSource = dataTable;
        DataList1.DataBind();
        if (pageIndex != 1)
            prevButton.Visible = true;
        else
            prevButton.Visible = false;
        if (pageIndex != Int32.Parse(Label2.Text.ToString()))
            nextButton.Visible = true;
        else
            nextButton.Visible = false;
       
    }
protected void  prevButton_Click(object sender, EventArgs e)
    {
        pageIndex = Int32.Parse(Label1.Text.ToString());
        pageIndex--;
        Label1.Text = pageIndex.ToString();
        //dataTable.Clear();
        dataTable = GetTable(countPerPage, pageIndex);
        DataList1.DataSource = dataTable;
        DataList1.DataBind();
        if (pageIndex != 1)
            prevButton.Visible = true;
        else
            prevButton.Visible = false;
        if (pageIndex != Int32.Parse(Label2.Text.ToString()))
            nextButton.Visible = true;
        else
            nextButton.Visible = false;
       }
}
