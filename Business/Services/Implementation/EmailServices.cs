using Business.Services.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implementation
{
    public abstract class GenericEmailService : IEmailService
    {
        public virtual Task SendPasswordRecoveryAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task SendRegistrationAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class MailJetOptions
    {
        public string APIKey { get; set; } = "";
        public string APISecret { get; set; } = "";
    }

    public class MailJetEmailService : GenericEmailService
    {
        private readonly MailJetOptions _options;

        public MailJetEmailService(IOptions<MailJetOptions> options)
        {
            _options = options.Value;
        }

        public override async Task SendRegistrationAsync()
        {
            MailjetClient client = new MailjetClient(_options.APIKey, _options.APISecret);

            MailjetRequest request = new MailjetRequest
            {
                // Managed to make the example work after checking this: https://github.com/mailjet/mailjet-apiv3-dotnet/issues/70#issuecomment-880691961. This is probably the ugliest design I have ever seen to declare something like this...
                Resource = SendV31.Resource,
            }
               .Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "flashmemo.mail@gmail.com"},
                  {"Name", "FlashMEMO"}
                  }},
                 {"To", new JArray {
                  new JObject {
                  {"Email", "flashmemo.mail@gmail.com"},
                  {"Name", "FlashMEMO"}
                   }
                  }},
                 {"Subject", "Your email flight plan!"},
                 {"TextPart", "Dear passenger 1, welcome to Mailjet! May the delivery force be with you!"},
                 {"HTMLPart", "<h3>Dear passenger 1, welcome to <a href=\"https://www.mailjet.com/\">Mailjet</a>!</h3><br />May the delivery force be with you!"}
                 }
                   });

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }
    }
}
