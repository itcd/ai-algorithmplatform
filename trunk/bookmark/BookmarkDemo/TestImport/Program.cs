using System;
using System.Collections.Generic;
using System.Text;
using Latermoon.Plugin.Bookmark;
using System.Xml;

namespace TestImport
{
    class Program
    {
        static void Main(string[] args)
        {
            BookmarkDocument bookmark = new BookmarkDocument();
            bookmark.Load(@"D:\\bookmarks.html");
            bookmark.Save(@"d:\b.xml");
            Console.WriteLine("分析保存完毕...");
            XmlNodeList folders = bookmark.DocumentElement.SelectNodes("//folder");
            XmlNodeList links = bookmark.DocumentElement.SelectNodes("//a");
            Console.WriteLine("共{0}个文件夹, {1}条网址", folders.Count, links.Count);
            Console.WriteLine("其中根目录有{0}个文件夹, {1}条网址", bookmark.RootFolder.ChildFolders.Count, bookmark.RootFolder.Links.Count);
            Console.ReadKey();
            //枚举一级文件夹
            foreach (BookmarkFolder folder in bookmark.RootFolder.ChildFolders)
            {
                Console.WriteLine("文件夹: {0}, 包含 {1} 条网址和 {2} 个子文件夹.", folder.Name, folder.Links.Count, folder.ChildFolders.Count);
            }
            Console.ReadKey();
            //枚举全部网址
            foreach (BookmarkLink link in bookmark.RootFolder.AllChildLinks)
            {
                Console.WriteLine("网址: {0} (Tag: {1})", link.Title, link.Tag);
            }
            Console.ReadKey();
        }
    }
}
