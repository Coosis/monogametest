using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogametest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D ui_image;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //UI ui = new UI(_spriteBatch, ui_image, new Vector2(35, 56), new Vector2(200, 45), new Vector2(10, 10));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ui_image = Content.Load<Texture2D>("ui/ui_image");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Content.Unload();
                //Exit();

            Window.Title = "MONOGAMEFUN";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// The ui class.
    /// </summary>
    public class UI{
        private SpriteBatch _spritebatch;

        /// <summary>
        /// The default texture.
        /// </summary>
        public Texture2D texture{
            get{ return _texture; }
            set{
                if(value != null){
                    _texture = value; 
                }
            }
        }
        private Texture2D _texture = null;
        /// <summary>
        /// The width of the ui(in pixels).
        /// </summary>
        public int width = 0;
        /// <summary>
        /// The height of the ui(in pixels).
        /// </summary>
        public int height = 0;
        /// <summary>
        /// The position of the ui(in pixels), relative to upper left corner.
        /// </summary>
        /// <returns></returns>
        public Vector2 position = new Vector2(0, 0);
        /// <summary>
        /// The angle of the ui.
        /// </summary>
        public float angle = 0;
        /// <summary>
        /// The depth/layer of the ui.
        /// </summary>
        public float layer = 0;
        /// <summary>
        /// The Center of the ui.
        /// </summary>
        /// <returns></returns>
        public Vector2 center = new Vector2(0, 0);
        /// <summary>
        /// The color of the ui, ranged from 0-255.
        /// </summary>
        /// <returns></returns>
        public Color color = new Color(255, 255, 255, 255);
        /// <summary>
        /// The filp of ui.
        /// </summary>
        public SpriteEffects flip = SpriteEffects.None;
        /// <summary>
        /// Side of the nine patches.
        /// </summary>
        /// <returns></returns>
        public Vector2 side = new Vector2(0, 0);
        /// <summary>
        /// Have ui stretch-mode nine patch?
        /// </summary>
        public bool ninePatches = true;
    
        /// <summary>
        /// The draw process of the ui.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <returns></returns>
        public bool Draw(SpriteBatch spriteBatch){
            if(texture == null || spriteBatch == null){
                return false;
            }

            if(ninePatches){
                spriteBatch.Begin();
                //4 corners
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X), (int)(position.Y - center.Y), (int)side.X, (int)side.Y), new Rectangle(0, 0, (int)side.X, (int)side.Y), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X - side.X + width), (int)(position.Y - center.Y), (int)side.X, (int)side.Y), new Rectangle(texture.Width - (int)side.X, 0, (int)side.X, (int)side.Y), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X), (int)(position.Y - center.Y - side.Y + height), (int)side.X, (int)side.Y), new Rectangle(0, texture.Height - (int)side.Y, (int)side.X, (int)side.Y), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X - side.X + width), (int)(position.Y - center.Y - side.Y + height), (int)side.X, (int)side.Y), new Rectangle(texture.Width - (int)side.X, texture.Height - (int)side.Y, (int)side.X, (int)side.Y), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.End();
                spriteBatch.Begin();
                
                //4 sides
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X + side.X), (int)(position.Y - center.Y), (int)(width - 2*side.X), (int)side.Y), new Rectangle((int)side.X, 0, (int)(texture.Width - 2*side.X), (int)side.Y), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X), (int)(position.Y - center.Y + side.Y), (int)side.X, (int)(height - 2*side.Y)), new Rectangle(0, (int)side.Y, (int)side.X, (int)(texture.Height - 2*side.Y)), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X - side.X + width), (int)(position.Y - center.Y + side.Y), (int)side.X, (int)(height - 2*side.Y)), new Rectangle((int)(texture.Width - side.X), (int)side.Y, (int)side.X, (int)(texture.Height - 2*side.Y)), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X + side.X), (int)(position.Y - center.Y - side.Y + height), (int)(width - 2*side.X), (int)side.Y), new Rectangle((int)side.X, (int)(texture.Height - side.Y), (int)(texture.Width - 2*side.X), (int)side.Y), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);
                //center
                spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X + side.X), (int)(position.Y - center.Y + side.Y), (int)(width - 2*side.X), (int)(height - 2*side.Y)), new Rectangle((int)side.X, (int)side.Y, (int)(texture.Width - 2*side.X), (int)(texture.Height - 2*side.Y)), color, angle, new Vector2(0, 0), SpriteEffects.None, layer);

                spriteBatch.End();
            }
            else{
                spriteBatch.Begin();
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), new Rectangle(0, 0, texture.Width, texture.Height), color, angle, center, flip, layer);
                spriteBatch.End();
            }

            return true;
        }

        
        /// <summary>
        /// Empty ui constructor.
        /// </summary>
        public UI(){}
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos){
            this._spritebatch = spriteBatch;

            this.texture = texture;
            this.position = pos;
            this.width = texture.Width;
            this.height = texture.Height;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Basic texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect){
            this._spritebatch = spriteBatch;

            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="side">The side's width and height of the nine-patched ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 side){
            this._spritebatch = spriteBatch;

            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.side = side;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="side">The side's width and height of the nine-patched ui.</param>
        /// <param name="center">The point used to calculate the position and such(in pixels).</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 side, Vector2 center){
            this._spritebatch = spriteBatch;

            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.side = side;
            this.center = center;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="side">The side's width and height of the nine-patched ui.</param>
        /// <param name="center">The point used to calculate the position and such(in pixels).</param>
        /// <param name="layer">The layer/depth to draw on.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 side, Vector2 center, float layer){
            this._spritebatch = spriteBatch;
            
            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.side = side;
            this.center = center;
            this.layer = layer;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="side">The side's width and height of the nine-patched ui.</param>
        /// <param name="center">The point used to calculate the position and such(in pixels).</param>
        /// <param name="layer">The layer/depth to draw on.</param>
        /// <param name="angle">The angle of the ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 side, Vector2 center, float layer, float angle){
            this._spritebatch = spriteBatch;
            
            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.side = side;
            this.center = center;
            this.layer = layer;
            this.angle = angle;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="center">The point used to calculate the position and such(in pixels).</param>
        /// <param name="ninePatches">NinePatches is on by default. Only use this to turn it off.</param>
        /// <param name="layer">The layer/depth to draw on.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 center, bool ninePatches, float layer){
            this._spritebatch = spriteBatch;
            
            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.center = center;
            this.ninePatches = false;
            this.layer = layer;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing(ninePatch on).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="center">The point used to calculate the position and such(in pixels).</param>
        /// <param name="ninePatches">NinePatches is on by default. Only use this to turn it off.</param>
        /// <param name="layer">The layer/depth to draw on.</param>
        /// <param name="angle">The angle of the ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 center, bool ninePatches, float layer, float angle){
            this._spritebatch = spriteBatch;
            
            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.center = center;
            this.ninePatches = false;
            this.layer = layer;
            this.angle = angle;

            Draw(_spritebatch);
        }
    }
}
