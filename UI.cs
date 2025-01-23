using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Alien_Attack;
using System.Net;
using System.Diagnostics;

public class UI
{
	//Instance variables
	private SpriteBatch spriteBatch;
	private SpriteFont font;
	private Texture2D textBox;
    private Texture2D heart;
    private Texture2D arrow;
	private int score;
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
    public UI(SpriteBatch _spriteBatch, SpriteFont testFont, Texture2D textBorder, Texture2D lifeIcon, Texture2D backArrow, Controls control)
	{
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
    }
	public int getScore() {
		return score;
	}

    public void increaseScore(int increase)
    {
        score += increase * modifier;
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
        if(drawButtons(textBox, topBox, "Left - " + left)) {
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

        if (drawButtons(arrow, new Rectangle(50, 50, 100, 50), "")) {
            Debug.WriteLine("Back arrow pressed");
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

        return "";
    }

    public void drawLives(int noOfLives) {
            //Draws the hearts from right to left for a certain number of them
        if (noOfLives > 0)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(heart, new Rectangle (60* noOfLives, 20, 50, 50), Color.White);
            spriteBatch.End();
            noOfLives -= 1;
            drawLives(noOfLives);
        }
    }
}
