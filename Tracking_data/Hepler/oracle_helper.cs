using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Tracking_data.Hepler
{
    public class oracle_helper
    {
        private readonly IConfiguration _config;
        public oracle_helper(IConfiguration config)
        {
            _config = config;
        }
        public async Task<int> ExecuteoracleNonQueryAsync(string sql)
        {
            using var conn = new OracleConnection(_config.GetConnectionString("OracleConnection"));
            using var cmd = new OracleCommand(sql, conn);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync(); // trả về số dòng bị ảnh hưởng
        }
        public async Task<DataTable> QueryOracleAsync(string query)
        {
            var dt = new DataTable();
            using var conn = new OracleConnection(_config.GetConnectionString("OracleConnection"));
            using var cmd = new OracleCommand(query, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }


        public OracleDataReader oracle_reader(string sql ,params OracleParameter[] parameters)
        {
            var conn = new OracleConnection( _config.GetConnectionString("OracleConnection"));
            conn.Open();

            var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
