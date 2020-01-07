using System;
using Xunit;

namespace CredentialManagement.Test
{
    public class CredentialTests
    {
        [Fact]
        public void Credential_Create_ShouldNotThrowNull()
        {
            var credential = new Credential();
            Assert.NotNull(credential);
        }

        [Fact]
        public void Credential_Create_With_Username_ShouldNotThrowNull()
        {
            var credential = new Credential("username");
            Assert.NotNull(credential);
        }

        [Fact]
        public void Credential_Create_With_Username_And_Password_ShouldNotThrowNull()
        {
            var credential = new Credential("username", "password");
            Assert.NotNull(credential);
        }
        [Fact]
        public void Credential_Create_With_Username_Password_Target_ShouldNotThrowNull()
        {
            var credential = new Credential("username", "password", "target");
            Assert.NotNull(credential);
        }

        [Fact]
        public void Credential_Save()
        {
            var saved = new Credential("username", "password", "target", CredentialType.Generic)
            {
                PersistanceType = PersistanceType.LocalComputer
            };
            var result = saved.Save();
            Assert.True(result);
        }

        [Fact]
        public void Credential_Delete()
        {
            new Credential("username", "password", "target").Save();
            var result = new Credential("username", "password", "target").Delete();
            Assert.True(result);
        }

        [Fact]
        public void Credential_Delete_NullTerminator()
        {
            var credential = new Credential(null, null, "\0", CredentialType.None)
            {
                Description = null
            };
            var result = credential.Delete();
            Assert.False(result);
        }

        [Fact]
        public void Credential_Load()
        {
            var setup = new Credential("username", "password", "target", CredentialType.Generic);
            setup.Save();

            var credential = new Credential { Target = "target", Type = CredentialType.Generic };
            Assert.True(credential.Load());

            Assert.Equal("username", credential.Username);
            Assert.Equal("password", credential.Password);
            Assert.Equal("target", credential.Target);
        }

        [Fact]
        public void Credential_Exists_Target_ShouldNotBeNull()
        {
            new Credential { Username = "username", Password = "password", Target = "target" }.Save();

            var existingCred = new Credential { Target = "target" };
            Assert.True(existingCred.Exists());

            existingCred.Delete();
        }
    }
}
