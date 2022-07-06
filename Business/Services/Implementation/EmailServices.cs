using Business.Services.Interfaces;
using Data.Models.Implementation;
using Mailjet.Client;

using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Business.Services.Implementation
{
    public interface ISMTPProvider
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string body);
    }

    public class MailJetOptions
    {
        public string APIKey { get; set; } = "";
        public string APISecret { get; set; } = "";
        public string Sender { get; set; } = "";
        public string SenderName { get; set; } = "";
    }

    public class MailJetSMTPProvider : ISMTPProvider
    {
        private readonly MailjetClient _client;
        private readonly MailJetOptions _options;

        public MailJetSMTPProvider(IOptions<MailJetOptions> options)
        {
            _options = options.Value;
            _client = new MailjetClient(_options.APIKey, _options.APISecret);
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string body)
        {
            MailjetRequest request = new MailjetRequest
            {
                // Managed to make the example work after checking this: https://github.com/mailjet/mailjet-apiv3-dotnet/issues/70#issuecomment-880691961. This is probably the ugliest design I have ever seen to declare something like this...
                Resource = Mailjet.Client.Resources.SendV31.Resource,
            }
               .Property(Mailjet.Client.Resources.Send.Messages, new JArray {
                    new JObject {
                     {"From", new JObject {
                      {"Email", _options.Sender },
                      {"Name", _options.SenderName }
                      }},
                     {"To", new JArray {
                      new JObject {
                      {"Email", toEmail },
                      {"Name", toName }
                       }
                      }},
                     {"Subject", subject },
                     {"TextPart", body },
                    }
               });

            MailjetResponse response = await _client.PostAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Email was not able to be send. Reason: {response.Content}");
            }
        }
    }

    public abstract class GenericEmailService : IEmailService
    {
        protected readonly ISMTPProvider _provider;

        public GenericEmailService(ISMTPProvider provider)
        {
            _provider = provider;
        }

        public async virtual Task SendPasswordRecoveryAsync(User user)
        {
            var subject = "Click here to reset your password";
            var body = @$"Hello {user.Name} {user.Surname},
                        As requested, please click on the following link to reset your password: PUT LINK HERE!
                        Kind regards,
                        the FlashMEMO team ☺";

            await _provider.SendEmailAsync(user.NormalizedEmail, $"{ user.Name} { user.Surname}", subject, body);
        }

        public async Task SendRegistrationAsync(User user)
        {
            var subject = "Just one more step: please confirm your email";
            var body = @$"Hello {user.Name} {user.Surname},
                        Thank you for registering on our website.
                        Please click on the following link to activate your account: PUT LINK HERE!
                        We look forward to having you on FlashMEMO, and we hope we can aid you in your learning endeavours.
                        See you very soon,
                        the FlashMEMO team ☺";

            await _provider.SendEmailAsync(user.NormalizedEmail, $"{ user.Name} { user.Surname}", subject, body);
        }
    }

    public class MailJetEmailService : GenericEmailService
    {
        public MailJetEmailService(ISMTPProvider provider) : base(provider) { }
    }
}
