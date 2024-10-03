using DataAccessLayer.BusinessModels;

namespace EXE101_API.Services.EmailService
{
    public interface IEmailService {
        void SendEmail(Message message);
    }
}
