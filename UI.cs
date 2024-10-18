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
	private int score;
	private Rectangle boxLeft;
    private Rectangle boxRight;
    private Rectangle boxShoot;
    private Rectangle boxPause;
	private Keys left;
	private Keys right;
	private Keys fire;
	private Keys pause;
	private MouseState mouseState;
    private MouseState previousMouseState;
	private int mouseX;
	private int mouseY;
    private bool isButtonPressed;
    Controls controls;

    public UI(SpriteBatch _spriteBatch, SpriteFont testFont, Texture2D textBorder, Controls control)
	{
		controls = control;
        spriteBatch = _spriteBatch;
		font = testFont;
		textBox = textBorder;
		getControls();
    }
	public int getScore() {
		return score;
	}
	public void drawScore() {
	}

	public void getControls() {
		//Gets the controls currently in use so that the user will later be able to see what the current controls are
		left = controls.getLeft();
		right = controls.getRight();
		fire = controls.getFire();
		pause = controls.getPause();
	}

	private void drawControlsMenu() {

		
		
            //Defines the rectangles that the text box textures will be put onto
		boxLeft  = new Rectangle(295, 140, 160, 40);
		boxRight = new Rectangle(295, 190, 160, 40);
		boxShoot = new Rectangle(295, 240, 160, 40);
		boxPause = new Rectangle(295, 290, 160, 40);
        spriteBatch.Begin();

            //Calls the drawing for the buttons
        if(drawButtons(textBox, boxLeft, "Left - " + left)) { 
			controls.setLeft();
            spriteBatch.Draw(textBox, boxLeft, Color.White);
            getControls();
        }

        //for right control changing
        if (drawButtons(textBox, boxRight, "Right - " + right))
        {
            controls.setRight();
            spriteBatch.Draw(textBox, boxRight, Color.White);
            getControls();
        }


        if (drawButtons(textBox, boxShoot, "Shoot - " + fire))
        {
            controls.setShoot(); 
            spriteBatch.Draw(textBox, boxShoot, Color.White);
            getControls();
        }

        //for pause control changing
        if (drawButtons(textBox, boxPause, "Pause - " + pause))
        {
            controls.setPause();
            spriteBatch.Draw(textBox, boxPause, Color.White);
            getControls();
        }
        spriteBatch.End();
    }

    private bool drawButtons(Texture2D boxTexture, Rectangle destinationRectangle, string text)
    {
            //Draw buttons and returns true when pressed
        previousMouseState = mouseState;
        mouseState = Mouse.GetState();
        mouseX = mouseState.X;
        mouseY = mouseState.Y;
        isButtonPressed = false;

        spriteBatch.Draw(boxTexture, destinationRectangle, Color.White);
        spriteBatch.DrawString(font, text, new Vector2(destinationRectangle.Left + 10, destinationRectangle.Top + 10), Color.White);

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
        return isButtonPressed;
    }

    public void drawPauseMenu() {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Game paused", new Vector2(300, 100), Color.Red);
        spriteBatch.End();
		drawControlsMenu();
    }

}
