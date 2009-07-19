using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Latermoon.Data
{
    /// <summary>
    /// 通用数据库访问
    /// </summary>
	public class GenericDatabase : Database
	{
		public GenericDatabase(string connectionString, DbProviderFactory providerFactory)
			: base(connectionString, providerFactory)
		{
		}
	}
}
