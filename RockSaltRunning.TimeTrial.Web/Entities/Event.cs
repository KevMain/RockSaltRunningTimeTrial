using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockSaltRunning.TimeTrial.Web.Entities
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Display(Name = "Distance")]
        public string Distance { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(150)]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}