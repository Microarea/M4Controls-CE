/*
M4-Controls CE - Tool di completamento delle descrizioni Json a partire dai source C++ 
Copyright (C) 2017 Microarea s.p.a.

This program is free software: you can redistribute it and/or modify it under the 
terms of the GNU General Public License as published by the Free Software Foundation, 
either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, 
but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. 

See the GNU General Public License for more details.
*/


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