using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Code360.Assessment.Web.Models
{
   
    public abstract class UserBase : IdentityUser
    {
        [Display(Name = "Image")]
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Name { get; set; }
       
        [Display(Name = "Photo")]
        
        public byte[] Picture { get; set; }
        [NotMapped]
        public bool HasPicture
        {
            get
            {
                if (Picture == null)
                    return false;
                else
                    return (Picture.Count() != 0);
            }

        }
        [NotMapped]
        public string PictureStr
        {
            get
            {
               
                if (Picture == null)
                    return string.Empty;
                return "data:image/jpg;base64," + Convert.ToBase64String(Picture);
            }

        }
    }
}