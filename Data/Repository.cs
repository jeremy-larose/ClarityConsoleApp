using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using ClarityEmailerLibrary;

namespace ClarityConsole.Data
{
    /// <summary>
    /// Repository class that provides various database queries
    /// and CRUD operations.
    /// </summary>
    public static class Repository
    {
        /// <summary>
        /// Private method that returns a database context.
        /// </summary>
        /// <returns>An instance of the Context class.</returns>
        static Context GetContext()
        {
            var context = new Context();
            context.Database.Log = (message) => Debug.WriteLine(message);
            return context;
        }

        /// <summary>
        /// Returns a count of the comic books.
        /// </summary>
        /// <returns>An integer count of the comic books.</returns>
        public static int GetEmailCount()
        {
            using (Context context = GetContext())
            {
                return context.Emails.Count();
            }
        }

        /// <summary>
        /// Returns a list of comic books ordered by the series title 
        /// and issue number.
        /// </summary>
        /// <returns>An IList collection of ComicBook entity instances.</returns>
        public static IList<ClarityMail> GetEmails()
        {
            using (Context context = GetContext())
            {
                return context.Emails
                    .ToList();
            }
        }

        public static List<string> GetAllMailboxAddresses()
        {
            var emailList = new List<string>();
            using (Context context = GetContext())
            {
                foreach (var email in GetEmails())   
                {
                    emailList.Add( email.MailTo );
                }

                return emailList;
            }
        }

        /// <summary>
        /// Returns a single comic book.
        /// </summary>
        /// <param name="comicBookId">The comic book ID to retrieve.</param>
        /// <returns>A fully populated ComicBook entity instance.</returns>
        public static ClarityMail GetEmail(int emailId)
        {
            using (Context context = GetContext())
            {
                return context.Emails
                    .Where(e => e.Id == emailId)
                    .SingleOrDefault();
            }
        }
        
        /// <summary>
        /// Adds a comic book.
        /// </summary>
        /// <param name="comicBook">The ComicBook entity instance to add.</param>
        public static void AddEmail(ClarityMail email)
        {
            using (Context context = GetContext())
            {
                context.Emails.Add(email);
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Deletes a comic book.
        /// </summary>
        /// <param name="comicBookId">The email ID to delete.</param>
        public static void DeleteEmail(int emailId)
        {
            using (Context context = GetContext())
            {
                var email = new ClarityMail() { Id = emailId };
                context.Entry(email).State = EntityState.Deleted;
                context.SaveChanges();
            }
        } 
/*
        /// <summary>
        /// Returns a list of series ordered by title.
        /// </summary>
        /// <returns>An IList collection of Series entity instances.</returns>
        public static IList<Series> GetSeries()
        {
            using (Context context = GetContext())
            {
                return context.Series
                    .OrderBy(s => s.Title)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns a single series.
        /// </summary>
        /// <param name="seriesId">The series ID to retrieve.</param>
        /// <returns>A Series entity instance.</returns>
        public static Series GetSeries(int seriesId)
        {
            using (Context context = GetContext())
            {
                return context.Series
                    .Where(s => s.Id == seriesId)
                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// Returns a list of artists ordered by name.
        /// </summary>
        /// <returns>An IList collection of Artist entity instances.</returns>
        public static IList<Artist> GetArtists()
        {
            using (Context context = GetContext())
            {
                return context.Artists
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns a list of roles ordered by name.
        /// </summary>
        /// <returns>An IList collection of Role entity instances.</returns>
        public static IList<Role> GetRoles()
        {
            using (Context context = GetContext())
            {
                return context.Roles
                    .OrderBy(r => r.Name)
                    .ToList();
            }
        }



        /// <summary>
        /// Updates a comic book.
        /// </summary>
        /// <param name="comicBook">The ComicBook entity instance to update.</param>
        public static void UpdateComicBook(ComicBook comicBook)
        {
            using (Context context = GetContext())
            {
                context.ComicBooks.Attach(comicBook);
                var comicBookEntry = context.Entry(comicBook);
                comicBookEntry.State = EntityState.Modified;
                //comicBookEntry.Property("IssueNumber").IsModified = false;

                context.SaveChanges();
            }
        }
*/

    } 
}
