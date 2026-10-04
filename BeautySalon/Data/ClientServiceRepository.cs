using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BeautySalon.Models;

namespace BeautySalon.Data
{
    /// <summary>Репозиторий таблицы dbo.ClientService.</summary>
    public class ClientServiceRepository
    {
        /// <summary>Возвращает ближайшие записи (StartTime >= сейчас).</summary>
        // В файле ClientServiceRepository.cs
        // Внутри ClientServiceRepository.cs
        public List<ClientService> GetUpcoming()
        {
            var list = new List<ClientService>();

            // Исправляем на client_service_id, client_id, service_id
            const string sql = @"
        SELECT cs.client_service_id, cs.client_id, cs.service_id, cs.StartTime, cs.Comment,
               c.LastName + ' ' + c.FirstName AS ClientName,
               s.Title
        FROM dbo.ClientService cs
        INNER JOIN dbo.Client  c ON c.client_id = cs.client_id
        INNER JOIN dbo.Service s ON s.service_id = cs.service_id
        WHERE cs.StartTime >= GETDATE()
        ORDER BY cs.StartTime;";

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new ClientService
                        {
                            Id = r.GetInt32(0), // client_service_id
                            ClientID = r.GetInt32(1),
                            ServiceID = r.GetInt32(2),
                            StartTime = r.GetDateTime(3),
                            Comment = r.IsDBNull(4) ? null : r.GetString(4),
                            ClientFullName = r.IsDBNull(5) ? "" : r.GetString(5),
                            ServiceTitle = r.IsDBNull(6) ? "" : r.GetString(6)
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>Добавляет запись клиента на услугу.</summary>
        public void Add(ClientService cs)
        {
            const string sql = @"
                INSERT INTO dbo.ClientService (client_id, service_id, StartTime, Comment)
                VALUES (@ClientID, @ServiceID, @StartTime, @Comment);";

            using (var conn = DatabaseContext.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ClientID", cs.ClientID);
                cmd.Parameters.AddWithValue("@ServiceID", cs.ServiceID);
                cmd.Parameters.AddWithValue("@StartTime", cs.StartTime);
                cmd.Parameters.AddWithValue("@Comment", (object)cs.Comment ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Удаляет запись по ID.</summary>
        public void Delete(int id)
        {
            // В методе Delete
            // Было: WHERE id = @Id
            // Стало:
            const string sql = "DELETE FROM dbo.ClientService WHERE client_service_id = @Id;";
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