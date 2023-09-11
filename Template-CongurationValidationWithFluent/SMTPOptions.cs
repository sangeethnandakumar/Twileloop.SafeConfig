namespace Template_CongurationValidationWithFluent
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
