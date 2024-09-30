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

    private void drawControlsMenu(KeyboardState keyboard) {
        //Gets the current state of the mouse and its coordinates
		previousMouseState = mouseState;
		mouseState = Mouse.GetState();
		mouseX = mouseState.X;
		mouseY = mouseState.Y;

		
		
        //Defines the rectangles that the text box textures will be put onto
		boxLeft  = new Rectangle(295, 140, 160, 40);
		boxRight = new Rectangle(295, 190, 160, 40);
		boxShoot = new Rectangle(295, 240, 160, 40);
		boxPause = new Rectangle(295, 290, 160, 40);


        spriteBatch.Begin();
        //for left control changing
		spriteBatch.Draw(textBox, boxLeft, Color.White);
		spriteBatch.DrawString(font, "Move left - " + left, new Vector2(305, 150), Color.White);

		//Changes color of text box to let the user know it is interactable and selected
		if (boxLeft.Intersects(new Rectangle(mouseX, mouseY, 1, 1))) {
            spriteBatch.Draw(textBox, boxLeft, Color.Green);
			//Calls setLeft in order for the left control to be updated
			if (mouseState.LeftButton == ButtonState.Pressed) {
                Debug.WriteLine("Button clicked");
				controls.setLeft(keyboard);
                spriteBatch.Draw(textBox, boxLeft, Color.White);
                getControls();
			}
        }

        //for right control changing
        spriteBatch.Draw(textBox, boxRight, Color.White);
        spriteBatch.DrawString(font, "Move right - " + right, new Vector2(305, 200), Color.White);

        //Changes color of text box to let the user know it is interactable and selected
        if (boxRight.Intersects(new Rectangle(mouseX, mouseY, 1, 1)))
        {
            spriteBatch.Draw(textBox, boxRight, Color.Green);
            //Calls setRight when clicked in order for the right keybind to be updated
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                controls.setRight();
                spriteBatch.Draw(textBox, boxRight, Color.White);
                getControls();
            }
        }

        //for shooting control changing
        spriteBatch.Draw(textBox, boxShoot, Color.White);
        spriteBatch.DrawString(font, "Shoot bullet - " + fire, new Vector2(305, 250), Color.White);

        //Changes color of text box to let the user know it is interactable and selected
        if (boxShoot.Intersects(new Rectangle(mouseX, mouseY, 1, 1)))
        {
            spriteBatch.Draw(textBox, boxShoot, Color.Green);
            //Calls setLeft in order for the left control to be updated
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                controls.setShoot();
                spriteBatch.Draw(textBox, boxShoot, Color.White);
                getControls();
            }
        }

        //for pause control changing
        spriteBatch.Draw(textBox, boxPause, Color.White);
        spriteBatch.DrawString(font, "Pause game - " + pause, new Vector2(305, 300), Color.White);

        //Changes color of text box to let the user know it is interactable and selected
        if (boxPause.Intersects(new Rectangle(mouseX, mouseY, 1, 1)))
        {
            spriteBatch.Draw(textBox, boxPause, Color.Green);
            //Calls setLeft in order for the left control to be updated
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                controls.setPause();
                spriteBatch.Draw(textBox, boxPause, Color.White);
                getControls();
            }
        }

        spriteBatch.End();
	}
	public void drawPauseMenu(KeyboardState keyboard) {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Game paused", new Vector2(300, 100), Color.Red);
        spriteBatch.End();
		drawControlsMenu(keyboard);
    }

}
