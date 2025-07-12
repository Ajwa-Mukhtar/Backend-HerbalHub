using Dapper;
using Microsoft.Data.SqlClient;
//using Amazon.S3;
using System.Data;
using System.Linq;

namespace HerbalHub.CommonMethods
{
    public static class CommonMethod
    {
       // private static IAmazonS3 _s3Client;
      //  private static string _bucketName;

      //  // Initialize S3 client and bucket name
      ////  public static void Initialize(IAmazonS3 s3Client, string wasabiBucket)
      //  {
      //      _s3Client = s3Client;
      //      _bucketName = wasabiBucket;
      //  }

        #region Database Helpers

        // Get your database connection string from configuration
        private static string GetConnectionString()
        {
            return "Server=DESKTOP-I9BRQED;Database=ProfessionalGym;Trusted_Connection=True;TrustServerCertificate=True;"; 
        }

        // Execute a raw SQL query and return true if any result is returned
        public static bool ExecuteQuery(string sqlString, DynamicParameters parameters = null)
        {
            string connectionString = GetConnectionString();
            using var sqlConnection = new SqlConnection(connectionString);
            return sqlConnection.Query<bool>(sqlString, parameters).Any();
        }

        // Execute a stored procedure and return a DataSet (used for reports or multiple tables)
        public static DataSet ExecuteStoredProcedureForDataSet(string storedProcedure, DynamicParameters parameters = null)
        {
            string connectionString = GetConnectionString();
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var name in parameters.ParameterNames)
                        {
                            var value = parameters.Get<dynamic>(name);
                            command.Parameters.Add(new SqlParameter(name, value ?? DBNull.Value));
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        adapter.Fill(dataSet);
                    }
                }
            }

            return dataSet;
        }

        // Execute a stored procedure and return the result as a list of dynamic objects
        public static List<dynamic> ExecuteStoredProcedure(DynamicParameters parameters, string storedProcedureName)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query<dynamic>(
                    storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return result;
            }
        }
        // Execute a stored procedure without parameters and return the result as a list of dynamic objects
        public static List<dynamic> ExecuteStoredProcedure(string storedProcedureName)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query<dynamic>(
                    storedProcedureName,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return result;
            }
        }


        #endregion
    }
}
