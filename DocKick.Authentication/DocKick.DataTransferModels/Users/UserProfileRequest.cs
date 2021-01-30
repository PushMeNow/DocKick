namespace DocKick.DataTransferModels.Users
{
    public record UserProfileRequest
    {
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Profession { get; set; }
    }
}