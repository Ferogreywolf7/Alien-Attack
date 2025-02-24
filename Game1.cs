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
        private Bunkers bunkers;
        private Database database;
        private List<BunkerPart> bunkerParts;
        private EnemyController enemies;
        private Vector2 player1StartPos;
        private Vector2 enemyStartPos;
        private Texture2D playerBulletTexture;
        private Texture2D textBorder;
        private Texture2D enemyTexture;
        private Texture2D bunkerAtlas;
        private Texture2D lifeIcon;
        private Texture2D backArrow;
        private Texture2D playerSpritSheet;
        private Texture2D explosionSpriteSheet;
        private Texture2D background;
        private SpriteFont font;
        private static KeyboardState currentKeyState;
        private KeyboardState previousKeyState;
        private Keys pause;
        private bool gamePaused;
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
        private bool inCustomiseMenu;
        private string wantsToSave;
        private string createNewUser;
        private List<string> word;
        private bool gameWon;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            database = new Database();
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
            database.tryConnectToDatabase();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loading all textures for the game
            textBorder = Content.Load<Texture2D>("textBorder");
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            enemyTexture = Content.Load<Texture2D>("enemyPlaceholder");
            bunkerAtlas = Content.Load<Texture2D>("combinedBlocks");
            lifeIcon = Content.Load<Texture2D>("playerHeartPlaceholder");
            backArrow = Content.Load<Texture2D>("backArrow");
            font = Content.Load<SpriteFont>("testFont");

            playerSpritSheet = Content.Load<Texture2D>("playerAnimationSpriteSheet");
            explosionSpriteSheet = Content.Load<Texture2D>("explosionAnimationSpriteSheet");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Instansiating the UI
            ui = new UI(_spriteBatch, font, textBorder, lifeIcon, backArrow, controls);
        }

        public void startNewGame() {
            gameMode = "Classic";
            deathReason = "";
            player1StartPos = new Vector2(50, 800);
            enemyStartPos = new Vector2(11, 50);
            player1 = new Player(playerSpritSheet, playerBulletTexture, player1StartPos, controls, ui);
            bunkers = new Bunkers(bunkerAtlas, 2);
            enemies = new EnemyController(_spriteBatch, enemyTexture, enemyStartPos, gameMode, explosionSpriteSheet);
            enemies.spawnEnemies(enemyRows, enemyCollums);            
            noOfLives = 5;
            gameStarted = true;
            gamePaused = false;
            noMenu = false;
            gameOver = false;
            gameWon = false;
            ui.startStopwatch();
            wantsToSave = "";
            createNewUser = "";
            word = new List<string>();
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
            checkIfGameWon();
            ui.drawStopwatchTime();
            ui.drawLevel();
                 //prevents any game processes from advance if the game is paused
            if (!gamePaused)
            {
                //Draws everything on the screen when it is running
                ui.drawLives(noOfLives);
                player1.drawPlayer(_spriteBatch);
                enemies.drawAllEnemies();
                bunkers.drawBunkers(_spriteBatch);
            }
                //Shows that the game is paused
            if (gamePaused && !noMenu) {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Game paused", new Vector2(320, 100), Color.Red);
                _spriteBatch.End();
            }

                //Handles all methods for the controls menu, allowing for keybinds to be changed
            if (gamePaused && inControlsMenu && !noMenu)
            {
                tempGetNewKeybindOf = ui.drawControlsMenu();
                controls.getKeybindToSet(tempGetNewKeybindOf);

                if (tempGetNewKeybindOf == "Back")
                {
                    inControlsMenu = false;
                }

            }
            if (gamePaused && inCustomiseMenu) {
                ui.drawCustomiseMenu(playerSpritSheet);
            }


            //Handles all methods in the main menu when the game is paused
            else if (gamePaused && !noMenu)
            {
                option = ui.drawPauseMenu();
                switch (option)
                {
                    case "Controls menu":
                        inControlsMenu = true;
                        break;
                    //Unpauses the game when start game is pressed if there is already a game going, otherwise it starts a new game
                    case "Start game":
                        if (gameStarted)
                        {
                            gamePaused = false;
                            ui.startStopwatch();
                            getControls();
                            player1.getControls();
                        }
                        else
                        {
                            startNewGame();
                        }
                        break;
                    case "Customise":
                        inCustomiseMenu = true;
                        break;
                }
            }
            base.Draw(gameTime);
        }

            //Gets the current keybinds for firing and pausing the game
        private void getControls()
        {
            pause = controls.getPause();
        }
            //Calls various subroutines to check collisions
        private void checkCollisions() { 
            player1.checkplayerBulletCollision(enemies, bunkers);
            checkEnemyBulletCollision();

        }

        private void checkEnemyBulletCollision() {
                //Goes through every part of the bunker and checks to see if the enemies bullets have hit it
            bunkerParts = bunkers.GetBunkerParts();
            foreach (BunkerPart part in bunkerParts)
            {
                partHitbox = part.getBunkerHitbox();
                if (enemies.checkBulletCollision(partHitbox))
                {
                    part.partHit();
                }
            }
                //Checks to see if the enemy bullets have hit the player and if so, reduces number of lives
            playerHit = enemies.checkBulletCollision(player1.getPlayerHitbox());
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
            if (EnemyController.getNumberOfEnemies() == -1)
            {
                gameWon = true;
            }

        }
        
            //Code to run when the player either wins or loses a game
        private void checkIfGameOver() {
            if (gameOver)
            {
                ui.stopStopwatch();
                gamePaused = true;
                noMenu = true;
                gameStarted = false;
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "Game Over", new Vector2(295, 90), Color.White);
                _spriteBatch.DrawString(font, deathReason, new Vector2(295, 120), Color.White);
                _spriteBatch.End();
                ui.drawScore();
                if (wantsToSave == "")
                {
                    wantsToSave = ui.checkIfUserWantsToSave();
                }
                if (wantsToSave == "save")
                {

                    if (createNewUser == "") {
                        createNewUser = ui.checkIfNewUserToBeMade();
                    }
                        //Creates a new username based off of text inputted
                    if (createNewUser == "create")
                    {
                            //Inputs text and shows it on screen
                        (bool, List<string>) outputs = ui.enterText(currentKeyState, previousKeyState, word);
                        if (outputs.Item1)
                        {
                            word = outputs.Item2;
                            Debug.WriteLine(String.Join(" ", word));
                            _spriteBatch.Begin();
                            _spriteBatch.DrawString(font, "Enter Username: (press enter to submit)", new Vector2(300, 250), Color.White);
                            _spriteBatch.DrawString(font, String.Join(" ", word), new Vector2(300, 300), Color.White);
                            _spriteBatch.End();
                        }
                        else
                        {
                            database.addUser(String.Join("",word));
                            wantsToSave = "dont save";
                        }
                    }
                    if (createNewUser == "login") {
                            //Inputs text and shows it on screen
                        (bool, List<string>) outputs = ui.enterText(currentKeyState, previousKeyState, word);
                        if (outputs.Item1)
                        {
                            word = outputs.Item2;
                            Debug.WriteLine(String.Join(" ", word));
                            _spriteBatch.Begin();
                            _spriteBatch.DrawString(font, "Enter an existing username: (press enter to submit)", new Vector2(300, 250), Color.White);
                            _spriteBatch.DrawString(font, String.Join(" ", word), new Vector2(300, 300), Color.White);
                            _spriteBatch.End();
                        }
                        else
                        {
                            database.checkIfUserExists(String.Join("", word));
                            wantsToSave = "dont save";
                        }
                    }
                }
                if (wantsToSave == "dont save")
                {
                    //If the user has or hasn't saved the data in the database, let the user go back to the main menu
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(font, "Click to continue", new Vector2(295, 170), Color.Green);
                    _spriteBatch.End();

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        ui.resetStopwatch();
                        level = 0;
                        gameOver = false;
                        noMenu = false;
                    }
                }
            }
        }

        private void checkIfGameWon() {
            if (gameWon) {
                ui.stopStopwatch();
                gamePaused = true;
                noMenu = true;
                gameStarted = false;
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "You won! The level of difficulty has been increased by 1", new Vector2(), Color.Green);
                _spriteBatch.DrawString(font, "Click to continue", new Vector2(295, 170), Color.Green);
                _spriteBatch.End();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    ui.increaseLevel();
                    startNewGame();
                }
            }
        }
        
    }
}
