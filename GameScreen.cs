
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Linq;


namespace Sharp_Blast
{
    internal class GameScreen
    {
        private Field field;

        private static int score = 0;
        private static int highscore = 0;

        public static RenderTarget2D screen;

        private TouchCollection touchLocation_old;

        GraphicsDevice GraphicsDevice;
        ContentManager content;

        private SpriteBatch _spriteBatch;

        private float scale;

        SpriteFont font;

        Texture2D resetButtonTexture;

        private Bricks activeBricks;

        private enum ScaleMode
        {
            StretchToFill,
            PreserveAspectFit
        }

        private ScaleMode _scaleMode = ScaleMode.PreserveAspectFit;

        public GameScreen(GraphicsDevice graphicsDevice, ContentManager Content)
        {
            GraphicsDevice = graphicsDevice;
            content = Content;

            touchLocation_old = TouchPanel.GetState();
            screen = new RenderTarget2D(GraphicsDevice, 1080, 2280);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        public void Ĺoad() { 
        
            
            highscore = FileManager.load();
            field = new Field(content);

            resetButtonTexture = content.Load<Texture2D>("reload");
            font = content.Load<SpriteFont>("Font");

            field = new Field(content);
            activeBricks = new Bricks(content, GraphicsDevice);
        }

        public void Draw()
        {
            GraphicsDevice.SetRenderTarget(screen);

            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();

            //reset button
            _spriteBatch.Draw(resetButtonTexture, new Rectangle(900, 200, 100, 100), Color.White);

            //scores
            _spriteBatch.DrawString(font, Convert.ToString(highscore), new Vector2(100, 200), Color.Gold, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 1.0f);
            _spriteBatch.DrawString(font, Convert.ToString(score), new Vector2(100, 300), Color.Snow, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);

            //end draw to custom render target
            _spriteBatch.End();

            field.render(_spriteBatch);

            activeBricks.render(_spriteBatch, GraphicsDevice);

            //Final draw to screen

            GraphicsDevice.SetRenderTarget(null);

            //GraphicsDevice.Clear(new Color(66, 90, 164));
            GraphicsDevice.Clear(new Color(50, 50, 50));

            // compute destination rectangle depending on chosen scale mode
            int backW = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int backH = GraphicsDevice.PresentationParameters.BackBufferHeight;
            Rectangle destRect;

            if (_scaleMode == ScaleMode.StretchToFill)
            {
                // Stretch to fill full backbuffer (may change aspect ratio)
                destRect = new Rectangle(0, 0, backW, backH);
            }
            else
            {
                // Preserve aspect ratio and center (letterbox / pillarbox)
                scale = Math.Min(backW / (float)screen.Width, backH / (float)screen.Height);
                int drawW = (int)(screen.Width * scale);
                int drawH = (int)(screen.Height * scale);
                destRect = new Rectangle((backW - drawW) / 2, (backH - drawH) / 2, drawW, drawH);
            }

            // Use PointClamp for pixel-perfect scaling (or LinearClamp for smooth)
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(screen, destRect, Color.White);
            _spriteBatch.End();
        }

        public void Update(TouchCollection touchLocation, GraphicsDevice graphicsDevice)
        {

            GraphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            if (touchLocation.Count > 0)
            {

                //Bricks.ActiveBricks[0].setCords(Convert.ToInt32(touchLocation[0].Position.X), Convert.ToInt32(touchLocation[0].Position.Y));

                if (touchLocation_old.Count == 0)
                {
                    Vector2 recalculatedTouch = ScreenToRenderTarget(touchLocation[0].Position);
                    Rectangle resetRect = new Rectangle(900, 200, 100, 100);
                    if (touchLocation[0].Position.Y > (GraphicsDevice.PresentationParameters.BackBufferHeight / 3) * 2)
                    {

                        if (touchLocation[0].Position.X < GraphicsDevice.PresentationParameters.BackBufferWidth / 3)
                        {
                            Bricks.grabed = 0;
                        }
                        else if (touchLocation[0].Position.X < (GraphicsDevice.PresentationParameters.BackBufferWidth / 3) * 2)
                        {
                            Bricks.grabed = 1;
                        }
                        else
                        {
                            Bricks.grabed = 2;
                        }
                    }
                    else if (resetRect.Contains(ScreenToRenderTarget(touchLocation[0].Position)))
                    {
                        Bricks.generateBricks();
                        Field.pole = new int[8, 8];
                        if (score > highscore)
                        {
                            FileManager.save(score);
                            highscore = score;
                        }
                        score = 0;

                    }




                }
                else if (Bricks.grabed > -1)
                {
                    Bricks.ActiveBricks[Bricks.grabed].setCords(Convert.ToInt32((touchLocation[0].Position.X / scale) - ((GraphicsDevice.PresentationParameters.BackBufferWidth / 24 * scale))), Convert.ToInt32((touchLocation[0].Position.Y / scale) - (GraphicsDevice.PresentationParameters.BackBufferHeight / 4 / scale)));
                }

            }
            else if (touchLocation.Count == 0 && touchLocation_old.Count > 0 && Bricks.grabed > -1)
            {
                field.place();
                Bricks.ActiveBricks[Bricks.grabed].resetCords(Bricks.grabed);
                Bricks.grabed = -1;
            }

            touchLocation_old = touchLocation;

            /*AI:
            * part2 in render
            */
            //Field.pole = Bricks.prediction;
            /*
            */

            score += field.checkField();

        }
        private Vector2 ScreenToRenderTarget(Vector2 screenPosition)
        {
            if (screen == null) return screenPosition;

            int backW = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int backH = GraphicsDevice.PresentationParameters.BackBufferHeight;

            if (_scaleMode == ScaleMode.StretchToFill)
            {
                // mapujeme přímo podle poměrů šířek/výšek
                float sx = screen.Width / (float)backW;
                float sy = screen.Height / (float)backH;
                return new Vector2(screenPosition.X * sx, screenPosition.Y * sy);
            }
            else
            {
                // zachování poměru stran + vystředění (letterbox/pillarbox)
                float s = Math.Min(backW / (float)screen.Width, backH / (float)screen.Height);
                float drawW = screen.Width * s;
                float drawH = screen.Height * s;
                float offsetX = (backW - drawW) / 2f;
                float offsetY = (backH - drawH) / 2f;

                // převedeme pozici do lokálních souřadnic vykresleného render targetu a pak do souřadnic render targetu
                float localX = (screenPosition.X - offsetX) / s;
                float localY = (screenPosition.Y - offsetY) / s;

                return new Vector2(localX, localY);
            }
        }

        private Point ScreenToRenderTargetPoint(Point screenPoint)
        {
            Vector2 v = ScreenToRenderTarget(new Vector2(screenPoint.X, screenPoint.Y));
            return new Point((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        }

    }
}
