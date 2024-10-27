using LibraryManagement.Domain.Entities.Base;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class Book : BaseEntity
    {
        public Book()
        {
            BorrowingHistory = new List<BorrowingRecord>();
            Status = BookStatus.Available;
        }

        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public BookStatus Status { get; set; }
        public int TotalCopies { get; set; }
        public int? AvailableCopies { get; set; }
        public virtual ICollection<BorrowingRecord> BorrowingHistory { get; set; }
    }
}