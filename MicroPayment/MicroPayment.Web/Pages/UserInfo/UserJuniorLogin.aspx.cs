using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MicroPayment.Web.Pages.UserInfo
{
    public partial class UserJuniorLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            MicroPayment.Model.UserInfo user = new MicroPayment.Model.UserInfo();
            user.MicroUserName = txtUser.Text.Trim();
            user.MicroUserJuniorPassword = txtPass.Text.Trim();
           // user.MicroLastLoginDate = DateTime.Today;
            MicroPayment.BLL.UserInfo lg = new MicroPayment.BLL.UserInfo();
            int i = lg.IsJuniorLogin(user);
            if (i == 1)
            {
                lbMsg.Text = "<font color=green>恭喜！登陆成功</font>";
            }
            else
            {
                lbMsg.Text = "<font color=red>对不起，登陆失败</font>";
            }
        }
    }
}
