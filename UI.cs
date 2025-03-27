using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Alien_Attack;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics.Tracing;

public class UI
{
    //Instance variables
    private SpriteBatch spriteBatch;
    private SpriteFont font;
    private Texture2D textBox;
    private Texture2D heart;
    private Texture2D arrow;
    private Rectangle topBox;
    private Rectangle upperBox;
    private Rectangle lowerBox;
    private Rectangle bottomBox;
    private Keys left;
    private Keys right;
    private Keys fire;
    private Keys pause;
    private MouseState mouseState;
    private MouseState previousMouseState;
    private int mouseX;
    private int mouseY;
    private bool isButtonPressed;
    private bool cont;
    private Controls controls;
    private Stopwatch timer;
    private string timeTaken;
    private double currentTime;
    private static int level;
    private static int score;
    private int counter;
    private int currentColour;
    private Rectangle upperBoxLeft;
    private Rectangle upperBoxRight;
    private Rectangle lowerBoxLeft;
    private Rectangle lowerBoxRight;
    private Rectangle sourceRectangle;
    private Rectangle leaderboardBoxLeft;
    private Rectangle leaderboardBoxMiddleLeft;
    private Rectangle leaderboardBoxMiddleRight;
    private Rectangle leaderboardBoxRight;
    private string text;
    public UI(SpriteBatch _spriteBatch, SpriteFont testFont, Texture2D textBorder, Texture2D lifeIcon, Texture2D backArrow, Controls control)
    {
        timer = new Stopwatch();
        controls = control;
        spriteBatch = _spriteBatch;
        font = testFont;
        textBox = textBorder;
        heart = lifeIcon;
        arrow = backArrow;
        getControls();
        //General box layout
        topBox = new Rectangle(295, 140, 160, 40);
        upperBox = new Rectangle(295, 190, 160, 40);
        lowerBox = new Rectangle(295, 240, 160, 40);
        bottomBox = new Rectangle(295, 290, 160, 40);

        upperBoxLeft = upperBox;
        upperBoxLeft.Offset(-200, 0);
        upperBoxRight = upperBox;
        upperBoxRight.Offset(200, 0);

        lowerBoxLeft = lowerBox;
        lowerBoxLeft.Offset(-200, 0);
        lowerBoxRight = lowerBox;
        lowerBoxRight.Offset(200, 0);

        leaderboardBoxLeft = new Rectangle(205, 240, 130, 40);
        leaderboardBoxMiddleLeft = new Rectangle(365, 240, 130, 40);
        leaderboardBoxMiddleRight = new Rectangle(525, 240, 130, 40);
        leaderboardBoxRight = new Rectangle(685, 240, 130, 40);

        level = 1;
        currentColour = 0;
        counter = 0;
        text = "loading";
    }
    public int getScore() {
        return score;
    }

    public static void increaseScore(int increase)
    {
        score += increase * level / 10;
    }

    public void drawScore() {
        drawText("Your score is " + score, new Vector2(295, 140));
    }

    public void getControls() {
        //Gets the controls currently in use so that the user will later be able to see what the current controls are
        left = controls.getLeft();
        right = controls.getRight();
        fire = controls.getFire();
        pause = controls.getPause();
    }

    public string drawControlsMenu() {
        drawText("Click on a box and enter a key to edit it's keybind", new Vector2(270, 120), Color.Brown);

        getControls();
        //Calls the drawing for the buttons
        if (drawButtons(textBox, topBox, "Left - " + left)) {
            return "Left";
        }

        //for right control changing
        if (drawButtons(textBox, upperBox, "Right - " + right))
        {
            return "Right";
        }


        if (drawButtons(textBox, lowerBox, "Shoot - " + fire))
        {
            return "Shoot";

        }

        //for pause control changing
        if (drawButtons(textBox, bottomBox, "Pause - " + pause))
        {
            return "Pause";

        }
            //For arrow going back to main menu
        if (drawButtons(arrow, new Rectangle(50, 50, 100, 50), "")) {
            return "Back";
        }

        return "";
    }

    private void getMouseState() {
        //Gets Mouse location and coordinates
        previousMouseState = mouseState;
        mouseState = Mouse.GetState();
        mouseX = mouseState.X;
        mouseY = mouseState.Y;
    }

