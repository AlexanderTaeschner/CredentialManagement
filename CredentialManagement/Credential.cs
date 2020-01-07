using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CredentialManagement
{
    public class Credential
    {
        public Credential()
            : this(null)
        {
        }

        public Credential(string username)
            : this(username, null)
        {
        }

        public Credential(string username, string password)
            : this(username, password, null)
        {
        }

        public Credential(string username, string password, string target)
            : this(username, password, target, CredentialType.Generic)
        {
        }

        public Credential(string username, string password, string target, CredentialType type)
        {
            Username = username;
            Password = password;
            Target = target;
            Type = type;
            PersistanceType = PersistanceType.Session;
            LastWriteTimeUtc = DateTime.MinValue;
        }


        public string Username { get; set; }
        public string Password { get; set; }

        public string Target { get; set; }

        public string Description { get; set; }

        public DateTime LastWriteTime => LastWriteTimeUtc.ToLocalTime();
        public DateTime LastWriteTimeUtc { get; private set; }

        public CredentialType Type { get; set; }

        public PersistanceType PersistanceType { get; set; }

        public bool Save()
        {
            var passwordBytes = Encoding.Unicode.GetBytes(Password);
            if (passwordBytes.Length > 512)
            {
                throw new InvalidOperationException("The password has exceeded 512 bytes.");
            }

            var credential = new NativeMethods.CREDENTIAL
            {
                TargetName = Target,
                UserName = Username,
                CredentialBlob = Marshal.StringToCoTaskMemUni(Password),
                CredentialBlobSize = passwordBytes.Length,
                Comment = Description,
                Type = (int)Type,
                Persist = (int)PersistanceType
            };

            var result = NativeMethods.CredWrite(ref credential, 0);
            if (!result)
            {
                return false;
            }
            LastWriteTimeUtc = DateTime.UtcNow;
            return true;
        }

        public bool Delete()
        {
            if (string.IsNullOrEmpty(Target))
            {
                throw new InvalidOperationException("Target must be specified to delete a credential.");
            }

            var target = string.IsNullOrEmpty(Target) ? new StringBuilder() : new StringBuilder(Target);
            var result = NativeMethods.CredDelete(target, Type, 0);
            return result;
        }

        public bool Load()
        {
            var result = NativeMethods.CredRead(Target, Type, 0, out var credPointer);
            if (!result)
            {
                return false;
            }
            using (var credentialHandle = new NativeMethods.CriticalCredentialHandle(credPointer))
            {
                LoadInternal(credentialHandle.GetCredential());
            }
            return true;
        }

        public bool Exists()
        {
            if (string.IsNullOrEmpty(Target))
            {
                throw new InvalidOperationException("Target must be specified to check existance of a credential.");
            }

            var existing = new Credential { Target = Target, Type = Type };
            return existing.Load();
        }

        internal void LoadInternal(NativeMethods.CREDENTIAL credential)
        {
            Username = credential.UserName;
            if (credential.CredentialBlobSize > 0)
            {
                Password = Marshal.PtrToStringUni(credential.CredentialBlob, credential.CredentialBlobSize / 2);
            }
            Target = credential.TargetName;
            Type = (CredentialType)credential.Type;
            PersistanceType = (PersistanceType)credential.Persist;
            Description = credential.Comment;
            LastWriteTimeUtc = DateTime.FromFileTimeUtc(credential.LastWritten);
        }
    }
}