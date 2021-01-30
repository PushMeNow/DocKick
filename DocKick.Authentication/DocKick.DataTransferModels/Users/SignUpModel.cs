namespace DocKick.DataTransferModels.Users
{
    public record SignUpModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}