//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demo.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Clients
    {
        public Clients()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int ClientID { get; set; }
        public string ClientFullName { get; set; }
        public int Password { get; set; }
        public string Address { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
