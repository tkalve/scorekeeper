using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using ScoreKeeper.Models;
using File = Google.Apis.Drive.v3.Data.File;

namespace ScoreKeeper.Windows
{
    public class SheetReader
    {
        private string[] Scopes;
        private string AppName;

        private SheetsService SheetsService { get; set; }
        private DriveService DriveService { get; set; }

        public SheetReader()
        {
            Scopes = new string[]{ SheetsService.Scope.SpreadsheetsReadonly, DriveService.Scope.Drive };
            AppName = "ScoreKeeper";
        }

        public bool Login()
        {
            try
            {
                UserCredential credential;

                using (var stream = new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/sheets.scorekeeper.json");

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                // Create Google Drive API service.
                DriveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = AppName
                });

                // Create Google Sheets API service.
                SheetsService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = AppName,
                });

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not log in", ex);
            }
        }

        public bool GetSheets(ObservableCollection<SheetObject> fileList)
        {
            try
            {
                var allFiles = new List<File>();

                var listRequest = DriveService.Files.List();
                listRequest.PageSize = 25;
                listRequest.Q = "mimeType='application/vnd.google-apps.spreadsheet'";
                listRequest.Fields = "nextPageToken, files(name, id)";
                do
                {
                    var fileBulk = listRequest.Execute();

                    allFiles.AddRange(fileBulk.Files);
                    listRequest.PageToken = fileBulk.NextPageToken;
                } while (listRequest.PageToken != null);

                fileList.Clear();
                foreach (var file in allFiles)
                    fileList.Add(new SheetObject() { Id = file.Id, Name = file.Name });

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get spreadsheets", ex);
            }
        }

        public void Load(string spreadsheetId, ObservableCollection<Game> gameList)
        {
            try
            {
                // Define request parameters.
                var range = "GameList!A2:F";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    SheetsService.Spreadsheets.Values.Get(spreadsheetId, range);

                ValueRange response = request.Execute();
                IList<IList<object>> values = response.Values;
                if (values != null && values.Count > 0)
                {
                    gameList.Clear();
                    foreach (var row in values)
                    {
                        if (row.Count > 4)
                        {
                            var blue = row[1].ToString();
                            var white = row[2].ToString();
                            var type = row[4].ToString();

                            if (!int.TryParse(row[5].ToString(), out var minutes))
                            {
                                minutes = 8;
                            }

                            gameList.Add(new Game(minutes)
                            {
                                Id = int.Parse(row[0].ToString()),
                                BlueTeamName = blue,
                                WhiteTeamName = white,
                                Type = type
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load game data", ex);
            }
        }
    }
}