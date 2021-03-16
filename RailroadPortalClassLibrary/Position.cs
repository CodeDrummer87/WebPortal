using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Abbreviation { get; set; }
        public byte IsActual { get; set; }
    }
}
