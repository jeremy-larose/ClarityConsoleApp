using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using ClarityMailLibrary;

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
        /// Returns a count of the emails.
        /// </summary>
        /// <returns>An integer count of the emails.</returns>
        public static int GetEmailCount()
        {
            using (Context context = GetContext())
            {
                return context.Emails.Count();
            }
        }

        /// <summary>
        /// Returns a list of emails.
        /// </summary>
        /// <returns>An IList collection of Email entity instances.</returns>
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
        /// Returns a single email.
        /// </summary>
        /// <param name="emailId">The email ID to retrieve.</param>
        /// <returns>A fully populated ClarityMail entity instance.</returns>
        public static ClarityMail GetEmail(int emailId)
        {
            using (Context context = GetContext())
            {
                return context.Emails
                    .SingleOrDefault(e => e.Id == emailId);
            }
        }
        
        /// <summary>
        /// Adds an email.
        /// </summary>
        /// <param name="email">The ClarityMail entity instance to add.</param>
        public static void AddEmail(ClarityMail email)
        {
            using (Context context = GetContext())
            {
                context.Emails.Add(email);
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Deletes an email.
        /// </summary>
        /// <param name="emailId">The email ID to delete.</param>
        public static void DeleteEmail(int emailId)
        {
            using (Context context = GetContext())
            {
                var email = new ClarityMail() { Id = emailId };
                context.Entry(email).State = EntityState.Deleted;
                context.SaveChanges();
            }
        } 

        /// <summary>
        /// Updates an email.
        /// </summary>
        /// <param name="email">The ClarityMail entity instance to update.</param>
        public static void UpdateEmail(ClarityMail email )
        {
            using (Context context = GetContext())
            {
                context.Emails.Attach(email);
                var comicBookEntry = context.Entry(email);
                comicBookEntry.State = EntityState.Modified;

                context.SaveChanges();
            }
        }
    } 
}
