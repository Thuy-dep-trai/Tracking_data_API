using Microsoft.Data.SqlClient;
using System.Data;

namespace Tracking_data.Hepler
{
    public class sql_helper
    {
        private readonly IConfiguration _config;
        public sql_helper(IConfiguration config)
        {
            _config = config;
        }
        public async Task<DataTable> QuerySqlServerAsync(string query)
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(_config.GetConnectionString("SqlServerConnection"));
            using var cmd = new SqlCommand(query, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }


        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("SqlServerConnection"));
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync(); // trả về số dòng bị ảnh hưởng
        }
    }
}
