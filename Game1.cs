using Alien_Attack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;


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
        private Vector2 enemyStartPos;
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
        private string tempGetNewKeybindOf;
        private string option;
        private string gameMode;
        private string deathReason;
        private MouseState mouseState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameStarted = false;
            gamePaused = true;
            noMenu = false;
            enemyRows = 2;
            enemyCollums = 5;
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

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Instansiating the UI
            ui = new UI(_spriteBatch, font, textBorder, lifeIcon, backArrow, controls);
        }

        public void startNewGame() {
            gameMode = "Endless";
            deathReason = "";
            player1StartPos = new Vector2(50, 800);
            enemyStartPos = new Vector2(11, 50);
            player1 = new Player(player1Texture, playerBulletTexture, player1StartPos, controls, ui);
            bunkers = new Bunkers(bunkerAtlas, 2);
            enemies = new EnemyController(_spriteBatch, enemyTexture, enemyStartPos, gameMode);
            enemies.spawnEnemies(enemyRows, enemyCollums);            
            noOfLives = 10;
            playerBulletActive = false;
            gameStarted = true;
            gamePaused = false;
            noMenu = false;
            gameOver = false;
            ui.startStopwatch();
        }

        protected override void Update(GameTime gameTime)
        {
            //Gets what keys are being pressed on the keyboard
            mouseState = Mouse.GetState();
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            pauseGame();
            //Only runs when game isn't paused
            if (!gamePaused)
            {
                CheckForGameOverConditions();
                //Moving player when keys pressed, moving enemies and changing bunker states
                player1.updatePlayer(currentKeyState, previousKeyState);
                enemies.updateAllEnemies();
                bunkers.updateBunkers();

                checkCollisions();

            }

            //All logic for changing keybinds while game is paused. In this class due to monogame restrictions
            if (gamePaused && inControlsMenu && !noMenu) {
                controls.setNewKeybind(currentKeyState);
                getControls();
            }
            base.Update(gameTime);
        }
        //Draws all textures in the game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //_spriteBatch.Begin();
            //_spriteBatch.Draw(background, backgroundBox, Color.White);
            //_spriteBatch.End();


            //Draws any required parts if the game is finished
            checkIfGameOver();
                 //prevents any game processes from advance if the game is paused
            if (!gamePaused)
            {
                ui.drawLives(noOfLives);
                player1.drawPlayer(_spriteBatch);
                enemies.drawAllEnemies();
                bunkers.drawBunkers(_spriteBatch);
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
            base.Draw(gameTime);
        }

        //Gets the current keybinds for firing and pausing the game
        private void getControls()
        {
            shoot = controls.getFire();
            pause = controls.getPause();
        }

        private void checkCollisions() {
            if (player1.isBulletActive())
            {
                player1.checkplayerBulletCollision(enemies, bunkers);
            }
            checkEnemyBulletCollision();

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


            //pause game when the assigned key is pressed and the game isnt already paused, otherwise unpauses it
        private void pauseGame()
        {
            if (currentKeyState.IsKeyDown(pause) && previousKeyState.IsKeyUp(pause) && gameStarted)
            {
                if (!gamePaused)
                {
                    gamePaused = true;
                    ui.stopStopwatch();
                    ui.getStopwatchTime();
                }

                else if (gamePaused)
                {
                    ui.startStopwatch();
                    gamePaused = false;
                    getControls();
                    player1.getControls();
                    //Done so that user will always end up back in the pause menu even if unpausing from control menu
                    inControlsMenu = false;
                }
            }
        }

        //Code to check all conditions for the game to be finished
        private void CheckForGameOverConditions() {
            if (noOfLives == 0)
            {
                deathReason = "You ran out of lives";
                gameOver = true;
            }
            if (enemies.getLowestEnemy() >= 700) {
                
                deathReason = "The enemies got too low and you were overrun";
                gameOver = true;
            }
            if (EnemyController.getNumberOfEnemies() == -1) {
                deathReason = "All enemies were cleared";
                gameOver = true;
            }
            if (EnemyController.getNumberOfEnemies() == -1)
            {
                //Code here for game won
            }

        }
        
            //Code to run when the player either wins or loses a game
        private void checkIfGameOver() {
            if (gameOver)
            {
                ui.stopStopwatch();
                ui.getStopwatchTime();
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
                }
            }
        }

        
    }
}
