using System;
using System.Runtime.InteropServices;

namespace CredentialManagement
{
    public abstract class BaseCredentialsPrompt
    {
        #region Fields

        string _username;
        string _password;
        string _message;
        string _title;

        #endregion

        #region Protected Methods

        protected void AddFlag(bool add, int flag)
        {
            if (add)
            {
                DialogFlags |= flag;
            }
            else
            {
                DialogFlags &= ~flag;
            }
        }

        internal virtual NativeMethods.CREDUI_INFO CreateCREDUI_INFO(IntPtr owner)
        {
            var credUI = new NativeMethods.CREDUI_INFO();
            credUI.cbSize = Marshal.SizeOf(credUI);
            credUI.hwndParent = owner;
            credUI.pszCaptionText = Title;
            credUI.pszMessageText = Message;
            return credUI;
        }

        #endregion

        #region Properties

        public bool SaveChecked { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (value.Length > NativeMethods.CREDUI_MAX_MESSAGE_LENGTH)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _message = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (value.Length > NativeMethods.CREDUI_MAX_CAPTION_LENGTH)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _title = value;
            }
        }

        public string Username
        {
            get => _username ?? string.Empty;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (value.Length > NativeMethods.CREDUI_MAX_USERNAME_LENGTH)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (value.Length > NativeMethods.CREDUI_MAX_PASSWORD_LENGTH)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _password = value;
            }
        }

        public int ErrorCode { get; set; }

        public abstract bool ShowSaveCheckBox { get; set; }

        public abstract bool GenericCredentials { get; set; }

        protected int DialogFlags { get; private set; }

        #endregion


        public virtual DialogResult ShowDialog() => ShowDialog(IntPtr.Zero);
        public abstract DialogResult ShowDialog(IntPtr owner);
    }
}
