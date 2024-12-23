namespace BURAYDAH_CENTRAL.Models
{
    public class Patient
    {

        public int Id { get; set; }
        public int UniqueNumber { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public IList<Pathology_analysis> pathology_Analyses { get; set; }=new List<Pathology_analysis>();
        public DateTime DateTime { get; set; }
    }
}
