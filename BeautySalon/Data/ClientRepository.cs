using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BeautySalon.Models;

namespace BeautySalon.Data
{
    public class ClientRepository
    {
        public List<Client> GetAll()
        {
            var list = new List<Client>();

            const string sql = @"
        SELECT client_id, FirstName, LastName, Patronymic, Birthday, 
               RegistrationDate, Email, Phone, gender_id, PhotoPath
        FROM dbo.Client";

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Client
                        {
                            Id = r.IsDBNull(0) ? 0 : Convert.ToInt32(r.GetValue(0)),
                            FirstName = r.IsDBNull(1) ? "" : Convert.ToString(r.GetValue(1)),
                            LastName = r.IsDBNull(2) ? "" : Convert.ToString(r.GetValue(2)),
                            Patronymic = r.IsDBNull(3) ? null : Convert.ToString(r.GetValue(3)),
                            Birthday = r.IsDBNull(4) ? (DateTime?)null : Convert.ToDateTime(r.GetValue(4)),
                            RegistrationDate = r.IsDBNull(5) ? (DateTime?)null : Convert.ToDateTime(r.GetValue(5)),
                            Email = r.IsDBNull(6) ? null : Convert.ToString(r.GetValue(6)),
                            Phone = r.IsDBNull(7) ? null : Convert.ToString(r.GetValue(7)),
                            GenderCode = r.IsDBNull(8) ? null : Convert.ToString(r.GetValue(8)),
                            PhotoPath = r.IsDBNull(9) ? null : Convert.ToString(r.GetValue(9))
                        });
                    }
                }
            }
            return list;
        }
    }
}