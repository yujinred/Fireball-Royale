using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FireBall_Royale
{

    public class FireBallGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Tiles and Fog Variables

        TileMap myMap = new TileMap();
        int squaresAcross = 18;
        int squaresDown = 11;

        Texture2D fogOfWar;
        FogOfWar mFogOfWar;

        #endregion

        //Song played during game
        Song battleSong;
        //Can the song be played or not
        bool bsPlay;

        //Sound effect played when a fire ball is fired
        SoundEffect fire;
        //Can player one play this sound
        bool fireOnePlay;
        //Can player two play this sound
        bool fireTwoPlay;

        //SoundEffect played When game is won
        SoundEffect cheer;

        //Texture of title poster
        Texture2D titleTexture;
        //Rectangle of title poster
        Rectangle titleRect;

        //Different varies instance of text being drawn
        DrawText text;
        DrawText credits;
        DrawText winnerText;
        DrawText helpText;

        #region Game Management

        //Different States of the game
        enum GameState
        {
            titleScreen,
            playingGame,
            endScreen,
            helpScreen
        }
        //Default state at the start of the game
        GameState state = GameState.titleScreen;
        GameState previousState;

        //Game start
        public void gameStart()
        {
            state = GameState.playingGame;
        }

        //Game over, variables reset
        public void gameOver()
        {
            myPrincess.Position = new Vector2(15, 15);
            myPrincessTwo.Position = new Vector2(600, 300);
            playerOneHealth.Health = 0;
            playerTwoHealth.Health = 0;
            state = GameState.endScreen;
            cheer.Play();
        }

        //Game return to title screen
        public void restartGame()
        {
            state = GameState.titleScreen;
        }

        public void help()
        {
            previousState = state;
            state = GameState.helpScreen;
        }

        public void exitHelp()
        {
            state = previousState;
        }

        #endregion

        #region Player, Fireball, and Health Variables

        //Properies of player 1
        Texture2D princess;
        MobileSprite myPrincess;
        int princessSpeed = 2;

        //properies of player 2
        Texture2D princessTwo;
        MobileSprite myPrincessTwo;
        int princessTwoSpeed = 3;       //player 2 is faster than player 1 to balance out the fog of war

        //texture of fire ball
        Texture2D fireBall;
        //instances of fire ball, each for a player to use
        MobileSprite myFireBall;
        MobileSprite myFireBallTwo;

        //Health bars
        Texture2D mBar;
        HealthBar playerOneHealth;
        HealthBar playerTwoHealth;
        //The max size of the health bar
        int healthSize;

        //Check if player one won or not
        bool firstPlayerWin;

        #endregion

        #region Display dimension values

        int displayWidth;   //screen width
        int displayHeight;  //screen height
        float minDisplayX;  //min screen distance x
        float maxDisplayX;  //max screen distance x
        float minDisplayY;  //min screen distance y
        float maxDisplayY;  //max screen distance y

        //set up screen size variables
        private void setScreenSizes()
        {
            displayWidth = graphics.GraphicsDevice.Viewport.Width;
            displayHeight = graphics.GraphicsDevice.Viewport.Height;

            minDisplayX = 0;
            minDisplayY = 0;

            maxDisplayX = displayWidth;
            maxDisplayY = displayHeight;
        }

        #endregion

        public FireBallGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Make song repeat
            MediaPlayer.IsRepeating = true;
            //Song will play
            bsPlay = true;  
            //Set up the size of the screen
            setScreenSizes();
            //Set up the size of the title poster
            titleRect = new Rectangle((int)minDisplayX, (int)minDisplayY, (int)maxDisplayX, (int)maxDisplayY);
            //Create new text objects
            text = new DrawText();
            credits = new DrawText();
            winnerText = new DrawText();
            helpText = new DrawText();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load tiles and fog
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part6_tileset");
            fogOfWar = Content.Load<Texture2D>(@"Textures\FogOfWar");

            //Load title poster
            titleTexture = Content.Load<Texture2D>(@"Textures\Title1");
            //Load font
            text.LoadContent(Content, @"Font\Font");
            credits.LoadContent(Content, @"Font\Font2");
            winnerText.LoadContent(Content, @"Font\Font");
            helpText.LoadContent(Content, @"Font\Font3");

            //Long the song and soundeffect
            battleSong = Content.Load<Song>(@"Sound/Music/BattleTheme");
            fire = Content.Load<SoundEffect>(@"Sound/SoundEffect/Boom");
            cheer = Content.Load<SoundEffect>(@"Sound/SoundEffect/Cheer");

            #region Setup Player One

            princess = Content.Load<Texture2D>(@"Textures\Player");
            myPrincess = new MobileSprite(princess);
            //Set up different varies types of animations
            myPrincess.Sprite.AddAnimation("upstop", 0, 198, 31, 66, 1, 0.1f);
            myPrincess.Sprite.AddAnimation("up", 0, 198, 31, 66, 4, 0.1f);
            myPrincess.Sprite.AddAnimation("leftstop", 0, 66, 31, 66, 1, 0.1f);
            myPrincess.Sprite.AddAnimation("left", 0, 66, 31, 66, 4, 0.1f);
            myPrincess.Sprite.AddAnimation("rightstop", 0, 132, 31, 66, 1, 0.1f);
            myPrincess.Sprite.AddAnimation("right", 0, 132, 31, 66, 4, 0.1f);
            myPrincess.Sprite.AddAnimation("downstop", 0, 0, 31, 66, 1, 0.1f);
            myPrincess.Sprite.AddAnimation("down", 0, 0, 31, 66, 4, 0.1f);
            //Starting animation
            myPrincess.Sprite.CurrentAnimation = "rightstop";
            //Starting position
            myPrincess.Position = new Vector2(15, 15);

            #endregion

            #region Setup Player Two

            princessTwo = Content.Load<Texture2D>(@"Textures\Player2");
            myPrincessTwo = new MobileSprite(princessTwo);
            //Set up different varies types of animation
            myPrincessTwo.Sprite.AddAnimation("upstop", 0, 198, 31, 66, 1, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("up", 0, 198, 31, 66, 4, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("leftstop", 0, 66, 31, 66, 1, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("left", 0, 66, 31, 66, 4, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("rightstop", 0, 132, 31, 66, 1, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("right", 0, 132, 31, 66, 4, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("downstop", 0, 0, 31, 66, 1, 0.1f);
            myPrincessTwo.Sprite.AddAnimation("down", 0, 0, 31, 66, 4, 0.1f);
            //starting animation
            myPrincessTwo.Sprite.CurrentAnimation = "rightstop";
            //Starting position
            myPrincessTwo.Position = new Vector2(600, 300);

            #endregion

            #region Setup FireBall

            fireBall = Content.Load<Texture2D>(@"Textures/FireBall");
            myFireBall = new MobileSprite(fireBall);
            myFireBall.Sprite.AddAnimation("fireleft", 0, 32, 32, 32, 3, 0.1f);
            myFireBall.Sprite.AddAnimation("fireright", 0, 64, 32, 32, 3, 0.1f);


            myFireBallTwo = new MobileSprite(fireBall);
            myFireBallTwo.Sprite.AddAnimation("fireleft", 0, 32, 32, 32, 3, 0.1f);
            myFireBallTwo.Sprite.AddAnimation("fireright", 0, 64, 32, 32, 3, 0.1f);

            #endregion

            #region Setup Health Bar

            mBar = Content.Load<Texture2D>(@"Textures/HealthBar");
            //The max health size equals the width of the health bar
            healthSize = mBar.Width;
            playerOneHealth = new HealthBar(mBar, (int)minDisplayX, (int)(maxDisplayY - mBar.Height), mBar.Width, mBar.Height, Color.Red);
            playerTwoHealth = new HealthBar(mBar, (int)(maxDisplayX - mBar.Width), (int)(maxDisplayY - mBar.Height), mBar.Width, mBar.Height, Color.Blue);

            #endregion

            //Set up Fog of War
            mFogOfWar = new FogOfWar(fogOfWar, 50, 50, 48, 48, 2, 5, 255, 25);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //Create a keyboard state
            KeyboardState ks = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                ks.IsKeyDown(Keys.Escape) == true)
                this.Exit();

            #region Keyboard Setup
            bool upkey = ks.IsKeyDown(Keys.W);
            bool downkey = ks.IsKeyDown(Keys.S);
            bool leftkey = ks.IsKeyDown(Keys.A);
            bool rightkey = ks.IsKeyDown(Keys.D);
            bool fkey = ks.IsKeyDown(Keys.F);
            bool qkey = ks.IsKeyDown(Keys.Q);
            bool ekey = ks.IsKeyDown(Keys.E);

            bool upkeyTwo = ks.IsKeyDown(Keys.Up);
            bool downkeyTwo = ks.IsKeyDown(Keys.Down);
            bool leftkeyTwo = ks.IsKeyDown(Keys.Left);
            bool rightkeyTwo = ks.IsKeyDown(Keys.Right);
            bool rightshiftkey = ks.IsKeyDown(Keys.RightShift);
            bool commakey = ks.IsKeyDown(Keys.OemComma);
            bool periodkey = ks.IsKeyDown(Keys.OemPeriod);

            bool enterkey = ks.IsKeyDown(Keys.Enter);
            bool spacekey = ks.IsKeyDown(Keys.Space);
            bool f1key = ks.IsKeyDown(Keys.F1);
            #endregion

            //Play Sound when it is allowed
            if (bsPlay == true)
            {
                MediaPlayer.Play(battleSong);
                //Stop .play() from being played immediately again
                bsPlay = false;
            }

            //Check what state and update what is necessary for that state
            switch (state)
            {
                case GameState.titleScreen:
                    //press enter to start game
                    if (enterkey)
                        gameStart();
                    if (f1key)
                        help();
                    break;
                case GameState.playingGame:
                    //keyboard control
                    ChangeDirection(myPrincess, myPrincessTwo, myFireBall, playerTwoHealth, qkey, ekey, fkey, ref fireOnePlay);
                    ChangeDirection(myPrincessTwo, myPrincess, myFireBallTwo, playerOneHealth, commakey, periodkey, rightshiftkey, ref fireTwoPlay);
                    KeyboardsControls(myPrincess, myFireBall, upkey, downkey, leftkey, rightkey, princessSpeed);
                    KeyboardsControls(myPrincessTwo, myFireBallTwo, upkeyTwo, downkeyTwo, leftkeyTwo, rightkeyTwo, princessTwoSpeed); myPrincess.Update(gameTime);                    
                    //check if player two win
                    if (healthSize == Math.Abs(playerOneHealth.Health))
                    {
                        firstPlayerWin = false;
                        gameOver();
                    }
                    //check if player one win
                    if (healthSize == Math.Abs(playerTwoHealth.Health))
                    {
                        firstPlayerWin = true;
                        gameOver();
                    }
                    if (f1key)
                        help();
                    break;
                case GameState.endScreen:
                    //Press space key to go back to title screen
                    if (spacekey)
                        restartGame();
                    break;
                case GameState.helpScreen:
                    if (spacekey)
                        exitHelp();
                    break;
            }

            //Update objects
            myPrincessTwo.Update(gameTime);
            myFireBall.Update(gameTime);
            myFireBallTwo.Update(gameTime);
            mFogOfWar.Update(myPrincess.Position, 0, 0, 0, 0, true);

            base.Update(gameTime);
        }

        private void ChangeDirection(MobileSprite player, MobileSprite enemy, MobileSprite fireBall, HealthBar enemyHealth,
            bool qkey, bool ekey, bool fkey, ref bool firePlay)
        {
            //Ability to hit enemy
            bool hitAbility; 

            if (qkey)
            {
                //If qkey is pressed, fireball will face left and be left of player
                fireBall.Sprite.CurrentAnimation = "fireleft";
                fireBall.Position = new Vector2((int)player.Position.X - 20, (int)player.Position.Y);
            }
            if (ekey)
            {
                //If ekey is pressed, fireball will face right and be right of player
                fireBall.Sprite.CurrentAnimation = "fireright";
                fireBall.Position = new Vector2((int)player.Position.X + 20, (int)player.Position.Y);
            }

            if (fkey)
            {
                if (fireBall.Sprite.CurrentAnimation == "fireright")
                {
                    //Move the fireball right when fireball is facing right
                    fireBall.Sprite.MoveBy(2, 0);
                }
                else if (fireBall.Sprite.CurrentAnimation == "fireleft")
                {
                    //Move the fireball left when fireball is facing left
                    fireBall.Sprite.MoveBy(-2, 0);
                }
                //Play sound when fireball is fired
                if (firePlay == true)
                {
                    fire.Play();
                    //Allow sound effect to be only played once when fkey is press
                    firePlay = false;
                }
                //Allow fireball to hit enemy when fkey is pressed
                hitAbility = true;
            }
            else
            {
                //Return fireball to default position when fkey is released
                if (fireBall.Sprite.CurrentAnimation == "fireright")
                {
                    fireBall.Position = new Vector2((int)player.Position.X + 20, (int)player.Position.Y);
                }
                if (fireBall.Sprite.CurrentAnimation == "fireleft")
                {
                    fireBall.Position = new Vector2((int)player.Position.X - 20, (int)player.Position.Y);
                }
                //Allow soundeffect to be played again
                firePlay = true;
                //Disallow fireball to hit enemy
                hitAbility = false;
            }

            //Don't allow health to decrease beyond 0
            if (healthSize > Math.Abs(enemyHealth.Health))
            {
                //check if fireball intersect enemy and if hitability is true
                if (fireBall.BoundingBox.Intersects(enemy.BoundingBox) && hitAbility == true)
                {
                    //enemy health decreased
                    enemyHealth.Health--;
                }
            }
        }

        private void KeyboardsControls(MobileSprite player, MobileSprite fireBall, bool upkey, bool downkey, bool leftkey, bool rightkey, int speed)
        {
            if (upkey)
            {
                //Player face up and go up when upkey is pressed
                if (player.Sprite.CurrentAnimation != "up")
                {
                    player.Sprite.CurrentAnimation = "up";
                }
                //Don't allow player to exit screen
                if (player.Position.Y <= minDisplayY)
                {
                    player.Sprite.MoveBy(0, 0);
                }
                else
                {
                    player.Sprite.MoveBy(0, -1 * speed);
                }
            }

            if (leftkey)
            {
                //Player face left and go left when left key is pressed
                if (player.Sprite.CurrentAnimation != "left")
                {
                    player.Sprite.CurrentAnimation = "left";
                }
                //Don't allow player to exit screen
                if (player.Position.X <= minDisplayX)
                {
                    player.Sprite.MoveBy(0, 0);
                }
                else
                {
                    player.Sprite.MoveBy(-1 * speed, 0);
                }
            }

            if (rightkey)
            {
                //Player face right and go right when right key is pressed
                if (player.Sprite.CurrentAnimation != "right")
                {
                    player.Sprite.CurrentAnimation = "right";
                }
                //Don't allow player to exit screen
                if (player.Position.X + player.Sprite.Width >= maxDisplayX)
                {
                    player.Sprite.MoveBy(0, 0);
                }
                else
                {
                    player.Sprite.MoveBy(1 * speed, 0);
                }
            }

            if (downkey)
            {
                //Player face down and go down when down key is pressed
                if (player.Sprite.CurrentAnimation != "down")
                {
                    player.Sprite.CurrentAnimation = "down";
                }
                //Don't allow player to exit screen
                if (player.Position.Y + player.Sprite.Height >= maxDisplayY)
                {
                    player.Sprite.MoveBy(0, 0);
                }
                else
                {
                    player.Sprite.MoveBy(0, 1 * speed);
                }
            }

            //When nothing is pressed, check state the charater is last and set appropriate stopping animation
            if (!leftkey && !rightkey && !upkey && !downkey)
            {
                if (player.Sprite.CurrentAnimation == "up")
                {
                    player.Sprite.CurrentAnimation = "upstop";
                }
                if (player.Sprite.CurrentAnimation == "left")
                {
                    player.Sprite.CurrentAnimation = "leftstop";
                }
                if (player.Sprite.CurrentAnimation == "right")
                {
                    player.Sprite.CurrentAnimation = "rightstop";
                }
                if (player.Sprite.CurrentAnimation == "down")
                {
                    player.Sprite.CurrentAnimation = "downstop";
                }
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            //Clear screen of clutter
            GraphicsDevice.Clear(Color.DarkRed);

            //Start spritbatch with some helper for fog of war
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            switch (state)
            {
                case GameState.titleScreen:
                    //Draw title screen
                    spriteBatch.Draw(titleTexture, titleRect, Color.White);
                    text.DrawString(spriteBatch, "FIREBALL \n   ROYALE", 100, 100, Color.White);
                    credits.DrawString(spriteBatch, "PRESS ENTER TO START", 50, 430, Color.White);
                    credits.DrawString(spriteBatch, "CREATED BY EUGENE Z", 400, 430, Color.White); 
                    break;
                case GameState.playingGame:
                    #region Draw Tiles

                    Vector2 firstSquare = new Vector2((int)minDisplayX / Tile.TileWidth, (int)minDisplayY / Tile.TileHeight);
                    int firstX = (int)firstSquare.X;
                    int firstY = (int)firstSquare.Y;

                    Vector2 squareOffset = new Vector2((int)minDisplayX % Tile.TileWidth, (int)minDisplayY % Tile.TileHeight);
                    int offsetX = (int)squareOffset.X;
                    int offsetY = (int)squareOffset.Y;

                    for (int y = 0; y < squaresDown; y++)
                    {
                        for (int x = 0; x < squaresAcross; x++)
                        {
                            foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].BaseTiles)
                            {
                                spriteBatch.Draw(
                                    Tile.TileSetTexture,
                                    new Rectangle(
                                        (x * Tile.TileWidth) - offsetX, (y * Tile.TileHeight) - offsetY,
                                        Tile.TileWidth, Tile.TileHeight),
                                    Tile.GetSourceRectangle(tileID),
                                    Color.White);
                            }
                        }
                    }

                    #endregion
                    //Draw every sprite for needed for gameplay
                    myFireBall.Draw(spriteBatch);
                    myFireBallTwo.Draw(spriteBatch);
                    myPrincess.Draw(spriteBatch);
                    myPrincessTwo.Draw(spriteBatch);
                    mFogOfWar.DrawFog(spriteBatch, 0, 0, 0, 0, true);
                    playerOneHealth.Draw(spriteBatch);
                    playerTwoHealth.Draw(spriteBatch);
                    break;
                case GameState.endScreen:
                    //check which player won, and set appropriate end screen
                    if (firstPlayerWin)
                    {
                        winnerText.DrawString(spriteBatch, "PLAYER ONE WINS!!!", 0, 0, Color.Red);
                    }
                    else
                    {
                        winnerText.DrawString(spriteBatch, "PLAYER TWO WINS!!!", 0, 0, Color.Blue);
                    }
                    winnerText.DrawString(spriteBatch, "PRESS SPACE \nTO PLAY AGAIN", (int)maxDisplayX / 6, (int)maxDisplayY / 2, Color.White);
                    break;
                case GameState.helpScreen:
                    helpText.DrawString(spriteBatch, @"HELP MENU

Two female warriors compete against one another to protect
their family honour.
Claris is the orange female paladin, and controlled by
player one. Claris lights her surroundings with the power
of light.
Katja is the purple female ninja, and controlled by
player two. Katja has unparallel speed and the ability to
hide in the Darkness.
Player one use the WASD key to control movement, with QE 
key to change the direction of the fireball. F to fire.
Player two use the arrow key to control movement, with 
comma and period key to change the direction of the 
fireball. Right shift key to fire.

Press space to exit help screen.",
                    10, 10, Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
