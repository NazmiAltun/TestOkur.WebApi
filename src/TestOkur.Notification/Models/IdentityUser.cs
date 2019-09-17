namespace TestOkur.Notification.Models
{
    using System;

    public class IdentityUser
    {
        public string Email { get; set; }

        public string Id { get; set; }

        public DateTime? ExpiryDateUtc { get; set; }

        public bool Active { get; set; }
    }
}
