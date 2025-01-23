using Alien_Attack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Alien_Attack
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player1;
        public Controls controls;
        private UI ui;
        private Bullets playerBullet;
        private Bunkers bunkers;
        private List<BunkerPart> bunkerParts;
        private EnemyController enemies;
        private Vector2 player1StartPos;
        private Texture2D player1Texture;
        private Texture2D playerBulletTexture;
        private Texture2D textBorder;
        private Texture2D enemyTexture;
        private Texture2D enemyBulletTexture;
        private Texture2D bunkerAtlas;
        private Texture2D lifeIcon;
        private Texture2D backArrow;
        private Texture2D background;
        private SpriteFont font;
        private static KeyboardState currentKeyState;
        private KeyboardState previousKeyState;
        private Keys shoot;
        private Keys pause;
        private bool gamePaused;
        private bool playerBulletActive;
        private bool bunkerHitByPlayer;
        private bool playerHit;
        private bool inControlsMenu;
        private bool noMenu;
        private bool gameStarted;
        private bool gameOver;
        private int extraXCoord;
        private int noOfLives;
        private int enemyRows;
        private int enemyCollums;
        private int level;
        private Rectangle bulletHitbox;
        private Rectangle partHitbox;
        private Rectangle backgroundBox;
        private string getNewKeybindOf;
        private string option;
        private string gameMode;
        private string deathReason;
        private MouseState mouseState;
        private int noOfLives;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
            
            gameMode = "endless";
            gameStarted = false;
            gamePaused = true;
            noMenu = false;
            enemyRows = 2;
            enemyCollums = 5;
            noOfLives = 3;
            //Set window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            backgroundBox = new Rectangle(0, 0, 800, 1000);
            controls = new Controls();
            getControls();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loading all textures for the game
            textBorder = Content.Load<Texture2D>("textBorder");
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            player1Texture = Content.Load<Texture2D>("player2");
            enemyTexture = Content.Load<Texture2D>("enemyPlaceholder");
            enemyBulletTexture = Content.Load<Texture2D>("enemyBulletPlaceholder");
            bunkerAtlas = Content.Load<Texture2D>("combinedBlocks");
            lifeIcon = Content.Load<Texture2D>("playerHeartPlaceholder");
            backArrow = Content.Load<Texture2D>("backArrow");
            font = Content.Load<SpriteFont>("testFont");

            //Instansiating the UI
            ui = new UI(_spriteBatch, font, textBorder, lifeIcon, backArrow, controls);
        }

        public void startNewGame() {
            deathReason = "";
            player1StartPos = new Vector2(50, 800);
            player1 = new Player(player1Texture, player1StartPos, controls);
            ui = new UI(_spriteBatch, font, textBorder, lifeIcon, controls);
            enemies = new EnemyController(_spriteBatch, enemyTexture, new Vector2(11, 50), gameMode);
            enemies.spawnEnemies(enemyRows, enemyCollums);            
            noOfLives = 4;
            playerBulletActive = false;
            gameStarted = true;
            gamePaused = false;
            noMenu = false;
            gameOver = false;
            enemies.spawnEnemies(); 
        }

        protected override void Update(GameTime gameTime)
            //Gets what keys are being pressed on the keyboard
            mouseState = Mouse.GetState();
                Exit();

            // TODO: Add your update logic here
            // Calls updates  
            // pauses the main game when the button is pressed
                //Gets what keys are being pressed on the keyboard
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            pauseGame();
            //Only runs when game isn't paused
            if (!gamePaused)
            {
                firePlayerBullet();
                //Moving player when keys pressed, moving enemies and changing 
                player1.updatePlayer(currentKeyState, previousKeyState);
                enemies.updateAllEnemies();
                    checkplayerBulletCollision();
                }
                checkEnemyBulletCollision();

                //Game over when the player runs out of lives
                if (noOfLives == 0) {
                    deathReason = "You ran out of lives";
                    gameOver = true;
                }
                //Game won when the number of enemies is 0
                if (EnemyController.getNumberOfEnemies() == -1  ) {
                        //Code here for game won
                }

            }

            //All logic for changing keybinds while game is paused. In this class due to monogame restrictions
            if (gamePaused && inControlsMenu && !noMenu) {
                controls.setNewKeybind(currentKeyState);
                getControls();
                   
                    //Goes through every part of the bunker and checks to see if the enemies bullets have hit it
                bunkerParts = bunkers.GetBunkerParts();
                foreach (BunkerPart part in bunkerParts)
                {
                    partHitbox = part.getBunkerHitbox();
                    if (enemies.checkPlayerCollision(partHitbox))
            
            //_spriteBatch.Begin();
            //_spriteBatch.Draw(background, backgroundBox, Color.White);
            //_spriteBatch.End();


            //Draws any required parts if the game is finished
            checkIfGameOver();
                 //prevents any game processes from advance if the game is paused
            if (!gamePaused)
            {
                ui.drawLives(noOfLives);
                }
                    //Checks to see if the enemy bullets have hit the player and if so, reduces number of lives
                playerHit = enemies.checkPlayerCollision(player1.getPlayerHitbox());
                if (playerHit) {
                    noOfLives -= 1;
                    Debug.WriteLine("Player hit");
                    playerHit = false;}
            }

                //All logic for changing keybinds while game is paused
            if (gamePaused) {
                if (!keyAccepted && !isKeyInput)
                {
                    //There is a temporary variable to make sure the keyboard check is repeated
                    if (tempGetNewKeybindOf != "") {
                        getNewKeybindOf = tempGetNewKeybindOf;
                    }
                    switch (getNewKeybindOf)
                    {
                        case "Left":
                            isKeyInput = controls.setLeft(currentKeyState);
                            break;
                        case "Right":
                            isKeyInput = controls.setRight(currentKeyState);
                            break;
            }
                //Handles all methods for outside the control menu when the game is paused
            else if (gamePaused && !noMenu)
            {
                option = ui.drawPauseMenu();
                switch (option)
                {
                    case "Controls menu":
                        inControlsMenu = true;
                        break;
                    case "Start game":
                        if (gameStarted)
                        {
                            gamePaused = false;
                        }
                        else
                        {
                            startNewGame();
                        }
                        break;
            }
        }
        //Draws all textures in the game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //prevents the bullet from moving when the game is paused
            ui.drawLives(noOfLives);
            if (gamePaused == false)
            {
                player1.drawPlayer(_spriteBatch);

        private void checkplayerBulletCollision() {
            bulletHitbox = playerBullet.getHitbox();
            playerBulletActive = !enemies.checkCollision(bulletHitbox);
            if (playerBullet.getBulletPos().Y <= -60)
            {
                playerBulletActive = false;
            }
            //Checks if the payers bullet hits the bunker
            bunkerHitByPlayer = bunkers.checkBunkerCollision(bulletHitbox);
            if (bunkerHitByPlayer)
            {
                playerBulletActive = false;
            }
        }

        private void checkEnemyBulletCollision() {
                //Goes through every part of the bunker and checks to see if the enemies bullets have hit it
            bunkerParts = bunkers.GetBunkerParts();
            foreach (BunkerPart part in bunkerParts)
            {
                partHitbox = part.getBunkerHitbox();
                if (enemies.checkPlayerCollision(partHitbox))
                {
                    part.partHit();
                }
            }
                //Checks to see if the enemy bullets have hit the player and if so, reduces number of lives
            playerHit = enemies.checkPlayerCollision(player1.getPlayerHitbox());
            if (playerHit)
            {
                noOfLives -= 1;
                Debug.WriteLine("Player hit");
                playerHit = false;
            }
        }
                enemies.drawAllEnemies();
                bunkers.drawBunkers(_spriteBatch);
                if (playerBulletActive)
                {
                    playerBullet.drawBullets(_spriteBatch);
                }
            }
                //Shows that the game is paused
            if (gamePaused) {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Game paused", new Vector2(320, 100), Color.Red);
                _spriteBatch.End();
            }
                //Handles all methods for the controls menu
            if (gamePaused && inControlsMenu && !noMenu)
            {
                tempGetNewKeybindOf = ui.drawControlsMenu();
                controls.getKeybindToSet(tempGetNewKeybindOf);

                if (tempGetNewKeybindOf == "Back")
                {
                    inControlsMenu = false;
                }

            if (gamePaused)
            {
                tempGetNewKeybindOf = ui.drawPauseMenu();
                if (tempGetNewKeybindOf == "") {
                    keyAccepted = false;
                }
                else
                {
                    //Done so that user will always end up back in the pause menu even if unpausing from control menu
                    inControlsMenu = false;
                }
            }
        }
        
            //Code to run when the player either wins or loses a game
        private void checkIfGameOver() {
            if (gameOver)
            {
                gamePaused = true;
                noMenu = true;
                gameStarted = false;
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Game Over", new Vector2(295, 90), Color.White);
                _spriteBatch.DrawString(font, deathReason, new Vector2(295, 120), Color.White);
                _spriteBatch.DrawString(font, "Click to continue", new Vector2(295, 170), Color.Green);
                _spriteBatch.End();
                ui.drawScore();
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    gameOver = false;
                    noMenu = false;
                    isKeyInput = false;
                }
                
            }
            base.Draw(gameTime);
        }

        //Gets the current keybinds for firing and pausing the game
        private void getControls()
        {
            shoot = controls.getFire();
            pause = controls.getPause();
        }


        private void firePlayerBullet()
        {
                //Bullet will only be fired when there is no other bullet on screen and the player has pressed the key for firing
            if (currentKeyState.IsKeyDown(shoot) && !playerBulletActive)
            {
                extraXCoord = player1.getPLayerWidth()/2;
                playerBullet = new Bullets(5, playerBulletTexture, "up", player1.getPosition(), extraXCoord);
                playerBulletActive = true;
            }
        }
            //Stops all bullet related functions being called for the player's bullet
        private void deactivateBullet() {
            playerBulletActive = false;
        }

            //pause game when the assigned key is pressed and the game isnt already paused, otherwise unpauses it
        private void pauseGame()
        {
            if (currentKeyState.IsKeyDown(pause) && previousKeyState.IsKeyUp(pause) && gameStarted)
            {
                if (!gamePaused)
                {
                    gamePaused = true;
                }

                else if (gamePaused)
                {
                    gamePaused = false;
                    getControls();
                    player1.getControls();
                }
            }
        }
    }
}
