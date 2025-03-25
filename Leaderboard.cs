using System;
using System.IO;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using CsvHelper.Configuration;
using Npgsql.Internal;

namespace Alien_Attack
{
    internal class Leaderboard
    {
        private CsvConfiguration config;
        private playerTableFormat record;
        private List<playerTableFormat> recordList;
        private UI ui;
        private string sort;
        public Leaderboard(UI ui) {
                //Makes it so the name of the variable types doesnt have to match the headings by case
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            this.ui = ui;
        }

        public string getDateTimeModified() {
            var lastModifiedDateTime = File.GetLastAccessTime("/Content/playerTable.csv");
            return lastModifiedDateTime.ToString("dd/mm/yy hh:mm:ss");
        }
            //Format for how the csv reader will layout the data
        public class playerTableFormat { 
            public string username { get; set; }
            public int highestLevel { get; set; }
            public string  longestSurvived { get; set; }
            public int highScore { get; set; }
        }

        public class gameTableFormat {
            public int score { get; set; }
            public string gameMode { get; set; }
            public string timesurvived { get; set; }
            public int level { get; set; }
        }
        
        //Uses default file reader to get text in the csv file and then uses the csv reader to parse it into usable data
        public List<playerTableFormat> readDataFromPlayerFile() {

            using (StreamReader reader = new StreamReader("/Content/playerTable.csv"))
            using (CsvReader csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<playerTableFormat>();
                recordList = records.ToList();
            }
            return recordList;
        }

        public void drawLeaderboard() {
            sort = ui.drawPlayerDataNames();
            switch (sort) {
                case "sortLevel":
                    break;
                case "sortTime":
                    break;
                case "sortScore":
                    break;
            }
        }

        public void clearFileAndWriteHeaders() {
            //Clears all text from the file and writes headers
            using (StreamWriter clearAllText = new StreamWriter("Content/playerTable.csv", false))
            using (CsvWriter writeHeaders = new CsvWriter(clearAllText, CultureInfo.InvariantCulture))
            {
                writeHeaders.WriteHeader<playerTableFormat>();
            }
        }

        public void writeDataToPlayerFile(string username, int highestLevel, string longestSurvived, int highScore) {

            record = convertToPlayerFormat(username, highestLevel, longestSurvived, highScore);
                //Using only creates these variables for specifically now
            using (StreamWriter writer = new StreamWriter("Content/playerTable.csv", true))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(record);
            }
        }

        private playerTableFormat convertToPlayerFormat(string username, int highestLevel, string longestSurvived, int highScore) {

            return new playerTableFormat { username = username, highestLevel = highestLevel, longestSurvived = longestSurvived, highScore = highScore };
        }

        //merge sort code
        public List<int> breakdownList(List<int> toSort)
        {
            int midpoint = toSort.Count / 2;
            List<int> leftHalf = toSort;
            List<int> rightHalf = toSort;

            if (toSort.Count > 1)
            {
                leftHalf = leftHalf.GetRange(0, midpoint);
                rightHalf = rightHalf.GetRange(midpoint, toSort.Count() - midpoint);
                //Continues breaking down the list until only one part remains, then it goes back out to when two remains and sorts those two
                leftHalf = breakdownList(leftHalf);
                rightHalf = breakdownList(rightHalf);
            }
            List<int> partMerged = merge(leftHalf, rightHalf);
            return partMerged;
        }

        private List<int> merge(List<int> left, List<int> right)
        {
            List<int> merged = new List<int>();
            //Merges the first item each time round
            while (left.Count() != 0 && right.Count() != 0)
            {
                if (left[0] > right[0])
                {
                    merged.Add(right[0]);
                    right.RemoveAt(0);
                }
                else
                {
                    merged.Add(left[0]);
                    left.RemoveAt(0);
                }
            }
            while (left.Count() != 0 && right.Count() == 0)
            {
                merged.Add(left[0]);
                left.RemoveAt(0);
            }
            while (left.Count() == 0 && right.Count() != 0)
            {
                merged.Add(right[0]);
                right.RemoveAt(0);
            }
            return merged;
        }
    }
}
