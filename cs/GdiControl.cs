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

        Bitmap native;

        bool imagesLoaded;

        bool systemCursor;

        int playerWorldTileX;
        int playerWorldTileY;
        
        int playerAnimationFrame = 0;
        int playerXWalkFrame = 0;
        int playerYWalkFrame = 0;

        bool isWalkingRight;
        bool isWalkingLeft;
        bool isWalkingUp;
        bool isWalkingDown;

        int scaleFactor = 3;
        int mouseX, mouseY;
        System.Timers.Timer gameTimer;

        public GdiControl()
        {
            debugFont = new Font("Arial", 16);
            debugBrush = new SolidBrush(Color.White);
            this.DoubleBuffered = true;

            systemCursor = true;

            playerWorldTileX = 1;
            playerWorldTileY = 1;
        }

        void DrawGameplay(Graphics g)
        {
            int t = 16;

            for (int yTile = 0; yTile <= 15; ++yTile)
            {
                for (int xTile = 0; xTile <= 20; ++xTile)
                {
                    int tileIndex = 1;

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

                Rectangle sourceRect = new Rectangle((4 + (playerAnimationFrame / 10)) * t, 1 * t, t, t);
                g.DrawImage(tileset, destRect, sourceRect, GraphicsUnit.Pixel);

            }
        }

        void DrawMessageBoxText(Graphics g)
        {
            // Draw message box content
            string text = "Hello, Hackforge!";

            int dstX = 52;
            int w = 8;
            int h = 16;

            for (int i=0; i<text.Length; ++i)
            {
                Rectangle destRect = new Rectangle(dstX, 197, w, h);

                char glyph = text[i];

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

        private void LoadImageAssets()
        {
            string dir = Directory.GetCurrentDirectory();
            string imagePath = dir;

#if DEBUG
            imagePath = dir + "\\..\\..\\..\\..\\";
#endif
            imagePath += "images\\";

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

            gameTimer = new System.Timers.Timer(32.0f);
            gameTimer.Elapsed += GameTimer_Elapsed;
            gameTimer.Start();
        }

        private void GameTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            playerAnimationFrame = (playerAnimationFrame + 1) % 20;

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

            if (!imagesLoaded)
            {
                LoadImageAssets();
                imagesLoaded = true;
            }

            {
                Graphics g = Graphics.FromImage(native);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImageUnscaled(reference, 0, 0, 320, 240);

                DrawGameplay(g);

                DrawMessageBoxText(g);

                // Draw the HUD
                g.DrawImageUnscaled(uiflavor, 0, 0);

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

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Right)
            {
                isWalkingRight = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                isWalkingLeft = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                isWalkingUp = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                isWalkingDown = true;
            }
        }
    }
}




