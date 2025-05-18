using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalLaboratory
{
    public partial class SystemUsers
    {
        public SystemUsers()
        {
            this.LoginHistory = new HashSet<LoginHistory>();
            this.UserServices = new HashSet<UserServices>();
        }

        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastEnterTime { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public byte[] Photo { get; set; }
        public bool IsBlocked { get; set; }
        public Nullable<System.DateTime> BlockedUntil { get; set; }
        public bool IsArchived { get; set; }

        // Новое свойство FullName
        public string FullName
        {
            get
            {
                // Формирование полного имени с обработкой null и пустых значений
                return string.Join(" ",
                    new[] { LastName, FirstName, MiddleName }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                );
            }
        }

        public virtual ICollection<LoginHistory> LoginHistory { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual ICollection<UserServices> UserServices { get; set; }
    }
}
