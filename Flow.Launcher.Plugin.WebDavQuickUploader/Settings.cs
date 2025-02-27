using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.WebDavQuickUploader
{
    public class Settings : INotifyPropertyChanged
    {
        private string _webdavUrl = "https://example.com/webdav/";
        private string _webdavUsername = "username";
        private string _webdavPassword = "password";
        private string _uploadPath = "uploads/%NAME%";
        private string _fileNameFormat = "%NAME%-%ID%.%EXTENSION%";
        private string _publicUrl = "https://example.com/uploads/%NAME%";
        private bool _copyPublicUrl = true;

        public string WebdavUrl {
            get => _webdavUrl;
            set
            {
                _webdavUrl = value;
                OnPropertyChanged();
            }
        }
        public string WebdavUsername {
            get => _webdavUsername;
            set
            {
                _webdavUsername = value;
                OnPropertyChanged();
            }
        }
        public string WebdavPassword {
            get => _webdavPassword;
            set {
                _webdavPassword = value;
                OnPropertyChanged();
            }
        }
        public string UploadPath
        {
            get => _uploadPath;
            set {
                _uploadPath = value;
                OnPropertyChanged();
            }
        }
        public string FileNameFormat {
            get => _fileNameFormat;
            set
            {
                _fileNameFormat = value;
                OnPropertyChanged();
            }
        }
        public string PublicUrl
        {
            get => _publicUrl;
            set
            {
                _publicUrl = value;
                OnPropertyChanged();
            }
        }
        public bool CopyPublicUrl {
            get => _copyPublicUrl;
            set
            {
                _copyPublicUrl = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
