namespace PetsWebsite.Models
{
    public class SendMailTokenIn
    {
        public string Email { get; set; }
    }

    public class SendMailTokenOut
    {
        public string ErrMsg { get; set; }
        public string ResultMsg { get; set; }
    }
}
