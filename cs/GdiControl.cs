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

        int playerWorldTileX;
        int playerWorldTileY;
        
        int playerAnimationFrame = 0;
        int playerXWalkFrame = 0;
        int playerYWalkFrame = 0;

        int textframe;
        string messageBoxString;

        bool isWalkingRight;
        bool isWalkingLeft;
        bool isWalkingUp;
        bool isWalkingDown;

        int scaleFactor = 3;
        int mouseX, mouseY;
        System.Timers.Timer gameTimer;

        ExampleLayer floor;

        public GdiControl()
        {
            debugFont = new Font("Arial", 16);
            debugBrush = new SolidBrush(Color.White);
            this.DoubleBuffered = true;

            systemCursor = true;

            playerWorldTileX = 1;
            playerWorldTileY = 1;

            textframe = 0;
            messageBoxString = "This is a test.";

            floor = new ExampleLayer();
        }

        void DrawGameplay(Graphics g)
        {
            int t = 16;

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


                    Rectangle destRect = new Rectangle(xTile * t, yTile * t, t, t);
                    Rectangle sourceRect = new Rectangle(tileIndex * t, 0, t, t);
                    g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);
                }
            }

            // Draw the player character
            {
                int xTile = playerWorldTileX;
                int yTile = playerWorldTileY;
                Rectangle destRect = new Rectangle(xTile * t + playerXWalkFrame, yTile * t + playerYWalkFrame, t, t);

                Rectangle sourceRect = new Rectangle((4 + (playerAnimationFrame / 20)) * t, 1 * t, t, t);
                g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);

            }
        }

        void DrawMessageBoxText(Graphics g)
        {
            int dstX = 52;
            int w = 8;
            int h = 16;

            for (int i=0; i<textframe; ++i)
            {
                Rectangle destRect = new Rectangle(dstX, 197, w, h);

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

            if (isWalkingLeft)
            {
                playerXWalkFrame -= 2;
                if (playerXWalkFrame <= -16)
                {
                    isWalkingLeft = false;
                    playerXWalkFrame = 0;
                    playerWorldTileX--;
                }
            }
            else if (isWalkingRight)
            {
                playerXWalkFrame += 2;
                if (playerXWalkFrame > 16)
                {
                    isWalkingRight = false;
                    playerXWalkFrame = 0;
                    playerWorldTileX++;
                }
            }
            else if (isWalkingUp)
            {
                playerYWalkFrame -= 2;
                if (playerYWalkFrame <= -16)
                {
                    isWalkingUp = false;
                    playerYWalkFrame = 0;
                    playerWorldTileY--;
                }
            }
            else if (isWalkingDown)
            {
                playerYWalkFrame += 2;
                if (playerYWalkFrame > 16)
                {
                    isWalkingDown = false;
                    playerYWalkFrame = 0;
                    playerWorldTileY++;
                }
            }
            if (messageBoxString.Length > 0 && textframe < messageBoxString.Length)
            {
                textframe++;
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

                // Draw the health bar
                g.FillRectangle(whiteBrush, 268, 193, 5, 4); // Fills 0..39

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.D)
            {
                isWalkingRight = true;
            }
            else if (e.KeyCode == Keys.A)
            {
                isWalkingLeft = true;
            }
            else if (e.KeyCode == Keys.W)
            {
                isWalkingUp = true;
            }
            else if (e.KeyCode == Keys.S)
            {
                isWalkingDown = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }
    }
}




