namespace MedicalLaboratory
{
    using System;
    using System.Collections.Generic;

    public partial class Services
    {
        public Services()
        {
            this.OrderServices = new HashSet<OrderServices>();
            this.UserServices = new HashSet<UserServices>();
            this.Analyzers = new HashSet<Analyzers>(); // Добавлена связь с анализаторами
        }

        public int ServiceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ExecutionTimeDays { get; set; }
        public Nullable<decimal> AverageDeviation { get; set; }
        public bool IsArchived { get; set; }
        public string AvailableAnalyzers { get; set; } // Добавлено поле для анализаторов

        public virtual ICollection<OrderServices> OrderServices { get; set; }
        public virtual ICollection<UserServices> UserServices { get; set; }
        public virtual ICollection<Analyzers> Analyzers { get; set; } // Навигационное свойство
    }
}