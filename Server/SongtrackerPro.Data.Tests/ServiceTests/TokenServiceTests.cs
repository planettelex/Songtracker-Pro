using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Data.Tests.ServiceTests
{
    [TestClass]
    public class TokenServiceTests
    {
        [TestMethod]
        public void ReplaceTokensTest()
        {
            ITokenService tokenService = new TokenService();
            var user = new User
            {
                Id = 55,
                FirstName = "Sam",
                LastName = "Adams",
                UserType = UserType.SystemUser,
                UserRoles = SystemUserRoles.Songwriter | SystemUserRoles.ArtistMember
            };
            
            var textWithTokens = "Hello {User.FirstName} {User.LastName} ID: {User.Id}";
            textWithTokens = tokenService.ReplaceTokens(textWithTokens, user);
            Assert.AreEqual("Hello Sam Adams ID: 55", textWithTokens);

            textWithTokens = "User Type: {User.UserType}";
            textWithTokens = tokenService.ReplaceTokens(textWithTokens, user);
            Assert.AreEqual("User Type: SystemUser", textWithTokens);
            Assert.IsTrue(user.UserRoles.HasFlag(SystemUserRoles.Songwriter));
            Assert.IsTrue(user.UserRoles.HasFlag(SystemUserRoles.ArtistMember));
            Assert.IsFalse(user.UserRoles.HasFlag(SystemUserRoles.ArtistManager));
        }
    }
}
