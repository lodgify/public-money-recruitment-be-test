using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace VacationRental.DAL
{
    public class SqlHelper
    {
        static string connectionString = string.Empty;
        public SqlHelper()
        {
            connectionString = @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename={0}\auxiliarDatabase\AuxiliarDataBase.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True";
            connectionString = string.Format(connectionString, Directory.GetCurrentDirectory());

          
        }


        public  Int32 ExecuteNonQuery( String commandText,
          CommandType commandType, params SqlParameter[] parameters)
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {                    
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
                
        public  Object ExecuteScalar(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public  SqlDataReader ExecuteReader(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }
    }

}