using System.Collections.Generic;
using System.Text;
using CoreDdd.Utilities.Extensions;
using EmailMaker.Core;
using EmailMaker.Domain.Emails;

namespace EmailMaker.Service.Handlers
{
    public class EmailHtmlBuilder : IEmailHtmlBuilder
    {
        public string BuildHtmlEmail(IEnumerable<EmailPart> emailParts)
        {
            var sb = new StringBuilder();
            emailParts.Each(part =>
            {
                if (part is HtmlEmailPart)
                {
                    sb.Append(((HtmlEmailPart)part).Html);
                }
                else if (part is VariableEmailPart)
                {
                    sb.Append(((VariableEmailPart)part).Value);
                }
                else
                {
                    throw new EmailMakerException("Unknown part type: " + part);
                }

            });
            return sb.ToString();
        }
    }
}