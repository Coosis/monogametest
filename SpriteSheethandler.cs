using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;

/// <summary>
/// A class used to cut a sprite sheet into multiple sprites.
/// </summary>
public class SpriteSheet{
    /// <summary>
    /// The sprite sheet to cut.
    /// </summary>
    public Texture2D sheet;
    /// <summary>
    /// The desired cutting width and height, starting from top left.
    /// </summary>
    public Vector2 cut_bounds;
    /// <summary>
    /// The sprites cut.
    /// </summary>
    public List<Texture2D> cut_texture;
    /// <summary>
    /// Cut the sheet and store them in cut_texture.
    /// </summary>
    /// <param name="device">The GraphicsDevice used.</param>
    /// <returns>cut_texture</returns>
    public List<Texture2D> Cut(GraphicsDevice device){
        cut_texture = new List<Texture2D>();

        int cut_bounds_x = (int)cut_bounds.X;
        int cut_bounds_y = (int)cut_bounds.Y;
        int extra_x = sheet.Width % cut_bounds_x;
        int extra_y = sheet.Height % cut_bounds_y;
        int line = (sheet.Height - extra_y) / cut_bounds_y;
        int row = (sheet.Width - extra_x) / cut_bounds_x;

        Color[] sheet_data = new Color[sheet.Width * sheet.Height];
        sheet.GetData<Color>(sheet_data);

        Color[] data = new Color[cut_bounds_x * cut_bounds_y];
        Texture2D target;
        for(int i = 0; i < line; ++i){
            for(int j = 0; j < row; ++j){
                for(int k = 0; k < cut_bounds_y; ++k){
                    int index = i*cut_bounds_y*sheet.Width + (j*cut_bounds_x) + (k*sheet.Width);
                    for(int l = 0; l < cut_bounds_x; ++l){
                        data[k*cut_bounds_x + l] = sheet_data[index + l];
                    }
                }
                target = new Texture2D(device, cut_bounds_x, cut_bounds_y);
                target.SetData<Color>(data);
                cut_texture.Add(target);
            }
        }
        return cut_texture;
    }
    //constructor
    public SpriteSheet(Texture2D _texture){
        sheet = _texture;
    }
}