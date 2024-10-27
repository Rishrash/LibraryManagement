using LibraryManagement.Domain.Entities.Base;
using LibraryManagement.Domain.Entities;

public class BorrowingRecord : BaseEntity
{
    public int BookId { get; set; }
    public int UserId { get; set; }
    public virtual Book Book { get; set; }
    public virtual User User { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
}