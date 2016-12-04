using System;

namespace Common.DTOs
{
    public class SessionDTO : BaseDTO
    {
        public override int Id { get; set; }
        public string AuthToken { get; set; }
        public int UserID { get; set; }
        public DateTime DateCreated { get; set; }

        public UserDTO User { get; set; }
    }
}