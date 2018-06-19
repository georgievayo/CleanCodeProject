using System;

namespace FindAndBook.Models
{
    public class Token
    {
        public string Value { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
