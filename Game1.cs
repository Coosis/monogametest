using System;
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
        private Texture2D ui_image, ui_circle;
        private SpriteFont PS2P;
        private UI myUI, sonUI;
        private float x1 = 0, y1 = 0;
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

            //var Viewport = new DefaultViewportAdapter(GraphicsDevice);
            var Viewport = new ScalingViewportAdapter(GraphicsDevice, 800, 480);
            //var Viewport = new WindowViewportAdapter(this.Window, GraphicsDevice);
            //var Viewport = new BoxingViewportAdapter(this.Window, GraphicsDevice);
            cam = new OrthographicCamera(Viewport);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ui_image = Content.Load<Texture2D>("ui/ui_image");
            ui_circle = Content.Load<Texture2D>("ui/ui_circle");
            PS2P = Content.Load<SpriteFont>("PS2P");

            Canvas ohboy = new Canvas();

            myUI = new UI(null, ui_circle, new Vector2(20, 20), new Vector2(200, 200), new Vector2(10, 10), ninePatches: false);
            sonUI = new UI(null, ui_image, new Vector2(5, 0), new Vector2(65, 65), new Vector2(10, 10));
            myUI.AddChild(sonUI);
            myUI.position = myUI.GetParentVertex(_graphics, new Vector2(0, 0));
            ohboy.AddUI(myUI);

            canvas.Add(ohboy);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Content.Unload();
                //Exit();

            Window.Title = "MONOGAMEFUN";

            keystate = Keyboard.GetState();
            mousestate = Mouse.GetState();
            
            //cam.Move(new Vector2((mousestate.ScrollWheelValue - previous_mousescroll)*0.1f, 0));
            if(keystate.IsKeyDown(Keys.A)){
                x1 -= 3;
            }
            if(keystate.IsKeyDown(Keys.D)){
                x1 += 3;
            }
            if(keystate.IsKeyDown(Keys.W)){
                y1 -= 3;
            }
            if(keystate.IsKeyDown(Keys.S)){
                y1 += 3;
            }

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

            _spriteBatch.Begin(transformMatrix: cam.GetViewMatrix(), samplerState: SamplerState.PointWrap);
            _spriteBatch.Draw(ui_image, new Vector2(x1, y1), new Rectangle(0, 0, ui_image.Width, ui_image.Height), new Color(255, 255, 255, 255));
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(PS2P, mousestate.Position.X + "/" + mousestate.Position.Y, new Vector2(0, 0), new Color(255, 266, 255, 0));
            _spriteBatch.DrawString(PS2P, myUI.GetMouseOver().ToString(), new Vector2(0, 15), new Color(255, 266, 255, 0));
            _spriteBatch.DrawString(PS2P, sonUI.GetMouseOver().ToString(), new Vector2(0, 30), new Color(255, 266, 255, 0));
            _spriteBatch.End();

            //Draw all ui, under all canvases
            if(canvas != null){
                foreach(Canvas c in canvas){
                    c.Update();
                    c.Draw(_spriteBatch);
                }
            }
            base.Draw(gameTime);
        }
    }
}
