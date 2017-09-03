// <copyright file="MSSqlDbHelper.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2016/12/23 04:05:32 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2016/12/23 04:05:32
//      修改描述：新建 MSSqlDbHelper.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace AutoServiceApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    using Dapper;

    /// <summary>
    /// The ms sql db helper.
    /// </summary>
    public class MSSqlDbHelper
    {
        #region Static Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private static readonly MSSqlDbHelper instance = new MSSqlDbHelper();

        #endregion

        // Remember to add <remove name="LocalSqlServer" > in ConnectionStrings section if using this, as otherwise it would be the first one.
        #region Fields

        private string currentConnectionStr;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="MSSqlDbHelper"/> class from being created.
        /// </summary>
        private MSSqlDbHelper()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MSSqlDbHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// proc with params returning Dynamic.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// Name of the connection.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<dynamic> DynamicProcWithParams<T>(
            string sql,
            dynamic parms,
            string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// proc with params single returning Dynamic object.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// Name of the connection.
        /// </param>
        /// <returns>
        /// The <see cref="DynamicObject"/>.
        /// </returns>
        public DynamicObject DynamicProcWithParamsSingle<T>(
            string sql,
            dynamic parms,
            string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// The get parameters from object.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="propertyNamesToIgnore">
        /// The property names to ignore.
        /// </param>
        /// <returns>
        /// The <see cref="DynamicParameters"/>.
        /// </returns>
        public static DynamicParameters GetParametersFromObject(object obj, string[] propertyNamesToIgnore)
        {
            if (propertyNamesToIgnore == null)
            {
                propertyNamesToIgnore = new[] { string.Empty };
            }

            var p = new DynamicParameters();
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in properties)
            {
                if (!propertyNamesToIgnore.Contains(prop.Name))
                {
                    p.Add("@" + prop.Name, prop.GetValue(obj, null));
                }
            }

            return p;
        }

        /// <summary>
        /// The get property value.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object theValue = null;
            foreach (PropertyInfo prop in properties)
            {
                if (string.Compare(prop.Name, propertyName, true) == 0)
                {
                    theValue = prop.GetValue(target, null);
                }
            }

            return theValue;
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// The connection Name.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Execute(sql, (object)parms);
            }
        }

        /// <summary>
        /// Insert update or delete stored proc.
        /// </summary>
        /// <param name="procName">
        /// Name of the proc.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// The connection Name.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int InsertUpdateOrDeleteStoredProc(string procName, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// The set identity.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="setId">
        /// The set id.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id").Single();
            var newId = (T)identity.Id;
            setId(newId);
        }

        /// <summary>
        /// The set property value.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetPropertyValue(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
            {
                return;
            }

            if (!info.CanWrite)
            {
                return;
            }

            info.SetValue(p, value, null);
        }

        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionnName">
        /// The connectionn Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<T> SqlWithParams<T>(string sql, dynamic parms, string connectionnName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionnName))
            {
                return connection.Query<T>(sql, (object)parms).ToList();
            }
        }

        /// <summary>
        /// SQLs the with params single.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// Name of the connection.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T SqlWithParamsSingle<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Query<T>(sql, (object)parms).FirstOrDefault();
            }
        }

        /// <summary>
        /// Stored proc.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="procname">
        /// The procname.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// The connection Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<T> StoredProcWithParams<T>(string procname, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Stored proc with params returning dynamic.
        /// </summary>
        /// <param name="procname">
        /// The procname.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// Name of the connection.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<dynamic> StoredProcWithParamsDynamic(
            string procname,
            dynamic parms,
            string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return connection.Query(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Stored proc with params returning single.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="procname">
        /// The procname.
        /// </param>
        /// <param name="parms">
        /// The parms.
        /// </param>
        /// <param name="connectionName">
        /// Name of the connection.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T StoredProcWithParamsSingle<T>(string procname, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                return
                    connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure)
                        .SingleOrDefault();
            }
        }

        /// <summary>
        /// The to data table.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public DataTable ToDataTable<T>(IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            var values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// Gets the open connection.
        /// </summary>
        /// <param name="connectionString">
        /// The connection String.
        /// </param>
        /// <returns>
        /// The <see cref="SqlConnection"/>.
        /// </returns>
        public SqlConnection GetOpenConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// The insert multiple.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <param name="connectionName">
        /// The connection name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null)
            where T : class, new()
        {
            using (SqlConnection cnn = this.GetOpenConnection(connectionName))
            {
                int records = 0;

                foreach (T entity in entities)
                {
                    records += cnn.Execute(sql, entity);
                }

                return records;
            }
        }

        /// <summary>
        /// Stored proc insert with ID.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object
        /// </typeparam>
        /// <typeparam name="U">
        /// The Type of the ID
        /// </typeparam>
        /// <param name="procName">
        /// Name of the proc.
        /// </param>
        /// <param name="parms">
        /// instance of DynamicParameters class. This should include a defined output parameter
        /// </param>
        /// <param name="connectionName">
        /// The connection Name.
        /// </param>
        /// <returns>
        /// U - the @@Identity value from output parameter
        /// </returns>
        public U StoredProcInsertWithID<T, U>(string procName, DynamicParameters parms, string connectionName = null)
        {
            using (SqlConnection connection = this.GetOpenConnection(connectionName))
            {
                int x = connection.Execute(procName, parms, commandType: CommandType.StoredProcedure);
                return parms.Get<U>("@ID");
            }
        }

        #endregion
    }
}