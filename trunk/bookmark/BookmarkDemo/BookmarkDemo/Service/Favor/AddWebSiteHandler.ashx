<%@ WebHandler Language="C#" Class="AddWebSiteHandler" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Latermoon.Web;

public class AddWebSiteHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Requset = context.Request;
        HttpResponse Response = context.Response;
        HttpServerUtility Server = context.Server;

        //存放返回到客户端的信息
        Hashtable result = new Hashtable();
        //获取参数
        string href = RequestUtility.GetString("href", "");

        string[] titles = null;
        string[] tags = null;
        string[] remarks = null;

        //查询并获得默认选项
        FavorsTableDataContext db = new FavorsTableDataContext();
        var query = from site in db.AppraiseOfWebSite where site.Href == href select site;

        foreach (var site in query)
        {
            titles = site.Titles.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            tags = site.Tags.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            remarks = site.Remarks.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            
            result.Add("titles", titles);
            result.Add("tags", tags);
            result.Add("remarks", remarks);
            result.Add("favorsLevel", site.FavorLevel);
            result.Add("queryFrequency", site.QueryFrequency);

            break;
        }

        //将Hashtable里的信息转换成JSON返回到客户端
        //titles，tags，remarks三个字符串数组分别按使用频率的顺序记录了这三个属性的默认选项。
        string json = JSON.ToJson(result);
        Response.ContentType = "text/plain";
        Response.Write(json);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}