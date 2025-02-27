# ☁️ WebDav Quick Uploader for Flow

A plugin for the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher).

### Usage

    upload

Note: if you don't have a file or an image in your clipboard, you'll be prompted to pick one.

### Setup

Firstly you'll need to fill in your WebDav credentials.

- Upload Path
  - This is where the uploaded files will be stored
  - Use `%NAME%` for the filename
    => `uploads/%NAME%`
- File Name Format
  - This is a format for your file names
  - Use `%ID%` for a random UUID
  - Use `%EXTENSION%` for the original file extension
  - Use `%DATE%` for the date (`yyyy-MM-dd_HH-mm-ss`)
  - Use `%NAME%` for the original filename
    => `upload-%NAME%-%ID%.%EXTENSION%`
- Copy Public Url
  - If you host your upload directory somewhere, check this option to automatically copy the correct url to your clipboard
  - Use `%NAME%` for the modified filename
    => `https://uploads.myhost.com/%NAME%`
