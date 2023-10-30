using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helper
{
    public class EmailTemplate
    {
        public string ReplacePlaceholders(string template, string recipientName, string confirmationToken)
        {
            // Replace placeholders in the HTML template
            template = template.Replace("{{RecipientName}}", recipientName);
            template = template.Replace("{{ConfirmationToken}}", confirmationToken);

            return template;
        }

        public string GetEmailConfirmationTemplate(string recipientName, string confirmationToken)
        {
            string template = @"
                    <!DOCTYPE HTML PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                    <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
                    <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f1f1f1;
                        }

                        p {
                            font-size: 16px;
                            color: #333;
                        }

                        a {
                            text-decoration: none;
                            background-color: #30e3ca;
                            color: #fff;
                            padding: 10px 15px;
                            border-radius: 5px;
                        }
                    </style>
                    </head>
                    <body>
                    <p>Hi {{RecipientName}},</p>
                    <p>Click the following link to confirm your email:</p>
                    <a href=""https://localhost:7090/api/Auth/verify?token={{ConfirmationToken}}"">Confirm Email</a>
                    </body>
                    </html>
                            ";

            // Replace placeholders with actual values
            return ReplacePlaceholders(template, recipientName, confirmationToken);
        }

                public string GetEmailChangeConfirmationTemplate(string recipientName, string confirmationToken)
        {
            string template = @"
                    <!DOCTYPE HTML PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                    <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
                    <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f1f1f1;
                        }

                        p {
                            font-size: 16px;
                            color: #333;
                        }

                        a {
                            text-decoration: none;
                            background-color: #30e3ca;
                            color: #fff;
                            padding: 10px 15px;
                            border-radius: 5px;
                        }
                    </style>
                    </head>
                    <body>
                    <p>Hi {{RecipientName}},</p>
                    <p>Click the following link to confirm your new email:</p>
                    <a href=""https://localhost:7090/api/User/verify?token={{ConfirmationToken}}"">Confirm new Email</a>
                    </body>
                    </html>
                            ";

            // Replace placeholders with actual values
            return ReplacePlaceholders(template, recipientName, confirmationToken);
        }

    }
}
