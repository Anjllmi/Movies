using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class Movie : BaseModel
    {
        //Ya no usa Id porque el Id lo hereda de BaseModel
        ////[Key]
        //public int Id { get; set; }

        //Crear propiedad rápidamente con «prop» + TAB + TAB    
        //Signo ? para permitir valores nulos
        [Required]
        public int? Year { get; set; }

        [Required]
        public string Title { get; set; }
        public int Duration { get; set; }

        //Llave foránea entre movie y category
        public int? CategoryForeignKey { get; set; }

        [ForeignKey("CategoryForeignKey")]
        public Category Category { get; set; }

    }
}
