using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalLaboratory
{
    [Table("Users")] // Указываем имя таблицы, если оно отличается
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public DateTime? LastEnter { get; set; }
        public string Services { get; set; }
        public int Type { get; set; }

        public virtual ICollection<LoginHistory> LoginHistories { get; set; }
    }
}