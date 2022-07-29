using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
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
        private Texture2D ui_image, ui_circle, matchman;
        private SpriteFont PS2P;
        private SpriteSheet ss;
        private Animation Idle, Jump, Running;
        private Node player, block;
        private Vector2 movement = new Vector2(0, 0);
        private int JumpPhase;
        private float JumpTime;
        private int intA, intD;
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
            //var Viewport = new WindowViewportAdapter(this.Window, GraphicsDevice);
            //var Viewport = new BoxingViewportAdapter(this.Window, GraphicsDevice);
            var Viewport = new ScalingViewportAdapter(GraphicsDevice, 800, 480);
            cam = new OrthographicCamera(Viewport);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ui_image = Content.Load<Texture2D>("ui/ui_image");
            ui_circle = Content.Load<Texture2D>("ui/ui_circle");
            PS2P = Content.Load<SpriteFont>("PS2P");
            
            matchman = Content.Load<Texture2D>("matchman");
            ss = new SpriteSheet(matchman);
            ss.cut_bounds = new Vector2(32, 48);
            ss.Cut(GraphicsDevice);
            Texture2D[] output = ss.cut_texture.ToArray();
            Texture2D[] idle_texture = new Texture2D[]{ output[0] };
            Texture2D[] jump_texture = Animation.Trim(output, 1, 3);
            Texture2D[] run_texture = Animation.Trim(output, 4, 6);
            Idle = new Animation(idle_texture, 1000);
            Jump = new Animation(jump_texture, 100);
            Jump.loop = false;
            Running = new Animation(run_texture, new float[]{100, 100, 150, 100, 100, 150});

            Canvas ohboy = new Canvas();
            canvas.Add(ohboy);

            player = new Node();
            player.position = new Vector2(120, 20);
            player.AddAnimator();
            player.width = 32;
            player.height = 48;
            player.animator.AnimationSet.Add("Jump", Jump);
            player.animator.AnimationSet.Add("Run", Running);
            player.animator.AnimationSet.Add("Idle", Idle);
            player.animator.SetAnimation("Idle");
            player.AddCollider(new Vector2(32, 48));
            player.AddPhysicalBody();
            player.physicalBody.externalVelocity.Add("move", movement);
            player.physicalBody.mass = 1;
            player.physicalBody.gravity_acceleration = new Vector2(0, 10);

            block = new Node();
            block.texture = ui_image;
            block.position = new Vector2(32, 200);
            block.width = 512;
            block.height = 64;
            block.AddCollider(new Vector2(512, 64));
            block.collider.elasticity = -0.8f;
            //block.AddPhysicalBody();
            //block.physicalBody.gravity_acceleration = new Vector2(0,0);
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
            bool KeyA = keystate.IsKeyDown(Keys.A);
            bool p_KeyA = previous_keystate.IsKeyDown(Keys.A);
            bool KeyD = keystate.IsKeyDown(Keys.D);
            bool p_KeyD = previous_keystate.IsKeyDown(Keys.D);
            bool KeyW = keystate.IsKeyDown(Keys.W);
            bool p_KeyW = previous_keystate.IsKeyDown(Keys.W);
            bool KeyS = keystate.IsKeyDown(Keys.S);
            bool p_KeyS = previous_keystate.IsKeyDown(Keys.S);
            if(KeyA){
                player.flip = SpriteEffects.FlipHorizontally;
                intA = 1;
                if(!p_KeyA)
                    player.animator.SetAnimation("Run");
            }
            else{ 
                intA = 0;
                if(p_KeyA && !KeyD)
                    player.animator.SetAnimation("Idle");
            }
            if(KeyD){
                player.flip = SpriteEffects.None;
                intD = 1;
                if(!p_KeyD)
                    player.animator.SetAnimation("Run");
            }
            else{ 
                intD = 0;
                if(p_KeyD && !KeyA)
                    player.animator.SetAnimation("Idle");
            }
            if(KeyW && !p_KeyW){
                player.animator.SetAnimation("Jump");
                JumpTime = 0;
                JumpPhase = 1;
            }
            if(JumpPhase == 1){
                JumpTime += gameTime.ElapsedGameTime.Milliseconds;
                if(JumpTime >= 200){
                    JumpTime = 0;
                    JumpPhase = 2;
                    player.physicalBody.SetExternalForce("jump", new Vector2(0, -50));
                }
            }
            if(JumpPhase == 2){
                JumpTime += gameTime.ElapsedGameTime.Milliseconds;
                if(JumpTime >= 100){
                    player.physicalBody.SetExternalForce("jump", new Vector2(0, 0));
                    JumpTime = 0;
                    JumpPhase = 0;
                }
            }
            if(player.physicalBody.velocity.Y >= -0.25f && player.physicalBody.velocity.Y <= 0.25f && intD == intA && JumpPhase == 0)
                player.animator.SetAnimation("Idle");
            movement.X = (intD - intA)*5;
            player.physicalBody.externalVelocity["move"] = movement;
            //Debug.WriteLine(cam.Zoom);
            //Debug.WriteLine(mousestate.ScrollWheelValue - previous_mousescroll);
            foreach(Node node in Node.NodePool){
                node.Update(gameTime);
            }
            foreach(PhysicalBody pb in PhysicalBody.physicalBodies){
                pb.Move();
            }

            //Core expressions
            previous_keystate = keystate;
            previous_mousestate = mousestate;
            previous_mousescroll = mousestate.ScrollWheelValue;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if(player.texture != null)
                player.Draw(_spriteBatch);
            block.Draw(_spriteBatch);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(PS2P, mousestate.Position.X + "/" + mousestate.Position.Y, new Vector2(0, 0), new Color(255, 266, 255, 0));
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
