<%@ WebHandler Language="C#" Class="FavorLevelUpdateHandler" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Latermoon.Web;

/// <summary>
/// 提供Ajax方法修改FavorLevel
/// </summary>
public class FavorLevelUpdateHandler : IHttpHandler {
    /// <summary>
    /// 处理HTTP请求
    /// </summary>
    /// <param name="context">HTTP请求</param>
    public void ProcessRequest (HttpContext context) {
        HttpRequest Requset = context.Request;
        HttpResponse Response = context.Response;
        HttpServerUtility Server = context.Server;

        //存放返回到客户端的信息
        Hashtable result = new Hashtable();
        //获取参数
        int favorId = RequestUtility.GetInt32("favorid", -1);
        short level = RequestUtility.GetInt16("level", -1);
        if (favorId == -1 || level == -1)
        {
            result.Add("success", false);
            result.Add("msg", "参数无效");
        }
        else
        {
            //使用Linq查询
            FavorsTableDataContext dc = new FavorsTableDataContext();
            var favors = from item in dc.Favors
                        where item.FavorID == favorId
                        select item;
            if (favors.Count() < 1)
            {
                result.Add("success", false);
                result.Add("msg", "找不到指定收藏 ID: " + favorId);
            }
            else
            {
                var favor = favors.ToList()[0];
                result.Add("original_level", favor.FavorLevel); //旧的level
                //修改数据库
                favor.FavorLevel = level;
                dc.SubmitChanges();
                result.Add("level", level);//新的level
                result.Add("success", true);
                result.Add("msg", "修改成功");
            }
        }

        //将Hashtable里的信息转换成JSON返回到客户端
        //将返回类似一下JSON文本 {"success":true,"level":10,"msg":"修改成功","orginal_level":10}
        string json = JSON.ToJson(result);
        Response.ContentType = "text/plain";
        Response.Write(json);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}