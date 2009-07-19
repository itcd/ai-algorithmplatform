using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Bookmark.Entity
{
    /// <summary>
    /// 表示一条网址收藏的数据行
    /// 使用Hashtable+static string的方法在原型开发时提高灵活性
    /// </summary>
    public class FavorField
    {
        public static readonly string FavorID = "FavorID";
        public static readonly string Username = "Username";
        public static readonly string Title = "Title";
        public static readonly string Href = "Href";
        public static readonly string Tag = "Tag";
        public static readonly string ReferSite = "ReferSite";
        public static readonly string SubmitTime = "SubmitTime";
        public static readonly string FavorLevel = "FavorLevel";
        public static readonly string LastModified = "LastModified";
        public static readonly string Privacy = "Privacy";
        public static readonly string VisitCount = "VisitCount";
        public static readonly string LastVisit = "LastVisit";

        /// <summary>
        /// 返回一条空数据行
        /// 必填字段: Username, Href
        /// </summary>
        /// <returns></returns>
        public static Hashtable EmptyRow()
        {
            Hashtable fields = new Hashtable();
            //fields.Add(FavorID, null);
            fields.Add(Username, null);
            fields.Add(Title, "");
            fields.Add(Href, null);
            fields.Add(Tag, "");
            fields.Add(ReferSite, "");
            fields.Add(SubmitTime, DateTime.Now.ToString());
            fields.Add(FavorLevel, 0);
            fields.Add(LastModified, DateTime.Now.ToString());
            fields.Add(Privacy, 0);
            fields.Add(VisitCount, 0);
            fields.Add(LastVisit, DateTime.Now.ToString());
            return fields;
        }
    }
}
