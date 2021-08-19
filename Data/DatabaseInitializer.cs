using System;
using System.Data.Entity;
using ClarityEmailerLibrary;

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
/*
            var comicBook4 = new ComicBook()
            {
                Series = seriesIronMan,
                IssueNumber = 1,
                Description = "A.I.M. manages to make three duplicates of the Iron Man armor.",
                PublishedOn = new DateTime(1968, 5, 1),
                AverageRating = 7.6m
            };
            comicBook4.AddArtist(artistArchieGoodwin, roleScript);
            comicBook4.AddArtist(artistGeneColan, rolePencils);
            context.ComicBooks.Add(comicBook4); */

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
