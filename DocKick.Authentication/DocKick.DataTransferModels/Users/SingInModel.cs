namespace DocKick.DataTransferModels.Users
{
    public record SingInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}