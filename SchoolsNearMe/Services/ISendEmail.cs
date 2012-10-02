namespace SchoolsNearMe.Services
{
    public interface ISendEmail
    {
        void Send(string fromEmail, string body);
    }
}