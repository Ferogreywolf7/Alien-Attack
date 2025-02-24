using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Alien_Attack;
using System.Net;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

public class UI
{
    //Instance variables
    private SpriteBatch spriteBatch;
    private SpriteFont font;
    private Texture2D textBox;
    private Texture2D heart;
    private Texture2D arrow;
    private int modifier;
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
    private Controls controls;
    private string selectedBox;
    private Stopwatch timer;
    private string timeTaken;
    private static int level;
    private static int score;
    private int currentColour;
    private Rectangle upperBoxLeft;
    private Rectangle upperBoxRight;
    private Rectangle lowerBoxLeft;
    private Rectangle lowerBoxRight;
    public UI(SpriteBatch _spriteBatch, SpriteFont testFont, Texture2D textBorder, Texture2D lifeIcon, Texture2D backArrow, Controls control)
    {
        Debug.WriteLine("timer instansiated");
        timer = new Stopwatch();
        modifier = 1;
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

        level = 1;
        currentColour = 0;
    }
    public int getScore() {
        return score;
    }

    public static void increaseScore(int increase)
    {
        score += increase * level / 10;
    }

    public void drawScore() {
       spriteBatch.Begin();
        spriteBatch.DrawString(font, "Your score is " + score, new Vector2(295, 140), Color.White);
       spriteBatch.End();
    }

    public void getControls() {
        //Gets the controls currently in use so that the user will later be able to see what the current controls are
        left = controls.getLeft();
        right = controls.getRight();
        fire = controls.getFire();
        pause = controls.getPause();
    }

