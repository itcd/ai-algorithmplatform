using System;
using System.Collections.Generic;
using System.Reflection;

namespace MicroPayment.DALFactory
{
    /// <summary>
    /// 创建数据访问层实例的类
    /// </summary>
    public sealed class DataAccess
    {
        /// <summary>
        /// 从WebConfig配置节点中获取路径的值
        /// </summary>
        private static readonly string Path = System.Configuration.ConfigurationManager.AppSettings["WebDAL"];

        /// <summary>
        /// 从WebConfig配置节点中获取用户类的值
        /// </summary>
        private static readonly string UserInfoClass = System.Configuration.ConfigurationManager.AppSettings["UserInfoClass"];


        /// <summary>
        /// 创建用户访问层的实例
        /// </summary>
        /// <returns>返回IUser接口实例</returns>
        public static MicroPayment.IDAL.IUserInfo CreateUser()
        {
            string classname = Path + UserInfoClass;
            return (MicroPayment.IDAL.IUserInfo)Assembly.Load(Path).CreateInstance(classname);
        }
    }
}
