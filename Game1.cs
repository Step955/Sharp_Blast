
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sharp_Blast
{

    public class Game1 : Game
    {
        private static GameScreen gameScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static void exit()
        {
            gameScreen.exit();
        }

        protected override void Initialize()
        {
            gameScreen = new GameScreen(base.GraphicsDevice, Content);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            gameScreen.Ĺoad();
        }

        protected override void Update(GameTime gameTime)
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            TouchCollection touchLocation = TouchPanel.GetState();
            gameScreen.Update(touchLocation, GraphicsDevice);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameScreen.Draw();
            base.Draw(gameTime);
        }

    }
}