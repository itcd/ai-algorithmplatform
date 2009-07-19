using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Latermoon.Data.Utility
{
	public class SqlBuilderUtility
	{
		public static string ToInsertSql(string table, DbParameter[] fields)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("insert into ");
			sb.Append(table);
			sb.Append("(");
			//插入字段名
			foreach (DbParameter item in fields)
			{
				sb.Append(item.ParameterName);
				sb.Append(",");
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			sb.Append(") values(");
			//插入字段变量(=@字段名)
			foreach (DbParameter item in fields)
			{
				sb.Append("@");
				sb.Append(item.ParameterName);
				sb.Append(",");
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			sb.Append(")");
			//获得sql语句
			string sql = sb.ToString();
			return sql;
		}

		public static string ToInsertSqlWithValue(string table, DbParameter[] fields)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("insert into ");
			sb.Append(table);
			sb.Append("(");
			//插入字段名
			foreach (DbParameter item in fields)
			{
				sb.Append(item.ParameterName);
				sb.Append(",");
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			sb.Append(") values(");
			//插入字段变量(=@字段名)
			foreach (DbParameter item in fields)
			{
				Type type = item.Value.GetType();
				if (type == typeof(string))
				{
					sb.Append("'");
					sb.Append(item.Value);
					sb.Append("'");
				}
				else if (type == typeof(Double))
				{
					double d = (double)item.Value;
					if (d > 10000000)
					{
						sb.Append("-1");
					}
					else
					{
						sb.Append(item.Value);
					}
				}
				else
				{
					sb.Append(item.Value);
				}
				sb.Append(",");
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			sb.Append(")");
			//获得sql语句
			string sql = sb.ToString();
			return sql;
		}
	}
}
