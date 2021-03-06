//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardsProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BusinessCard
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public string DOB { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }
        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
