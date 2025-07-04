//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalLaboratory
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patients
    {
        public Patients()
        {
            this.Orders = new HashSet<Orders>();
        }
    
        public int PatientId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public string InsurancePolicyType { get; set; }
        public int InsuranceCompanyId { get; set; }
        public byte[] Photo { get; set; }
        public bool IsArchived { get; set; }
    
        public virtual InsuranceCompanies InsuranceCompanies { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
