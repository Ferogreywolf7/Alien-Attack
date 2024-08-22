using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Alien_Attack;
using System.Net;

public class UI
{
	//Instance variables
	private SpriteBatch spriteBatch;
	private SpriteFont font;
	private Texture2D textBox;
	private int score;
	private Rectangle box1;
    private Rectangle box2;
    private Rectangle box3;
    private Rectangle box4;
	private Keys left;
	private Keys right;
	private Keys fire;
	private Keys pause;
	private Controls controls;
	private MouseState mouseState;
    private MouseState previousMouseState;
	private int x;
	private int y;

    public UI(SpriteBatch _spriteBatch, SpriteFont testFont, Texture2D textBorder)
	{
		spriteBatch = _spriteBatch;
		font = testFont;
		textBox = textBorder;
		getCurrentControls();
    }
	public int getScore() {
		return score;
	}
	public void drawScore() {
	}

	public void getCurrentControls() {
		controls = new Controls();
		left = controls.getLeft();
		right = controls.getRight();
		fire = controls.getFire();
		pause = controls.getPause();
	}

	public void drawControlsMenu() {
		previousMouseState = mouseState;
		mouseState = Mouse.GetState();
		x = mouseState.X;
		y = mouseState.Y;

		//draws the basic layout
		box1 = new Rectangle(270, 97, 150, 150);
		
		spriteBatch.Begin();
		spriteBatch.Draw(textBox, box1, Color.White);
		spriteBatch.DrawString(font, "Move left: " + left, new Vector2(305, 150), Color.White);

		if (box1.Intersects(new Rectangle(x, y, 1, 1))) {
            spriteBatch.Draw(textBox, box1, Color.Green);
        }

		spriteBatch.End();
	}
	public void drawPauseMenu() {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Game paused", new Vector2(300, 100), Color.Red);
        spriteBatch.End();
		drawControlsMenu();
    }

}
