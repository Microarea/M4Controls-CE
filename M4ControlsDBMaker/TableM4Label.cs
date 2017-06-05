
using System.Collections.Generic;
using System.Data.SqlClient;

namespace M4ControlsDBMaker
{
    internal class TableM4Label
    {
        private static string create =
            @"CREATE TABLE  dbo.[Label] ( [IDC] VARCHAR(128) primary key NOT NULL);";

        public static bool Create()
        {
            return SQLServerManagement.ExecuteNonQuery(create) >= 0;
        }

        public static bool IsEmpty()
        {
            bool e = true;

            string query = "SELECT [IDC] FROM [Label]";

            if (SQLServerManagement.ExecuteReader(query, null) != null)
                e = !SQLServerManagement.Read();
            SQLServerManagement.ReaderClose();

            return e;
        }

        public static bool exist(string IDC)
        {
            string v = string.Empty;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@IDC", IDC));

            string query = "SELECT [IDC] FROM [Label] WHERE [IDC] = @IDC";

            if (SQLServerManagement.ExecuteReader(query, param) != null)
            {
                while (SQLServerManagement.Read())
                    v = SQLServerManagement.GetValue<string>("IDC");
            }
            SQLServerManagement.ReaderClose();

            return v == IDC;
        }

        public static int Insert(string IDC)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("IDC", IDC.Trim()));
            

            string query = string.Format("INSERT into [Label] ([IDC]) VALUES (@IDC)");

            return SQLServerManagement.ExecuteNonQuery(query, param);
        }
        public static int Delete()
        {
            string query = "DELETE FROM [Label]";
            return SQLServerManagement.ExecuteNonQuery(query);
        }
    }
}