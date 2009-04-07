using System;
using System.Collections.Generic;
using System.Text;
using MicroPayment.IDAL;
using MicroPayment.SQLServer;
using System.Data.SqlClient;
using System.Data;

namespace MicroPayment.SQLServer
{
    /// <summary>
    ///  用户数据库操作类
    /// </summary>
    class UserInfo : IUserInfo
    {
        #region 用户基础信息常量参数

        private const string PARAM_USERID = "@MicroUserID";
        private const string PARAM_USERNAME = "@MicroUserName";
        private const string PARAM_USERREALNAME = "@MicroUserRealName";
        private const string PARAM_USERPETNAME = "@MicroUserPetName";
        private const string PARAM_USERMAIL = "@MicroUserMail";
        private const string PARAM_USERJUNIORPWD = "@MicroUserJuniorPassword";
        private const string PARAM_USERSENIORPWD = "@MicroUserSeniorPassword";
        private const string PARAM_USERAUTHORITY = "@MicroUserAuthority";
        private const string PARAM_CREATEDATE = "@MicroCreateDate";
        private const string PARAM_LASTLOGINIP = "@MicroLastLoginIP";
        private const string PARAM_LASTLOGINDATE = "@MicroLastLoginDate";
        private const string PARAM_USERADDRESS = "@MicroUserAddress";
        private const string PARAM_USERTEL = "@MicroUserTelephone";
        private const string PARAM_USERMOBILE = "@MicroUserMobile";
        private const string PARAM_USERLEVEL = "@MicroUserLevel";
        private const string PARAM_USERSEX = "@MicroUserSex";
        private const string PARAM_USERBIRTH = "@MicroUserBirthDay";
        private const string PARAM_CONSTELLATION = "@MicroConstellation";
        private const string PARAM_ISMARRY = "@IsMarry";
        private const string PARAM_COUNTRY = "@Country";
        private const string PARAM_PROVINCE = "@Province";
        private const string PARAM_CITY = "@City";

        #endregion

        #region 用户基础信息常量属性

        /// <summary>
        /// 用户ID
        /// </summary>
        private static SqlParameter UserID
        {
            get
            {
                return new SqlParameter(PARAM_USERID, SqlDbType.Int);
            }
        }

        /// <summary>
        /// 用户登陆账号
        /// </summary>
        private static SqlParameter UserName
        {
            get
            {
                return new SqlParameter(PARAM_USERNAME, SqlDbType.VarChar, 50);
            }
        }

        /// <summary>
        /// 用户初始密码
        /// </summary>
        private static SqlParameter UserJuniorPassword
        {
            get
            {
                return new SqlParameter(PARAM_USERJUNIORPWD, SqlDbType.VarChar, 50);
            }
        }

        #endregion

        #region  MicroUser SQL操作语句

        /// <summary>
        /// 判断用户是否登陆成功
        /// </summary>
        private const string SQL_MICROUSER_ISJUNIORLOGIN = "select * from MicroUser where MicroUserName=@MicrouserName and MicroUserJuniorPassword=@MicroUserJuniorPassword";
      
        #endregion

        #region IUserInfo 成员

        /// <summary>
        /// 用户初始登陆SQL操作
        /// </summary>
        /// <param name="userInfo">用户基础信息类</param>
        /// <returns>返回数值:1为登陆成功,0为登陆失败</returns>
        public int IsJuniorLogin(MicroPayment.Model.UserInfo userInfo)
        {
            SqlParameter[] isLoginParameters = GetIsLoginParameters();
            int loginState;
            isLoginParameters[0].Value = userInfo.MicroUserName;
            isLoginParameters[1].Value = userInfo.MicroUserJuniorPassword;

            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.Con, CommandType.Text, SQL_MICROUSER_ISJUNIORLOGIN, isLoginParameters))
            {
                loginState = sdr.Read() ? 1 : 0;
                sdr.Dispose();
            }
            return loginState;
        }

        #endregion

        #region SQL参数设置

        /// <summary>
        /// 获取IsLogin方法SQL参数缓存
        /// </summary>
        /// <returns>参数数组</returns>
        private static SqlParameter[] GetIsLoginParameters()
        {
            SqlParameter[] parms = SQLHelper.GetCachedParameters(SQL_MICROUSER_ISJUNIORLOGIN);

            if (parms == null)
            {
                parms = new SqlParameter[]{UserName,
                    UserJuniorPassword
                  
                };
                SQLHelper.CacheParameters(SQL_MICROUSER_ISJUNIORLOGIN, parms);
            }

            return parms;
        }

        #endregion


    }
}