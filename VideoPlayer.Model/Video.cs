using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VideoPlayer.Model
{
    public abstract class Video : EntityBase
    {
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Name in English")]
        public string Name_ENG { get; set; }

        [NotMapped]
        public List<Category> Categories { get; set; }

        [Display(Name = "Categories")]
        [Column("Categories")]
        public string CategoriesString
        {
            get { return string.Join(",", Categories); }
            set
            {
                var tmp = value.Split(',').ToList();
                Categories = new List<Category>();
                foreach (string category in tmp)
                    Categories.Add((Category)(Enum.Parse(typeof(Category), category)));
            }
        }

        [Display(Name = "Subtitle URL")]
        [Url]
        public string SubtitleURL { get; set; }

        [Display(Name = "Video URL")]
        [Required]
        [Url]
        public string VideoURL { get; set; }

        [Display(Name = "Cover image URL")]
        [Url]
        public string ImgURL { get; set; }

        [Display(Name = "Trailer URL")]
        [Url]
        public string TrailerURL { get; set; }

        [Display(Name = "IMDB URL")]
        [Url]
        public string ImdbURL { get; set; }
    }
}
