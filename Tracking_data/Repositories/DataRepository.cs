using Microsoft.AspNetCore.Identity;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Tracking_data.Model;
using Tracking_data.Hepler;

namespace Tracking_data.Repositories
{
    public class DataRepository
    {
        
        private readonly oracle_helper _db;

        public DataRepository(oracle_helper db)
        {
            _db = db;
        }



        public User GetByUsername(string emp_no , string class_name)
        {          

            string sql = @"
            SELECT emp_no, emp_name, emp_rank  , password
            FROM SFIS1.c_emp_desc_t
            WHERE emp_no = :emp_no and CLASS_NAME=:class_name
        ";

            var reader = _db.oracle_reader(
                sql,
                new OracleParameter(":emp_no", emp_no),
                new OracleParameter(":class_name", class_name)
            ); 
            if (!reader.Read())
                return null;

            return new User
            {
                emp_no = reader.GetString(0),
                emp_name = reader.GetString(1),
                emp_rank = reader.GetString(2),
                password = reader.GetString(3),
            };
        
    }
    }
}
