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
    
    public partial class Luong_Part
    {
        public int id_Luong_Part { get; set; }
        public string id_Phong_Ban { get; set; }
        public Nullable<int> Luong_Part1 { get; set; }
    
        public virtual Phong_Ban Phong_Ban { get; set; }
    }
}