    public string drawControlsMenu() {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Click on a box and enter a key to edit it's keybind", new Vector2(270, 120), Color.Brown);
        spriteBatch.End();

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


    private bool drawButtons(Texture2D boxTexture, Rectangle destinationRectangle, string text)
    {
        //Draw buttons and returns true when pressed
        //Gets Mouse location and coordinates
        previousMouseState = mouseState;
        mouseState = Mouse.GetState();
        mouseX = mouseState.X;
        mouseY = mouseState.Y;
        isButtonPressed = false;

        spriteBatch.Begin();
        spriteBatch.Draw(boxTexture, destinationRectangle, Color.White);
        spriteBatch.DrawString(font, text, new Vector2(destinationRectangle.Left + 10, destinationRectangle.Top + 10), Color.White);

        //Keeps selected box highlighted

        //Changes color of text box to let the user know it is interactable and selected
        if (destinationRectangle.Intersects(new Rectangle(mouseX, mouseY, 1, 1)))
        {
            spriteBatch.Draw(boxTexture, destinationRectangle, Color.Green);
            //Calls setLeft in order for the left control to be updated
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
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Time survived: " + timeTaken, new Vector2(620, 920), Color.White);
        spriteBatch.End();
    }

    public void resetStopwatch() {
        timer.Restart();
    }

    public static int getLevel() {
        return level;
    }

    public void increaseLevel() {
        level++;
    }

    public void drawLevel() {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Level: " + level, new Vector2(200, 920), Color.White);
        spriteBatch.End();
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
        return "";

    }

    //Adds characters to word based on what keys are being pressed
    public (bool,List<string>) enterText(KeyboardState currentKeyState,KeyboardState previousKeyState, List<string> word)
    {
        bool cont = true;
        if (currentKeyState.GetPressedKeyCount() >= 1) { 
            Keys key = currentKeyState.GetPressedKeys()[0];
                //Makes sure that one press isn't registered as multiple due to fast updates
            if (!previousKeyState.IsKeyDown(key)) {
                    //Cases for keys when holding shift
                if (currentKeyState.IsKeyDown(Keys.LeftShift) || currentKeyState.IsKeyDown(Keys.RightShift))
                {
                    switch (key)
                    {
                        case Keys.A:
                            word.Add("A");
                            break;
                        case Keys.B:
                            word.Add("B");
                            break;
                        case Keys.C:
                            word.Add("C");
                            break;
                        case Keys.D:
                            word.Add("D");
                            break;
                        case Keys.E:
                            word.Add("E");
                            break;
                        case Keys.F:
                            word.Add("F");
                            break;
                        case Keys.G:
                            word.Add("G");
                            break;
                        case Keys.H:
                            word.Add("H");
                            break;
                        case Keys.I:
                            word.Add("I");
                            break;
                        case Keys.J:
                            word.Add("J");
                            break;
                        case Keys.K:
                            word.Add("K");
                            break;
                        case Keys.L:
                            word.Add("L");
                            break;
                        case Keys.M:
                            word.Add("M");
                            break;
                        case Keys.N:
                            word.Add("N");
                            break;
                        case Keys.O:
                            word.Add("O");
                            break;
                        case Keys.P:
                            word.Add("P");
                            break;
                        case Keys.Q:
                            word.Add("Q");
                            break;
                        case Keys.R:
                            word.Add("R");
                            break;
                        case Keys.S:
                            word.Add("S");
                            break;
                        case Keys.T:
                            word.Add("T");
                            break;
                        case Keys.U:
                            word.Add("U");
                            break;
                        case Keys.V:
                            word.Add("V");
                            break;
                        case Keys.W:
                            word.Add("W");
                            break;
                        case Keys.X:
                            word.Add("X");
                            break;
                        case Keys.Y:
                            word.Add("Y");
                            break;
                        case Keys.Z:
                            word.Add("Z");
                            break;
                        case Keys.D0:
                            word.Add(")");
                            break;
                        case Keys.D1:
                            word.Add("!");
                            break;
                        case Keys.D2:
                            word.Add('"'.ToString());
                            break;
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
                else
                {
                    switch (key)
                    {
                        case Keys.A:
                            word.Add("a");
                            break;
                        case Keys.B:
                            word.Add("b");
                            break;
                        case Keys.C:
                            word.Add("c");
                            break;
                        case Keys.D:
                            word.Add("d");
                            break;
                        case Keys.E:
                            word.Add("e");
                            break;
                        case Keys.F:
                            word.Add("f");
                            break;
                        case Keys.G:
                            word.Add("g");
                            break;
                        case Keys.H:
                            word.Add("h");
                            break;
                        case Keys.I:
                            word.Add("i");
                            break;
                        case Keys.J:
                            word.Add("j");
                            break;
                        case Keys.K:
                            word.Add("k");
                            break;
                        case Keys.L:
                            word.Add("l");
                            break;
                        case Keys.M:
                            word.Add("m");
                            break;
                        case Keys.N:
                            word.Add("n");
                            break;
                        case Keys.O:
                            word.Add("o");
                            break;
                        case Keys.P:
                            word.Add("p");
                            break;
                        case Keys.Q:
                            word.Add("q");
                            break;
                        case Keys.R:
                            word.Add("r");
                            break;
                        case Keys.S:
                            word.Add("s");
                            break;
                        case Keys.T:
                            word.Add("t");
                            break;
                        case Keys.U:
                            word.Add("u");
                            break;
                        case Keys.V:
                            word.Add("v");
                            break;
                        case Keys.W:
                            word.Add("w");
                            break;
                        case Keys.X:
                            word.Add("x");
                            break;
                        case Keys.Y:
                            word.Add("y");
                            break;
                        case Keys.Z:
                            word.Add("z");
                            break;
                        case Keys.D0:
                            word.Add("0");
                            break;
                        case Keys.D1:
                            word.Add("1");
                            break;
                        case Keys.D2:
                            word.Add("2");
                            break;
                        case Keys.D3:
                            word.Add("3");
                            break;
                        case Keys.D4:
                            word.Add("4");
                            break;
                        case Keys.D5:
                            word.Add("5");
                            break;
                        case Keys.D6:
                            word.Add("6");
                            break;
                        case Keys.D7:
                            word.Add("7");
                            break;
                        case Keys.D8:
                            word.Add("8");
                            break;
                        case Keys.D9:
                            word.Add("9");
                            break;
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
        return (cont, word);
    }
}
