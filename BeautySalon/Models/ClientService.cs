using System;

namespace BeautySalon.Models
{
    public class ClientService
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public int ServiceID { get; set; }
        public DateTime StartTime { get; set; }
        public string Comment { get; set; }

        // Дополнительные свойства для отображения в интерфейсе
        public string ClientFullName { get; set; }
        public string ServiceTitle { get; set; }
    }
}