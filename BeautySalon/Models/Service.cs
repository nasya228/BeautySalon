namespace BeautySalon.Models
{
    /// <summary>Сущность "Услуга салона красоты".</summary>
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public int DurationInSeconds { get; set; }
        public double? Discount { get; set; }
        public string MainImagePath { get; set; }

        /// <summary>Длительность в минутах для отображения.</summary>
        public int DurationInMinutes => DurationInSeconds / 60;

        /// <summary>Стоимость с учётом скидки.</summary>
        public decimal CostWithDiscount =>
            Discount.HasValue
                ? Cost * (1 - (decimal)Discount.Value / 100m)
                : Cost;

        /// <summary>Признак наличия скидки.</summary>
        public bool HasDiscount => Discount.HasValue && Discount.Value > 0;
    }
}