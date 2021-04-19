using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class Column
    {
        [Key]
        public int Id { get; set; }
        public int Specialization { get; set; }
        public int Trainer { get; set; }
        public byte IsActual { get; set; }
    }
}
