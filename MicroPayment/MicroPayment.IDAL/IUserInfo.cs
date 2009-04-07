using System;
using System.Collections.Generic;
using System.Text;
using MicroPayment.Model;

namespace MicroPayment.IDAL
{
    /// <summary>
    /// 用户基础信息类接口
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// 用户初始登陆是否成功
        /// </summary>
        /// <param name="userInfo">用户实体类</param>
        /// <returns>返回int数值,大于0表示成功</returns>
        int IsJuniorLogin(UserInfo userInfo);
    }
}
