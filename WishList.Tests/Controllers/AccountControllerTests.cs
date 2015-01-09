using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishList.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WishList.Models;
using System.Web.Mvc;
namespace WishList.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        [TestMethod()]
        public void RegisterTest()
        {
            RegisterModel model = new RegisterModel() { UserName = "TestUser", Password = "TestPassword", ConfirmPassword = "TestPassword" };
            AccountController accountController = new AccountController();
            UserController userController = new UserController();

            accountController.Register(model);
            ViewResult result = userController.Index() as ViewResult;
            

            Assert.IsNotNull(result);
        }
    }
}
