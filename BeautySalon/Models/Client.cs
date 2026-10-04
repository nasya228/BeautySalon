using System;

namespace BeautySalon.Models
{
    /// <summary>Сущность "Клиент".</summary>
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GenderCode { get; set; }
        public string PhotoPath { get; set; }

        /// <summary>ФИО одной строкой.</summary>
        public string FullName =>
            $"{LastName} {FirstName} {Patronymic}".Trim();
    }
}