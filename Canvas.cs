using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogametest{
    /// <summary>
    /// A class needed for ui drawing.
    /// </summary>
    public class Canvas{
        public List<UI> ui = new List<UI>();
        public void AddUI(UI _ui){
            if(!ui.Contains(_ui)){
                ui.Add(_ui);
            }
        }
        public void RemoveUI(UI _ui){
            if(ui.Contains(_ui)){
                ui.Remove(_ui);
            }
        }

        public void Update(){
            foreach(UI _ui in ui){
                _ui.Update();
            }
        }
        public void Draw(SpriteBatch _spriteBatch){
            foreach(UI i in ui){
                i.Draw(_spriteBatch);
            }
        }

        //Constructor
        public Canvas(){
            ui = new List<UI>();
        }
    }
}