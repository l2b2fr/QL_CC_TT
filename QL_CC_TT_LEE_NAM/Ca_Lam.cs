//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QL_CC_TT_LEE_NAM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ca_Lam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ca_Lam()
        {
            this.Cham_Cong_Part_Time = new HashSet<Cham_Cong_Part_Time>();
        }
    
        public string id_Ca_Lam { get; set; }
        public string Ten_Ca { get; set; }
        public Nullable<System.TimeSpan> Gio_Bat_Dau { get; set; }
        public Nullable<System.TimeSpan> Gio_Ket_Thuc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cham_Cong_Part_Time> Cham_Cong_Part_Time { get; set; }
    }
}