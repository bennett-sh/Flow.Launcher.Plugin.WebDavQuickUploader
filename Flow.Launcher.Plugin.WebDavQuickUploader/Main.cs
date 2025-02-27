using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Flow.Launcher.Plugin.WebDavQuickUploader.Views;
using Microsoft.Win32;
using WebDav;

namespace Flow.Launcher.Plugin.WebDavQuickUploader
{
    public class WebDavQuickUploader : IPlugin, ISettingProvider
    {
        private PluginInitContext _context;
        private Settings _settings;

        public void Init(PluginInitContext context)
        {
            _context = context;
            _settings = _context.API.LoadSettingJsonStorage<Settings>();
        }

        public List<Result> Query(Query query)
        {
            return new List<Result>() {
                new Result
                {
                    Title = "Upload from clipboard",
                    SubTitle = "Upload a file or an image from your clipboard",
                    AsyncAction = async c => await UploadClipboard(false),
                    IcoPath = "icon.png"
                },
                new Result {
                    Title = "Upload from picker",
                    SubTitle = "Select a file to upload",
                    AsyncAction = async c => await UploadClipboard(true),
                    IcoPath = "icon.png"
                }
            };
        }

        public Control CreateSettingPanel()
        {
            return new SettingsControl(_settings);
        }

        private async Task<bool> UploadFile(string file)
        {
            using(Stream stream = File.OpenRead(file))
            {
                return await UploadStream(file, stream);
            }
        }

        private async Task<bool> UploadImage(BitmapSource image)
        {
            JpegBitmapEncoder encoder = new();
            encoder.QualityLevel = 100;
            using(MemoryStream stream = new())
            {
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
                return await UploadStream($"image-{new Random().Next(100000, 100000000)}.png", stream);
            }
        }

        private async Task<bool> UploadStream(string path, Stream stream) {
            var client = GetDavClient();

            string name = FilePathToDavPath(path);
            var response = await client.PutFile(name, stream);
            stream.Close();
            if (!response.IsSuccessful)
            {
                _context.API.ShowMsgError(path + " failed to upload: " + response.Description);
                return false;
            }

            if (_settings.CopyPublicUrl)
            {
                _context.API.CopyToClipboard(_settings.PublicUrl.Replace("%NAME%", Path.GetFileName(name)));
            }
            else
            {
                _context.API.ShowMsg($"File uploaded as {name}");
            }

            return true;
        }

        private async Task<bool> UploadClipboard(bool forcePick)
        {
            string file = null;
            if (Clipboard.ContainsFileDropList() && !forcePick)
            {
                file = Clipboard.GetFileDropList()[0];
            }
            if (Clipboard.ContainsImage() && !forcePick)
            {
                return await UploadImage(Clipboard.GetImage());
            }
            if (file == null)
            {
                file = AskForFile();
                if (file == null)
                {
                    _context.API.ShowMsgError("No file selected");
                    return false;
                }
            }
            return await UploadFile(file);
        }

        private string AskForFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.RestoreDirectory = true;
            dialog.Multiselect = false;
            dialog.Title = "Select a file to upload";

            if (dialog.ShowDialog() != true) return null;
            return dialog.FileName;
        }

        private WebDavClient GetDavClient()
        {
            return new WebDavClient(
                new WebDavClientParams()
                {
                    BaseAddress = new Uri(_settings.WebdavUrl),
                    Credentials = new NetworkCredential(_settings.WebdavUsername, _settings.WebdavPassword),
                }
            );
        }

        private string CleanExtension(string extension)
        {
            if(extension.Length < 1)
            {
                return "unknown";
            }
            return extension[1..];
        }

        private string FilePathToDavPath(string filePath)
        {
            string name = _settings.FileNameFormat
                .Replace("%EXTENSION%", CleanExtension(Path.GetExtension(filePath)))
                .Replace("%NAME%", Path.GetFileNameWithoutExtension(filePath))
                .Replace("%DATE%", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"))
                .Replace("%ID%", Guid.NewGuid().ToString());
            return _settings.UploadPath.Replace("%NAME%", name);
        }
    }
}