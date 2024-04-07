
namespace SoftDeleteDemo.Data
{
    public class Book : ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get ; set ; }
    }
}
