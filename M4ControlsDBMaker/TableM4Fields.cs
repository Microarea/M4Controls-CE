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
    internal class TableM4Fields
    {
        
        private static string create =
            @"CREATE TABLE  [dbo].[Fields] (
                                        [TableName] VARCHAR(64) NOT NULL,
                                        [FieldName] VARCHAR(64) NOT NULL,
                                        [FieldNamespace] VARCHAR(64) NULL,
                                        PRIMARY KEY ([TableName], [FieldName])
                                    );";

        public static bool Create()
        {
            return SQLServerManagement.ExecuteNonQuery(create) >= 0;
        }

        public static bool IsEmpty()
        {
            bool e = true;

            string query = "SELECT [FieldNamespace] FROM [Fields]";

            if (SQLServerManagement.ExecuteReader(query, null) != null)
                e = !SQLServerManagement.Read();
            SQLServerManagement.ReaderClose();

            return e;
        }

        public static string GetNamespace(string aTable, string aField)
        {
            string v = string.Empty;
            List<SqlParameter> param =new  List<SqlParameter>();
            param.Add(new SqlParameter("Table", aTable));
            param.Add(new SqlParameter("Field", aField));
            string query = "SELECT [FieldNamespace] FROM [Fields] WHERE [TableName] = @Table AND [FieldName] = @Field";
            SqlDataReader r = SQLServerManagement.ExecuteReader(query, param);
            if (r != null)
            {
                while (r.Read())
                    v = (string)r["FieldNamespace"];
            }
            SQLServerManagement.ReaderClose();

            return v;
        }

        public static int Insert(string aTable, string aField, string aFieldNamespace)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("Table", aTable.Trim()));
            param.Add(new SqlParameter("Field", aField.Trim()));
            param.Add(new SqlParameter("FieldNamespace", aFieldNamespace.Trim()));

            string query = string.Format("INSERT INTO [Fields] ([TableName], [FieldName], [FieldNamespace]) VALUES ( @Table, @Field, @FieldNamespace)");

            return SQLServerManagement.ExecuteNonQuery(query, param);
        }
        public static int Delete()
        {
            string query = "DELETE FROM [Fields]";
            return SQLServerManagement.ExecuteNonQuery(query);
        }

        

    }
}