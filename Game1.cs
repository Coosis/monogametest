using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace monogametest
{
    public class Game1 : Game
    {
        //[warning: DO NOT TOUCH.] Monogame core code
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //core variables
        private List<Canvas> canvas = new List<Canvas>();
        private KeyboardState keystate;
        private KeyboardState previous_keystate;
        private MouseState mousestate;
        private MouseState previous_mousestate;
        private float previous_mousescroll;

        //testing
        private Texture2D ui_image;
        private UI myUI, sonUI;
        private float f1;
        private OrthographicCamera cam;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            previous_keystate = Keyboard.GetState();
            previous_mousestate = Mouse.GetState();
            previous_mousescroll = previous_mousestate.ScrollWheelValue;

            var Viewport = new DefaultViewportAdapter(GraphicsDevice);
            cam = new OrthographicCamera(Viewport);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ui_image = Content.Load<Texture2D>("ui/ui_image");

            Canvas ohboy = new Canvas();

            myUI = new UI(null, ui_image, new Vector2(20, 20), new Vector2(200, 200), new Vector2(10, 10));
            sonUI = new UI(null, ui_image, new Vector2(f1, 0), new Vector2(65, 65), new Vector2(10, 10));
            myUI.AddChild(sonUI);
            myUI.position = myUI.GetParentBounds(_graphics, new Vector2(0, 0));
            ohboy.AddUI(myUI);

            canvas.Add(ohboy);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Content.Unload();
                //Exit();

            Window.Title = "MONOGAMEFUN";
            f1 += 5;

            keystate = Keyboard.GetState();
            mousestate = Mouse.GetState();

            
            cam.Move(new Vector2((mousestate.ScrollWheelValue - previous_mousescroll)*0.1f, 0));
            //cam.ZoomIn((mousestate.ScrollWheelValue - previous_mousescroll)*0.001f);

            //Debug.WriteLine(cam.Zoom);
            //Debug.WriteLine(mousestate.ScrollWheelValue - previous_mousescroll);

            //Core expressions
            previous_keystate = keystate;
            previous_mousestate = mousestate;
            previous_mousescroll = mousestate.ScrollWheelValue;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw all ui, under all canvases
            if(canvas != null){
                foreach(Canvas c in canvas){
                    c.Draw(_spriteBatch);
                }
            }

            _spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());
            _spriteBatch.Draw(ui_image, new Vector2(0, 0), new Rectangle(0, 0, ui_image.Width, ui_image.Height), new Color(255, 255, 255, 255));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
