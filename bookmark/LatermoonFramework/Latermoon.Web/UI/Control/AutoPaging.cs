using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Latermoon.Web.UI.Control
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:AutoPaging runat=server></{0}:AutoPaging>")]
    public class AutoPaging : WebControl
    {
        public AutoPaging()
            : base("div")
        {
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        [Bindable(true)]
        [Category("分页")]
        [DefaultValue(0)]
        [Localizable(true)]
        public int PageIndex
        {
            get { return (int)ViewState["PageIndex"]; }
            set { ViewState["PageIndex"] = value; }
        }

        [Bindable(true)]
        [Category("分页")]
        [DefaultValue(0)]
        [Localizable(true)]
        public int PageCount
        {
            get { return (int)ViewState["PageCount"]; }
            set { ViewState["PageCount"] = value; }
        }

        /// <summary>
        /// 当前的URL
        /// </summary>
        [Bindable(true)]
        [Category("分页")]
        [DefaultValue("{path}{query}")]
        [Localizable(true)]
        public string LinkTemplate
        {
            get { return (string)ViewState["LinkTemplate"]; }
            set { ViewState["LinkTemplate"] = value; }
        }

        /// <summary>
        /// 当前的URL生成器
        /// </summary>
        public UrlBuilder Url { get; set; }

        protected override void RenderContents(HtmlTextWriter output)
        {
            // 输出上一页
            if (PageIndex > 0)
            {
                Url["page"] = (PageIndex - 1).ToString();
                output.Write("<a href='{0}'>上一页</a>", Url.ToString(LinkTemplate));
            }
            // 输出中间页码
            for (int i = 0; i < PageCount; i++)
            {
                if (i != 0 && i != PageCount - 1)
                {
                    //跳过
                    if (PageIndex - i > 5 || i - PageIndex > 5)
                    {
                        if (PageIndex - 5 > 1 && i == 1)
                        {
                            output.Write("<a>...</a>");
                        }
                        if (i - PageIndex > 5 && i == PageCount - 2)
                        {
                            output.Write("<a>...</a>");
                        }
                        continue;
                    }
                }
                Url["page"] = (i).ToString();
                output.Write("<a {2} href='{0}'>{1}</a>", Url.ToString(LinkTemplate), i + 1, PageIndex == i ? "class='current'" : "");
            }
            // 输出下一页
            if (PageIndex < PageCount - 1)
            {
                Url["page"] = (PageIndex + 1).ToString();
                output.Write("<a href='{0}'>下一页</a>", Url.ToString(LinkTemplate));
            }
        }
    }
}
