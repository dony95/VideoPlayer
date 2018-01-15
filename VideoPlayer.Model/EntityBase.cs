using System;
using System.ComponentModel.DataAnnotations;

namespace VideoPlayer.Model
{
    public abstract class EntityBase
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public string Description { get; set; }

        public int Year { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
