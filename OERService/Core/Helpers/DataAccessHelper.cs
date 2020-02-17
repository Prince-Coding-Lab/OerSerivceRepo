using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class DataAccessHelper : IDisposable
    {
        private SqlCommand command;

        /// <summary>
        /// Returns connneciton string form configuration 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public string GetConnection(IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection");
        }


        /// <summary>
        /// Run SP without parameters
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="configuration"></param>
        public DataAccessHelper(string sprocName, IConfiguration configuration)
        {
            //creating command object with connection name and proc name, and open connection for the command
            command = new SqlCommand(sprocName, new SqlConnection(GetConnection(configuration)));
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(
                new SqlParameter("ReturnValue",
                    SqlDbType.Int,
                /* int size */ 4,
                    ParameterDirection.ReturnValue,
                /* bool isNullable */ false,
                /* byte precision */ 0,
                /* byte scale */ 0,
                /* string srcColumn */ string.Empty,
                    DataRowVersion.Default,
                /* value */ null
                )
            );
            command.CommandTimeout = 0;
            command.Connection.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        /// <param name="configuration"></param>
        public DataAccessHelper(string sprocName, SqlParameter[] parameters, IConfiguration configuration)
        {
            // prepare a command object with procedure and parameters
            command = new SqlCommand(sprocName, new SqlConnection(GetConnection(configuration)));
            command.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter parameter in parameters)
                command.Parameters.Add(parameter);

            command.Parameters.Add(
                new SqlParameter("ReturnValue",
                    SqlDbType.Int,
                /* int size */ 4,
                    ParameterDirection.ReturnValue,
                /* bool isNullable */ false,
                /* byte precision */ 0,
                /* byte scale */ 0,
                /* string srcColumn */ string.Empty,
                    DataRowVersion.Default,
                /* value */ null
                )
            );
            command.CommandTimeout = 0;
            command.Connection.Open();
        }


        /// <summary>
        /// Run command with executeNonQuery
        /// </summary>
        /// <returns></returns>
       public async Task<int> RunAsync()
        {
            return await Task.Run (()=>
            {
                // Execute this stored procedure.  Int32 value returned by the stored procedure
                if (command == null)
                    throw new ObjectDisposedException(GetType().FullName);
                command.ExecuteNonQuery();
                return (int)command.Parameters["ReturnValue"].Value;
            });
        }
    /// <summary>
    /// Run command  with data adapter: fill datatable 
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public async Task<int> RunAsync(DataTable dataTable)
    {

       return await Task.Run(() =>
        {
            //	Fill a DataTable with the result of executing this stored procedure.
            if (command == null)
                throw new ObjectDisposedException(GetType().FullName);

            SqlDataAdapter dataAdapter = new SqlDataAdapter();

            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataTable);

            return (int)command.Parameters["ReturnValue"].Value;
        });        
       
    }


    /// <summary>
    /// Run command  with data adapter: fill dataset
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>

    public async Task<int> RunAsync(DataSet dataSet)
     {
          return  await Task.Run(() =>
            {
                //	Fill a DataSet with the result of executing this stored procedure.
                if (command == null)
                    throw new ObjectDisposedException(GetType().FullName);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(dataSet);
                return (int)command.Parameters["ReturnValue"].Value;
            });
        }


        /// <summary>
        /// Dispose connection and command objects
        /// </summary>
        public void Dispose()
        {
            //	Dispose of this StoredProcedure.
            if (command != null)
            {
                SqlConnection connection = command.Connection;
                Debug.Assert(connection != null);
                connection.Close();
                command.Dispose();
                command = null;
                connection.Dispose();
            }
        }

    }
}
