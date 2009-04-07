using System;
using System.Collections.Generic;
using System.Text;
using MicroPayment.IDAL;

namespace MicroPayment.BLL
{
    public class UserInfo
    {
        /// <summary>
        /// 利用反射生成实例
        /// </summary>
        private static readonly IUserInfo userInfoInstance = MicroPayment.DALFactory.DataAccess.CreateUser();

        /// <summary>
        /// 用户是否登陆：1是，0否；
        /// </summary>
        /// <param name="user">用户实体类</param>
        /// <returns>返回结果;用户是否登陆--1是，0否</returns>
        public int IsJuniorLogin(MicroPayment.Model.UserInfo user)
        {
            return userInfoInstance.IsJuniorLogin(user);
        }
    }
}
