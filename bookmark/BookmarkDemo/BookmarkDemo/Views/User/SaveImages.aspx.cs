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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;



public partial class Views_User_Default : System.Web.UI.Page
{
    private WebBrowser _webBrowser;


    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        SaveImageList();
    }

    protected void SaveImageListHelp(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            url = Request.Url.ToString();
        }

        
        _webBrowser.ScrollBarsEnabled = false; //不显示滚动条
        _webBrowser.Navigate(url);
        //_webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Completed);

        var current = System.DateTime.Now;
        
        int times = 0;
        while (_webBrowser.ReadyState != WebBrowserReadyState.Complete)
        {

            {

                System.Windows.Forms.Application.DoEvents(); //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件
                Thread.Sleep(10);
                times++;

                double ms = (System.DateTime.Now - current).TotalMilliseconds;

                if (ms > 20000)
                    break;
            }
        }

        //Thread.Sleep(1000);

        if (_webBrowser.ReadyState == WebBrowserReadyState.Complete)
        {
            ReadyStateComplete = true;
        }

        if ((_webBrowser.Document != null) && (_webBrowser.Document.Body != null))
        {
            NullDocument = true;

            //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
            _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
            _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
            if (_webBrowser.Width > 0 && _webBrowser.Height > 0)
            {
                using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
                {
                    _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(ImageUrl, ImageFormat.Png);
                }
            }
        }

    }


    DataSet dataSet;
    SqlDataAdapter dataAdapter;
    DataTable dataTable;
    protected void SaveImageList()
    {
        String CommandText = "SELECT top 501 *  FROM AppraiseOfWebSite";
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["bookmark"].ConnectionString.ToString();

        SqlCommand cmd = cnn.CreateCommand();
        cmd.CommandText = CommandText;

        dataAdapter = new SqlDataAdapter(cmd);
        dataSet = new DataSet();
        dataAdapter.Fill(dataSet, "AppraiseOfWebSite");

        dataTable = dataSet.Tables["AppraiseOfWebSite"];

        ImageUrl = null;

        _webBrowser = new WebBrowser();
        _webBrowser.ScriptErrorsSuppressed = false;

        //foreach (DataRow dr in dataTable.Rows)
        for (count = 400; count < 500; count++)
        {
            DataRow dr = dataTable.Rows[count];

            string url = dr["Href"].ToString();

            string str2 = url;

            //str1 用于Select出当前网页在数据库存储的行
            string str1 = "Href = \'" + str2 + "\'";

            // 对str2 进行特殊符号的替换
            str2 = str2.Replace(@"\", @"_");
            str2 = str2.Replace(@"/", @"_");
            str2 = str2.Replace(@":", @"_");
            str2 = str2.Replace(@"*", @"_");
            str2 = str2.Replace(@"?", @"_");
            str2 = str2.Replace(@"%", @"_");
            str2 = str2.Replace(@"<", @"_");
            str2 = str2.Replace(@">", @"_");
            str2 = str2.Replace(@".", @"_");

            //ImageUrl 是图片将被保存的路径
            //ImageUrl = @"/BookmarkDemo/Content/images/";
            ImageUrl = @"D:\images\";
            ImageUrl += count.ToString();
            ImageUrl += ".png";

            //DataRow[] rows = dataTable.Select(str1);
            //rows[0]["ImageUrl"] = ImageUrl;

            SaveImageListHelp(url);

            Response.Write(count.ToString() + "     " + url + "        " + "  ReadyStateComplete: " + ReadyStateComplete.ToString() + "  NullDocument: " + NullDocument.ToString() + "<BR/>");

            NullDocument = false;
            ReadyStateComplete = false;
        }

        SqlCommandBuilder bldr = new SqlCommandBuilder(dataAdapter);
        dataAdapter.Update(dataTable);
    }

    string ImageUrl = null;
    int count = 0;
    bool NullDocument = false;
    bool ReadyStateComplete = false;
    
    public void Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        //lock (this)
        {
            //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
            _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
            _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
            using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
            {
                _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(ImageUrl, ImageFormat.Png);
            }
        }
    }




    protected void SaveImage(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            url = Request.Url.ToString();
        }

        _webBrowser = new WebBrowser();
        _webBrowser.ScrollBarsEnabled = false; //不显示滚动条
        _webBrowser.Navigate(url);
        _webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Completed1);
        
        int times = 0;
        while (_webBrowser.ReadyState != WebBrowserReadyState.Complete && times < 5)
        {
            {
                //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件
                System.Windows.Forms.Application.DoEvents(); 
                Thread.Sleep(2000);
                times++;
            }
        }
    }


    protected void SaveImage()
    {
        SaveImage(null);
    }

    public void Completed1(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
        _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
        _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
        using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
        {
            _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            bmp.Save("D:\\Capture.png", ImageFormat.Png);
        }
    }

}
