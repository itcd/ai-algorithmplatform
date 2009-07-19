using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace Latermoon.Web
{
    /// <summary>
    /// 提供一组方法获取文件的ContentType
    /// </summary>
    public static class ContentType
    {
        /// <summary>
        /// 二进制文件的ContentType
        /// </summary>
        public const string BinaryContentType = "application/octet-stream";

        /// <summary>
        /// 存放预设ContentType配对
        /// </summary>
        private static IDictionary<string, string> map;

        static ContentType()
        {
            map = new Dictionary<string, string>();
            map.Add(".jpg", "image/jpeg");
            map.Add(".jpeg", "image/jpeg");
            map.Add(".gif", "image/gif");
            map.Add(".bmp", "image/bmp");
            map.Add(".png", "image/png");
            map.Add(".txt", "text/plain");
            map.Add(".xls", "application/vnd.ms-excel");
            map.Add(".zip", "application/zip");
            map.Add(".doc", "application/msword");
            map.Add(".pdf", "application/pdf");
            map.Add(".torrent", "application/x-bittorrent");
            map.Add(".swf", "application/x-shockwave-flash");
            map.Add(".mp3", "audio/mpeg");
            map.Add(".wma", "audio/x-ms-wma");
            map.Add(".wav", "audio/x-wav");
            map.Add(".css", "text/css");
            map.Add(".html", "text/html");
            map.Add(".htm", "text/html");
            map.Add(".js", "text/javascript");
            map.Add(".xml", "text/xml");
            map.Add(".mpeg", "video/mpeg");
            map.Add(".mpg", "video/mpeg");
            map.Add(".tar", "application/x-tar");

        }
        /// <summary>
        /// 根据文件后缀返回ContentType
        /// </summary>
        /// <param name="extension">如[.jpg]或[jpg], 不区分大小写</param>
        /// <returns>例子: 后缀[.jpg]返回[image/jpeg]</returns>
        public static string FromExtension(string extension, string defaultValue)
        {
            string suffix = extension.StartsWith(".") ? extension.ToLower() : "." + extension.ToLower();
            if (map.ContainsKey(suffix))
            {
                return map[suffix];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 根据文件后缀返回ContentType
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string FromExtension(string extension)
        {
            return FromExtension(extension, string.Empty);
        }

        /// <summary>
        /// 根据文件路径返回ContentType
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FromPath(string path)
        {
            return FromExtension(Path.GetExtension(path));
        }
    }
}
