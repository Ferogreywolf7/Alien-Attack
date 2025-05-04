using System;
using System.IO;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using CsvHelper.Configuration;
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
        private string lastSort;
        private List<playerTableFormat> listToDisplay;
        private string lastModified;
        private int yCoordToWriteText;
        private int numRecordToStartAt;
        private string buttonPressed;
        private int numOfRecordsTotal;
        private bool displayListChanged;
        private List<Double> leftHalf;
        private List<Double> rightHalf;
        private List<double> partMerged;
        public Leaderboard(UI ui) {
                //Makes it so the name of the variable types doesnt have to match the headings by case
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            this.ui = ui;
            initialiseLeaderboard();
        }

        private string getDateTimeModified() {
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
        private void readDataFromPlayerFile() {
            using (StreamReader reader = new StreamReader("Content/playerTable.csv"))
            using (CsvReader csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<playerTableFormat>();
                recordList = records.ToList();
            }
            
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
            //Takes the indiviual sorted pieces of data and matches them up to the right records
        private List<playerTableFormat> reorganiseRecords(List<Double> toReorganise, int index) {
            List<playerTableFormat> reorganised = new List<playerTableFormat>();
            toReorganise = toReorganise.Distinct().ToList();    //Only allows unique values in the list to be sorted to prevent data from being sorted multiple times
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
                //System for showing the next/previous 10 data
            if (buttonPressed == "next")
            {
                numRecordToStartAt += 10;
                sort = lastSort;
            }
            if (buttonPressed == "previous")
            {
                numRecordToStartAt -= 10;
                sort = lastSort;
            }
            //Prevents the counter from going over the total number of items or under 0 - Defensive programming
            if (numRecordToStartAt > numOfRecordsTotal)
            {
                numRecordToStartAt -= 10;
                sort = "";
            }
            if (numRecordToStartAt < 0)
            {
                numRecordToStartAt += 10;
                sort = "";
            }
            switch (sort)
            {   
                    //Gets data sorted based off of what button the user presses
                case "sortLevel":
                    listToDisplay = reorganiseRecords(breakdownList(getSpecificData(1)), 1);  //Gets specific data from record, then sorts by that data and puts the rest of the record back in
                    reverseRecords();
                    displayListChanged = true;
                    lastSort = sort;
                    break;
                case "sortTime":
                    listToDisplay = reorganiseRecords(breakdownList(getSpecificData(2)), 2);
                    reverseRecords();
                    displayListChanged = true;
                    lastSort = sort;
                    break;
                case "sortScore":
                    listToDisplay = reorganiseRecords(breakdownList(getSpecificData(3)), 3);
                    reverseRecords();
                    displayListChanged = true;
                    lastSort = sort;
                    break;
            }
            ui.drawText("Last updated: " + lastModified, new Vector2(550, 850));
            displayData();
            sort = ui.drawPlayerDataNames();
        }

        private void reverseRecords() {
                listToDisplay.Reverse();
        }
		
            //Displays data for all users
        private void displayData() {
            yCoordToWriteText = 300;
            numOfRecordsTotal = recordList.Count();
            buttonPressed = ui.drawNewPages();
                //Makes sure that only 10 records at a time are viewed and that what 10 those are can be changed
            if (numOfRecordsTotal > 10) {
                if (displayListChanged)
                {
                    if ((numOfRecordsTotal - numRecordToStartAt) > 10)
                    {
                        listToDisplay.RemoveRange(numRecordToStartAt + 10, (numOfRecordsTotal - (numRecordToStartAt+10)));
                    }
                    if (numRecordToStartAt >= 10)
                    {
                        listToDisplay.RemoveRange(0, numRecordToStartAt);
                    }
                }
                displayListChanged = false;
            }
                //Writes all of the data to be displayed onto the screen
            foreach (playerTableFormat Record in listToDisplay)
            {
                ui.drawText(truncate(Record.username, true), new Vector2(75, yCoordToWriteText));
                ui.drawText(Record.highestLevel.ToString(), new Vector2(265, yCoordToWriteText));
                ui.drawText(truncate(Record.longestSurvived, false, 11), new Vector2(395, yCoordToWriteText));
                ui.drawText(Record.highScore.ToString(), new Vector2(585, yCoordToWriteText));
                yCoordToWriteText += 50;
            }
        }
            //Methods for shortening data names to prevent any overlapping with other data
        private string truncate(string stringToShorten, bool useDots) {
            if (stringToShorten.Length > 10)
            {
                stringToShorten = stringToShorten.Remove(10);
                if (useDots)
                {
                    stringToShorten += "...";
                }
            }
            return stringToShorten;
        }

        private string truncate(string stringToShorten, bool useDots, int shortenBy)
        {
            if (stringToShorten.Length > shortenBy)
            {
                stringToShorten = stringToShorten.Remove(shortenBy);
                if (useDots)
                {
                    stringToShorten += "...";
                }
            }
            return stringToShorten;
        }
            
        public void initialiseLeaderboard() {
                //Run on start up
            readDataFromPlayerFile();
            listToDisplay = recordList;
            lastModified = getDateTimeModified();
            numRecordToStartAt = 0;
            sort = "sortLevel";
        }

        public void clearFileAndWriteHeaders() {
                //Clears all text from the file and writes headers
            using (StreamWriter clearAllText = new StreamWriter("Content/playerTable.csv", false))
            using (CsvWriter writeHeaders = new CsvWriter(clearAllText, CultureInfo.InvariantCulture))
            {
                writeHeaders.WriteHeader<playerTableFormat>();
                writeHeaders.NextRecord();
            }
        }

        public void writeDataToPlayerFile(string username, int highestLevel, string longestSurvived, int highScore) {
            record = convertToPlayerFormat(username, highestLevel, longestSurvived, highScore);
                //Using only creates these variables for specifically now
            using (StreamWriter writer = new StreamWriter("Content/playerTable.csv", true))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }

        private playerTableFormat convertToPlayerFormat(string username, int highestLevel, string longestSurvived, int highScore) {
            return new playerTableFormat { username = username, highestLevel = highestLevel, longestSurvived = longestSurvived, highScore = highScore };
        }

        //merge sort code
        private List<Double> breakdownList(List<Double> toSort)
        {
            int midpoint = toSort.Count / 2;
            if (toSort.Count > 1)
            {
                leftHalf = toSort.GetRange(0, midpoint);
                rightHalf = toSort.GetRange(midpoint, toSort.Count() - midpoint);
                //Continues breaking down the list until only one part remains, then it goes back out to when two remains and sorts those two
                leftHalf = breakdownList(leftHalf);
                rightHalf = breakdownList(rightHalf);
            }
            partMerged = merge(leftHalf, rightHalf);
            return partMerged;
        }

        private List<Double> merge(List<Double> left, List<Double> right)
        {
                //The sorting part of the merge sort
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