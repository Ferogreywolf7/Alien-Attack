﻿using Alien_Attack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        private bool gameStarted;
        private int extraXCoord;
        private int noOfLives;
        private int enemyRows;
        private int enemyCollums;
        private Rectangle bulletHitbox;
        private Rectangle partHitbox;
        private string tempGetNewKeybindOf;
        private string option;
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
            enemyRows = 2;
            enemyCollums = 5;
            //Set window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            controls = new Controls();
            getControls();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loading all textures for the game
            textBorder = Content.Load<Texture2D>("textBorder");
            playerBulletTexture = Content.Load<Texture2D>("bulletPlaceholder");
            player1Texture = Content.Load<Texture2D>("playerPlaceholder");
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
            player1StartPos = new Vector2(50, 800);
            player1 = new Player(player1Texture, player1StartPos, controls);
            bunkers = new Bunkers(bunkerAtlas, 2);
            enemies = new EnemyController(_spriteBatch, enemyTexture, enemyRows, enemyCollums, new Vector2(200, 50));
            enemies.spawnEnemies();
            noOfLives = 4;
            playerBulletActive = false;
            gameStarted = true;
            gamePaused = false;
        }

        protected override void Update(GameTime gameTime)
        {
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
                bunkers.updateBunkers();

                //Moving the players bullet and checkign any collision
                if (playerBulletActive)
                {
                    playerBullet.updateBullets();
                    checkplayerBulletCollision();
                }
                checkEnemyBulletCollision();
            }

            //All logic for changing keybinds while game is paused. In this class due to monogame restrictions
            if (gamePaused && inControlsMenu) {
                controls.setNewKeybind(currentKeyState);
                getControls();
            }
            base.Update(gameTime);
        }
        //Draws all textures in the game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //prevents the bullet from moving when the game is paused
            if (gamePaused == false)
            {
                ui.drawLives(noOfLives);
                player1.drawPlayer(_spriteBatch);
                enemies.drawAllEnemies();
                bunkers.drawBunkers(_spriteBatch);
                if (playerBulletActive)
                {
                    playerBullet.drawBullets(_spriteBatch);
                }
            }

            if (gamePaused && inControlsMenu)
            {
                tempGetNewKeybindOf = ui.drawControlsMenu();
                controls.getKeybindToSet(tempGetNewKeybindOf);

                if (tempGetNewKeybindOf == "Back") {
                    inControlsMenu = false;
                }

            }
            else if (gamePaused) {
                option = ui.drawPauseMenu();
                switch (option) {
                    case "Controls menu":
                        inControlsMenu = true;
                        break;
                    case "Start game":
                        if (gameStarted)
                        {
                            gamePaused = false;
                        }
                        else {
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
                    //Done so that user will always end up back in the pause menu even if unpausing from control menu
                    inControlsMenu = false;
                }
            }
        }
    }
}
