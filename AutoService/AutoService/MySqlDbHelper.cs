// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MySqlDbHelper.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:40:22 </date>
// <summary>
//   The db opt.
// </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:40:22
//      修改描述：新建 MySqlDbHelper.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoService
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Dapper;

    using Infrastructure.Log;

    using MySql.Data.MySqlClient;

    /// <summary>
    ///     The db opt.
    /// </summary>
    public class MySqlDbHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Add<T>(IDbConnection conn, string sql, T t) where T : new()
        {
            return ExecuteSql(conn, sql, t);
        }

        /// <summary>
        /// The add and get id.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <typeparam name="T">
        /// BaseEntity
        /// </typeparam>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long AddAndGetId<T>(IDbConnection conn, string sql, T t) where T : new()
        {
            // conn.Execute(sql, t);
            // dynamic identity = conn.Query(sql + "SELECT @@identity AS NewId;", t).Single();
            // t.Id = (long)identity.NewId;

            // return t.Id;
            return 0;
        }

        /// <summary>
        /// The begin tran.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <returns>
        /// The <see cref="IDbTransaction"/>.
        /// </returns>
        public static IDbTransaction BeginTran(IDbConnection conn)
        {
            return conn.BeginTransaction();
        }

        /// <summary>
        /// The commit.
        /// </summary>
        /// <param name="tran">
        /// The tran.
        /// </param>
        public static void Commit(IDbTransaction tran)
        {
            tran.Commit();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Delete<T>(IDbConnection conn, string sql, T t) where T : new()
        {
            return ExecuteSql(conn, sql, t);
        }

        /// <summary>
        /// The execute sql.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ExecuteSql(IDbConnection conn, string sql)
        {
            int result = conn.Execute(sql);

            if (result > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The execute sql procedure.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        public static void ExecuteSqlProcedure(IDbConnection conn, string sql)
        {
            try
            {
                TraceManager.Info.Write("DM2.EventReportServce.ExecuteSqlProcedure", sql);
                conn.Execute(sql);
            }
            catch (Exception exception)
            {
                TraceManager.Error.Write("DM2.EventReportServce.DbOpt", "ExecuteSqlProcedure : " + exception.Message);
            }
        }

        /// <summary>
        /// The get connection.
        /// </summary>
        /// <param name="sqlConnection">
        /// The sql connection.
        /// </param>
        /// <returns>
        /// The <see cref="IDbConnection"/>.
        /// </returns>
        public static IDbConnection GetConnection(string sqlConnection)
        {
            if (string.IsNullOrWhiteSpace(sqlConnection))
            {
                return null;
            }

            return new MySqlConnection(sqlConnection);
        }

        /// <summary>
        /// The query.
        /// </summary>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<T> Query<T>(IDbConnection conn, string sql) where T : new()
        {
            return conn.Query<T>(sql, null).ToList();
        }

        /// <summary>
        /// The query.
        /// </summary>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Query<T>(IDbConnection conn, string sql, string id) where T : new()
        {
            return conn.Query<T>(sql, new { id }).SingleOrDefault<T>();
        }

        /// <summary>
        /// The query.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Query<T>(IDbConnection conn, string sql, T t) where T : new()
        {
            return conn.Query<T>(sql, t).SingleOrDefault<T>();
        }

        /// <summary>
        /// The query list.
        /// </summary>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<T> QueryList<T>(IDbConnection conn, string sql)
        {
            return conn.Query<T>(sql, null).ToList();
        }

        /// <summary>
        /// The roll back.
        /// </summary>
        /// <param name="tran">
        /// The tran.
        /// </param>
        public static void RollBack(IDbTransaction tran)
        {
            tran.Rollback();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Update<T>(IDbConnection conn, string sql, T t) where T : new()
        {
            return ExecuteSql(conn, sql, t);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The execute sql.
        /// </summary>
        /// <typeparam name="T">
        /// t
        /// </typeparam>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool ExecuteSql<T>(IDbConnection conn, string sql, T t) where T : new()
        {
            int result = conn.Execute(sql, t);

            if (result > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// The db name.
    /// </summary>
    public class DbName
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the schem a_ name.
        /// </summary>
        public string SCHEMA_NAME { get; set; }

        #endregion
    }
}