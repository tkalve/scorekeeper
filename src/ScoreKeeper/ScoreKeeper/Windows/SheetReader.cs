using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using ScoreKeeper.Models;

namespace ScoreKeeper.Windows
{
    public class SheetReader
    {
        private string[] Scopes;
        private string AppName;

        public SheetReader()
        {
            Scopes = new string[]{ SheetsService.Scope.SpreadsheetsReadonly };
            AppName = "ScoreKeeper";
        }

        public void Load(ObservableCollection<Game> gameList)
        {
            try
            {
                UserCredential credential;

                using (var stream =
                    new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
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


                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = AppName,
                });

                // Define request parameters.
                String spreadsheetId = "13pDNxOSsj6uzkNoGyzuPlGShbMJfxl11BJt7sNKsTtg";
                String range = "GameList!A2:F";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

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
                            int minutes;
                            if (!int.TryParse(row[5].ToString(), out minutes))
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
                MessageBox.Show("An error occured while attempting to load games. Are you connected to the interwebs?\r\n\r\nPlease let Thomas know if you are connected and something else is wrong.\r\n\r\nActual error: " + ex.Message, "Unable to load games");
            }
        }
    }
}