using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Latermoon.Data;
using System.Data.Common;
using Bookmark.Entity;
using System.Data;

namespace Bookmark.Model
{
    public class DefaultOption
    {
        public DataTable GetList()
        {
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                string sql = string.Format("select * from [Favors] order by [Href]");
                
                //查询返回数据集
                return db.Execute<DataTable>(sql);
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="fieldsList"></param>
        /// <returns></returns>
        public void Add(IDictionary[] fieldsList)
        {
            // 获取数据库连接并使用using约束连接的生命周期
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                //开始一次事务
                db.BeginTransaction();

                int i = 0;
                try
                {
                    //foreach (IDictionary fields in fieldsList)
                    for (i = 0; i < fieldsList.Length;i++ )
                    {
                        IDictionary fields = fieldsList[i];

                        //string href = (string)fields["Href"];
                        ////判断是否已添加该网址
                        //Hashtable existsRow = db.ExecuteObject<Hashtable>("select top 1 * from [AppraiseOfWebSite] where [Href]=@Href", new DbParameter[] {                            
                        //    db.CreateParameter("@Href", href) });
                        //if (existsRow != null)
                        //{
                        //    //如果已存在该网址，则合并Tag 或忽略
                        //    continue;
                        //}

                        //将包含字段的Hashtable转成DbParameter[]
                        DbParameter[] prams = db.ToParameters(fields);
                        //使用自动方法插入数据行到Favors表
                        db.Insert("AppraiseOfWebSite", prams);
                    }
                    //提交事务
                    db.Commit();
                }
                catch (Exception e)
                {
                    //撤销事务
                    db.Rollback();
                    throw new Exception(e.Message, e);
                }
            }
        }
    }
}
