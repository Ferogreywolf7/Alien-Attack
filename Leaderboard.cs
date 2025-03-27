using System;
using System.IO;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using CsvHelper.Configuration;
using Npgsql.Internal;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Alien_Attack
{
    internal class Leaderboard
    {
        private CsvConfiguration config;
        private playerTableFormat record;
        private List<playerTableFormat> recordList;
        private UI ui;
        private string sort;
        private List<playerTableFormat> listToDisplay;
        private string lastModified;
        private int numOfRecords;
        private int yCoordToWriteText;
        private int numRecordToStartAt;
        private string buttonPressed;
        public Leaderboard(UI ui) {
                //Makes it so the name of the variable types doesnt have to match the headings by case
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            this.ui = ui;
            initialiseLeaderboard();
        }

        public string getDateTimeModified() {
            var lastModifiedDateTime = File.GetLastAccessTime("Content/playerTable.csv");
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

            using (StreamReader reader = new StreamReader("Content/playerTable.csv"))
            using (CsvReader csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<playerTableFormat>();
                recordList = records.ToList();
            }
            return recordList;
        }
            //Gets the specific data from the list
        private List<double> getSpecificData(int index) {
            List<double> listToSort = new List<double>();
            foreach (playerTableFormat Record in recordList) {
                switch (index) {
                    case 1:
                        listToSort.Add(Record.highestLevel);
                        break;
                    case 2:
                        listToSort.Add(Convert.ToDouble(Record.longestSurvived.Replace(":", "")));
                        break;
                    case 3:
                        listToSort.Add(Record.highScore);
                        break;
                }
            }
            return listToSort;
        }

        private List<playerTableFormat> reorganiseRecords(List<Double> toReorganise, int index) {
            List<playerTableFormat> reorganised = new List<playerTableFormat>();
            foreach (Double data in toReorganise) {
                foreach (playerTableFormat Record in recordList)
                {
                    switch (index)
                    {
                        case 1:
                            if (Record.highestLevel == data) {
                                reorganised.Add(Record);
                            }
                            break;
                        case 2:
                            if (Convert.ToDouble(Record.longestSurvived.Replace(":", "")) == data)
                            {
                                reorganised.Add(Record);
                            }
                            break;
                        case 3:
                            if (Record.highScore == data) {
                                reorganised.Add(Record);
                            }
                            break;
                    }
                } 
            }
            return reorganised;
        }
            //Sorts by the specific data when the button is pressed
        public void drawLeaderboard() {
            sort = ui.drawPlayerDataNames();
            switch (sort)
            {
                case "sortLevel":
                    listToDisplay = reorganiseRecords(breakdownList(getSpecificData(1)), 1);  //Gets specific data from record, then sorts by that data and puts the rest of the record back in
                    reverseRecords();
                    break;
                case "sortTime":
                    listToDisplay = reorganiseRecords(breakdownList(getSpecificData(2)), 2);
                    reverseRecords();
                    break;
                case "sortScore":
                    listToDisplay = reorganiseRecords(breakdownList(getSpecificData(3)), 3);
                    reverseRecords();
                    break;
            }
            ui.drawText("Last updated: " + lastModified, new Vector2(550, 850));
            displayData();
        }

        private void reverseRecords() {
                listToDisplay.Reverse();
        }
            //Displays data for all users
        private void displayData() {
            yCoordToWriteText = 300;
            numOfRecords = listToDisplay.Count();
            
            if(numOfRecords > 10) {
                buttonPressed = ui.drawNewPages();
                if (buttonPressed == "next") {
                    numRecordToStartAt += 10;
                }
                if (buttonPressed == "previous") {
                    numRecordToStartAt -= 10;
                }
                if (numRecordToStartAt > numOfRecords) {
                    numRecordToStartAt -= 10;
                }

                if (numOfRecords - numRecordToStartAt > 10)
                {
                    listToDisplay.RemoveRange(numRecordToStartAt + 10, numOfRecords-numRecordToStartAt-11);
                }
                else if(numOfRecords - numRecordToStartAt < 10 && numOfRecords - numRecordToStartAt < 0){
                    listToDisplay.RemoveRange(numRecordToStartAt + (numOfRecords - numRecordToStartAt), numOfRecords - 1);
                }
            }

            foreach (playerTableFormat Record in listToDisplay)
            {
                ui.drawText(truncate(Record.username), new Vector2(75, yCoordToWriteText));
                ui.drawText(Record.highestLevel.ToString(), new Vector2(265, yCoordToWriteText));
                ui.drawText(Record.longestSurvived, new Vector2(395, yCoordToWriteText));
                ui.drawText(Record.highScore.ToString(), new Vector2(585, yCoordToWriteText));
                yCoordToWriteText += 50;
            }
        }
            //Displays data for specific users
        private void displayData(string username) { 
        
        }

        private string truncate(string stringToShorten) {
            if (stringToShorten.Length > 10) {
                stringToShorten = stringToShorten.Remove(10);
                stringToShorten += "..."; }
            return stringToShorten;
        }

        public void initialiseLeaderboard() {
            listToDisplay = readDataFromPlayerFile();
            lastModified = getDateTimeModified();
            numRecordToStartAt = 0;
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
        public List<Double> breakdownList(List<Double> toSort)
        {
            int midpoint = toSort.Count / 2;
            List<Double> leftHalf = toSort;
            List<Double> rightHalf = toSort;

            if (toSort.Count > 1)
            {
                leftHalf = leftHalf.GetRange(0, midpoint);
                rightHalf = rightHalf.GetRange(midpoint, toSort.Count() - midpoint);
                //Continues breaking down the list until only one part remains, then it goes back out to when two remains and sorts those two
                leftHalf = breakdownList(leftHalf);
                rightHalf = breakdownList(rightHalf);
            }
            List<double> partMerged = merge(leftHalf, rightHalf);
            return partMerged;
        }

        private List<Double> merge(List<Double> left, List<Double> right)
        {
            List<Double> merged = new List<Double>();
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
