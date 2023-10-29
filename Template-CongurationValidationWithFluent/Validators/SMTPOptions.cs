namespace Template_CongurationValidationWithFluent.Validators
{
    public enum ServerType
    {
        GOOGLE,
        MICROSOFT
    }

    public class SMTPOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string ServerType { get; set; }
    }
}
