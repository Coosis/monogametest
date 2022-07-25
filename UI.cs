using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogametest{
    public class UI{
        private SpriteBatch _spritebatch;
        //ui basic properties
        /// <summary>
        /// The default texture.
        /// </summary>
        public Texture2D texture{
            get{ return _texture; }
            set{
                if(value != null){
                    _texture = value; 
                    color_data = new Color[value.Width * value.Height];
                    value.GetData<Color>(color_data);
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
        public Vector2 position{
            get{ 
                if(parent == null){
                    return _position;
                }
                else {
                    return _position - parent.position;
                }
            }

            set{
                if(parent == null){
                    _position = value;
                }
                else{
                    _position = parent.position + value;
                }

                foreach(UI ui in children){
                    ui.position += position;
                }
            }
        }
        /// <summary>
        /// The absolute position of the ui.
        /// </summary>
        /// <value></value>
        public Vector2 abs_position{
            get{ return _position; }
            set{ _position = value; }
        }
        /// <summary>
        /// The angle of the ui(does not work with 9-patch).
        /// </summary>
        public float angle = 0;
        /// <summary>
        /// The depth/layer of the ui.
        /// </summary>
        public float layer = 0;
        /// <summary>
        /// The Center of the ui(in pixels).
        /// </summary>
        /// <returns></returns>
        public Vector2 center = new Vector2(0, 0);
        /// <summary>
        /// The color of the ui, ranged from 0-255.
        /// </summary>
        /// <returns></returns>
        public Color color = new Color(255, 255, 255, 255);
        /// <summary>
        /// The filp of ui(Does not work with 9-patch).
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
        public bool ninePatches{
            get { return _ninePatches; }
            set {
                _ninePatches = value;
                if(value){
                    angle = 0;
                    flip = SpriteEffects.None;
                }
            }
        }
        /// <summary>
        /// The parent of this ui.
        /// </summary>
        public UI parent = null;
        /// <summary>
        /// The children of this ui.
        /// </summary>
        public List<UI> children = new List<UI>();
        public MouseEventMode mouseEventMode = MouseEventMode.Stop;

        //inner property fields
        /// <summary>
        /// The color of every pixel of the texture.
        /// </summary>
        protected Color[] color_data;
        protected bool MouseOver = false;
        protected bool previous_MouseOver = false;
        protected bool NonAlphaMouseOver = false;
        protected bool previous_NonAlphaMouseOver = false;
        protected bool _ninePatches = true;
        protected MouseState mouseState, previous_mouseState;
        protected Vector2 _position = new Vector2(0, 0);

        //ui functions
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
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                //4 corners
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X), (int)(_position.Y - center.Y), (int)side.X, (int)side.Y), new Rectangle(0, 0, (int)side.X, (int)side.Y), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X - side.X + width), (int)(_position.Y - center.Y), (int)side.X, (int)side.Y), new Rectangle(texture.Width - (int)side.X, 0, (int)side.X, (int)side.Y), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X), (int)(_position.Y - center.Y - side.Y + height), (int)side.X, (int)side.Y), new Rectangle(0, texture.Height - (int)side.Y, (int)side.X, (int)side.Y), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X - side.X + width), (int)(_position.Y - center.Y - side.Y + height), (int)side.X, (int)side.Y), new Rectangle(texture.Width - (int)side.X, texture.Height - (int)side.Y, (int)side.X, (int)side.Y), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                
                //4 sides
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X + side.X), (int)(_position.Y - center.Y), (int)(width - 2*side.X), (int)side.Y), new Rectangle((int)side.X, 0, (int)(texture.Width - 2*side.X), (int)side.Y), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X), (int)(_position.Y - center.Y + side.Y), (int)side.X, (int)(height - 2*side.Y)), new Rectangle(0, (int)side.Y, (int)side.X, (int)(texture.Height - 2*side.Y)), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X - side.X + width), (int)(_position.Y - center.Y + side.Y), (int)side.X, (int)(height - 2*side.Y)), new Rectangle((int)(texture.Width - side.X), (int)side.Y, (int)side.X, (int)(texture.Height - 2*side.Y)), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X + side.X), (int)(_position.Y - center.Y - side.Y + height), (int)(width - 2*side.X), (int)side.Y), new Rectangle((int)side.X, (int)(texture.Height - side.Y), (int)(texture.Width - 2*side.X), (int)side.Y), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);
                //center
                spriteBatch.Draw(texture, new Rectangle((int)(_position.X - center.X + side.X), (int)(_position.Y - center.Y + side.Y), (int)(width - 2*side.X), (int)(height - 2*side.Y)), new Rectangle((int)side.X, (int)side.Y, (int)(texture.Width - 2*side.X), (int)(texture.Height - 2*side.Y)), color, 0, new Vector2(0, 0), SpriteEffects.None, layer);

                spriteBatch.End();
            }
            else{
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                spriteBatch.Draw(texture, new Rectangle((int)_position.X, (int)_position.Y, width, height), new Rectangle(0, 0, texture.Width, texture.Height), color, angle, center, flip, layer);
                spriteBatch.End();
            }

            foreach(UI i in children){
                i.Draw(spriteBatch);
            }

            return true;
        }
        public void Update(){
            mouseState = Mouse.GetState();
            Vector2 mouse_pos = new Vector2(mouseState.X, mouseState.Y) - _position + center;

            NonAlphaMouseOver = (mouse_pos.X >= 0) && (mouse_pos.X <= width) && (mouse_pos.Y >= 0) && (mouse_pos.Y <= height); 
            int index = ((int)(mouse_pos.Y/height*texture.Height) - 1)*texture.Width + ((int)(mouse_pos.X/width*texture.Width));
            index = MathHelper.Clamp(index, 0, color_data.Length - 1);
            MouseOver = (!(color_data[index].A == 0) && NonAlphaMouseOver);

            foreach(UI child in children){
                child.Update();
            }
            
            if(children.Count == 0 && NonAlphaMouseOver){
                List<UI> tree = new List<UI>();
                UI container = this;
                tree.Add(this);
                while(container.parent != null){
                    container = container.parent;
                    tree.Add(container);
                }
                bool FirstStop = false;
                foreach(UI ui in tree){
                    if(FirstStop){
                        ui.MouseOver = false;
                        ui.NonAlphaMouseOver = false;
                    }
                    else{
                        if(ui.mouseEventMode == MouseEventMode.Stop){
                            FirstStop = true;
                        }
                        else if(ui.mouseEventMode == MouseEventMode.Ignore){
                            ui.MouseOver = false;
                            ui.NonAlphaMouseOver = false;
                        }
                    }
                }
            }
            
            //Mouse event handling
            if(ninePatches){
                if(!previous_NonAlphaMouseOver && NonAlphaMouseOver)
                    if(OnMouseEnter != null)
                        OnMouseEnter();
                else if(previous_NonAlphaMouseOver && !NonAlphaMouseOver)
                    if(OnMouseExit != null)
                        OnMouseExit();

                if(NonAlphaMouseOver){
                    if(mouseState.LeftButton == ButtonState.Pressed && previous_mouseState.LeftButton != ButtonState.Pressed){
                        if(OnMouseClick != null)
                            OnMouseClick(MouseClickMode.Left);
                    }
                    else if(mouseState.RightButton == ButtonState.Pressed && previous_mouseState.RightButton != ButtonState.Pressed){
                        if(OnMouseClick != null)
                            OnMouseClick(MouseClickMode.Right);
                    }
                    else if(mouseState.MiddleButton == ButtonState.Pressed && previous_mouseState.MiddleButton != ButtonState.Pressed){
                        if(OnMouseClick != null)
                            OnMouseClick(MouseClickMode.Middle);
                    }
                }
            }
            else{
                if(!previous_MouseOver && MouseOver)
                    if(OnMouseEnter != null)
                        OnMouseEnter();
                else if(previous_MouseOver && !MouseOver)
                    if(OnMouseExit != null)
                        OnMouseExit();

                if(MouseOver){
                    if(mouseState.LeftButton == ButtonState.Pressed && previous_mouseState.LeftButton != ButtonState.Pressed){
                        if(OnMouseClick != null)
                            OnMouseClick(MouseClickMode.Left);
                    }
                    else if(mouseState.RightButton == ButtonState.Pressed && previous_mouseState.RightButton != ButtonState.Pressed){
                        if(OnMouseClick != null)
                            OnMouseClick(MouseClickMode.Right);
                    }
                    else if(mouseState.MiddleButton == ButtonState.Pressed && previous_mouseState.MiddleButton != ButtonState.Pressed){
                        if(OnMouseClick != null)
                            OnMouseClick(MouseClickMode.Middle);
                    }
                }
            }
            
            previous_MouseOver = MouseOver;
            previous_NonAlphaMouseOver = NonAlphaMouseOver;
            previous_mouseState = mouseState;
        }
        /// <summary>
        /// Returns whether mouse is over the texture(alpha channel included, DOES NOT WORK WELL WITH 9-PATCH).
        /// </summary>
        /// <returns></returns>
        public bool GetMouseOver(){
            return MouseOver;
        }
        public bool GetNonAlphaMouseOver(){
            return NonAlphaMouseOver;
        }
        public void AddParent(UI parent){
            if(parent != null){
                Vector2 pos = position;
                if(this.parent != null)
                    this.parent.children.Remove(this);
                this.parent = parent;
                if(!parent.children.Contains(this))
                    parent.children.Add(this);
                this.position = pos;
            }
        }
        public void AddChild(UI Child){
            if(!children.Contains(Child)){
                Vector2 pos = Child.position;
                children.Add(Child);
                if(Child.parent != null)
                    Child.parent.children.Remove(Child);
                Child.parent = this;
                Child.position = pos;
            }
        }
        /// <summary>
        /// Return the parent's special vertexes' positions(in pixels).
        /// </summary>
        /// <param name="_graphics">The GraphicsDeviceManager used.</param>
        /// <param name="vector">The specified vertexes(0 is the center, 1 is the right edge).</param>
        /// <returns></returns>
        public Vector2 GetParentVertex(GraphicsDeviceManager _graphics, Vector2 vector){
            Vector2 target = new Vector2(0, 0);
            if(parent == null){
                target.X = (_graphics.PreferredBackBufferWidth/2)*(1+vector.X);
                target.Y = (_graphics.PreferredBackBufferHeight/2)*(1+vector.Y);
            }
            else {
                target.X = parent.position.X - parent.center.X + (parent.width/2)*(1+vector.X);
                target.Y = parent.position.Y - parent.center.Y + (parent.height/2)*(1+vector.Y);
            }
            return target;
        }
        /// <summary>
        /// Get the parent's bounds.
        /// </summary>
        /// <param name="_graphics">The GraphicsDeviceManager used.</param>
        /// <returns></returns>
        public Vector2 GetParentBounds(GraphicsDeviceManager _graphics){
            Vector2 target = new Vector2(0, 0);
            if(parent == null){
                target.X = _graphics.PreferredBackBufferWidth;
                target.Y = _graphics.PreferredBackBufferHeight;
            }
            else {
                target.X = parent.width;
                target.Y = parent.height;
            }
            return target;
        }
        
        //Events
        public delegate void MouseHoverHandler();
        public event MouseHoverHandler OnMouseEnter, OnMouseExit;
        public delegate void MouseClickHandler(MouseClickMode mcm);
        public event MouseClickHandler OnMouseClick;
        
        //constructors
        /// <summary>
        /// Empty ui constructor.
        /// </summary>
        public UI(){}
        /// <summary>
        /// Basic texture drawing.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos){
            this._spritebatch = spriteBatch;

            this.texture = texture;
            this.position = pos;
            this.width = texture.Width;
            this.height = texture.Height;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Basic texture drawing.
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
        /// Texture drawing.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="side">The side's width and height of the nine-patched ui.</param>
        /// <param name="ninePatches">NinePatches is on by default. Only use this to turn it off.</param>
        /// <param name="layer">The layer/depth to draw on.</param>
        /// <param name="angle">The angle of the ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 side, bool ninePatches = true, float layer = 0, float angle = 0){
            this._spritebatch = spriteBatch;
            
            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.side = side;
            this.ninePatches = false;
            this.layer = layer;
            this.angle = angle;

            Draw(_spritebatch);
        }
        /// <summary>
        /// Texture drawing.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the ui.</param>
        /// <param name="texture">The texture to be drawn.</param>
        /// <param name="pos">The position of the ui.</param>
        /// <param name="rect">The width and height of the ui.</param>
        /// <param name="side">The side's width and height of the nine-patched ui.</param>
        /// <param name="center">The point used to calculate the position and such(in pixels).</param>
        /// <param name="ninePatches">Use 9-patch or not.</param>
        /// <param name="layer">The layer/depth to draw on.</param>
        /// <param name="angle">The angle of the ui.</param>
        public UI(SpriteBatch spriteBatch, Texture2D texture, Vector2 pos, Vector2 rect, Vector2 side, Vector2 center, bool ninePatches = true, float layer = 0, float angle = 0){
            this._spritebatch = spriteBatch;
            
            this.texture = texture;
            this.position = pos;
            this.width = (int)rect.X;
            this.height = (int)rect.Y;
            this.side = side;
            this.center = center;
            this.ninePatches = ninePatches;
            this.layer = layer;
            this.angle = angle;

            Draw(_spritebatch);
        }
    }
}