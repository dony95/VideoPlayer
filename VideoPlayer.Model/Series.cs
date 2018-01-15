using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VideoPlayer.Model
{
    public class Series : EntityBase
    {
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Name in English")]
        public string Name_ENG { get; set; }

        [Display(Name = "Cover image URL")]
        [Url]
        public string ImgURL { get; set; }

        [Display(Name = "Trailer URL")]
        [Url]
        public string TrailerURL { get; set; }

        [Display(Name = "IMDB URL")]
        [Url]
        public string ImdbURL { get; set; }

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

        [NotMapped]
        public List<string> Actors { get; set; }

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

        public Language Language { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }

    public class Season : EntityBase
    {
        [Display(Name = "Season number")]
        public int SeasonNumber { get; set; }

        [ForeignKey("Series")]
        public int SeriesId { get; set; }
        public virtual Series Series { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
    }

    public class Episode : EntityBase
    {
        [Display(Name = "Episode number")]
        public int EpisodeNumber { get; set; }

        [Display(Name = "Subtitle URL")]
        [Url]
        public string SubtitleURL { get; set; }

        [Display(Name = "Video URL")]
        [Required]
        [Url]
        public string VideoURL { get; set; }

        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }

    }
}
