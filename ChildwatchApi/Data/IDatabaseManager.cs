using System;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi.Data
{
    /// <summary>
    /// Class that works with a database using a connection. Optimized to keep an open connection while the object is alive.
    /// </summary>
    public abstract class IDatabaseManager : IDisposable
    {
        /// <summary>
        /// Reference to the SqlConnection of this manager.
        /// </summary>
        public  String ConnectionString { get; set; }
        public SqlConnection CurrentConnection { get; internal set; }
        
        /// <summary>
        /// Command that will work on the database.
        /// </summary>
        protected SqlCommand command;

        public IDatabaseManager(SqlConnection connector)
        {
            command = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connector
            };

            ConnectionString = connector.ConnectionString;
            CurrentConnection = connector;
        }
        public IDatabaseManager(string s)
        {
            command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure
            };

            ConnectionString = s;
        }

        ~IDatabaseManager()
        {
            Dispose();
        }
        /// <summary>
        /// Closes the underlying database connection before releasing resources.
        /// </summary>
        public void Dispose()
        {
            if (CurrentConnection != null)
                CurrentConnection.Dispose();
        }
        /// <summary>
        /// Clears and adds the new SqlParameters to the SqlCommand
        /// </summary>
        /// <param name="prams">Array of SqlParameters to add</param>
        protected void AddParameters(SqlParameter[] prams)
        {
            command.Parameters.Clear();
            command.Parameters.AddRange(prams);
        }
        /// <summary>
        /// Clears the parameters of the SQLCommand and adds a new one.
        /// </summary>
        /// <param name="parameter">Sql Parameter to add</param>
        protected void AddParameters(SqlParameter parameter)
        {
            AddParameters(new SqlParameter[]
            {
                parameter
            });
        }
        /// <summary>
        /// Ensures that the connection to the database is open and switches the stored procedure name.
        /// </summary>
        /// <param name="s">Name of the procedure to execute.</param>
        protected void OpenConnection(string s = "")
        {
            CurrentConnection = new SqlConnection(ConnectionString);
            command.Connection = CurrentConnection;
            
            if(!string.IsNullOrEmpty(s))
                command.CommandText = s;

            CurrentConnection.Open();
        }
        protected void CloseConnection()
        {
            if (CurrentConnection != null)
                CurrentConnection.Dispose();
        }
        protected SqlDataReader RunData(string s, SqlParameter[] list)
        {
            OpenConnection(s);
            AddParameters(list);
            SqlDataReader reader = command.ExecuteReader();
            
            return reader;
        }
        protected object RunValue(string s, SqlParameter[] list)
        {
            OpenConnection(s);
            AddParameters(list);
            object done = command.ExecuteScalar();
            CloseConnection();
            return done;
        }
        protected bool Run(string s, SqlParameter[] list)
        {
            OpenConnection(s);
            AddParameters(list);
            bool done = command.ExecuteNonQuery() > 0;
            CloseConnection();
            return done;
        }
        protected void CloseReader(SqlDataReader reader)
        {
            if (!reader.IsClosed)
                reader.Close();
        }

    }
}
