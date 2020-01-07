using System;
using Xunit;

namespace CredentialManagement.Test
{
    public class CredentialSetTests
    {
        [Fact]
        public void CredentialSet_Create()
        {
            var credentialSet = new CredentialSet();
            Assert.NotNull(credentialSet);
        }

        [Fact]
        public void CredentialSet_Create_WithTarget()
        {
            var credentialSet = new CredentialSet("target");
            Assert.NotNull(credentialSet);
        }

        [Fact]
        public void CredentialSet_Load()
        {
            var credential = new Credential
            {
                Username = "username",
                Password = "password",
                Target = "target",
                Type = CredentialType.Generic
            };
            credential.Save();

            var set = new CredentialSet();
            set.Load();
            Assert.NotNull(set);
            Assert.NotEmpty(set);

            credential.Delete();
        }

        [Fact]
        public void CredentialSet_Load_ShouldReturn_Self()
        {
            var set = new CredentialSet();
            var credentialSet = set.Load();
            Assert.IsType<CredentialSet>(credentialSet);
        }

        [Fact]
        public void CredentialSet_Load_With_TargetFilter()
        {
            var credential = new Credential
            {
                Username = "filteruser",
                Password = "filterpassword",
                Target = "filtertarget"
            };
            credential.Save();

            var set = new CredentialSet("filtertarget");
            var credentialSet = set.Load();
            Assert.Single(credentialSet);
        }
    }
}
