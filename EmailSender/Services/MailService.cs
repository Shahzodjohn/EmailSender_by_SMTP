using EmailSender.Context;
using EmailSender.Models;
using EmailSender.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        private readonly AppDbContext _context;
        public MailService(IOptions<MailSettings> options, AppDbContext context)
        {
            _settings = options.Value;
            _context = context;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var users = _context.Userss.Select(x => MailboxAddress.Parse( x.Email)).ToList();
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.Mail);
            email.To.AddRange(users);
            //email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            
            var builder = new BodyBuilder();
            if(mailRequest.Attechments != null)
            {
                byte[] filebytes;
                foreach (var file in mailRequest.Attechments)
                {
                    if(file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            filebytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using(var smtp = new SmtpClient())
            {
                smtp.Connect(_settings.Host, _settings.Port,SecureSocketOptions.StartTls);
                smtp.Authenticate(_settings.Mail, _settings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }
    }
}
