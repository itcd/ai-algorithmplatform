using System;
using System.Collections.Generic;
using System.Text;

namespace Latermoon.Web
{
	public class PageScript
	{
		private const string SCRIPT_START = "<script type=\"text/javascript\">\r\n";
		private const string SCRIPT_END = "</script>\r\n";

		/// <summary>
		/// 弹出信息提示框
		/// </summary>
		/// <param name="obj">任意可以转换为JSON文本的对象</param>
		/// <returns></returns>
		public static string Alert(object obj)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			sb.Append("alert(");
			//sb.Append(JsonUtil.ToJson(obj));
			sb.Append(");\r\n");
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}

		/// <summary>
		/// 用script标签包裹代码
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string Wrap(object obj)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			sb.Append(obj);
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}

		/// <summary>
		/// 窗口跳转
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string Redirect(string url)
		{
			return Redirect(url, 0);
		}

		/// <summary>
		/// 延时窗口跳转
		/// </summary>
		/// <param name="url"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static string Redirect(string url, int timeout)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			if (timeout <= 0)
			{
				sb.Append("window.location='");
				sb.Append(url);
				sb.Append("';\r\n");
			}
			else
			{
				sb.Append("window.setTimeout(function(){window.location='");
				sb.Append(url);
				sb.Append("';},");
				sb.Append(timeout);
				sb.Append(");\r\n");
			}
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}

		/// <summary>
		/// 立即后退
		/// </summary>
		/// <returns></returns>
		public static string GoBack()
		{
			return GoBack(0);
		}

		/// <summary>
		/// 延时后退
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static string GoBack(int timeout)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			if (timeout <= 0)
			{
				sb.Append("history.go(-1);\r\n");
			}
			else
			{
				sb.Append("window.setTimeout('history.go(-1)', ");
				sb.Append(timeout);
				sb.Append(");\r\n");
			}
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}

		/// <summary>
		/// 立即关闭窗口
		/// </summary>
		/// <returns></returns>
		public static string CloseWindow()
		{
			return CloseWindow(0);
		}

		/// <summary>
		/// 定时关闭窗口
		/// </summary>
		/// <param name="timeout">毫秒数</param>
		/// <returns></returns>
		public static string CloseWindow(int timeout)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			if (timeout <= 0)
			{
				sb.Append("window.close();\r\n");
			}
			else
			{
				sb.Append("window.setTimeout('window.close()', ");
				sb.Append(timeout);
				sb.Append(");\r\n");
			}
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}

		/// <summary>
		/// 执行iframe里面的函数
		/// </summary>
		/// <param name="framename"></param>
		/// <param name="function">如：showImage(1)</param>
		/// <returns></returns>
		public static string CallFrameFunction(string framename, string function)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			sb.Append("window.frames['");
			sb.Append(framename);
			sb.Append("'].");
			sb.Append(function);
			sb.Append(";\r\n");
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}

		//执行父窗口里面的函数
		public static string CallParentFunction(string function)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(SCRIPT_START);
			sb.Append("window.parent.");
			sb.Append(function);
			sb.Append(";\r\n");
			sb.Append(SCRIPT_END);
			return sb.ToString();
		}
	}
}
