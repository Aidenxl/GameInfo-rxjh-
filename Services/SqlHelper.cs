using Models.DBModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Services
{
    public static class SqlHelper
    {
        private readonly static string connString = "Data Source=118.24.18.189;Initial Catalog=GameInfo;Persist Security Info=True;User ID=sa;Password=ylj970301!";


        public static bool Add<T>(this T model) where T : BaseModel
        {
            Type type = (model).GetType();
            string sql = $"insert into [{type.Name}] ({string.Join(",", type.GetNoIdProperties().Select(i => $"[{i.Name}]"))}) values({string.Join(",", type.GetNoIdProperties().Select(i => $"@{i.Name}"))})";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                List<SqlParameter> parameter = new List<SqlParameter>();

                foreach (var item in type.GetNoIdProperties())
                {
                    parameter.Add(new SqlParameter($"@{item.Name}", item.GetValue(model)));
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameter.ToArray());
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool AddRange<T>(this List<T> model) where T : BaseModel
        {
            Type type = typeof(T);
            string sql = string.Empty;
            int p = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                List<SqlParameter> parameter = new List<SqlParameter>();

                foreach (var t in model)
                {
                    sql += $"insert into [{type.Name}] ({string.Join(",", type.GetNoIdProperties().Select(i => $"[{i.Name}]"))}) values({string.Join(",", type.GetNoIdProperties().Select(i => $"@{i.Name}p"))})";

                    foreach (var item in type.GetNoIdProperties())
                    {
                        parameter.Add(new SqlParameter($"@{item.Name}p", item.GetValue(t)));
                    }

                    p++;
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameter.ToArray());
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Update<T>(this T model) where T : BaseModel
        {
            Type type = (model).GetType();
            string sql = $"update [{type.Name}] set {string.Join(",", type.GetNoIdProperties().Select(i => $"[{i.Name}]=@{i.Name}"))} where Id={model.Id}";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                List<SqlParameter> parameter = new List<SqlParameter>();

                foreach (var item in type.GetNoIdProperties())
                {
                    parameter.Add(new SqlParameter($"@{item.Name}", item.GetValue(model)));
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameter.ToArray());
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static List<T> Query<T>(this T model) where T : BaseModel
        {
            Type type = (model).GetType();
            string sql = $"select {string.Join(",", type.GetProperties().Select(i => $"[{i.Name}]")) } from [{type.Name}]";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                var reder = cmd.ExecuteReader();
                List<T> list = new List<T>();
                while (reder.Read())
                {
                    var tModel = Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(tModel, reder[item.Name] is DBNull ? null : reder[item.Name]);
                    }
                    list.Add((T)tModel);
                }
                return list;
            }
        }

        public static T QueryByCondition<T>(this T model, string Condition, List<SqlParameter> queryParameter) where T : BaseModel
        {
            Type type = (model).GetType();
            string sql = $"select {string.Join(",", type.GetProperties().Select(i => $"[{i.Name}]")) } from [{type.Name}] where {Condition}";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(queryParameter.ToArray());
                var reder = cmd.ExecuteReader();
                if (reder.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(model, reder[item.Name] is DBNull ? null : reder[item.Name]);
                    }
                    return model;
                }
                return null;
            }
        }

        /// <summary>
        /// 获得排除ID外的属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetNoIdProperties(this Type type)
        {
            return type.GetProperties().Where(i => i.Name != "Id");
        }

    }
}