    private bool drawButtons(Texture2D boxTexture, Rectangle destinationRectangle, string text)
    {
        //Draw buttons and returns true when pressed
        getMouseState();
        isButtonPressed = false;

        spriteBatch.Begin();
        spriteBatch.Draw(boxTexture, destinationRectangle, Color.White);
        spriteBatch.End();
        drawText(text, new Vector2(destinationRectangle.Left + 10, destinationRectangle.Top + 10));

        //Keeps selected box highlighted
        spriteBatch.Begin();
        //Changes color of text box to let the user know it is interactable and selected
        if (destinationRectangle.Intersects(new Rectangle(mouseX, mouseY, 1, 1)))
        { 
            spriteBatch.Draw(boxTexture, destinationRectangle, Color.Green);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                isButtonPressed = true;
            }
        }
        spriteBatch.End();
        return isButtonPressed;
    }

    public string drawPauseMenu() {
        if (drawButtons(textBox, topBox, "Play Game/Continue")) {
            return "Start game";
        }

        if (drawButtons(textBox, upperBox, "Controls"))
        {
            return "Controls menu";
        }

        /*if (drawButtons(textBox, lowerBox, "Customise")) {
            return "Customise";
        }*/

        return "";
    }

    public bool closeLeaderboard() {
        return drawButtons(textBox, new Rectangle(630, 100, 160, 40), "Close leaderboard");
    }

    public string drawPlayerDataNames()
    {
        drawText("Username", new Vector2(75, 250));
        if (drawButtons(textBox, leaderboardBoxLeft, "Highest level"))
        {
            return "sortLevel";
        }
        if (drawButtons(textBox, leaderboardBoxMiddleLeft, "Longest survived"))
        {
            return "sortTime";
        }
        if (drawButtons(textBox, leaderboardBoxMiddleRight, "High score"))
        {
            return "sortScore";
        }
        return "";
    }


    public string drawNewPages() {
        if (drawButtons(arrow, new Rectangle(), "")) {
            return "previous";
        }
        if (drawButtons(arrow, new Rectangle(), "")) {
            return "next";
        }

        return "";
    }

    public void drawLives(int noOfLives) {
        //Draws the hearts from right to left for a certain number of them
        if (noOfLives > 0)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(heart, new Rectangle(60 * noOfLives, 20, 50, 50), Color.White);
            spriteBatch.End();
            noOfLives -= 1;
            drawLives(noOfLives);
        }
    }
    //Stopwatch functions
    //Uses c# timespan as a timer that only runs when the game is unpaused
    public void startStopwatch()
    {

        timer.Start();
        Debug.WriteLine("Timer started");
    }

    public void stopStopwatch()
    {
        timer.Stop();
    }

    public string getStopwatchTime()
    {
        TimeSpan timeElapsed = timer.Elapsed;
        //Sets the time taken to a string with a format of five digits of seconds and two digits of miliseconds
        timeTaken = timeElapsed.ToString(@"mm\:ss\.ff");
        return timeTaken;
    }

    public void drawStopwatchTime() {
        getStopwatchTime();
        drawText("Time survived: " + timeTaken, new Vector2(620, 920));
    }

    public void resetStopwatch() {
        timer.Reset();
    }
        //Checks to see if the cooldown has elapsed by minusing the start time from the current time and checking to see if it is greater than the cooldown time. It then returns true if that is the case as the cooldown has ended
    public bool checkCooldown(double startTime, double cooldownTime) {
        currentTime = Convert.ToDouble(getStopwatchTime().Replace(":", ""));
        if ((currentTime - startTime) >= cooldownTime)
        {
            return true;
        }
        return false;
    }

    public static int getLevel() {
        return level;
    }

    public void increaseLevel() {
        level++;
    }

    public void drawLevel() {
        drawText("Level: " + level, new Vector2(200, 920));
    }

    public void drawCustomiseMenu(Texture2D texture)
    {
        
        List<Color> colours = new List<Color> { Color.White, Color.Yellow, Color.Blue, Color.Red, Color.Green };
        spriteBatch.Begin();
        spriteBatch.Draw(texture, upperBox, new Rectangle(0, 0, 250, 330), colours[currentColour]);
        spriteBatch.End();
        if (drawButtons(textBox, upperBoxLeft, "Previous")) {
            currentColour--;
        }
        if (drawButtons(textBox, upperBoxRight, "Next")) {
            currentColour++;
        }
        if (currentColour >= colours.Count) {
            currentColour = 0;
        }
    }

        //Asks the user if they want to save their score in the database
    public string checkIfUserWantsToSave() {
        if (drawButtons(textBox, upperBoxLeft, "I want to save my score"))
        {
            return "save";
        }
        if (drawButtons(textBox, upperBoxRight, "I dont want to save my score")) {
            return "dont save";
        }
        return "";
        
    }

    public string checkIfNewUserToBeMade()
    {
        if (drawButtons(textBox, lowerBoxLeft, "Use existing user"))
        {
            return "login";
        }
        if (drawButtons(textBox, lowerBoxRight, "Create a new user"))
        {
            return "create";
        }
        if (drawButtons(arrow, new Rectangle(50, 50, 100, 50), ""))
        {
            return "back";
        }
        return "";

    }

    public string checkIfLeaderboardShown() {
        if (drawButtons(textBox, lowerBoxLeft, "Display leaderboard"))
        {
            return "display";
        }
        if (drawButtons(textBox, lowerBoxRight, "Skip leaderboard"))
        {
            return "dont display";
        }
        return "";
    }

    //Adds characters to word based on what keys are being pressed
    public (bool, List<string>) enterText(KeyboardState currentKeyState, KeyboardState previousKeyState, List<string> word)
    {
        cont = true;
        if (currentKeyState.GetPressedKeyCount() >= 1)
        {
            Keys key = currentKeyState.GetPressedKeys()[0];
            Debug.WriteLine("Key = "+key.ToString());
            //Makes sure that one press isn't registered as multiple due to fast updates
            if (!previousKeyState.IsKeyDown(key))
            {
                //Cases for keys when holding shift
                if (currentKeyState.IsKeyDown(Keys.LeftShift) || currentKeyState.IsKeyDown(Keys.RightShift))
                {
                    if (key >= Keys.A && key <= Keys.Z)
                    {
                        word.Add(letterChecking(key).ToUpper());
                    }
                    else
                    {
                        switch (key)
                        {
                            case Keys.D0:
                                word.Add(")");
                                break;
                            case Keys.D1:
                                word.Add("!");
                                break;
                            case Keys.D2:
                                word.Add('"'.ToString());
                                break;
                            //Removed £ and $ as they break the font
                            case Keys.D5:
                                word.Add("%");
                                break;
                            case Keys.D6:
                                word.Add("^");
                                break;
                            case Keys.D7:
                                word.Add("&");
                                break;
                            case Keys.D8:
                                word.Add("*");
                                break;
                            case Keys.D9:
                                word.Add("(");
                                break;
                            case Keys.Back:
                                word.RemoveAt(word.Count - 1);
                                break;
                            case Keys.Enter:
                                cont = false;
                                break;
                        }
                    }
                }
                else
                {
                    //Keys when not holding shift
                    if (key >= Keys.A && key <= Keys.Z)
                    {
                        word.Add(letterChecking(key));
                    }
                    else if (key >= Keys.D0 && key <= Keys.D9) {
                        word.Add(key.ToString()[1].ToString());
                    }
                    else
                    {
                        switch (key)
                        {
                            case Keys.Back:
                                if (word.Count > 0)
                                {
                                    word.RemoveAt(word.Count - 1);
                                }
                                break;
                            case Keys.Enter:
                                cont = false;
                                break;
                        }
                    }
                }
            }
        }
                return (cont, word);
    }
        //Gets letters form user input
    private string letterChecking(Keys key) {
        return key.ToString().ToLower();
        }

        //Draws text on the screen
    public void drawText(string text, Vector2 position) {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, text, position, Color.White);
        spriteBatch.End();
    }
        //Polymorphism incase different colours are wanted
    public void drawText(string text, Vector2 position, Color colour) {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, text, position, colour);
        spriteBatch.End();
    }

    public void loadingScreen(Texture2D loadingSpriteSheet) {
        if (counter > 250) {
            counter = 0;
            text = "loading";
        }
        if (counter % 50 == 0)
        {
            sourceRectangle = new Rectangle(200 * (int)counter / 50, 0, 200, 200);
            text += ".";
        }
        spriteBatch.Begin();
        spriteBatch.Draw(loadingSpriteSheet, new Rectangle(500, 100, 40, 40), sourceRectangle, Color.White);
        spriteBatch.End();
        drawText(text, new Vector2(497-counter/50, 145));
        counter++;
        
    }

}

