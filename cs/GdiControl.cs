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

        Bitmap native;

        bool gameplayAssetsLoaded;

        bool systemCursor;
        
        int playerAnimationFrame = 0;
        int playerXWalkFrame = 0;
        int playerYWalkFrame = 0;

        int textframe;
        string messageBoxString;

        bool isWalkingRight;
        bool isWalkingLeft;
        bool isWalkingUp;
        bool isWalkingDown;
        Mob mobHydraIsFighting;

        int scaleFactor = 3;
        int mouseX, mouseY;
        System.Timers.Timer gameTimer;

        Layer floor;
        private Hydra hydra;

        public GdiControl()
        {
            debugFont = new Font("Arial", 16);
            debugBrush = new SolidBrush(Color.White);
            this.DoubleBuffered = true;

            systemCursor = true;

            floor = GameState.theGame.CurrentLayer;
            floor.ConsoleWrite();

            hydra = GameState.theGame.hydra;

            textframe = 0;
            messageBoxString = "Monty must escape!";

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
                default:
                    return 0;
            }
        }
            
        void DrawMobs(Graphics g)
        {
            foreach (var mob in GameState.theGame.LayerMobs())
            {
                Rectangle destRect = new Rectangle(mob.X * (t + 1), mob.Y * (t + 1), t, t);

                Rectangle sourceRect = new Rectangle((mobFrame(mob.type)) * t, (1 + playerAnimationFrame / 20) * t, t, t);

                g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);

                // Draw a mask on the mob as needed
                if (mob is Hydra)
                {
                    if (hydra.CurrentlyWornMask != null)
                    {
                        int currentMaskFrame = maskWornFrame(hydra.CurrentlyWornMask.Value);
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
            foreach (var item in GameState.theGame.Items)
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
            if (hydra.CurrentlyWornMask == null)
                return;

            int uiX = 16;
            int uiY = 198;
            Rectangle destRect = new Rectangle(uiX, uiY, t, t);

            int currentMaskFrame = maskOnGroundItemFrame(hydra.CurrentlyWornMask.Value);
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

        private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
        {
            playerAnimationFrame = (playerAnimationFrame + 1) % 40;

            bool moved = false;

            if (isWalkingLeft)
            {
                playerXWalkFrame -= 2;
                if (playerXWalkFrame <= -16)
                {
                    isWalkingLeft = false;
                    playerXWalkFrame = 0;
                    hydra.X--;
                    moved = true;
                }
            }
            else if (isWalkingRight)
            {
                playerXWalkFrame += 2;
                if (playerXWalkFrame > 16)
                {
                    isWalkingRight = false;
                    playerXWalkFrame = 0;
                    hydra.X++;
                    moved = true;
                }
            }
            else if (isWalkingUp)
            {
                playerYWalkFrame -= 2;
                if (playerYWalkFrame <= -16)
                {
                    isWalkingUp = false;
                    playerYWalkFrame = 0;
                    hydra.Y--;
                    moved = true;
                }
            }
            else if (isWalkingDown)
            {
                playerYWalkFrame += 2;
                if (playerYWalkFrame > 16)
                {
                    isWalkingDown = false;
                    playerYWalkFrame = 0;
                    hydra.Y++;
                    moved = true;
                }
            }

            if (mobHydraIsFighting != null)
            {
                // Check if wearing the clown mask
                if (hydra.CurrentlyWornMask == EFeature.Clown)
                {
                    textframe = 0;
                    messageBoxString = "Did 1 damage using\nClown Mask.";
                }
                else
                {
                    // Lose the fight
                    hydra.HP--;
                    if (hydra.HP <= 0)
                    {
                        textframe = 0;
                        messageBoxString = "Monty ran out of\nstrength.";
                        // Game over here.
                    }
                    else
                    {
                        textframe = 0;
                        messageBoxString = "Monty took 1 damage.";
                    }
                }

                mobHydraIsFighting = null;
            }

            if (messageBoxString.Length > 0 && textframe < messageBoxString.Length)
            {
                textframe++;
            }

            if (moved)
            {
                // Check if we stepped on an item
                for (int i = 0; i < GameState.theGame.Items.Count; ++i)
                {
                    var item = GameState.theGame.Items[i];
                    if (hydra.X == item.X && hydra.Y == item.Y)
                    {
                        // Put on the mask
                        hydra.CurrentlyWornMask = item.mask.WhichEffect;

                        // Take the mask off the floor
                        GameState.theGame.Items.RemoveAt(i);

                        // Apply the effect of the item and report a message

                        if (hydra.CurrentlyWornMask == EFeature.Construction)
                        {
                            textframe = 0;
                            messageBoxString = "Monty put on\nConstruction Hat.";
                            floor.SpawnLadder();
                        }
                        else if (hydra.CurrentlyWornMask == EFeature.Clown)
                        {
                            textframe = 0;
                            messageBoxString = "Monty put on Clown\nMask.";
                        }

                        break;
                    }
                }
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
                g.DrawImageUnscaled(reference, 0, 0, 320, 240);

                DrawGameplay(g);

                // Draw the HUD
                g.DrawImageUnscaled(uiflavor, 0, 0);

                DrawCurrentlyWornMaskUI(g);

                // Draw the health bar
                g.FillRectangle(whiteBrush, 268, 193, hydra.HP, 4); // Fills 0..39

                // Draw the XP bar
                g.FillRectangle(whiteBrush, 268, 202, 20, 4); // Fills 0..39

                if (messageBoxString.Length > 0)
                {
                    DrawMessageBoxText(g);
                }

                // Draw the mouse pointer
                g.DrawImageUnscaled(pointer, mouseX, mouseY);
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

            bool systemCursorNext = false;

            if (e.X > 320 * scaleFactor)
            {
                systemCursorNext = true;
            }

            if (e.Y > 240 * scaleFactor)
            {
                systemCursorNext = true;
            }

            if (!systemCursorNext)
            {
                mouseX = e.X / scaleFactor;
                mouseY = e.Y / scaleFactor;
            }

            if (systemCursor != systemCursorNext)
            {
                systemCursor = systemCursorNext;

                if (systemCursor)
                {
                    Cursor.Show();
                }
                else
                {
                    Cursor.Hide();
                }
            }

            this.Invalidate();
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
            foreach (var mob in GameState.theGame.LayerMobs())
            {
                if (mob.X == forecastedPositionX && mob.Y == forecastedPositionY && !(mob is Hydra))
                {
                    mobToBeFightingWith = mob;
                    return false;
                }
            }

            return true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.D)
            {
                if (CanPass(hydra.X + 1, hydra.Y, out mobHydraIsFighting))
                {
                    isWalkingRight = true;
                }
            }
            else if (e.KeyCode == Keys.A)
            {
                if (CanPass(hydra.X - 1, hydra.Y, out mobHydraIsFighting))
                {
                    isWalkingLeft = true;
                }
            }
            else if (e.KeyCode == Keys.W)
            {
                if (CanPass(hydra.X, hydra.Y - 1, out mobHydraIsFighting))
                {
                    isWalkingUp = true;
                }
            }
            else if (e.KeyCode == Keys.S)
            {
                if (CanPass(hydra.X, hydra.Y + 1, out mobHydraIsFighting))
                {
                    isWalkingDown = true;
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }
    }
}




