using System.ComponentModel.DataAnnotations.Schema;

namespace BURAYDAH_CENTRAL.DTOs
{
    public class Pathology_AnalysisDTO
    {
        public string Name { get; set; }
        public string? Result { get; set; }
        
        public int PatientID { get; set; }
    }
}
