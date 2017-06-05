//using Finisar.Sql;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace M4ControlsDBMaker
{
    internal class TableM4ControlsClasses
    {
        private static string create =
            @"CREATE TABLE  dbo.[ControlsClasses] (
                                                [JsonName] VARCHAR(64) NOT NULL,
                                                [ControlClass] VARCHAR(64) NOT NULL,
                                                PRIMARY KEY ([ControlClass])
                                            );";

        public static bool Create()
        {
            return SQLServerManagemant.ExecuteNonQuery(create) >= 0;
        }

        public static bool IsEmpty()
        {
            bool e = true;

            string query = "SELECT [ControlClass] FROM [ControlsClasses]";

            if (SQLServerManagemant.ExecuteReader(query, null) != null)
                e = !SQLServerManagemant.Read();
            SQLServerManagemant.ReaderClose();

            return e;
        }

        public static string GetControlJsonName(string aControlClass)
        {
            string v = string.Empty;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ControlClass", aControlClass));

            string query = "SELECT [JsonName] FROM [ControlsClasses] WHERE [ControlClass] = @ControlClass";

            if (SQLServerManagemant.ExecuteReader(query, param) != null)
            {
                while (SQLServerManagemant.Read())
                    v = SQLServerManagemant.GetValue<string>("JsonName");
            }
            SQLServerManagemant.ReaderClose();

            return v;
        }

        public static int Insert(string aControlClass, string aJsonName)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("ControlClass", aControlClass.Trim()));
            param.Add(new SqlParameter("JsonName", aJsonName.Trim()));

            string query = string.Format("INSERT INTO [ControlsClasses] ([ControlClass], [JsonName]) VALUES ( @ControlClass, @JsonName)");

            return SQLServerManagemant.ExecuteNonQuery(query,param);
        }
        public static int Delete()
        {
            string query = "DELETE FROM [ControlsClasses]";
            return SQLServerManagemant.ExecuteNonQuery(query);
        }
    }
}