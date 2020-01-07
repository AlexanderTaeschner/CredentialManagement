using System;
using System.Text;
using Xunit;

namespace CredentialManagement.Test
{
    public class CredentialsPromptTests
    {
        static readonly string s_maxLengthValidationText = Initialize();

        public static string Initialize()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 50000; i++)
            {
                sb.Append('A');
            }

            return sb.ToString();
        }


        [Fact]
        public void CredentialsPrompt_Create_ShouldNotBeNull()
        {
            var credentialsPrompt = new CredentialsPrompt();
            Assert.NotNull(credentialsPrompt);
        }

        [Fact]
        public void CredentialsPrompt_Username_MaxLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new CredentialsPrompt { Username = s_maxLengthValidationText });
        }

        [Fact]
        public void CredentialsPrompt_Username_NullValue()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CredentialsPrompt { Username = null });
        }

        [Fact]
        public void CredentialsPrompt_Password_NullValue()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CredentialsPrompt { Password = null });
        }

        [Fact]
        public void CredentialsPrompt_Message_MaxLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new CredentialsPrompt { Message = s_maxLengthValidationText });
        }

        [Fact]
        public void CredentialsPrompt_Message_NullValue()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CredentialsPrompt { Message = null });
        }

        [Fact]
        public void CredentialsPrompt_Title_MaxLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new CredentialsPrompt { Title = s_maxLengthValidationText });
        }

        [Fact]
        public void CredentialsPrompt_Title_NullValue()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CredentialsPrompt { Title = null });
        }

        [Fact(Skip = "UI test")]
        public void CredentialsPrompt_ShowDialog_ShouldNotThrowError()
        {
            var prompt = new CredentialsPrompt();
            prompt.ShowDialog();
        }

        [Fact(Skip = "UI test")]
        public void CredentialsPrompt_ShowDialog_WithParent_ShouldNotThrowError()
        {
            var prompt = new CredentialsPrompt();
            prompt.ShowDialog(IntPtr.Zero);
        }

        [Fact(Skip = "UI test")]
        public void CredentialsPrompt_ShowDialog_With_Username()
        {
            var prompt = new CredentialsPrompt
            {
                Username = "username"
            };
            Assert.Equal(DialogResult.OK, prompt.ShowDialog());
        }

        [Fact(Skip = "UI test")]
        public void CredentialsPrompt_ShowDialog_GenericCredentials()
        {
            var prompt = new CredentialsPrompt
            {
                Title = "Please provide credentials",
                GenericCredentials = true
            };
            prompt.ShowDialog(IntPtr.Zero);
        }

        [Fact(Skip = "UI test")]
        public void CredentialsPrompt_ShowDialog_ShowSaveCheckBox()
        {
            var prompt = new CredentialsPrompt
            {
                ShowSaveCheckBox = true
            };
            prompt.ShowDialog(IntPtr.Zero);
        }
    }
}
