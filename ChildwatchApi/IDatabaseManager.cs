using System;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi
{
    /// <summary>
    /// Class that works with a database using a connection. Optimized to keep an open connection while the object is alive.
    /// </summary>
    public abstract class IDatabaseManager : IDisposable
    {
        /// <summary>
        /// Reference to the SqlConnection of this manager.
        /// </summary>
        protected  SqlConnection Database
        {
            get { return command.Connection; }
        }
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
            if (Database.State == ConnectionState.Open)
                Database.Close();
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
            if (Database.State == ConnectionState.Closed)
                Database.Open();

            if(!string.IsNullOrEmpty(s))
                command.CommandText = s;
        }
    }
}
