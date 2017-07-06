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
    internal class TableM4Tables
    {
        private static string create =
            @"CREATE TABLE  dbo.[Tables] (
                                                [ParentTableName] VARCHAR(64) NOT NULL,
                                                [TableName] VARCHAR(64) NOT NULL,
                                                 PRIMARY KEY ([ParentTableName], [TableName])
                                            );";

        public static bool Create()
        {
            return SQLServerManagement.ExecuteNonQuery(create) >= 0;
        }

        public static bool IsEmpty()
        {
            bool e = true;

            string query = "SELECT [TableName] FROM [Tables]";

            if (SQLServerManagement.ExecuteReader(query, null) != null)
                e = !SQLServerManagement.Read();
            SQLServerManagement.ReaderClose();

            return e;
        }

        public static string GetParentTableName(string aTable)
        {
            string v = string.Empty;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Table", aTable));

            string query = "SELECT [ParentTableName] FROM [Tables] WHERE [TableName] = @Table";

            if (SQLServerManagement.ExecuteReader(query, param) != null)
            {
                while (SQLServerManagement.Read())
                    v = SQLServerManagement.GetValue<string>("ParentTableName");
            }
            SQLServerManagement.ReaderClose();

            return v;
        }

        public static int Insert(string aParentTable, string aTable)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("ParentTable", aParentTable.Trim()));
            param.Add(new SqlParameter("Table", aTable.Trim()));

            string query = string.Format("INSERT INTO [Tables] ([ParentTableName], [TableName]) VALUES ( @ParentTable, @Table)");

            return SQLServerManagement.ExecuteNonQuery(query, param);
        }
        public static int Delete()
        {
            string query = "DELETE FROM [Tables]";
            return SQLServerManagement.ExecuteNonQuery(query);
        }
    }
}