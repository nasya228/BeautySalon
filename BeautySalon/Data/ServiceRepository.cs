using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BeautySalon.Models;

namespace BeautySalon.Data
{
    /// <summary>Репозиторий таблицы dbo.Service.</summary>
    public class ServiceRepository
    {
        /// <summary>Возвращает список всех услуг.</summary>
        public List<Service> GetAll()
        {
            var list = new List<Service>();
            const string sql = "SELECT service_id, Title, Cost, DurationInSeconds, Discount, MainImagePath FROM dbo.Service";

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Service
                        {
                            Id = r.IsDBNull(0) ? 0 : Convert.ToInt32(r.GetValue(0)),
                            Title = r.IsDBNull(1) ? "" : Convert.ToString(r.GetValue(1)),
                            Cost = r.IsDBNull(2) ? 0m : Convert.ToDecimal(r.GetValue(2)),
                            DurationInSeconds = r.IsDBNull(3) ? 0 : Convert.ToInt32(r.GetValue(3)),
                            // Discount может быть null или double
                            Discount = r.IsDBNull(4) ? (double?)null : Convert.ToDouble(r.GetValue(4)),
                            MainImagePath = r.IsDBNull(5) ? null : Convert.ToString(r.GetValue(5))
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>Добавляет новую услугу.</summary>
        public void Add(Service service)
        {
            const string sql = @"
                INSERT INTO dbo.Service (Title, Cost, DurationInSeconds, Discount, MainImagePath) 
                VALUES (@Title, @Cost, @Duration, @Discount, @ImagePath);
                SELECT SCOPE_IDENTITY();";

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Title", service.Title);
                cmd.Parameters.AddWithValue("@Cost", service.Cost);
                cmd.Parameters.AddWithValue("@Duration", service.DurationInSeconds);
                cmd.Parameters.AddWithValue("@Discount", (object)service.Discount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ImagePath", (object)service.MainImagePath ?? DBNull.Value);

                conn.Open();
                // Получаем новый ID и записываем его в объект
                service.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>Обновляет существующую услугу.</summary>
        public void Update(Service service)
        {
            const string sql = @"
                UPDATE dbo.Service 
                SET Title = @Title, Cost = @Cost, DurationInSeconds = @Duration, 
                    Discount = @Discount, MainImagePath = @ImagePath
                WHERE service_id = @Id"; // ВАЖНО: WHERE service_id

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", service.Id);
                cmd.Parameters.AddWithValue("@Title", service.Title);
                cmd.Parameters.AddWithValue("@Cost", service.Cost);
                cmd.Parameters.AddWithValue("@Duration", service.DurationInSeconds);
                cmd.Parameters.AddWithValue("@Discount", (object)service.Discount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ImagePath", (object)service.MainImagePath ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Удаляет услугу по ID.</summary>
        public void Delete(int id)
        {
            // ВАЖНО: WHERE service_id
            const string sql = "DELETE FROM dbo.Service WHERE service_id = @Id";

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}