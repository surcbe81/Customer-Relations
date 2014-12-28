//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace CustomerRelations.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.Customers = new HashSet<Customer>();
            this.CustomerNotes = new HashSet<CustomerNote>();
        }
    
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Email Address")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<CustomerNote> CustomerNotes { get; set; }
    }
}
