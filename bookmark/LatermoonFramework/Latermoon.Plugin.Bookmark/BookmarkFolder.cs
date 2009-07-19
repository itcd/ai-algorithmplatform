using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Latermoon.Plugin.Bookmark
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public class BookmarkFolder
    {
        public BookmarkFolder(XmlNode folderNode)
        {
            OwnerNode = folderNode;
        }

        /// <summary>
        /// 绑定的Xml节点
        /// </summary>
        public XmlNode OwnerNode { get; private set; }

        public string this[string name]
        {
            get
            {
                XmlAttribute attribute = OwnerNode.Attributes[name];
                return attribute == null ? null : attribute.Value;
            }
        }

        public string Name
        {
            get
            {
                string attribute = this["name"];
                return attribute == null ? "" : attribute;
            }
        }

        /// <summary>
        /// 获取子文件夹
        /// </summary>
        public List<BookmarkFolder> ChildFolders
        {
            get
            {
                List<BookmarkFolder> childFolders = new List<BookmarkFolder>();
                XmlNodeList list = OwnerNode.SelectNodes("folder");
                foreach (XmlNode node in list)
                {
                    BookmarkFolder folder = new BookmarkFolder(node);
                    childFolders.Add(folder);
                }
                return childFolders;
            }
        }

        /// <summary>
        /// 获取当前文件夹下的网址
        /// </summary>
        public List<BookmarkLink> Links
        {
            get
            {
                List<BookmarkLink> links = new List<BookmarkLink>();
                XmlNodeList list = OwnerNode.SelectNodes("a");
                foreach (XmlNode node in list)
                {
                    BookmarkLink link = new BookmarkLink(node);
                    links.Add(link);
                }
                return links;
            }
        }

        /// <summary>
        /// 全部子目录的网址
        /// </summary>
        public List<BookmarkLink> AllChildLinks
        {
            get
            {
                List<BookmarkLink> links = new List<BookmarkLink>();
                XmlNodeList list = OwnerNode.SelectNodes("//a");
                foreach (XmlNode node in list)
                {
                    BookmarkLink link = new BookmarkLink(node);
                    links.Add(link);
                }
                return links;
            }
        }
    }
}
