using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockSaltRunning.TimeTrial.Web.Entities
{
    public class TimerResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Import")]
        public int ImportId { get; set; }

        [ForeignKey("Athlete")]
        public int? AthleteId { get; set; }

        [Required]
        [Display(Name = "Position")]
        public int Position { get; set; }

        [Required]
        public int Time { get; set; }

        public Import Import { get; set; }

        public Athlete Athlete { get; set; }
    }
}