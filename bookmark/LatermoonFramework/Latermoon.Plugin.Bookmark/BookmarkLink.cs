using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Latermoon.Plugin.Bookmark
{
    /// <summary>
    /// 网址
    /// </summary>
    public class BookmarkLink
    {
        public BookmarkLink(XmlNode folderNode)
        {
            OwnerNode = folderNode;
        }

        public string this[string name]
        {
            get
            {
                XmlAttribute attribute = OwnerNode.Attributes[name];
                return attribute == null ? null : attribute.Value;
            }
        }

        /// <summary>
        /// 绑定的Xml节点
        /// </summary>
        public XmlNode OwnerNode { get; private set; }

        public string Title
        {
            get
            {
                return OwnerNode.InnerText;
            }
        }

        public string Href
        {
            get
            {
                string attribute = this["href"];
                return attribute == null ? "" : attribute;
            }
        }

        public string Tag
        {
            get
            {
                string attribute = this["tag"];
                return attribute == null ? "" : attribute;
            }
        }
    }
}
