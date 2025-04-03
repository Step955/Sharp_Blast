using Android.Text.Method;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace Sharp_Blast;

public class Game1 : Game
{
    private Bricks activeBricks;

    private Field field;

    public static RenderTarget2D screen;

    SpriteFont font;

    Texture2D resetButtonTexture;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private TouchCollection touchLocation_old;

    private int score = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        touchLocation_old = TouchPanel.GetState();

        screen = new RenderTarget2D(GraphicsDevice, 1080, 2280);

        _spriteBatch = new SpriteBatch(base.GraphicsDevice);

        base.Initialize();

        GraphicsDevice.Viewport = new Viewport(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
    }

    protected override void LoadContent()
    {

        resetButtonTexture = Content.Load<Texture2D>("reload");
        font = Content.Load<SpriteFont>("Font");
        
        field = new Field(Content);
        activeBricks = new Bricks(Content);
        
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //field = new Field(Content);

        // TODO: Add your update logic here

        //activeBricks.generateBricks();

        TouchCollection touchLocation = TouchPanel.GetState();
        
        if (touchLocation.Count > 0)
        {
            
            //Bricks.ActiveBricks[0].setCords(Convert.ToInt32(touchLocation[0].Position.X), Convert.ToInt32(touchLocation[0].Position.Y));

            if (touchLocation_old.Count == 0)
            {
                if (touchLocation[0].Position.Y > 1700)
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
                else if (new Rectangle(900+ ((GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 540), 200+ ((GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 1140), 100, 100).Contains(touchLocation[0].Position))
                {
                    Bricks.generateBricks();
                    Field.pole = new int[8, 8];
                    score = 0;

                }
            }
            else if(Bricks.grabed > -1)
            {
                Bricks.ActiveBricks[Bricks.grabed].setCords(Convert.ToInt32(touchLocation[0].Position.X-((GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 490)), Convert.ToInt32(touchLocation[0].Position.Y-((GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - 1090)));
            }

        }else if(touchLocation.Count == 0 && touchLocation_old.Count > 0 && Bricks.grabed > -1)
        {
            field.place();
            Bricks.ActiveBricks[Bricks.grabed].resetCords(Bricks.grabed);
            Bricks.grabed = -1;
        }

            touchLocation_old = touchLocation;

        score += field.checkField();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(screen);

        GraphicsDevice.Clear(Color.Transparent);

        _spriteBatch.Begin();

        _spriteBatch.Draw(resetButtonTexture, new Rectangle(900, 200, 100, 100), Color.White);
        _spriteBatch.DrawString(font, Convert.ToString(score), new Vector2(100, 300), Color.White, 0, new Vector2(0, 0), 10.0f, SpriteEffects.None,0.5f);

        //_spriteBatch.Draw(Field.pole_texture, new Rectangle(Convert.ToInt32(1040), Convert.ToInt32(2240), 40, 40), Color.White);
        //_spriteBatch.Draw(Field.pole_texture, new Rectangle(Convert.ToInt32(0), Convert.ToInt32(90), 40, 40), Color.White);
        _spriteBatch.End();

        field.render(_spriteBatch);

        activeBricks.render(_spriteBatch, GraphicsDevice);



        GraphicsDevice.SetRenderTarget(null);
        
        GraphicsDevice.Clear(new Color(66, 90, 164));

        _spriteBatch.Begin();

        _spriteBatch.Draw(screen, new Vector2(((GraphicsDevice.PresentationParameters.BackBufferWidth/2)-540), ((GraphicsDevice.PresentationParameters.BackBufferHeight/2)-1140)), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
