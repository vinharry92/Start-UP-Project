using System.Threading.Tasks;
namespace No_Core_Auth.Email
{
    public interface IEmailSender
    {
        // Sends Email with the given information

        Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}
