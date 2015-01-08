using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WishList.Models
{
    public class WishListInitializer : System.Data.Entity.CreateDatabaseIfNotExists<WishListDBContext>
    {
        protected override void Seed(WishListDBContext context)
        {
            UsersContext userContext = new UsersContext();

            context.Database.Initialize(true);
            //For all profiles in userContexts that dont have their usernames already in context,
            //add them to context with name = userName. User can edit it later
            foreach (UserProfile profile in userContext.UserProfiles)
            {
                User p = context.People.SingleOrDefault(n => n.userName.Equals(profile.UserName));

                if (p == null)
                    context.People.Add(new User() { Name = profile.UserName, userName = profile.UserName });
            }
            context.SaveChanges();
        }
    }

    public class WishListDBContext : DbContext
    {
        public DbSet<User> People { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WishListItem> Gifts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WishListItem>()
                .HasOptional<User>(c => c.giver)
                .WithMany()
                .WillCascadeOnDelete(false);
        }

        public DbSet<ProductLink> ProductLinks { get; set; }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            int timeout = createPersistentCookie ? 24 * 60 : 30; //time in minutes for cookie, 1 day if persistent, 30 minutes if not
            var ticket = new FormsAuthenticationTicket(userName, createPersistentCookie, timeout);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public String getLoggedUser()
        {
            return HttpContext.Current.User.Identity.Name;
   
        }

        public void KeepProduct(string productName)
        {
            HttpContext.Current.Session.Timeout = 20;
            HttpContext.Current.Session.Add("Product", productName);
        }

        public Product getKeptProduct()
        {
            String name = HttpContext.Current.Session["Product"].ToString();
            return this.Products.SingleOrDefault(c => c.Name.Equals(name));
        }
    }


    public class User
    {
        public User()
        {
            givenItems = new List<WishListItem>();
            WishListItems = new List<WishListItem>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string gender { get; set; }
        public string userName { get; set; }

        [InverseProperty("giver")]
        public virtual ICollection<WishListItem> givenItems { get; set; }
        [InverseProperty("receiver")]
        public virtual ICollection<WishListItem> WishListItems { get; set; }
    }

    public class WishListItem
    {
        [Key]
        public int Id { get; set; }
        public int? giverID { get; set; }
        public int receiverID { get; set; }
        public int productID { get; set; }

        [ForeignKey("giverID")]
        public virtual User giver { get; set; }
        [ForeignKey("receiverID")]
        public virtual User receiver { get; set; }
        [ForeignKey("productID")]
        public virtual Product product { get; set; }
    }

    public class Product
    {
        public Product()
        {
            Links = new HashSet<ProductLink>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProductLink> Links { get; set; }
    }

    public class ProductLink
    {
        [Key]
        public int Id { get; set; }
        public string Store { get; set; }
        public string URL { get; set; }
        public double? Price { get; set; }

        public virtual Product Product { get; set; }
    }
}