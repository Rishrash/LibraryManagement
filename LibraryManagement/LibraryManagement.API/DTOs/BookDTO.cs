namespace LibraryManagement.API.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public int TotalCopies { get; set; }
        public int? AvailableCopies { get; set; }
    }

    public class CreateBookDTO
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int TotalCopies { get; set; }
    }
}