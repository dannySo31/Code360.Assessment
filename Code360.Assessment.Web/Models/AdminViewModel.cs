using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using Code360.Assessment.Web.Models;

namespace IdentitySample.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel : UserBase
    {

       

        public IEnumerable<SelectListItem> RolesList { get; set; }
       
       
    }
}