using System;
using System.Collections.Generic;
using System.Text;
using Latermoon.Plugin.Bookmark;
using System.Collections;
using Bookmark.Entity;

namespace Bookmark.Model
{
    /// <summary>
    /// 用户类
    /// </summary>
    public class BookmarkUser
    {
        public BookmarkUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username");
            }
            Username = username;
        }

        public string Username { get; private set; }

        private BookmarkFavors _Favors;
        public BookmarkFavors Favors
        {
            get
            {
                return _Favors == null ? (_Favors = new BookmarkFavors(this)) : _Favors;
            }
        }

        /// <summary>
        /// 导入收藏夹
        /// </summary>
        /// <param name="document"></param>
        public void Import(BookmarkDocument document)
        {
            List<Hashtable> fieldsList = new List<Hashtable>();
            List<BookmarkLink> links = document.RootFolder.AllChildLinks;
            foreach (BookmarkLink link in links)
            {
                //获取一条数据库空行
                Hashtable fields = FavorField.EmptyRow();
                fields[FavorField.Username] = Username;
                fields[FavorField.Title] = link.Title.Length > 250 ? link.Title.Substring(0, 250) : link.Title;
                fields[FavorField.Href] = link.Href;
                fields[FavorField.Tag] = link.Tag;
                fieldsList.Add(fields);
            }
            //批量添加
            long[] idList = Favors.Add(fieldsList.ToArray());
            Console.WriteLine("Insert ID Count: {0}", idList.Length);
        }
    }
}
