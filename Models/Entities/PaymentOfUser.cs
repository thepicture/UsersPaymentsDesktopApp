//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UserPaymentsDesktopApp.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentOfUser
    {
        public int Id { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public int PaymentTypeId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public bool IsPayed { get; set; }
    
        public virtual PaymentType PaymentType { get; set; }
        public virtual User User { get; set; }
    }
}
