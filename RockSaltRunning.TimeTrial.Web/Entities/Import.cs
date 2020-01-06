using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockSaltRunning.TimeTrial.Web.Entities
{
    public class Import
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        [Display(Name = "EventDate")]
        public DateTime EventDate { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        [Display(Name = "ImportDate")]
        public DateTime ImportDate { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        [Display(Name = "IsScannerUploaded")]
        [DefaultValue(true)]
        public bool IsScannerUploaded { get; set; } = false;
    }
}