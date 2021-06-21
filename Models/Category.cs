using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class Category: BaseModel
    {
        //public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        
    }
}
