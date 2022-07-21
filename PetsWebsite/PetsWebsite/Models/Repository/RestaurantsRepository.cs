using System.Data.SqlClient;

namespace PetsWebsite.Models.Repository
{
    public class RestaurantsRepository
    {
        private readonly DbConfig _dbConfig;
        private SqlConnection _connection;

        public RestaurantsRepository(DbConfig dbConfig)
        {
            _dbConfig = dbConfig;
            _connection = new SqlConnection(_dbConfig.ConnectionString);
        }
        public List<string> GetCity() {
            //裝載資料
            _connection.Open();
            SqlCommand cmd = _connection.CreateCommand();
            string Sql = "Select distinct City From Restaurants";
            cmd.CommandText = Sql;
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            List<string> data = new List<string>();
            while (reader.Read())
            {
                data.Add(Convert.ToString(reader["City"]));
            }
            _connection.Close();
            return data;
        }

        public List<string> GetRegion() {
            string SqlR = "Select distinct Region From Restaurants";
            _connection.Open();
            SqlDataReader reader2 = new SqlCommand(SqlR, _connection).ExecuteReader();
            List<string> data2 = new List<string>();
            while (reader2.Read())
            {
                data2.Add(Convert.ToString(reader2["Region"]));
            }
            _connection.Close();
            return data2;
        }
    }
}
