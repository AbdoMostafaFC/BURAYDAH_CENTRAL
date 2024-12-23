using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BURAYDAH_CENTRAL.Models
{
    public class Pathology_analysis
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Result  { get; set; }

       
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [JsonIgnore]
        public Patient Patient { get; set; }
    }
}
