using LibraryManagement.Domain.Entities.Base;

namespace LibraryManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            BorrowingHistory = new List<BorrowingRecord>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ICollection<BorrowingRecord> BorrowingHistory { get; set; }
        
    }
}