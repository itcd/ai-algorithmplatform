using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Views_Template_Pop : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Alert(string msg)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("alert('{0}')", msg), true);
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(new FormRewriteTextWriter(writer));
    }

    /// <summary>
    /// 让ASP.NET服务器控件在客户端呈现自定义的格式
    /// </summary>
    public class FormRewriteTextWriter : HtmlTextWriter
    {
        public FormRewriteTextWriter(TextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer is HtmlTextWriter ? (writer as HtmlTextWriter).InnerWriter : writer;
        }

        public override void WriteAttribute(string name, string value, bool fEncode)
        {
            if (name == "action")
            {
                value = "";// HttpContext.Current.Request.RawUrl;
            }
            base.WriteAttribute(name, value, fEncode);
        }
    }

}
