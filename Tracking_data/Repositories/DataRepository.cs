using Microsoft.AspNetCore.Identity;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Tracking_data.Model;
using Tracking_data.Hepler;
using Microsoft.Data.SqlClient;

namespace Tracking_data.Repositories
{
    public class DataRepository
    {
        
        private readonly oracle_helper _db_orcl;
        private readonly sql_helper _sql_helper;
        public DataRepository(oracle_helper db , sql_helper sql_Helper)
        {
            _db_orcl = db;
            _sql_helper = sql_Helper;
        }



        public User GetByUsername(string emp_no , string class_name)
        {          

            string sql = @"
            SELECT emp_no, emp_name, emp_rank  , password
            FROM SFIS1.c_emp_desc_t
            WHERE emp_no = :emp_no and CLASS_NAME=:class_name
        ";

            var reader = _db_orcl.oracle_reader(
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

        public Avc_userinfo user_info(string emp , string c_mail)
        {
            string sql = "SELECT emp_no , emp_cname , emp_birthday , emp_efdte1 , emp_haddr , emp_qdate , emp_email " +
                "FROM RMS.dbo.emp_mstr a " +
                "where    a.emp_no  = @emp_no or lower(emp_email) like @emp_email";
            var reader = _sql_helper.sql_reader(sql, 
                                                    new SqlParameter("@emp_no", emp),
                                                    new SqlParameter("@emp_email","%"+ c_mail +"%")
                                                    );
            if (!reader.Read())
            {
                return null;
            }    
            return new Avc_userinfo
            {
                emp_no = reader["emp_no"]?.ToString(),
                emp_name = reader["emp_cname"]?.ToString(),
                emp_birthday = reader["emp_birthday"]?.ToString(),
                emp_inAVCdate = reader["emp_efdte1"]?.ToString(),
                emp_address = reader["emp_haddr"]?.ToString(),
                emp_qdate = reader["emp_qdate"]?.ToString(),
                email = reader["emp_email"]?.ToString(),
            };
        }
    }
}
