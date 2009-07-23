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
    //private WebBrowser _webBrowser;


    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        SaveImageList();
        SqlCommandBuilder cmdb = new SqlCommandBuilder(dataAdapter);
        dataAdapter.Update(dataTable);
    }

    //private ManualResetEvent _doneEvent;

    //private void ThreadPoolCallback(Object threadContext)
    //{
    //    var pairUrlAndIndex = (RairUrlAndEvent)threadContext;
    //    SaveImageListHelp(pairUrlAndIndex.WebBrowser, pairUrlAndIndex.Url, pairUrlAndIndex.Index);
    //}

    //[STAThread]
    //protected void SaveImageListHelp(WebBrowser _webBrowser, string url, int index)
    //{
    //    if (string.IsNullOrEmpty(url))
    //    {
    //        url = Request.Url.ToString();
    //    }

    //    bool NullDocument = false;
    //    bool ReadyStateComplete = false;        
        
    //    _webBrowser.Navigate(url);
    //    //_webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Completed);

    //    var current = System.DateTime.Now;
        
    //    //int times = 0;
    //    while (_webBrowser.ReadyState != WebBrowserReadyState.Complete)
    //    {
    //        {
    //            System.Windows.Forms.Application.DoEvents(); //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件
    //            Thread.Sleep(100);
    //            //times++;

    //            double ms = (System.DateTime.Now - current).TotalMilliseconds;

    //            if (ms > 30000)
    //                break;
    //        }
    //    }

    //    //Thread.Sleep(1000);

    //    if (_webBrowser.ReadyState == WebBrowserReadyState.Complete)
    //    {
    //        ReadyStateComplete = true;
    //    }

    //    if ((_webBrowser.Document != null) && (_webBrowser.Document.Body != null))
    //    {
    //        NullDocument = true;

    //        //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
    //        _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
    //        _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
    //        if (_webBrowser.Width > 0 && _webBrowser.Height > 0)
    //        {
    //            using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
    //            {
    //                _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
    //                string ImageUrl = @"D:\images\";
    //                ImageUrl += index.ToString();
    //                ImageUrl += ".png";
    //                bmp.Save(ImageUrl, ImageFormat.Png);
    //            }
    //        }
    //    }

    //    Response.Write(count.ToString() + "     " + url + "        " + "  ReadyStateComplete: " + ReadyStateComplete.ToString() + "  NullDocument: " + NullDocument.ToString() + "<BR/>");

    //    completeCount++;
    //    //doEvent.Set();
    //}

    class RairUrlAndEvent
    {
        int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        WebBrowser webBrowser;

        public WebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { webBrowser = value; }
        }
    }

    DataSet dataSet;
    SqlDataAdapter dataAdapter;
    DataTable dataTable;

    delegate string dGetUrlByIndex(int index);
    delegate void dSaveFileNameToDB(string fileName, int index);

    string GetUrlByIndexByDataTable(int index)
    {
        return dataTable.Rows[index]["Href"].ToString();
    }

    void SaveFileNameToDB(string fileName, int index)
    { dataTable.Rows[index]["ImageUrl"] = fileName; }

    [STAThread]
    protected void SaveImageList()
    {
        String CommandText = "SELECT top 51 *  FROM AppraiseOfWebSite";
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["bookmark"].ConnectionString.ToString();

        SqlCommand cmd = cnn.CreateCommand();
        cmd.CommandText = CommandText;

        dataAdapter = new SqlDataAdapter(cmd);
        dataSet = new DataSet();
        dataAdapter.Fill(dataSet, "AppraiseOfWebSite");

        dataTable = dataSet.Tables["AppraiseOfWebSite"];

        dGetUrlByIndex getUrlByIndex = GetUrlByIndexByDataTable;
        dSaveFileNameToDB saveFileNameToDB =SaveFileNameToDB;

        SaveImageByUrlWithBrowsers(0, 50, 10, @"D:\", getUrlByIndex, saveFileNameToDB);

        SqlCommandBuilder bldr = new SqlCommandBuilder(dataAdapter);
        //dataAdapter.Update(dataTable);

        //foreach (DataRow dr in dataTable.Rows)
        //for (count = 400; count < 401; count++)
        //{
        //    DataRow dr = dataTable.Rows[count];

        //    string url = dr["Href"].ToString();

        //    //string str2 = url;

        //    ////str1 用于Select出当前网页在数据库存储的行
        //    //string str1 = "Href = \'" + str2 + "\'";

        //    //// 对str2 进行特殊符号的替换
        //    //str2 = str2.Replace(@"\", @"_");
        //    //str2 = str2.Replace(@"/", @"_");
        //    //str2 = str2.Replace(@":", @"_");
        //    //str2 = str2.Replace(@"*", @"_");
        //    //str2 = str2.Replace(@"?", @"_");
        //    //str2 = str2.Replace(@"%", @"_");
        //    //str2 = str2.Replace(@"<", @"_");
        //    //str2 = str2.Replace(@">", @"_");
        //    //str2 = str2.Replace(@".", @"_");

        //    ////ImageUrl 是图片将被保存的路径
        //    ////ImageUrl = @"/BookmarkDemo/Content/images/";
        //    //ImageUrl = @"D:\images\";
        //    //ImageUrl += count.ToString();
        //    //ImageUrl += ".png";

        //    //DataRow[] rows = dataTable.Select(str1);
        //    //rows[0]["ImageUrl"] = ImageUrl;

        //    //int index = count - 400;

        //    //doneEvents[index] = new ManualResetEvent(false);

        //    WebBrowser _webBrowser = new WebBrowser();
        //    _webBrowser.ScriptErrorsSuppressed = false;
        //    _webBrowser.ScrollBarsEnabled = false; //不显示滚动条

        //    ThreadPool.QueueUserWorkItem(ThreadPoolCallback, new RairUrlAndEvent() {WebBrowser = _webBrowser, Index = count, Url = url });
              
        //SqlCommandBuilder bldr = new SqlCommandBuilder(dataAdapter);
        //dataAdapter.Update(dataTable);
    }

    private void SaveImageByUrlWithBrowsers(int beginIndex, int endIndex, int webBrowsersCount, string FilePath, dGetUrlByIndex getUrlByIndex, dSaveFileNameToDB saveFileNameToDB)
    {
        WebBrowser[] webBrowsers = new WebBrowser[webBrowsersCount];
        System.DateTime[] startTimes = new System.DateTime[webBrowsersCount];
        int[] webSiteIndexs = new int[webBrowsersCount];
        string[] webSiteHrefs = new string[webBrowsersCount];

        for (int j = 0; j < webBrowsers.Length; j++)
        {
            webBrowsers[j] = new WebBrowser();
            webBrowsers[j].ScriptErrorsSuppressed = true;
            webBrowsers[j].ScrollBarsEnabled = false; //不显示滚动条
        }

        int completeWebSiteCount = 0;
        int waitForBeginIndex = 0;
        //这里设置开始的编号
        int nextWebSiteIndex = beginIndex;

        //一开始给每个Browser一个任务
        for (int i = 0; i < webBrowsers.Length; i++)
        {
            //string url = dataTable.Rows[nextWebSiteIndex]["Href"].ToString();

            string url = getUrlByIndex(nextWebSiteIndex);

            startTimes[i] = System.DateTime.Now;
            webBrowsers[i].Navigate(url);
            webSiteIndexs[i] = nextWebSiteIndex;
            webSiteHrefs[i] = url;

            if (nextWebSiteIndex < endIndex)
            {
                nextWebSiteIndex++;
            }
        }

        bool isWaitForBegin = false;

        while (completeWebSiteCount < endIndex)
        {
            if (isWaitForBegin)
            {
                //string url = dataTable.Rows[nextWebSiteIndex]["Href"].ToString();
                string url = getUrlByIndex(nextWebSiteIndex);

                startTimes[waitForBeginIndex] = System.DateTime.Now;
                webBrowsers[waitForBeginIndex].Navigate(url);
                webSiteIndexs[waitForBeginIndex] = nextWebSiteIndex;
                webSiteHrefs[waitForBeginIndex] = url;
                nextWebSiteIndex++;
                isWaitForBegin = false;
            }

            for (int k = 0; k < webBrowsers.Length; k++)
            {
                var _webBrowser = webBrowsers[k];

                double ms = (System.DateTime.Now - startTimes[k]).TotalMilliseconds;

                //如果加载网页未完毕且加载未超时，则继续等待
                if (_webBrowser.ReadyState != WebBrowserReadyState.Complete && ms < 30000)
                {
                    continue;
                }

                //如果加载网页完毕或者加载超时
                bool NullDocument = false;
                bool ReadyStateComplete = false;

                if (_webBrowser.ReadyState == WebBrowserReadyState.Complete)
                {
                    ReadyStateComplete = true;
                }

                if ((_webBrowser.Document != null) && (_webBrowser.Document.Body != null))
                {
                    NullDocument = true;

                    try
                    {
                        //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
                        _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
                        _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
                        if (_webBrowser.Width > 0 && _webBrowser.Height > 0 && _webBrowser.Width < 9999 && _webBrowser.Height < 9999)
                        {
                            using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
                            {
                                _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                string ImageUrl = FilePath + webSiteIndexs[k].ToString() + ".png";
                                bmp.Save(ImageUrl, ImageFormat.Png);

                               // saveFileNameToDB(webSiteIndexs[k].ToString());
                                SaveFileNameToDB(ImageUrl, webSiteIndexs[k]);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Response.Write(webSiteHrefs[k].ToString() + "发生异常：" + e.ToString() + "<BR/>");
                        //System.Console.WriteLine(webSiteHrefs[k].ToString() + "发生异常：" + e.ToString());
                    }
                    finally
                    { }

                }

                //System.Console.WriteLine(webSiteIndexs[k].ToString() + "     " + webSiteHrefs[k] + "        " + "  ReadyStateComplete: " + ReadyStateComplete.ToString() + "  NullDocument: " + NullDocument.ToString());
                Response.Write(webSiteIndexs[k].ToString() + "     " + webSiteHrefs[k] + "        " + "  ReadyStateComplete: " + ReadyStateComplete.ToString() + "  NullDocument: " + NullDocument.ToString() + "<BR/>");

                completeWebSiteCount++;
                waitForBeginIndex = k;

                //如果列表里还有website需要处理
                if (nextWebSiteIndex < endIndex)
                {
                    isWaitForBegin = true;
                }

                break;
            }

            //如果某个Browser已经完成上一个网页，则马上给它新任务
            if (isWaitForBegin)
            {
                System.Windows.Forms.Application.DoEvents(); //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件                    
                Thread.Sleep(100);
            }
        }
    }

    //string ImageUrl = null;
    
    //public void Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    //{
    //    //lock (this)
    //    {
    //        //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
    //        _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
    //        _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
    //        using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
    //        {
    //            _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
    //            bmp.Save(ImageUrl, ImageFormat.Png);
    //        }
    //    }
    //}




    //protected void SaveImage(string url)
    //{
    //    if (string.IsNullOrEmpty(url))
    //    {
    //        url = Request.Url.ToString();
    //    }

    //    _webBrowser = new WebBrowser();
    //    _webBrowser.ScrollBarsEnabled = false; //不显示滚动条
    //    _webBrowser.Navigate(url);
    //    _webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Completed1);
        
    //    int times = 0;
    //    while (_webBrowser.ReadyState != WebBrowserReadyState.Complete && times < 5)
    //    {
    //        {
    //            //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件
    //            System.Windows.Forms.Application.DoEvents(); 
    //            Thread.Sleep(2000);
    //            times++;
    //        }
    //    }
    //}


    //protected void SaveImage()
    //{
    //    SaveImage(null);
    //}

    //public void Completed1(object sender, WebBrowserDocumentCompletedEventArgs e)
    //{
    //    //设置浏览器宽度、高度为文档宽度、高度，以便截取整个网页。
    //    _webBrowser.Width = _webBrowser.Document.Body.ScrollRectangle.Width;
    //    _webBrowser.Height = _webBrowser.Document.Body.ScrollRectangle.Height;
    //    using (Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height))
    //    {
    //        _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
    //        bmp.Save("D:\\Capture.png", ImageFormat.Png);
    //    }
    //}

}
