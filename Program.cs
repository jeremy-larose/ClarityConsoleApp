using System;
using System.Collections.Generic;
using ClarityConsole.Data;
using ClarityConsole.Helpers;
using ClarityMailLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ClarityConsole
{
    class Program
    {
        private const string CommandListEmails = "l";
        private const string CommandListEmail = "i";
        private const string CommandAddEmail = "a";
        private const string CommandDeleteEmail = "d";
        private const string CommandCancel = "c";
        private const string CommandQuit = "q";
        private const string CommandSendAllEmail = "s";

        private static List<string> _recipients = new List<string>();
        private const int Retries = 3;
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            string command = CommandListEmails;
            IList<int> emailIds = null;

            var configuration = new ConfigurationBuilder()
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IClarityMail, ClarityMail>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.RollingFile(@"C:\Logs\ClarityConsole-{Date}A.txt", retainedFileCountLimit: 10 )
                .CreateLogger();
            
            try
            {
                Log.Information("Application Starting Up.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The application failed to start correctly.");
                throw;
            }
            finally
            {
                Log.Information("Application Finished.");
                Log.CloseAndFlush();
            }

            while (command != CommandQuit)
            {
                switch (command)
                {
                    case CommandListEmails:
                        emailIds = ListEmails();
                        break;
                    case CommandAddEmail:
                        AddEmail();
                        command = CommandListEmails;
                        continue;
                    case CommandSendAllEmail:
                        SendAllEmails();
                        break;
                    default:
                        if (AttemptDisplayEmail(command, emailIds))
                        {
                            command = CommandListEmails;
                            continue;
                        }
                        else
                        {
                            ConsoleHelper.OutputLine("Sorry, I didn't understand that line.");
                        }

                        break;
                }

                // List the available commands
                ConsoleHelper.OutputBlankLine();
                ConsoleHelper.Output("Commands: ");
                int emailCount = Repository.GetEmailCount();
                if (emailCount > 0)
                {
                    ConsoleHelper.Output($"Enter a Number 1-{emailCount} | ");
                }

                ConsoleHelper.OutputLine("[S]end All, [A]dd, [D]elete, or [Q]uit :", false);

                // Get the  next command from user.
                command = ConsoleHelper.ReadInput("Enter a command: ", true);
            }
        }

        private static bool AttemptDisplayEmail(string command, IList<int> emailIds)
        {
            var successful = false;
            int? emailId = null;

            if (emailIds != null)
            {
                int lineNumber = 0;
                int.TryParse(command, out lineNumber);

                if (lineNumber > 0 && lineNumber <= emailIds.Count)
                {
                    emailId = emailIds[lineNumber - 1];
                    successful = true;
                }
            }

            // Successfully returned an emailId
            if (emailId != null)
            {
                DisplayEmail(emailId.Value);
            }

            return successful;
        }

        private static void AddEmail()
        {
            ConsoleHelper.ClearOutput();
            ConsoleHelper.OutputLine("ADD EMAIL ADDRESS");

            // Get the email details from the user 
            var email = new ClarityMail()
            {
                MailDisplayName = GetMailDisplayName(),
                MailFrom = GetMailFrom(),
                MailTo = GetMailTo(),
                MailSubject = GetMailSubject(),
                MailBody = GetMailBody()
            };
            Repository.AddEmail(email);
        }
        private static string GetMailDisplayName() => ConsoleHelper.ReadInput("Enter the display name of the sender: " );
        private static string GetMailBody() => ConsoleHelper.ReadInput("Enter the body of the email: ");
        private static string GetMailFrom() => ConsoleHelper.ReadInput("Enter the return address for the email: ");
        private static string GetMailSubject() => ConsoleHelper.ReadInput("Enter the subject for the email: ");
        private static string GetMailTo() => ConsoleHelper.ReadInput("Enter the email address of the recipient: ");


        private static async void SendAllEmails()
        {
            ConsoleHelper.ClearOutput();
            ConsoleHelper.OutputLine("SEND EMAIL");
            foreach (var email in Repository.GetEmails())
            {
                // NOTE: Uncommenting below will result in sending via actual SMTP service and will require changing appsettings.json
                // await email.SendAsync(email.MailTo, email.MailTo, email.MailFrom, email.MailSubject, email.MailBody, Retries);

                // This sends via localhost port 35 with SSL disabled for Papercut testing. 
                await email.SendTestAsync(email.MailDisplayName, email.MailTo, email.MailFrom, email.MailSubject, email.MailBody, Retries);
            }
        }

        private static IList<int> ListEmails()
        {
            var EmailIds = new List<int>();
            IList<ClarityMail> emails = Repository.GetEmails();
            ConsoleHelper.ClearOutput();
            ConsoleHelper.OutputLine("EMAILS:");
            ConsoleHelper.OutputBlankLine();

            foreach (var email in emails)
            {
                EmailIds.Add(email.Id);
                ConsoleHelper.OutputLine($"{emails.IndexOf(email) + 1}) {email.MailTo}");
            }

            return EmailIds;
        }

        private static void DisplayEmail(int emailId)
        {
            string command = CommandListEmail;

            while (command != CommandCancel)
            {
                switch (command)
                {
                    case CommandListEmails:
                        ListEmail(emailId);
                        break;
                    case CommandDeleteEmail:
                        if (DeleteEmail(emailId))
                        {
                            command = CommandCancel;
                        }
                        else
                        {
                            command = CommandListEmail;
                        }
                        continue;
                    default:
                        ConsoleHelper.OutputLine("Sorry, I don't understand that command.");
                        break;
                }

                ConsoleHelper.OutputBlankLine();
                ConsoleHelper.Output("Commands: ");
                ConsoleHelper.OutputLine("D - Delete, C - Cancel", false);
                // Get the next command
                command = ConsoleHelper.ReadInput("Enter a Command: ", true);
            }
        }

        private static bool DeleteEmail(int emailId)
        {
            var successful = false;

            string input = ConsoleHelper.ReadInput("Are you sure you want to delete this email (Y/N)? ", true);

            // If the user entered "y", delete the email
            if (input == "y")
            {
                Repository.DeleteEmail(emailId);
                successful = true;
            }

            return successful;
        }

        private static void ListEmail(int emailId)
        {
            ClarityMail email = Repository.GetEmail(emailId);
            ConsoleHelper.ClearOutput();

            ConsoleHelper.OutputLine("EMAIL DETAIL");
            ConsoleHelper.OutputLine($"Email From: {email.MailFrom}");
            ConsoleHelper.OutputLine($"Email To: {email.MailTo}");
            ConsoleHelper.OutputLine($"Email Subject: {email.MailSubject}");
            ConsoleHelper.OutputLine($"Email Body: {email.MailBody}");
        }
    }
}