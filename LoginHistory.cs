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
    
    public partial class LoginHistory
    {
        public int LoginHistoryId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Login { get; set; }
        public System.DateTime AttemptTime { get; set; }
        public bool IsSuccess { get; set; }
        public string IPAddress { get; set; }
    
        public virtual SystemUsers SystemUsers { get; set; }
        public object User { get; internal set; }
    }
}
