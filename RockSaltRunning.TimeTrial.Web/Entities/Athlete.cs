using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockSaltRunning.TimeTrial.Web.Entities
{
    public class Athlete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(15)]
        [Display(Name = "parkrunNo")]
        public string ParkrunNumber { get; set; }
    }
}