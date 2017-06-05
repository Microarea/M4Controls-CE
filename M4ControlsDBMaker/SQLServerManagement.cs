using System;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace M4ControlsDBMaker
{
    internal class SQLServerManagement
    {
        private static SqlConnection conn = null;
        private static SqlDataReader reader;

        public static string Connection
        {
            get { return string.Format(@"Data Source={0}; Initial Catalog=M4ControlsNew; Integrated Security=True", System.Environment.MachineName); }
        }
        public System.Data.ConnectionState  State{get{ return conn.State; } }


        
        public static bool ClearDB(bool deleteFild)
        {
            List<string> tabelle= new List<string>();
            tabelle.Add(@"[dbo].[Controls]");
            if (deleteFild)
            {
                tabelle.Add(@"[dbo].[Fields]");
                tabelle.Add(@"[dbo].[Tables]");
                tabelle.Add(@"[dbo].[ControlsClasses]");
            }
            conn = new SqlConnection(Connection);
            conn.Open();
            SqlCommand command;
            string query="";
            foreach (string tab in tabelle)
            {
                query =string.Format("DELETE FROM {0} WHERE 1 = 1;", tab);
                command = new SqlCommand(query, conn);
                command.ExecuteNonQuery();
            }
            conn.Close();
            return true;
        }
        public static bool ExistDB(string nameDB = "M4ControlsNew")
        {
            bool result = false; 
            SqlConnection myConn = new SqlConnection(string.Format("Server={0};Integrated security=True;database=master", System.Environment.MachineName));
            SqlCommand c = new SqlCommand("SELECT  name FROM sys.databases WHERE sys.databases.name = @nameDB",myConn);
            c.Parameters.Add(new SqlParameter("nameDB", nameDB));
            using (myConn)
            {
                myConn.Open();
                SqlDataReader reader = c.ExecuteReader();
                while (reader.Read())
                    if (nameDB == (string)reader["name"])
                        result= true;
                myConn.Close();
            }
            return result;
        }
        public static bool ExistDBTable(string table,string nameDB = "M4ControlsNew")
        {
            bool result = false;
            SqlConnection myConn = new SqlConnection(string.Format("Server={0};Integrated security=True;database=master", System.Environment.MachineName));
            SqlCommand c = new SqlCommand(string.Format("SELECT TABLE_NAME FROM {0}.INFORMATION_SCHEMA.Tables",nameDB), myConn);
            using (myConn)
            {
                myConn.Open();
                SqlDataReader reader = c.ExecuteReader();
                while (reader.Read())
                {

                    string s = (string)reader["TABLE_NAME"];
                    if (table == (string)reader["TABLE_NAME"])
                        result = true;
                }
                    
                myConn.Close();
            }
            return result;
        }


        public static bool CreateDB(string dbName)
        {
            bool result = true;
            SqlCommand myCommand;
            SqlConnection myConn = new SqlConnection(string.Format("Server={0};Integrated security=True;database=master", System.Environment.MachineName));
            if (!ExistDB())
            {
                string str;
                
                str = string.Format("CREATE DATABASE {0}",dbName);

                myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message, "CREATE DATABASE");
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                        myConn.Close();
                    
                }
            }
            conn = new SqlConnection(Connection);
            conn.Open();
                    if (!ExistDBTable("Fields"))
                        if (TableM4Fields.Create())
                            result = false;
                    if (!ExistDBTable("Tables"))
                        if (TableM4Tables.Create())
                        result = false;
                    if (!ExistDBTable("ControlsClasses"))
                        if (TableM4ControlsClasses.Create())
                            result = false;
                    if (!ExistDBTable("Controls"))
                        if (TableM4Controls.Create())
                        result = false;
                    if (!ExistDBTable("Label"))
                        if (TableM4Label.Create())
                            result = false;
            if (conn.State == ConnectionState.Open)
                        conn.Close();
            return result;
        }

        public static bool OpenDB(string dbName)
        {
            try
            {
                if(conn==null )
                    conn = new SqlConnection(Connection);
                if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                    conn.ConnectionString = Connection;
                if(conn.State!= ConnectionState.Open)
                    conn.Open();
            }
            catch (SqlException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                return false;
            }
            return true;
        }

        public static void CloseDB()
        {
            if (conn != null)
                conn.Close();

            conn = null;
        }
        
        public static int ExecuteNonQuery(string pCmdText, List<SqlParameter> paramitar = null)
    {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand command = new SqlCommand(pCmdText, conn);
            if(paramitar!=null)
                foreach (SqlParameter p in paramitar)
                    command.Parameters.Add(p);
            try
            {
                return command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message, "ExecuteNonQuery");
            }
            return -1;
        }
        
        public static object ExecuteScalar(string pCmdText, SqlParameter[] parameters = null)
        {
            SqlCommand comm = new SqlCommand(pCmdText, conn);
            if (parameters != null)
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);
            try
            {
                return comm.ExecuteScalar();
            }
            catch (SqlException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message, "ExecuteScalar");
            }
            return -1;
        }
        
        public static SqlDataReader ExecuteReader(string pCmdText, List<SqlParameter> parameters = null)
        {
            SqlCommand comm = new SqlCommand(pCmdText, conn);
            if (parameters != null)
                foreach (SqlParameter param in parameters)
                    comm.Parameters.Add(param);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                reader = comm.ExecuteReader();
                return reader;
            }
            catch (SqlException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message, "ExecuteReader");
            }
            return null;
        }

        public static bool Read()
        {
            if (reader != null)
                return reader.Read();
            else
                return false;
        }

        public static void ReaderClose()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
        }

        public static T GetValue<T>(string column)
        {
            try
            {
                if (reader != null)
                    return (T)Convert.ChangeType(reader[column], typeof(T));
                else
                    return default(T);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message, "GetValue");
            }
            return default(T);
        }

        
    }
}