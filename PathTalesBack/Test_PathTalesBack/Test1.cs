using PathTalesBack.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Test_PathTalesBack
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Role_DefaultValue_IsMember()
        {
            //ARRANGE
            User user = new User();

            //ACT
            string role = user.Role;

            //ASSERT
            Assert.AreEqual("member", role);

        }

        [TestMethod]
        public void Role_Null_IsMember()
        {
            //ARRANGE   
            User user = new User();

            //ACT
            user.Role = null;

            //ASSERT
            Assert.AreEqual("member", user.Role);
        }

        [TestMethod]
        public void Role_Admin_IsAdmin()
        {
            User user = new User();

            //ACT
            user.Role = "admin";

            //ASSERT
            Assert.AreEqual("admin", user.Role);
        }

        [TestMethod]
        public void Role_Member_IsUser()
        {
            //ARRANGE
            User user = new User();

            //ACT
            user.Role = "member";

            //ASSERT
            Assert.AreEqual("member", user.Role);
        }

    }
}
