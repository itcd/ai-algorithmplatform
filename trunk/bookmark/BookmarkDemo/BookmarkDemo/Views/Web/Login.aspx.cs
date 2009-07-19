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
using System.Data.Linq;
using System.Linq;

public partial class Views_Web_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtUsername.Text == "latermoon" && txtPassword.Text == "1234")
        {
            FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, true);
        }
    }
}
