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
    /// <summary>
    /// 管理用户网址收藏数据
    /// </summary>
    public class BookmarkFavors
    {
        public BookmarkFavors(BookmarkUser user)
        {
            User = user;
        }

        public BookmarkUser User { get; private set; }

        /// <summary>
        /// 添加一条网址
        /// </summary>
        /// <param name="fields">使用FavorField.EmptyRow()创建并填充的Hashtable</param>
        /// <returns>网址ID</returns>
        public long Add(IDictionary fields)
        {
            // 获取数据库连接并使用using约束连接的生命周期
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                //开始一次事务
                db.BeginTransaction();
                try
                {
                    string href = (string)fields[FavorField.Href];
                    //判断是否已添加该网址
                    Hashtable existsRow = db.Execute<Hashtable>("select top 1 * from [Favors] where [Username]=@Username and [Href]=@Href", new DbParameter[] {
                            db.CreateParameter("@Username", User.Username),
                            db.CreateParameter("@Href", href) });
                    if (existsRow != null)
                    {
                        //如果已存在该网址，则合并Tag 或忽略
                        return -1;
                    }
                    //将包含字段的Hashtable转成DbParameter[]
                    DbParameter[] prams = db.ToParameters(fields);
                    //使用自动方法插入数据行到Favors表
                    db.Insert("Favors", prams);
                    //获取自动递增ID
                    long id = Convert.ToInt64(db.ExecuteScalar("select @@IDENTITY"));
                    //提交事务
                    db.Commit();
                    return id;
                }
                catch (Exception e)
                {
                    //撤销事务
                    db.Rollback();
                    throw new Exception(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="fieldsList"></param>
        /// <returns></returns>
        public long[] Add(IDictionary[] fieldsList)
        {
            // 获取数据库连接并使用using约束连接的生命周期
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                List<long> idList = new List<long>();
                //开始一次事务
                db.BeginTransaction();
                try
                {
                    foreach (IDictionary fields in fieldsList)
                    {
                        string href = (string)fields[FavorField.Href];
                        //判断是否已添加该网址
                        Hashtable existsRow = db.Execute<Hashtable>("select top 1 * from [Favors] where [Username]=@Username and [Href]=@Href", new DbParameter[] {
                            db.CreateParameter("@Username", User.Username),
                            db.CreateParameter("@Href", href) });
                        if (existsRow != null)
                        {
                            //如果已存在该网址，则合并Tag 或忽略
                            continue;
                        }
                        //将包含字段的Hashtable转成DbParameter[]
                        DbParameter[] prams = db.ToParameters(fields);
                        //使用自动方法插入数据行到Favors表
                        db.Insert("Favors", prams);
                        //获取自动递增ID
                        long id = Convert.ToInt64(db.ExecuteScalar("select @@IDENTITY"));
                        idList.Add(id);
                    }
                    //提交事务
                    db.Commit();
                    return idList.ToArray();
                }
                catch (Exception e)
                {
                    //撤销事务
                    db.Rollback();
                    throw new Exception(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 获取用户网址列表
        /// </summary>
        /// <param name="privacy">获取公开图片还是全部图片</param>
        /// <param name="pageIndex">页码，从0开始</param>
        /// <param name="count">每页条数</param>
        /// <param name="orderBy">按时间顺序还是逆序</param>
        /// <returns>将每行数据放到Hashtable中</returns>
        public DataTable GetList(Privacy privacy, int pageIndex, int count, OrderBy orderBy)
        {
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                string sql;
                if (privacy == Privacy.PUBLIC)
                {
                    sql = string.Format("select top {0} * from [Favors] where [Username]=@Username and [Privacy]=0 and [FavorID] not in(select top {1} [FavorID] from [Favors] where  [Username]=@Username and [Privacy]=0 order by [FavorID] {2}) order by [FavorID] {2}",
                           count,
                           pageIndex * count,
                           orderBy == OrderBy.ASC ? "asc" : "desc");

                    //sql = string.Format("select * from [Favors] where [Username]=@Username and [FavorID] order by [FavorID] {2} limit {1},{0}",
                    //       count,
                    //       pageIndex * count,
                    //       orderBy == OrderBy.ASC ? "asc" : "desc");
                }
                else
                {
                    sql = string.Format("select top {0} * from [Favors] where [Username]=@Username and [FavorID] not in(select top {1} [FavorID] from [Favors] where  [Username]=@Username order by [FavorID] {2}) order by [FavorID] {2}",
                           count,
                           pageIndex * count,
                           orderBy == OrderBy.ASC ? "asc" : "desc");
                }
                //查询返回数据集
                return db.Execute<DataTable>(sql, new DbParameter[]{
                    db.CreateParameter("@Username", User.Username)
                });
            }
        }

        /// <summary>
        /// 获取用户网址数
        /// </summary>
        /// <param name="privacy"></param>
        /// <returns></returns>
        public int GetCount(Privacy privacy)
        {
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                string sql;
                if (privacy == Privacy.PUBLIC)
                {
                    sql = "select count(*) from [Favors] where [Username]=@Username and [Privacy]=0";
                }
                else
                {
                    sql = "select count(*) from [Favors] where [Username]=@Username";
                }
                object count = db.ExecuteScalar(sql, new DbParameter[]{
                    db.CreateParameter("@Username", User.Username)
                });

                return Convert.ToInt32(count);
            }
        }

        public List<Hashtable> GetByLevel(int level, Privacy privacy, int pageIndex, int count, OrderBy orderBy)
        {
            throw new NotImplementedException();
        }

        public int GetCountByLevel(int level, Privacy privacy)
        {
            throw new NotImplementedException();
        }

        public List<Hashtable> GetByTag(string tag, Privacy privacy, int pageIndex, int count, OrderBy orderBy)
        {
            throw new NotImplementedException();
        }

        public int GetCountByTag(string tag, Privacy privacy)
        {
            throw new NotImplementedException();
        }

        public List<Hashtable> GetByTitleSearch(string keyword, Privacy privacy, int pageIndex, int count, OrderBy orderBy)
        {
            throw new NotImplementedException();
        }

        public int GetCountByTitleSearch(string keyword, Privacy privacy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 以Tag和UserName为约束条件来筛选网址数据
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="privacy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="count"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>

        public DataTable GetByTags(string tag, Privacy privacy, int pageIndex, int count, OrderBy orderBy)
        {
            using (Database db = DatabaseFactory.CreateDatabase("bookmark"))
            {
                string sql;
                if (privacy == Privacy.PUBLIC)
                {
                    sql = string.Format("select top {0} * from [Favors] where [Tag]= '{3}' and  [Username]=@Username and [Privacy]=0 and [FavorID] not in(select top {1} [FavorID] from [Favors] where [Tag]='{3}' and  [Username]=@Username  and [Privacy]=0 order by [FavorID] {2}) order by [FavorID] {2}",
                           count,
                           pageIndex * count,
                           orderBy == OrderBy.ASC ? "asc" : "desc",
                           tag);
                }
                else
                {
                    sql = string.Format("select top {0} * from [Favors] where [Tag]= '{3}'and [Username]=@Username  and [FavorID] not in(select top {1} [FavorID] from [Favors] where [Tag]='{3}' and  [Username]=@Username order by [FavorID] {2}) order by [FavorID] {2}",
                           count,
                           pageIndex * count,
                           orderBy == OrderBy.ASC ? "asc" : "desc",
                           tag);
                }
                //可以用来测试的字符串: '新我的链接,编程技术,jQuery'
                //查询返回数据集
                return db.Execute<DataTable>(sql, new DbParameter[]{
                    db.CreateParameter("@Username", User.Username)
                });
            }
        }
    }
}
