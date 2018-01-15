using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;  

namespace VideoPlayer.Model
{
    public class Film : Video
    {
        [NotMapped]
        public List<string> Actors { get; set; }

        [StringLength(150, MinimumLength = 3)]
        [Display(Name = "Star Actors")]
        public string ListActors
        {
            get
            {
                if (Actors == null)
                    return "";
                return string.Join(",", Actors);
            }
            set { Actors = value.Split(',').ToList(); }
        }

        public int Length { get; set; }
    }
}
