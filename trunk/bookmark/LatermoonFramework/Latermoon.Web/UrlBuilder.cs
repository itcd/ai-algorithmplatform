using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace Latermoon.Web
{
    /// <summary>
    /// 为统一资源标识符 (URI) 提供自定义构造函数
    /// 并加强对QueryString的操作
    /// </summary>
    public class UrlBuilder : UriBuilder
    {
        public UrlBuilder()
        {
            QueryString = new NameValueCollection();
        }

        /// <summary>
        /// 用指定的URL地址初始化实例
        /// </summary>
        /// <param name="uri"></param>
        public UrlBuilder(string uri)
            : base(uri)
        {
            QueryString = new NameValueCollection();
            UpdateQueryString(base.Query);
        }

        /// <summary>
        /// 用指定的Uri初始化实例
        /// </summary>
        /// <param name="uri"></param>
        public UrlBuilder(Uri uri)
            : base(uri)
        {
            QueryString = new NameValueCollection();
            UpdateQueryString(base.Query);
        }

        /// <summary>
        /// 获取查询信息集合
        /// </summary>
        public NameValueCollection QueryString
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取或设置查询信息
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public string this[string name]
        {
            get { return QueryString[name]; }
            set { QueryString[name] = value; }
        }

        /// <summary>
        /// 将查询字符串更新到QueryString集合
        /// </summary>
        /// <param name="query"></param>
        private void UpdateQueryString(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return;
            }
            if (query.StartsWith("?"))
            {
                query = query.Substring(1);
            }
            string[] items = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string nv in items)
            {
                string[] pair = nv.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                QueryString[pair[0]] = pair.Length > 1 ? HttpUtility.UrlDecode(pair[1]) : null;
            }
        }

        /// <summary>
        /// 获取或设置 URI 中包括的任何查询信息。
        /// </summary>
        public new string Query
        {
            get
            {
                if (QueryString.AllKeys.Length > 0)
                {
                    StringBuilder buf = new StringBuilder();
                    buf.Append("?");
                    for (int i = 0; i < QueryString.AllKeys.Length; i++)
                    {
                        string name = QueryString.AllKeys[i];
                        buf.Append(name);
                        buf.Append("=");
                        buf.Append(HttpUtility.UrlEncode(QueryString[name]));
                        if (i < QueryString.AllKeys.Length - 1)
                        {
                            buf.Append("&");
                        }
                    }
                    return buf.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                // 清空原键值对
                QueryString.Clear();
                UpdateQueryString(value);
            }
        }

        /// <summary>
        /// 返回完整的URL字符串。
        /// </summary>
        /// <returns>完整的URL字符串</returns>
        public override string ToString()
        {
            // 替换Query部分
            string query = this.Query;
            base.Query = query.Length > 0 ? query.Substring(1) : string.Empty;
            return base.ToString();
        }

        /// <summary>
        /// 格式化URL字符串
        /// {scheme}://{username}:{password}@{host}:{port}{path}{query}{fragment}
        /// </summary>
        /// <param name="format">{scheme}://{username}:{password}@{host}:{port}{path}{query}{fragment}</param>
        /// <returns></returns>
        public string ToString(string template)
        {
            // 替换模板元素
            return template
                .Replace("{scheme}", this.Scheme)
                .Replace("{username}", this.UserName)
                .Replace("{password}", this.Password)
                .Replace("{host}", this.Host)
                .Replace("{port}", this.Port.ToString())
                .Replace("{path}", this.Path)
                .Replace("{query}", this.Query)
                .Replace("{fragment}", this.Fragment);
        }
    }
}
