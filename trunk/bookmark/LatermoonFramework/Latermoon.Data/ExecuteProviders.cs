using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Reflection;

namespace Latermoon.Data
{
    /// <summary>
    /// 提供一组方法扩展数据获取
    /// 用于扩展Databas.Execute&lt;T&gt;方法
    /// </summary>
    public static class ExecuteProviders
    {
        public static object ExecuteScalar(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            return database.ExecuteScalar(commandText, parameters, commandType);
        }

        public static List<string> ExecuteStringList(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            List<string> list = new List<string>();
            using (DbDataReader reader = database.ExecuteReader(commandText, parameters, commandType))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        public static string[] ExecuteStringArray(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            List<string> list = ExecuteStringList(database, commandText, parameters, commandType) as List<string>;
            return list.ToArray();
        }

        public static Dictionary<string, object> ExecuteDictionary(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            Dictionary<string, object> map = null;
            using (DbDataReader reader = database.ExecuteReader(commandText, parameters, commandType))
            {
                if (reader.Read())
                {
                    map = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        int nameIndex = 1;
                        string name = reader.GetName(i);
                        //依次加1找到没有被占用的字段名
                        while (map.ContainsKey(name))
                        {
                            name = reader.GetName(i) + nameIndex.ToString();
                        }
                        map.Add(name, reader.GetValue(i));
                    }
                }
            }
            return map;
        }

        public static List<Dictionary<string, object>> ExecuteDictionaryList(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            using (DbDataReader reader = database.ExecuteReader(commandText, parameters, commandType))
            {
                while (reader.Read())
                {
                    Dictionary<string, object> table = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        int nameIndex = 1;
                        string name = reader.GetName(i);
                        //依次加1找到没有被占用的字段名
                        while (table.ContainsKey(name))
                        {
                            name = reader.GetName(i) + nameIndex.ToString();
                        }
                        table.Add(name, reader.GetValue(i));
                    }
                    list.Add(table);
                }
            }
            return list;
        }

        public static Hashtable ExecuteHashtable(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            Hashtable map = null;
            using (DbDataReader reader = database.ExecuteReader(commandText, parameters, commandType))
            {
                if (reader.Read())
                {
                    map = new Hashtable();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        int nameIndex = 1;
                        string name = reader.GetName(i);
                        //依次加1找到没有被占用的字段名
                        while (map.Contains(name))
                        {
                            name = reader.GetName(i) + nameIndex.ToString();
                        }
                        map.Add(name, reader.GetValue(i));
                    }
                }
            }
            return map;
        }

        public static List<Hashtable> ExecuteHashtableList(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            List<Hashtable> list = new List<Hashtable>();
            using (DbDataReader reader = database.ExecuteReader(commandText, parameters, commandType))
            {
                while (reader.Read())
                {
                    Hashtable table = new Hashtable();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        int nameIndex = 1;
                        string name = reader.GetName(i);
                        //依次加1找到没有被占用的字段名
                        while (table.Contains(name))
                        {
                            name = reader.GetName(i) + nameIndex.ToString();
                        }
                        table.Add(name, reader.GetValue(i));
                    }
                    list.Add(table);
                }
            }
            return list;
        }

        public static DataTable ExecuteDataTable(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            DbDataAdapter adpter = database.DbProviderFactory.CreateDataAdapter();
            adpter.SelectCommand = database.CreateCommand(commandText, parameters, commandType);
            DataTable table = new DataTable();
            adpter.Fill(table);
            return table;
        }

        public static DataRow ExecuteDataRow(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            DbDataAdapter adpter = database.DbProviderFactory.CreateDataAdapter();
            adpter.SelectCommand = database.CreateCommand(commandText, parameters, commandType);
            DataTable table = new DataTable();
            adpter.Fill(table);
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }

        public static DataSet ExecuteDataSet(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            DbDataAdapter adpter = database.DbProviderFactory.CreateDataAdapter();
            adpter.SelectCommand = database.CreateCommand(commandText, parameters, commandType);
            DataSet ds = new DataSet();
            adpter.Fill(ds);
            return ds;
        }

       /*
        public static T ExecuteClass<T>(Database database, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            using (DbDataReader reader = database.ExecuteReader(commandText, parameters, commandType))
            {
                Type type = typeof(T);
                MemberInfo[] menbers = type.GetMembers();
                T obj = Activator.CreateInstance<T>();
                foreach (MemberInfo member in menbers)
                {
                    //取出绑定的字段信息
                    DataFieldAttribute attr = (DataFieldAttribute)Attribute.GetCustomAttribute(member, typeof(DataFieldAttribute));
                    if (attr != null)
                    {
                        //获取字段值
                        object value = reader[attr.FieldName];
                        if (value != null)
                        {
                            //设置带Setter的属性
                            if (member is PropertyInfo && (member as PropertyInfo).CanWrite)
                            {
                                (member as PropertyInfo).SetValue(obj, reader[attr.FieldName], null);
                            }
                            //设置字段
                            else if (member is FieldInfo)
                            {
                                (member as FieldInfo).SetValue(obj, reader[attr.FieldName]);
                            }
                        }
                    }
                }
                return obj;
            }
        }*/
    }
}
