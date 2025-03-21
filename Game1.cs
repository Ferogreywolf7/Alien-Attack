﻿using Alien_Attack;
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
        private Texture2D playerExplosionSpriteSheet;
        private Texture2D background;
        private Texture2D reloadBar;
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
        private int noOfLives;
        private int enemyRows;
        private int enemyCollums;
        private int level;
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
        private List<string> username;
        private bool gameWon;
        private int userID;
        private bool databaseOnline;
        private double timeCooldownStarted;
        private double invulnerableCooldown;
        private bool cooldownEnded;
        private Rectangle explosionFrameRectangle;
        private Rectangle explosionDestinationRectangle;
        private int topLeftOfFrame;
        private int counter;

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
            databaseOnline = database.tryConnectToDatabase();
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
            reloadBar = Content.Load<Texture2D>("reloadBar");
            font = Content.Load<SpriteFont>("testFont");

            playerSpritSheet = Content.Load<Texture2D>("playerAnimationSpriteSheet");
            explosionSpriteSheet = Content.Load<Texture2D>("explosionAnimationSpriteSheet");
            playerExplosionSpriteSheet = Content.Load<Texture2D>("playerExplosion");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Instansiating the UI
            ui = new UI(_spriteBatch, font, textBorder, lifeIcon, backArrow, controls);
        }

        public void startNewGame() {
            gameMode = "Classic";
            deathReason = "";
            player1StartPos = new Vector2(50, 800);
            enemyStartPos = new Vector2(11, 50);
            player1 = new Player(playerSpritSheet, playerBulletTexture, reloadBar, textBorder, player1StartPos, controls, ui);
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
            username = new List<string>();
            topLeftOfFrame = 0;
            invulnerableCooldown = 1.5;
            timeCooldownStarted = 00.00;
            counter = 0;
            cooldownEnded = true;
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
                if (!cooldownEnded) {
                    drawPlayerExplosionAnimation();
                }
            }
                //Shows that the game is paused
            if (gamePaused && !noMenu) {
                ui.drawText("Game paused", new Vector2(320, 100), Color.Red);
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
            else if (gamePaused && inCustomiseMenu) {
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

        private void getCooldownStartTime() {
            timeCooldownStarted = Convert.ToDouble(ui.getStopwatchTime().Replace(":", ""));
            Debug.WriteLine("Cooldown started at :" + timeCooldownStarted);
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
                //Player can only be hit again when the cooldown has ended
            cooldownEnded = ui.checkCooldown(timeCooldownStarted, invulnerableCooldown);
            if (playerHit && cooldownEnded) {
                noOfLives -= 1;
                Debug.WriteLine("Player hit");
                explosionDestinationRectangle = new Rectangle((int) player1.getPosition().X, (int) player1.getPosition().Y-30, 75, 75);
                getCooldownStartTime();
                cooldownEnded = false;
                topLeftOfFrame = 0;
            }

        }

        private void drawPlayerExplosionAnimation() {
            explosionFrameRectangle = new Rectangle(topLeftOfFrame, 0, 175, 175);
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerExplosionSpriteSheet, explosionDestinationRectangle ,explosionFrameRectangle ,Color.White);
            _spriteBatch.End();
            counter++;
            if (counter == 5) {
                topLeftOfFrame += 175;
                counter = 0;
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
        private void checkIfGameOver()
        {
            if (gameOver)
            {
                ui.stopStopwatch();
                gamePaused = true;
                noMenu = true;
                gameStarted = false;
                ui.drawText("Game Over", new Vector2(295, 90));
                ui.drawText(deathReason, new Vector2(295, 120));
                ui.drawScore();
                //databaseOnline = database.tryConnectToDatabase();     Opening a connection to the database takes a while and nothing can be done while checking if the connection is open due to it having to not be an async task

                

                if (wantsToSave == "")
                {
                    wantsToSave = ui.checkIfUserWantsToSave();
                }
                if (!databaseOnline)
                {
                    ui.drawText("Can't connect to database", new Vector2(295, 200));
                    wantsToSave = "dont save";
                }
                else
                {
                    if (wantsToSave == "save")
                    {
                        databaseAdding();
                    }
                    wantsToSave = "dont save";
                }

                if (wantsToSave == "dont save")
                {
                    ui.loadingScreen(bunkerAtlas);
                    //If the user has or hasn't saved the data in the database, let the user go back to the main menu
                    ui.drawText("Click to continue", new Vector2(295, 170));

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

        private void databaseAdding(){
             if (createNewUser == "") {
                createNewUser = ui.checkIfNewUserToBeMade();
             }
                //Creates a new username based off of text inputted
            if (createNewUser == "create")
            {
                    //Inputs text and shows it on screen
                (bool, List<string>) outputs = ui.enterText(currentKeyState, previousKeyState, username);
                    //if continuing text input
                if (outputs.Item1)
                {
                        //Writes the text that the user is entering onto the screen
                    username = outputs.Item2;
                    Debug.WriteLine(String.Join(" ", username));
                    ui.drawText("Enter Username: (press enter to submit)", new Vector2(300, 250));
                    ui.drawText(String.Join(" ", username), new Vector2(300, 300));
                }
                    //User has finished entering text
                else
                {
                    database.addUser(String.Join("", username));
                    wantsToSave = "dont save";
                }
            }
            if (createNewUser == "login")
            {
                //Inputs text and shows it on screen
                (bool, List<string>) outputs = ui.enterText(currentKeyState, previousKeyState, username);
                if (outputs.Item1)
                {
                    username = outputs.Item2;
                    Debug.WriteLine(String.Join(" ", username));
                    ui.drawText("Enter an existing username: (press enter to submit)", new Vector2(300, 250));
                    ui.drawText(String.Join(" ", username), new Vector2(300, 300));
                }
                else
                {
                        //Checks if the user exists in the database
                    if (database.checkIfUserExists(String.Join("", username)))
                    {
                        //User exists
                        userID = database.getUserID(string.Join("", username));
                        database.insertGameValues(userID, ui.getScore(), gameMode, ui.getStopwatchTime(), UI.getLevel());
                    }
                    else {
                        //User doesnt exist
                        createNewUser = "";
                    }

                    wantsToSave = "dont save"; //Goes back to click to continue screen
                }
            }
        }
            //Code to go to a new level
        private void checkIfGameWon() {
            if (gameWon) {
                ui.stopStopwatch();
                gamePaused = true;
                noMenu = true;
                gameStarted = false;
                ui.drawText("You won! The level of difficulty has been increased by 1 to level " + UI.getLevel()+1, new Vector2(250, 150));
                ui.drawText("Click to continue", new Vector2(295, 170), Color.Green);
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    ui.increaseLevel();
                    startNewGame();
                }
            }
        }
        
    }
}
