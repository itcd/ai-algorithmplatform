using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Latermoon.Web
{
    /// <summary>
    /// 提供一组方法获取当前的请求信息
    /// </summary>
    public static class RequestUtility
    {
        /// <summary>
        /// 获取Url或表单参数的字符串值
        /// </summary>
        /// <param name="name">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string name)
        {
            return GetString(name, string.Empty);
        }

        /// <summary>
        /// 获取Url或表单参数的字符串值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(string name, string defaultValue)
        {
            string value = HttpContext.Current.Request[name];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// 获取Url或表单参数的16位整型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short GetInt16(string name, short defaultValue)
        {
            string value = HttpContext.Current.Request[name];
            try
            {
                return string.IsNullOrEmpty(value) ? defaultValue : Convert.ToInt16(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取Url或表单参数的整型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt32(string name, int defaultValue)
        {
            string value = HttpContext.Current.Request[name];
            try
            {
                return string.IsNullOrEmpty(value) ? defaultValue : Convert.ToInt32(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取Url或表单参数的浮点值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float GetFloat(string name, float defaultValue)
        {
            string value = HttpContext.Current.Request[name];
            try
            {
                return string.IsNullOrEmpty(value) ? defaultValue : Convert.ToSingle(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取Url或表单参数的长整形值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetInt64(string name, long defaultValue)
        {
            string value = HttpContext.Current.Request[name];
            try
            {
                return string.IsNullOrEmpty(value) ? defaultValue : Convert.ToInt64(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 判断当前请求是否包含指定参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        public static bool Contains(string name)
        {
            return HttpContext.Current.Request[name] != null;
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result  = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result) || !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
            {
                return "127.0.0.1";
            }
            return result;
        }
    }
}
