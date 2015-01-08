using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishList.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace WishList.Models.Tests
{
    [TestClass()]
    public class WishListDBContextTests
    {
        WishListDBContext db = new WishListDBContext();
        [TestMethod()]
        public void SignInTest()
        {
            db.SignIn("tester", true);
            String userName = db.getLoggedUser();
            Assert.IsTrue(userName.Equals("tester"));

        }
    }
}
