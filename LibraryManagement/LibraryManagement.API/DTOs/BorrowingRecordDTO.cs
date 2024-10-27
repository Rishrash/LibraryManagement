namespace LibraryManagement.API.DTOs
{
    public class BorrowingRecordDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public BookDTO Book { get; set; }
        public UserDTO User { get; set; }
    }

    public class CreateBorrowingRecordDTO
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
}