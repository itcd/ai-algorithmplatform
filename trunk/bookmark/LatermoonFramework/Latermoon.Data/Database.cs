using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Latermoon.Data
{
	/// <summary>
	/// 数据库访问对象
    /// 使用DatabaseFactory创建Database实例
	/// </summary>
	public abstract class Database : IDisposable
	{
        /// <summary>
        /// Database构建函数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerFactory"></param>
        public Database(string connectionString, DbProviderFactory providerFactory)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentException("connectionString");
			}
			if (providerFactory == null)
			{
				throw new ArgumentException("providerFactory");
			}
			ConnectionString = connectionString;
			DbProviderFactory = providerFactory;
            //创建数据库连接
			Connection = DbProviderFactory.CreateConnection();
			Connection.ConnectionString = ConnectionString;
			Connection.StateChange += delegate(object sender, StateChangeEventArgs e)
			{
				Console.WriteLine(e.CurrentState);
			};
		}

        /// <summary>
        /// 获取当前数据库连接字符串
        /// </summary>
        public virtual string ConnectionString { get; private set; }

        /// <summary>
        /// 当前数据库提供程序
        /// </summary>
        public virtual DbProviderFactory DbProviderFactory { get; private set; }

        /// <summary>
        /// 当前数据库连接
        /// </summary>
        public virtual DbConnection Connection { get; private set; }

		/// <summary>
		/// 获取一个打开的数据库连接
		/// </summary>
        protected virtual DbConnection OpenedConnection
		{
			get
			{
				if (Connection.State == ConnectionState.Closed)
				{
					Open();
				}
				return Connection;
			}
		}

        /// <summary>
        /// 当前数据库事务
        /// </summary>
        protected virtual DbTransaction Transaction { get; private set; }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public virtual void Open()
		{
			Connection.Open();
		}

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public virtual void Close()
		{
			Connection.Close();
		}

        /// <summary>
        /// 开始一次事务操作
        /// </summary>
        /// <returns></returns>
        public virtual DbTransaction BeginTransaction()
		{
			return Transaction = OpenedConnection.BeginTransaction();
		}

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void Commit()
		{
			Transaction.Commit();
		}

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void Rollback()
		{
			Transaction.Rollback();
		}

        /// <summary>
        /// 将键值表转换成参数数组
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DbParameter[] ToParameters(IDictionary parameters)
		{
			if (parameters == null)
			{
				return null;
			}
			List<DbParameter> list = new List<DbParameter>();
			foreach (DictionaryEntry item in parameters)
			{
				DbParameter param = CreateParameter(item.Key.ToString(), item.Value);
				list.Add(param);
			}
			return list.ToArray();
		}

        /// <summary>
        /// 创建适合当前数据库的参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual DbParameter CreateParameter(string name, object value, DbType dbType)
		{
			DbParameter param = DbProviderFactory.CreateParameter();
			param.ParameterName = name;
			param.Value = value;
            param.DbType = dbType;
			return param;
		}

        /// <summary>
        /// 创建适合当前数据库的参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = DbProviderFactory.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            return param;
        }

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual DbCommand CreateCommand(string commandText, DbParameter[] parameters, CommandType commandType)
		{
			DbCommand cmd = DbProviderFactory.CreateCommand();
			cmd.Connection = this.OpenedConnection;
			cmd.CommandText = commandText;
			cmd.CommandType = commandType;
			if (Transaction != null)
			{
				cmd.Transaction = Transaction;
			}
			if (parameters != null)
			{
				cmd.Parameters.AddRange(parameters);
			}
			return cmd;
		}

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DbCommand CreateCommand(string commandText, DbParameter[] parameters)
		{
			return CreateCommand(commandText, parameters, CommandType.Text);
		}

        /// <summary>
        /// 使用SQL语句创建DbCommand对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual DbCommand CreateCommand(string commandText)
		{
			return CreateCommand(commandText, null);
		}

        /// <summary>
        /// 执行SQL语句或存储过程，返回DbDataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>一个DbDataReader 对象。</returns>
        public virtual DbDataReader ExecuteReader(string commandText, DbParameter[] parameters, CommandType commandType)
		{
			using (DbCommand command = CreateCommand(commandText, parameters, commandType))
			{
				if (Transaction != null)
				{
					return command.ExecuteReader();
				}
				else
				{
					return command.ExecuteReader(CommandBehavior.CloseConnection);
				}
			}
		}

        /// <summary>
        /// 执行SQL语句，返回DbDataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns>一个DbDataReader 对象。</returns>
        public virtual DbDataReader ExecuteReader(string commandText, DbParameter[] parameters)
		{
			return ExecuteReader(commandText, parameters, CommandType.Text);
		}

        /// <summary>
        /// 执行SQL语句，返回DbDataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns>一个DbDataReader 对象。</returns>
        public virtual DbDataReader ExecuteReader(string commandText)
		{
			return ExecuteReader(commandText, null);
		}

        /// <summary>
        /// 对连接对象执行 SQL 语句。
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>受影响的行数。</returns>
        public virtual int ExecuteNonQuery(string commandText, DbParameter[] parameters, CommandType commandType)
		{
			using (DbCommand command = CreateCommand(commandText, parameters, commandType))
			{
				return command.ExecuteNonQuery();
			}
		}

        /// <summary>
        /// 对连接对象执行 SQL 语句。
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns>受影响的行数。</returns>
        public virtual int ExecuteNonQuery(string commandText, DbParameter[] parameters)
		{
			return ExecuteNonQuery(commandText, parameters, CommandType.Text);
		}

        /// <summary>
        /// 对连接对象执行 SQL 语句。
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns>受影响的行数。</returns>
        public virtual int ExecuteNonQuery(string commandText)
		{
			return ExecuteNonQuery(commandText, null);
		}

        /// <summary>
        ///  执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>结果集中第一行的第一列。</returns>
        public virtual object ExecuteScalar(string commandText, DbParameter[] parameters, CommandType commandType)
		{
			using (DbCommand command = CreateCommand(commandText, parameters, commandType))
			{
				return command.ExecuteScalar();
			}
		}

        /// <summary>
        ///  执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns>结果集中第一行的第一列。</returns>
        public virtual object ExecuteScalar(string commandText, DbParameter[] parameters)
		{
			return ExecuteScalar(commandText, parameters, CommandType.Text);
		}

        /// <summary>
        ///  执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns>结果集中第一行的第一列。</returns>
        public virtual object ExecuteScalar(string commandText)
		{
			return ExecuteScalar(commandText, null);
		}

        /// <summary>
        /// 提供程序列表
        /// </summary>
        private static Dictionary<Type, ExecuteProvider> ExecuteProviderMap = new Dictionary<Type, ExecuteProvider>();

        /// <summary>
        /// 是否包含指定的提供程序
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasExecuteProvider(Type type)
        {
            return ExecuteProviderMap.ContainsKey(type);
        }

        /// <summary>
        /// 获取指定类型的提供程序
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
		public static ExecuteProvider GetExecuteProvider(Type type)
        {
            return ExecuteProviderMap[type];
        }

        /// <summary>
        /// 注册或修改指定类型的提供程序
        /// </summary>
        /// <param name="type"></param>
        /// <param name="provider"></param>
        public static void RegisterExecuteProvider(Type type, ExecuteProvider provider)
        {
            ExecuteProviderMap[type] = provider;
        }

		/// <summary>
		/// 使用自定义解释程序获取数据
        /// 若要扩展该函数，请使用RegisterExecuteProvider注册新方法
		/// </summary>
		/// <typeparam name="T">目前预设支持:
        /// 基本值类型(int, long ...), string, string[], List&lt;string&gt;, Hashtable, List&lt;Hashtable&gt;, Dictionary&lt;string, object&gt;, List&lt;Dictionary&lt;string, object&gt;&gt; DataSet, DataTable, DataRow</typeparam>
		/// <param name="commandText">SQL语句或存储过程</param>
		/// <param name="parameters">执行所需要的参数</param>
		/// <param name="commandType">命令类型</param>
		/// <returns></returns>
        public virtual T Execute<T>(string commandText, DbParameter[] parameters, CommandType commandType)
		{
            // 根据要返回的类型选择相应的解释方法
            Type type = typeof(T);
            if (HasExecuteProvider(type))
            {
                return (T)GetExecuteProvider(type).Invoke(this, commandText, parameters, commandType);
            }
            else
            {
                throw new NotSupportedException("找不到该类型对应的解释程序: " + type.ToString());
            }
		}

        /// <summary>
        /// 使用自定义解释程序获取数据
        /// 若要扩展该函数，请使用RegisterExecuteProvider注册新方法
        /// </summary>
        /// <typeparam name="T">目前预设支持:
        /// 基本值类型(int, long ...), string, string[], List&lt;string&gt;, Hashtable, List&lt;Hashtable&gt;, Dictionary&lt;string, object&gt;, List&lt;Dictionary&lt;string, object&gt;&gt; DataSet, DataTable, DataRow</typeparam>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">执行所需要的参数</param>
        /// <returns></returns>
        public virtual T Execute<T>(string commandText, DbParameter[] parameters)
		{
			return Execute<T>(commandText, parameters, CommandType.Text);
		}

        /// <summary>
        /// 使用自定义解释程序获取数据
        /// 若要扩展该函数，请使用RegisterExecuteProvider注册新方法
        /// </summary>
        /// <typeparam name="T">目前预设支持:
        /// 基本值类型(int, long ...), string, string[], List&lt;string&gt;, Hashtable, List&lt;Hashtable&gt;, Dictionary&lt;string, object&gt;, List&lt;Dictionary&lt;string, object&gt;&gt; DataSet, DataTable, DataRow</typeparam>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public virtual T Execute<T>(string commandText)
		{
			return Execute<T>(commandText, null);
		}

		/// <summary>
		/// 使用指定参数和条件更新表
		/// 不适用于MySQL, 要自动支持MySQL，需注入自定义Database，如
		/// DatabaseFactory.RegisterDatabaseProvider("MySql.Data.MySqlClient", "Latermoon.Data.Provider.MySqlDatabase,Latermoon.Data.Provider");
		/// </summary>
		/// <param name="table">表名</param>
		/// <param name="where">SQL中where子句</param>
		/// <param name="fields">要更新的参数</param>
		/// <returns>返回受影响的行数</returns>
		public virtual int Update(string table, string where, DbParameter[] fields)
		{
			return Update(table, where, fields, null);
		}

        /// <summary>
        /// 使用指定参数和条件更新表
        /// 不适用于MySQL, 要自动支持MySQL，需注入自定义Database，如
        /// DatabaseFactory.RegisterDatabaseProvider("MySql.Data.MySqlClient", "Latermoon.Data.Provider.MySqlDatabase,Latermoon.Data.Provider");
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="where">SQL中where子句</param>
        /// <param name="fields">要更新的参数</param>
		/// <param name="wherePrams">where子句里要用的参数</param>
        /// <returns>返回受影响的行数</returns>
        public virtual int Update(string table, string where, DbParameter[] fields, DbParameter[] wherePrams)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("update ");
			sb.Append(table);
			sb.Append(" set ");
			//插入字段名
			foreach (DbParameter item in fields)
			{
                if (item.ParameterName.StartsWith("@"))
                {
                    sb.Append(item.ParameterName.Substring(1));
                    sb.Append("=");
                    sb.Append(item.ParameterName);
                    sb.Append(",");
                }
                else
                {
                    sb.Append(item.ParameterName);
                    sb.Append("=@");
                    sb.Append(item.ParameterName);
                    sb.Append(",");
                }
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			if (!string.IsNullOrEmpty(where))
			{
				sb.Append(" where ");
				sb.Append(where);
				//获得两组SQL参数
				if (wherePrams != null && wherePrams.Length > 0)
				{
					List<DbParameter> list = new List<DbParameter>(fields.Length + wherePrams.Length);
					list.AddRange(fields);
					list.AddRange(wherePrams);
					fields = list.ToArray();
				}
			}
			//执行sql语句
			string sql = sb.ToString();
			return this.ExecuteNonQuery(sql, fields);
		}

        /// <summary>
        /// 使用指定参数自动构建Insert语句插入新数据行
        /// 不适用于MySQL, 要自动支持MySQL，需注入自定义Database，如
        /// DatabaseFactory.RegisterDatabaseProvider("MySql.Data.MySqlClient", "Latermoon.Data.Provider.MySqlDatabase,Latermoon.Data.Provider");
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="fields">参数表</param>
        /// <returns>返回受影响的行数</returns>
		public virtual int Insert(string table, DbParameter[] fields)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("insert into ");
			sb.Append(table);
			sb.Append("(");
			//插入字段名
			foreach (DbParameter item in fields)
			{
				sb.Append(item.ParameterName.StartsWith("@") ? item.ParameterName.Substring(1) : item.ParameterName);
				sb.Append(",");
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			sb.Append(") values(");
			//插入字段变量(=@字段名)
			foreach (DbParameter item in fields)
			{
				if (!item.ParameterName.StartsWith("@"))
				{
					sb.Append("@");
				}
				sb.Append(item.ParameterName);
				sb.Append(",");
			}
			sb.Remove(sb.Length - 1, 1);//移除最后一个逗号
			sb.Append(")");
			//执行sql语句
			string sql = sb.ToString();
			return ExecuteNonQuery(sql, fields);
		}

        /// <summary>
        /// 释放由数据库连接使用的资源。
        /// </summary>
		public void Dispose()
		{
			Connection.Dispose();
		}

	}

    /// <summary>
    /// Execute方法注入入口
    /// </summary>
    /// <param name="database">Database实例</param>
    /// <param name="commandText">SQL命令或存储过程</param>
    /// <param name="parameters">参数</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    public delegate object ExecuteProvider(Database database, string commandText, DbParameter[] parameters, CommandType commandType);
}
