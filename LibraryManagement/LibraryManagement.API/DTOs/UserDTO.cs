namespace LibraryManagement.API.DTOs
{
    public class UserDTO
    {
        public UserDTO()
        {
            BorrowingHistory = new List<BorrowingRecordDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<BorrowingRecordDTO> BorrowingHistory { get; set; }
    }

    public class CreateUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}