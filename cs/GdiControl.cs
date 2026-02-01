using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;

//tremnonal
namespace mask
{
    // idea: class Tile.  draws itself and border, using bitmap image.  maybe applies mask filter, or maybe image does and we use it.
    public class GdiControl : Control
    {
        Font debugFont;
        Brush debugBrush;

        Bitmap reference;
        Bitmap pointer;
        Bitmap uiflavor;
        Bitmap glyphs;
        Bitmap tileset;
        Brush whiteBrush;
        Bitmap splashScreen;
        Bitmap gameOverScreen;
        Bitmap youWinScreen;

        Bitmap native;

        bool gameplayAssetsLoaded;
        
        int playerAnimationFrame = 0;
        int playerXWalkFrame = 0;
        int playerYWalkFrame = 0;

        int textframe;
        string messageBoxString;


        int scaleFactor = 3;
        int mouseX, mouseY;
        System.Timers.Timer gameTimer;

        Layer floor;
        List<MaskOnGround> removedMasksOnGround = new List<MaskOnGround>();
        List<Mob> killedMobs = new List<Mob>();
        MaskOnGround spawnedPartyHat;

        enum PrimaryState
        {
            SplashScreen,

            Gameplay,

            GameOverPending,
            GameOverCommitted,

            YouWinPending,
            YouWinCommitted
        }
        PrimaryState CurrentPrimaryState;
        GameState game;

        public GdiControl()
        {
            game = GameState.theGame;
            debugFont = new Font("Arial", 16);
            debugBrush = new SolidBrush(Color.White);
            this.DoubleBuffered = true;

            CurrentPrimaryState = PrimaryState.SplashScreen;

            floor = game.CurrentLayer;
            floor.ConsoleWrite();

            textframe = 0;
            messageBoxString = "Press enter to start.";
        }

        const int t = 16; // tile size

        void DrawGameplay(Graphics g)
        {

            for (int yTile = 0; yTile <= 15; ++yTile)
            {
                for (int xTile = 0; xTile <= 20; ++xTile)
                {
                    if (xTile >= floor.MapX)
                    {
                        continue; // Off map
                    }
                    if (yTile >= floor.MapY)
                    {
                        continue; // Off map
                    }

                    Tile thisTile = floor.tiles[xTile, yTile];
                    ETile tileType = thisTile.tileType;

                    int tileIndex = 0;
                    if (tileType == ETile.Snow)
                    {
                        tileIndex = 4;
                    }
                    else if (tileType == ETile.Rock)
                    {
                        tileIndex = 0;
                    }
                    else if (tileType == ETile.Dirt)
                    {
                        tileIndex = 2;
                    }
                    else if (tileType == ETile.Ladder)
                    {
                        tileIndex = 3;
                    }
                    else if (tileType == ETile.Bridge)
                    {
                        tileIndex = 5;
                    }
                    else if (tileType == ETile.None)
                        tileIndex = 1;

                        Rectangle destRect = new Rectangle(xTile * (t + 1), yTile * (t + 1), t, t);
                    Rectangle sourceRect = new Rectangle(tileIndex * t, 0, t, t);
                    g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);
                }
            }

            DrawMobs(g);
            DrawItems(g);
        }

        int mobFrame(EMob e)
        {
            switch (e)
            {
                case EMob.None:
                    return 0;
                case EMob.Hydra:
                    return 10;
                case EMob.Slime:
                    return 6;
                case EMob.Skeleton:
                    return 7;
                case EMob.Ghost:
                    return 8;
                default:
                    return 0;
            }
        }
        int maskOnGroundItemFrame(EFeature e)
        {
            switch (e)
            {
                case EFeature.Clown: return 7;
                case EFeature.Construction: return 8;
                case EFeature.Party: return 9;
                default:
                    return 0;
            }
        }

        int maskWornFrame(EFeature e)
        {
            switch (e)
            {
                case EFeature.Clown: return 10;
                case EFeature.Construction: return 11;
                case EFeature.Party: return 12;
                default:
                    return 0;
            }
        }
            
