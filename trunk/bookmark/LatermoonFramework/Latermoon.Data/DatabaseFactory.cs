using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Reflection;
using System.Data;
using System.Collections;

namespace Latermoon.Data
{
    /// <summary>
    /// 提供一组方法管理数据库实例
    /// </summary>
	public class DatabaseFactory
	{
        // 静态初始化入口
        static DatabaseFactory()
        {
            // 注册Database.Execute<T>需要的预设数据解释程序
            Database.RegisterExecuteProvider(typeof(string), new ExecuteProvider(ExecuteProviders.ExecuteScalar));
            Database.RegisterExecuteProvider(typeof(int), new ExecuteProvider(ExecuteProviders.ExecuteScalar));
            Database.RegisterExecuteProvider(typeof(long), new ExecuteProvider(ExecuteProviders.ExecuteScalar));
            Database.RegisterExecuteProvider(typeof(float), new ExecuteProvider(ExecuteProviders.ExecuteScalar));
            Database.RegisterExecuteProvider(typeof(DateTime), new ExecuteProvider(ExecuteProviders.ExecuteScalar));
            Database.RegisterExecuteProvider(typeof(List<string>), new ExecuteProvider(ExecuteProviders.ExecuteStringList));
            Database.RegisterExecuteProvider(typeof(string[]), new ExecuteProvider(ExecuteProviders.ExecuteStringArray));
            Database.RegisterExecuteProvider(typeof(Dictionary<string, object>), new ExecuteProvider(ExecuteProviders.ExecuteDictionary));
            Database.RegisterExecuteProvider(typeof(List<Dictionary<string, object>>), new ExecuteProvider(ExecuteProviders.ExecuteDictionaryList));
            Database.RegisterExecuteProvider(typeof(DataTable), new ExecuteProvider(ExecuteProviders.ExecuteDataTable));
            Database.RegisterExecuteProvider(typeof(Hashtable), new ExecuteProvider(ExecuteProviders.ExecuteHashtable));
            Database.RegisterExecuteProvider(typeof(List<Hashtable>), new ExecuteProvider(ExecuteProviders.ExecuteHashtableList));
            Database.RegisterExecuteProvider(typeof(DataRow), new ExecuteProvider(ExecuteProviders.ExecuteDataRow));
            Database.RegisterExecuteProvider(typeof(DataSet), new ExecuteProvider(ExecuteProviders.ExecuteDataSet));
            Database.RegisterExecuteProvider(typeof(object),
                delegate(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
                {
                    return database.ExecuteScalar(commandText, parameters, commandType);
                });
        }
		/// <summary>
		/// 根据ConnectionStrings配置创建数据库访问对象
		/// </summary>
		/// <param name="connectionStringName"></param>
		/// <returns></returns>
		public static Database CreateDatabase(string connectionStringName)
		{
			string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			string providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
			return CreateDatabase(connectionString, providerName);
		}

		/// <summary>
		/// 根据连接字符串和提供程序创建数据库访问对象
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="providerName"></param>
		/// <returns></returns>
		public static Database CreateDatabase(string connectionString, string providerName)
		{
            //获取提供程序
			DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerName);
            //查看有无匹配提供程序的Database
			if (HasDatabaseProvider(providerName))
            {
                //使用自定义的Database
				Type databaseType = GetDatabaseProvider(providerName);
                return (Database)Activator.CreateInstance(databaseType, connectionString, providerFactory);
            }
            else
            {
                //使用默认通用Database
                return new GenericDatabase(connectionString, providerFactory);
            }
		}

        /// <summary>
        /// 提供程序映射, 存放DbProviderFactory和Database的名称对应关系
        /// </summary>
        private static IDictionary<string, Type> ProviderMap = new Dictionary<string, Type>();

        /// <summary>
        /// 注册自定义的Database实现
        /// 例子: RegisterDatabaseProvider("System.Data.SqlClient", "Latermoon.Data.SqlDatabase,Latermoon.Data");
        /// </summary>
        /// <param name="providerName">提供程序的固定名称</param>
        /// <param name="assemblyQualifiedName">Database派生类的固定名称</param>
		public static void RegisterDatabaseProvider(string providerName, string assemblyQualifiedName)
		{
            Type type = Type.GetType(assemblyQualifiedName);
			if (type == null)
			{
				throw new ArgumentException("assemblyQualifiedName");
			}
            RegisterDatabaseProvider(providerName, type);
		}

        /// <summary>
        /// 注册自定义的Database实现
        /// 例子: RegisterDatabaseProvider("System.Data.SqlClient", typeof(Latermoon.Data.SqlDatabase));
        /// </summary>
        /// <param name="providerName">提供程序的固定名称</param>
        /// <param name="databaseType">Database派生类的类型</param>
        public static void RegisterDatabaseProvider(string providerName, Type databaseType)
        {
            if (databaseType == null)
            {
				throw new ArgumentNullException("databaseType");
            }
			ProviderMap[providerName] = databaseType;
        }

		/// <summary>
		/// 判断是否存在Database提供程序
		/// </summary>
		/// <param name="providerName"></param>
		/// <returns></returns>
		public static bool HasDatabaseProvider(string providerName)
		{
			return ProviderMap.ContainsKey(providerName);
		}

        /// <summary>
        /// 获取提供程序的Type类型
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
		public static Type GetDatabaseProvider(string providerName)
		{
			return ProviderMap[providerName];
		}
	}
}
