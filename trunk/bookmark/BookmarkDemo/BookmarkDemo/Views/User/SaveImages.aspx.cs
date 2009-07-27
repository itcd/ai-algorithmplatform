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
    String str = "ABCDEF";

    public String Str
    {
        get { return str; }
        set { str = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        String CommandText = "SELECT * FROM AppraiseOfWebSite";
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["bookmark"].ConnectionString.ToString();

        SqlCommand cmd = cnn.CreateCommand();
        cmd.CommandText = CommandText;

        var dataAdapter = new SqlDataAdapter(cmd);
        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet, "AppraiseOfWebSite");

        dataTable = dataSet.Tables["AppraiseOfWebSite"];

        //SaveImageByUrlWithBrowsers(0, dataTable.Rows.Count, 10, @"D:\WebSiteImages\", @"D:/WebSiteSmallImages/", GetUrlByIndexByDataTable, SaveFileNameToDB);

        SaveImageByUrlWithBrowsers(0, dataTable.Rows.Count, 10, Server.MapPath("~/Content/images/WebSiteImages/"), Server.MapPath("~/Content/images/WebSiteSmallImages/"), GetUrlByIndexByDataTable, SaveFileNameToDB);

        SqlCommandBuilder cmdb = new SqlCommandBuilder(dataAdapter);
        dataAdapter.Update(dataTable);
        
    }

    protected void button1_Click(object sender, EventArgs e)
    {
        Thumb("D:/WebSiteImages3/323.png", "D:/1.jpeg");
    }

    DataTable dataTable;

    delegate string dGetUrlByIndex(int index);
    delegate void dSaveFileNameToDB(string fileName, int index);

    string GetUrlByIndexByDataTable(int index)
    {
        return dataTable.Rows[index]["Href"].ToString();
    }

    void SaveFileNameToDB(string fileName, int index)
    { dataTable.Rows[index]["ImageUrl"] = fileName; }

    
    private Bitmap GetBrowserBitmapByUrl(string url, int maxLoadingTime)
    {
        WebBrowser webBrowser = new WebBrowser();
        webBrowser.ScriptErrorsSuppressed = true;
        webBrowser.ScrollBarsEnabled = false; //不显示滚动条

        var startTimes = System.DateTime.Now;
        webBrowser.Navigate(url, false);

        double ms = 0;

        //如果加载网页未完毕且加载未超时，则继续等待
        while (webBrowser.ReadyState != WebBrowserReadyState.Complete && ms < maxLoadingTime)
        {
            System.Windows.Forms.Application.DoEvents(); //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件                    
            Thread.Sleep(100);

            ms = (System.DateTime.Now - startTimes).TotalMilliseconds;
        }

        bool NullDocument = false;
        bool ReadyStateComplete = false;

        return GetBrowserBitmap(webBrowser, out NullDocument, out ReadyStateComplete);
    }

    private void SaveImageByUrlWithBrowsers(int beginIndex, int endIndex, int webBrowsersCount, string FilePath, string smallImageFilePath, dGetUrlByIndex getUrlByIndex, dSaveFileNameToDB saveFileNameToDB)
    {
        int MaxLoadingTime = 30000;

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
            webBrowsers[i].Navigate(url, false);
            webSiteIndexs[i] = nextWebSiteIndex;
            webSiteHrefs[i] = url;

            if (nextWebSiteIndex < endIndex)
            {
                nextWebSiteIndex++;
            }
            else
            {
                break;
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
                //如果该浏览器资源还没被释放
                if (webBrowsers[k] != null)
                {
                    var _webBrowser = webBrowsers[k];

                    double ms = (System.DateTime.Now - startTimes[k]).TotalMilliseconds;

                    //如果加载网页未完毕且加载未超时，则继续等待
                    if (_webBrowser.ReadyState != WebBrowserReadyState.Complete && ms < MaxLoadingTime)
                    {
                        continue;
                    }

                    bool saveIsSuccess = false;

                    bool NullDocument;
                    bool ReadyStateComplete;

                    var BrowserBitmap = GetBrowserBitmap(_webBrowser, out NullDocument, out ReadyStateComplete);
                    
                    if(BrowserBitmap != null)
                    {
                        try
                        {
                            //处理图片的准备工作
                            System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                            var ps = new EncoderParameters(1);
                            ps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                            var CodecInfo = GetCodecInfo("image/jpeg");

                            //g.DrawImage(srcBitmap, destRectangle/*显示图像的大小*/, srcX, srcY/*从此X，Y坐标开始截取*/, srcBitmap.Width / 4/*截取宽*/, srcBitmap.Height / 4/*截取高*/, GraphicsUnit.Pixel);

                            //在这里对图片进行进一步的处理，比如去掉白边和生成缩略图，调整大小等
                            //把图片缩小到宽度等于width
                            int width = 500;
                            int height = (int)((width / (float)BrowserBitmap.Width) * BrowserBitmap.Height);
                            System.Drawing.Image thumbnailImage = BrowserBitmap.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);

                            //BrowserBitmap.Save(ImageUrl, ImageFormat.Jpeg);

                            string ImageUrl = FilePath + webSiteIndexs[k].ToString() + ".jpeg";

                            //保存缩略图
                            thumbnailImage.Save(ImageUrl,ImageFormat.Jpeg);

                            //缩小并裁减缩略图，生成更小的缩略图
                            int smallImageWidth = 200;
                            int smallImageHeight = (int)((smallImageWidth / (float)BrowserBitmap.Width) * BrowserBitmap.Height);
                            System.Drawing.Image tempImage = BrowserBitmap.GetThumbnailImage(smallImageWidth, smallImageHeight, myCallback, IntPtr.Zero);

                            smallImageHeight = 200;

                            Bitmap smallBmp = new Bitmap(smallImageWidth, smallImageHeight);
                            Graphics g = Graphics.FromImage(smallBmp);
                            Rectangle destRectangle = new Rectangle(0, 0, smallImageWidth, smallImageHeight);
                            g.DrawImage(tempImage, destRectangle/*显示图像的大小*/, 0, 0/*从此X，Y坐标开始截取*/, smallImageWidth/*截取宽*/, smallImageHeight/*截取高*/, GraphicsUnit.Pixel);

                            smallBmp.Save(smallImageFilePath + webSiteIndexs[k].ToString() + ".jpeg", ImageFormat.Jpeg);

                            saveFileNameToDB(webSiteIndexs[k].ToString(), webSiteIndexs[k]);

                            thumbnailImage.Dispose();
                            BrowserBitmap.Dispose();
                            tempImage.Dispose();
                            smallBmp.Dispose();
                            g.Dispose();

                            saveIsSuccess = true;
                        }
                        catch (Exception e)
                        { }
                        finally
                        { }
                    }
                    
                    Response.Write(webSiteIndexs[k].ToString() + "     " + webSiteHrefs[k] + "        " + "  ReadyStateComplete: " + ReadyStateComplete.ToString() + "  NullDocument: " + NullDocument.ToString() + "saveSuccess: " + saveIsSuccess.ToString() + "<BR/>");

                    completeWebSiteCount++;
                    waitForBeginIndex = k;

                    //如果列表里还有website需要处理就马上让该浏览器去进行处理
                    if (nextWebSiteIndex < endIndex)
                    {
                        isWaitForBegin = true;
                        break;
                    }
                    //否则释放改浏览器资源
                    else
                    {
                        webBrowsers[k] = null;
                    }
                }
            }

            //如果某个Browser已经完成上一个网页，则马上给它新任务
            if (isWaitForBegin)
            {
                System.Windows.Forms.Application.DoEvents(); //避免假死，若去掉则可能无法触发 DocumentCompleted 事件。??处理所有在队列中的消息， 就是在等待DocumentComPleted事件                    
                Thread.Sleep(100);
            }
        }
    }

    private Bitmap GetBrowserBitmap(WebBrowser _webBrowser, out bool NullDocument, out bool ReadyStateComplete)
    {
        //如果加载网页完毕或者加载超时则进行截图
        NullDocument = false;
        ReadyStateComplete = false;

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
                    Bitmap bmp = new Bitmap(_webBrowser.Width, _webBrowser.Height);

                    {
                        _webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                        return bmp;
                    }
                }
            }
            catch (Exception e)
            {
                Response.Write("发生异常：" + e.ToString() + "<BR/>");
                //System.Console.WriteLine(webSiteHrefs[k].ToString() + "发生异常：" + e.ToString());
            }
            finally
            { }
        }

        return null;
    }

    /// <summary>
    /// 取消操作回调
    /// </summary>
    /// <returns></returns>
    private bool ThumbnailCallback()
    {
        return false;
    }

    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="imgPath"></param>
    /// <param name="ThumbPath"></param>
    private void Thumb(string imgPath, string ThumbPath)
    {
        System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
        System.Drawing.Image img1 = System.Drawing.Image.FromFile(imgPath);//通过文件构造
        //生成缩略图
        System.Drawing.Image myThumbnail = img1.GetThumbnailImage(350, 200, myCallback, IntPtr.Zero);
        //myThumbnail.Save(ThumbPath);//保存缩略图

        KiSaveAsJPEG(myThumbnail, ThumbPath, 100);
    }

    /// <summary>
    /// 保存JPG时用
    /// </summary>
    /// <param name="mimeType"></param>
    /// <returns>得到指定mimeType的ImageCodecInfo</returns>
    private static ImageCodecInfo GetCodecInfo(string mimeType)
    {
        ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
        foreach (ImageCodecInfo ici in CodecInfo)
        {
            if (ici.MimeType == mimeType) return ici;
        }
        return null;
    }


    /// <summary>
    /// 保存为JPEG格式，支持压缩质量选项
    /// </summary>
    /// <param name="bmp"></param>
    /// <param name="FileName"></param>
    /// <param name="Qty"></param>
    /// <returns></returns>
    public static bool KiSaveAsJPEG(System.Drawing.Image image, string FileName, int Qty)
    {
        try
        {
            EncoderParameter p;
            EncoderParameters ps;

            ps = new EncoderParameters(1);

            p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
            ps.Param[0] = p;

            image.Save(FileName, GetCodecInfo("image/jpeg"), ps);

            return true;
        }
        catch
        {
            return false;
        }

    }
}
