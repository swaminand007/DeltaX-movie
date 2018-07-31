using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace MoviesDownload.Models
{
    public class Movies
    {
        public Movies()
        {
            this.Actors = new HashSet<Actors>();
        }
        [Key]
        public int MovieId { get; set; }

        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime Yearofrelease  { get; set; }
        public string Plot { get; set; }
        public byte[] Poster { get; set; }

        public int ProducerId { get; set; }
        public  Producers Producer { get; set; }

        public virtual ICollection<Actors> Actors { get; set; }
    }
    public class MovieModel
    {
        public int MovieId { get; set; }
        [Required(ErrorMessage = "Movie Name is required")]
        [StringLength(50, MinimumLength = 02)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Release date is required")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Yearofrelease { get; set; }

        [Required(ErrorMessage = "Plot is required")]
        [StringLength(1000, MinimumLength = 10)]
        public string Plot { get; set; }

        [Required(ErrorMessage = "Poster is required")]
        public byte[] Poster { get; set; }

        public IEnumerable<int> ActorsId { get; set; }
        public int ProducerId { get; set; }

        public List<SelectListItem> Producers { get; set; }
        public List<SelectListItem> Actors { get; set; }

    }
}