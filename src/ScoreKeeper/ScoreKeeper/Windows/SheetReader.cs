﻿using System;
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
            String spreadsheetId = "17Bb9XC1M5Ev9oaFbMBtCGP56uPynzI9ugrRnBSRsbgw";
            String range = "GameList!A2:F";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    var blue = row[2].ToString().Split(' ');
                    var white = row[3].ToString().Split(' ');
                    var type = (blue.Length > 1) ? blue[1].TrimStart('(').TrimEnd(')') : "";

                    gameList.Add(new Game()
                    {
                        Id = int.Parse(row[0].ToString()),
                        BlueTeamName = blue[0],
                        WhiteTeamName = white[0],
                        Type = type
                    });
                }
            }
        }
    }
}