        void DrawMobs(Graphics g)
        {
            foreach (var mob in game.LayerMobs())
            {
                Rectangle destRect = new Rectangle(mob.X * (t + 1), mob.Y * (t + 1), t, t);

                Rectangle sourceRect = new Rectangle((mobFrame(mob.type)) * t, (1 + playerAnimationFrame / 20) * t, t, t);

                g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);

                // Draw a mask on the mob as needed
                if (mob is Hydra)
                {
                    if (game.hydra.CurrentlyWornMask != null)
                    {
                        int currentMaskFrame = maskWornFrame(game.hydra.CurrentlyWornMask.Value);
                        Rectangle sourceRect2 = sourceRect;
                        sourceRect2.X = currentMaskFrame * t;
                        sourceRect2.Y += t * 2;
                        g.DrawImage(tileset, destRect, sourceRect2, GraphicsUnit.Pixel);

                    }
                }
            }
        }
        void DrawItems(Graphics g)
        {
            foreach (var item in game.Items)
            {
                if (item is MaskOnGround)
                {
                    MaskOnGround m = item;
                    EFeature eff = m.mask.WhichEffect;
                    int currentMaskFrame = maskOnGroundItemFrame(eff);

                    Rectangle destRect = new Rectangle(item.X * (t + 1), item.Y * (t + 1), t, t);

                    Rectangle sourceRect = new Rectangle(currentMaskFrame * t, 0 * t, t, t);
                    g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);
                }
            }
        }

        void DrawCurrentlyWornMaskUI(Graphics g)
        {
            if (game.hydra.CurrentlyWornMask == null)
                return;

            int uiX = 16;
            int uiY = 198;
            Rectangle destRect = new Rectangle(uiX, uiY, t, t);

            int currentMaskFrame = maskOnGroundItemFrame(game.hydra.CurrentlyWornMask.Value);
            Rectangle sourceRect = new Rectangle(currentMaskFrame * t, 0, t, t);

            g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);

        }

        void DrawMessageBoxText(Graphics g)
        {
            int leftMargin = 52;
            int topMargin = 197;

            int dstX = leftMargin;
            int dstY = topMargin;
            int w = 8;
            int h = 16;

            for (int i=0; i<textframe; ++i)
            {
                Rectangle destRect = new Rectangle(dstX, dstY, w, h);

                char glyph = messageBoxString[i];

                int srcX = -1;

                if (glyph >= 'A' && glyph <= 'Z')
                {
                    srcX = (glyph - 65) * 8;
                }
                else if (glyph >= 'a' && glyph <= 'z')
                {
                    srcX = (glyph - 97 + 26) * 8;
                }
                else if (glyph == ',')
                {
                    srcX = 52 * 8;
                }
                else if (glyph == '.')
                {
                    srcX = 53 * 8;
                }
                else if (glyph == ':')
                {
                    srcX = 54 * 8;
                }
                else if (glyph == ';')
                {
                    srcX = 55 * 8;
                }
                else if (glyph == '\'')
                {
                    srcX = 56 * 8;
                }
                else if (glyph >= '0' && glyph <= '9')
                {
                    srcX = (glyph - 48 + 57) * 8;
                }
                else if (glyph == '!')
                {
                    srcX = 67 * 8;
                }
                else if (glyph == ' ')
                {
                    // Leave srcX as -1; nothing gets drawn
                }
                else if (glyph == '\n')
                {
                    // Leave srcX as -1; nothing gets drawn
                    dstX = leftMargin - w;
                    dstY += 20;

                }

                if (srcX > 0)
                {
                    Rectangle sourceRect = new Rectangle(srcX, 0, w, h);

                    g.DrawImage(glyphs, destRect, sourceRect, GraphicsUnit.Pixel);
                }

                dstX += w;
            }
        }

        private void LoadGameplayAssets()
        {
            string dir = Directory.GetCurrentDirectory();
            string imagePath = dir;

#if DEBUG
            imagePath = dir + "\\..\\..\\..\\..";
#endif
            imagePath += "\\images\\";

            reference = new Bitmap(imagePath + "Reference.png");
            reference.SetResolution(96, 96);

            splashScreen = new Bitmap(imagePath + "SplashScreen.png");
            splashScreen.SetResolution(96, 96);

            gameOverScreen = new Bitmap(imagePath + "GameOver.png");
            gameOverScreen.SetResolution(96, 96);

            youWinScreen = new Bitmap(imagePath + "YouWin.png");
            youWinScreen.SetResolution(96, 96);

            pointer = new Bitmap(imagePath + "Pointer.png");
            pointer.SetResolution(96, 96);

            uiflavor = new Bitmap(imagePath + "UI.png");
            uiflavor.SetResolution(96, 96);

            glyphs = new Bitmap(imagePath + "glyphs8x16.png");
            glyphs.SetResolution(96, 96);

            tileset = new Bitmap(imagePath + "Tileset.png");
            tileset.SetResolution(96, 96);

            native = new Bitmap(320, 240);
            native.SetResolution(96, 96);

            whiteBrush = new SolidBrush(Color.White);

            gameTimer = new System.Timers.Timer(17.0f);
            gameTimer.Elapsed += OnTick;
            gameTimer.Start();
        }
        static bool ticking = false;

        private void GameplayTick()
        {
            if (ticking) return;
            ticking = true;
            playerAnimationFrame = (playerAnimationFrame + 1) % 40;

            bool moved = false;

            if (game.isWalkingLeft)
            {
                playerXWalkFrame -= 2;
                if (playerXWalkFrame <= -16)
                {
                    game.isWalkingLeft = false;
                    playerXWalkFrame = 0;
                    game.hydra.X--;
                    moved = true;
                }
            }
            else if (game.isWalkingRight)
            {
                playerXWalkFrame += 2;
                if (playerXWalkFrame > 16)
                {
                    game.isWalkingRight = false;
                    playerXWalkFrame = 0;
                    game.hydra.X++;
                    moved = true;
                }
            }
            else if (game.isWalkingUp)
            {
                playerYWalkFrame -= 2;
                if (playerYWalkFrame <= -16)
                {
                    game.isWalkingUp = false;
                    playerYWalkFrame = 0;
                    game.hydra.Y--;
                    moved = true;
                }
            }
            else if (game.isWalkingDown)
            {
                playerYWalkFrame += 2;
                if (playerYWalkFrame > 16)
                {
                    game.isWalkingDown = false;
                    playerYWalkFrame = 0;
                    game.hydra.Y++;
                    moved = true;
                }
            }

            if (game.mobHydraIsFighting != null)
            {
                // Check if wearing the clown mask
                if (game.hydra.CurrentlyWornMask == EFeature.Clown)
                {
                    game.mobHydraIsFighting.Health--;

                    if (game.mobHydraIsFighting.Health == 0)
                    {
                        textframe = 0;
                        messageBoxString = "The slime was\nvanquished.";

                        killedMobs.Add(game.mobHydraIsFighting);
                        game.Mobs.Remove(game.mobHydraIsFighting);

                        game.mobHydraIsFighting = null;

                        // Check if all the slimes were vanquished
                        if (killedMobs.Count == 9)
                        {
                            // Spawn the party hat
                            spawnedPartyHat = new MaskOnGround(new Mask(ETile.Bridge, EFeature.Party), 1, 15, 7);
                            game.Items.Add(spawnedPartyHat);
                        }
                    }
                    else
                    {
                        textframe = 0;
                        messageBoxString = "Did 1 damage using\nClown Mask.";
                    }

                }
                else
                {
                    // Lose the fight
                    game.hydra.HP--;
                    if (game.hydra.HP <= 0)
                    {
                        textframe = 0;
                        messageBoxString = "Monty ran out of\nstrength.";
                        CurrentPrimaryState = PrimaryState.GameOverPending;
                    }
                    else
                    {
                        textframe = 0;
                        messageBoxString = "Monty took 1 damage.";
                    }
                }

                if (game.mobHydraIsFighting != null)
                {
                    game.enemyHealth = game.mobHydraIsFighting.Health;
                }
                else
                {
                    game.enemyHealth = 0;
                }

                game.mobHydraIsFighting = null;
            }

            TextBoxTick();

            if (moved)
            {
                // Check if we stepped on an item
                for (int i = 0; i < game.Items.Count; ++i)
                {
                    var item = game.Items[i];
                    if (game.hydra.X == item.X && game.hydra.Y == item.Y)
                    {
                        // Put on the mask
                        game.hydra.CurrentlyWornMask = item.mask.WhichEffect;

                        // Take the mask off the floor
                        removedMasksOnGround.Add(item);
                        game.Items.RemoveAt(i);

                        // Apply the effect of the item and report a message

                        if (game.hydra.CurrentlyWornMask == EFeature.Construction)
                        {
                            textframe = 0;
                            messageBoxString = "Monty put on\nConstruction Hat.";
                            floor.SpawnLadder();
                        }
                        else if (game.hydra.CurrentlyWornMask == EFeature.Clown)
                        {
                            textframe = 0;
                            messageBoxString = "Monty put on Clown\nMask.";
                        }
                        else if (game.hydra.CurrentlyWornMask == EFeature.Party)
                        {
                            textframe = 0;
                            messageBoxString = "Monty put on Party\nHat.";

                            floor.SpawnExit();
                        }

                        break;
                    }
                }

                // Check if we stepped on the bridge
                Tile thisTile = floor.tiles[game.hydra.X, game.hydra.Y];
                ETile tileType = thisTile.tileType;
                if (tileType == ETile.Bridge)
                {
                    textframe = 0;
                    messageBoxString = "Monty successfully\nescaped!";
                    CurrentPrimaryState = PrimaryState.YouWinPending;
                }
            }
            ticking = false;
        }

        private void TextBoxTick()
        {
            if (messageBoxString.Length > 0 && textframe < messageBoxString.Length)
            {
                textframe++;
            }
        }

        private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
        {

            if (CurrentPrimaryState == PrimaryState.SplashScreen)
            {
                TextBoxTick();
            }
            else if (CurrentPrimaryState == PrimaryState.GameOverPending)
            {
                TextBoxTick();
            }
            else if (CurrentPrimaryState == PrimaryState.GameOverCommitted)
            {
                TextBoxTick();
            }
            else if (CurrentPrimaryState == PrimaryState.YouWinPending)
            {
                TextBoxTick();
            }
            else if (CurrentPrimaryState == PrimaryState.YouWinCommitted)
            {
                TextBoxTick();
            }
            else if (CurrentPrimaryState == PrimaryState.Gameplay)
            {
                GameplayTick();
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawString("Placeholder because otherwise the Visual Studio Forms\n designer will take a lock on your bitmaps.", debugFont, debugBrush, 0, 0);
                return;
            }

            if (!gameplayAssetsLoaded)
            {
                LoadGameplayAssets();
                gameplayAssetsLoaded = true;
            }

            {
                Graphics g = Graphics.FromImage(native);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                if (CurrentPrimaryState == PrimaryState.SplashScreen)
                {
                    g.DrawImageUnscaled(splashScreen, 0, 0);

                    if (messageBoxString.Length > 0)
                    {
                        DrawMessageBoxText(g);
                    }
                }
                else if (CurrentPrimaryState == PrimaryState.GameOverPending)
                {
                    g.Clear(Color.Black);

                    // Draw the HUD
                    g.DrawImageUnscaled(uiflavor, 0, 0);

                    if (messageBoxString.Length > 0)
                    {
                        DrawMessageBoxText(g);
                    }
                }
                else if (CurrentPrimaryState == PrimaryState.GameOverCommitted)
                {
                    g.DrawImageUnscaled(gameOverScreen, 0, 0);

                    if (messageBoxString.Length > 0)
                    {
                        DrawMessageBoxText(g);
                    }
                }
                else if (CurrentPrimaryState == PrimaryState.YouWinPending)
                {
                    g.Clear(Color.Black);

                    // Draw the HUD
                    g.DrawImageUnscaled(uiflavor, 0, 0);

                    if (messageBoxString.Length > 0)
                    {
                        DrawMessageBoxText(g);
                    }
                }
                else if (CurrentPrimaryState == PrimaryState.YouWinCommitted)
                {
                    g.DrawImageUnscaled(youWinScreen, 0, 0);

                    if (messageBoxString.Length > 0)
                    {
                        DrawMessageBoxText(g);
                    }
                }
                else if (CurrentPrimaryState == PrimaryState.Gameplay)
                {
                    g.DrawImageUnscaled(reference, 0, 0, 320, 240);

                    DrawGameplay(g);

                    // Draw the HUD
                    g.DrawImageUnscaled(uiflavor, 0, 0);

                    DrawCurrentlyWornMaskUI(g);

                    // Draw the player health bar
                    {
                        int healthBarLength = game.hydra.HP;
                        if (healthBarLength > 39)
                        {
                            healthBarLength = 39;
                        }
                        g.FillRectangle(whiteBrush, 268, 193, healthBarLength, 4); // Fills 0..39
                    }

                    // Draw the enemy health bar
                    if (game.enemyHealth > 0)
                    {
                        int healthBarLength = game.enemyHealth;
                        if (healthBarLength > 39)
                        {
                            healthBarLength = 39;
                        }
                        g.FillRectangle(whiteBrush, 268, 202, healthBarLength, 4); // Fills 0..39
                    }

                    if (messageBoxString.Length > 0)
                    {
                        DrawMessageBoxText(g);
                    }
                }
            }

            // Draw native to final target with 3x scaling
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(Color.Black);
            e.Graphics.DrawImage(native, 0, 0, 320 * scaleFactor, 240 * scaleFactor);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        bool CanPass(int forecastedPositionX, int forecastedPositionY, out Mob mobToBeFightingWith)
        {
            mobToBeFightingWith = null;

            if (forecastedPositionX < 0) return false;
            if (forecastedPositionY < 0) return false;

            if (forecastedPositionX >= floor.MapX) return false;
            if (forecastedPositionY >= floor.MapY) return false;

            if (forecastedPositionX >= 18) return false; // TODO: the map should actually be smaller.
            if (forecastedPositionY >= 18) return false;

            Tile thisTile = floor.tiles[forecastedPositionX, forecastedPositionY];
            ETile tileType = thisTile.tileType;
            if (tileType == ETile.None)
                return false;

            // Check if there's a mob here
            foreach (var mob in game.LayerMobs())
            {
                if (mob.X == forecastedPositionX && mob.Y == forecastedPositionY && !(mob is Hydra))
                {
                    mobToBeFightingWith = mob;
                    return false;
                }
            }

            return true;
        }

        private void InitializeGameplay()
        {
            // Transition to gameplay
            CurrentPrimaryState = PrimaryState.Gameplay;

            textframe = 0;
            messageBoxString = "Monty must escape!";
            game.hydra.HP = 10;
            game.hydra.CurrentlyWornMask = null;
        }

        void ResetGame()
        {
            game.Reset();
            InitializeGameplay();
        }

        public enum Op
        { 
            Left,
            Right,
            Up,
            Down,
            Enter
        }
        public record KeyOp(Keys k, Op o);
        KeyOp[] KeyOps = [
            new KeyOp(Keys.Enter, Op.Enter),
            new KeyOp(Keys.A, Op.Left),
            new KeyOp(Keys.S, Op.Down),
            new KeyOp(Keys.D, Op.Right),
            new KeyOp(Keys.W, Op.Up),
            new KeyOp(Keys.Oemcomma, Op.Up),
            new KeyOp(Keys.O, Op.Down),
            new KeyOp(Keys.E, Op.Right),
            ];
        public Op? FindOp(Keys k) 
        {
            foreach (var ko in KeyOps) { if (ko.k == k) return ko.o; }
                return null; }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Op? op = FindOp(e.KeyCode);

            bool pressedEligibleKey = op != null;


            if (CurrentPrimaryState == PrimaryState.SplashScreen)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    InitializeGameplay();
                }
            }
            else if (CurrentPrimaryState == PrimaryState.GameOverPending)
            {
                if (pressedEligibleKey && textframe == messageBoxString.Length)
                {
                    CurrentPrimaryState = PrimaryState.GameOverCommitted;
                    textframe = 0;
                    messageBoxString = "Press enter to try again.";
                }
            }
            else if (CurrentPrimaryState == PrimaryState.GameOverCommitted)
            {
                if (e.KeyCode == Keys.Enter && textframe == messageBoxString.Length)
                {
                    ResetGame();
                }
            }
            else if (CurrentPrimaryState == PrimaryState.YouWinPending)
            {
                if (pressedEligibleKey && textframe == messageBoxString.Length)
                {
                    CurrentPrimaryState = PrimaryState.YouWinCommitted;
                    textframe = 0;
                    messageBoxString = "Press enter to play again.";
                }
            }
            else if (CurrentPrimaryState == PrimaryState.YouWinCommitted)
            {
                if (e.KeyCode == Keys.Enter && textframe == messageBoxString.Length)
                {
                    ResetGame();
                }
            }
            else if (CurrentPrimaryState == PrimaryState.Gameplay)
            {
                if (op == Op.Right)
                {
                    if (CanPass(game.hydra.X + 1, game.hydra.Y, out game.mobHydraIsFighting))
                    {
                        game.isWalkingRight = true;
                    }
                }
                else if (op == Op.Left)
                {
                    if (CanPass(game.hydra.X - 1, game.hydra.Y, out game.mobHydraIsFighting))
                    {
                        game.isWalkingLeft = true;
                    }
                }
                else if (op == Op.Up)
                {
                    if (CanPass(game.hydra.X, game.hydra.Y - 1, out game.mobHydraIsFighting))
                    {
                        game.isWalkingUp = true;
                    }
                }
                else if (op == Op.Down)
                {
                    if (CanPass(game.hydra.X, game.hydra.Y + 1, out game.mobHydraIsFighting))
                    {
                        game.isWalkingDown = true;
                    }
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }
    }
}




