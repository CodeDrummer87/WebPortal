using System.ComponentModel.DataAnnotations;

namespace RailroadPortalClassLibrary
{
    public class StaffOfColumn
    {
        [Key]
        public int Id { get; set; }
        public int ColumnId { get; set; }
        public int UserId { get; set; }
        public byte IsCrew { get; set; }
    }
}
