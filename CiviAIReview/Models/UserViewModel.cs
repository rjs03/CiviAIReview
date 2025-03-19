﻿namespace CiviAIReview.Models
{
    public class UserViewModel
    {
        public string UserID { get; set; }
        public string MemberType { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
