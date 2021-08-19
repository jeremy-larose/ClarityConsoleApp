using System;
using System.Data.Entity;
using ClarityMailLibrary;

namespace ClarityConsole.Data
{
    /// <summary>
    /// Custom database initializer class used to populate
    /// the database with seed data.
    /// </summary>
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            ClarityMail email1 = new()
            {
                MailTo = "jlarose@gmail.com",
                MailFrom = "clarity@clarityventures.com",
                MailSubject = "Testing this new email server",
                MailBody = "This is just a test."
            };
            ClarityMail email2 = new()
            {
                MailTo = "mlarose@gmail.com",
                MailFrom = "clarity@clarityventures.com",
                MailSubject = "Testing this new email server",
                MailBody = "This is just a test."
            };
            ClarityMail email3 = new()
            {
                MailTo = "kblouw@gmail.com",
                MailFrom = "clarity@clarityventures.com",
                MailSubject = "Testing this new email server",
                MailBody = "This is just a test."
            };

            context.Emails.Add( email1 );
            context.Emails.Add( email2 );
            context.Emails.Add( email3 );
            context.SaveChanges();
        }
    }
}
