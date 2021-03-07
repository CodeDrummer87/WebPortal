using System.ComponentModel.DataAnnotations;

namespace PortalGate.Models
{
    public class Industry
    {
        [Key]
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string FullTitle { get; set; }
    }
}
