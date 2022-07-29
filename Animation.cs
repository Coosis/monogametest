using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;

/// <summary>
/// Contains the texture and time information for animation displaying. 
/// </summary>
public class Animation{
    /// <summary>
    /// Whether the animation plays again when it finishes or not.
    /// </summary>
    public bool loop = true;
    /// <summary>
    /// The frames(in order) for the animation.
    /// </summary>
    public Texture2D[] textures;
    /// <summary>
    /// The time the corresponding frame lasts(in milliseconds).
    /// </summary>
    public float[] frameTime;
    /// <summary>
    /// Used for trimming texture into smaller parts of textures.
    /// </summary>
    /// <param name="source">The source textures.</param>
    /// <param name="start_index">The index to start from.</param>
    /// <param name="count">The quantity of textures to get.</param>
    /// <returns></returns>
    public static Texture2D[] Trim(Texture2D[] source, int start_index, int count){
        Texture2D[] result = new Texture2D[count];
        for(int i = 0; i < count; ++i){
            result[i] = source[start_index + i];
        }
        return result;
    }
    /// <summary>
    /// Creates an animation, making every frame last _frameTime
    /// </summary>
    /// <param name="_textures">The frames(in order) for the animation.</param>
    /// <param name="_frameTime">The time the corresponding frame lasts(in milliseconds).</param>
    public Animation(Texture2D[] _textures, float _frameTime){
        textures = _textures;
        frameTime = new float[_textures.Length];
        for(int i = 0; i < frameTime.Length; ++i){
            frameTime[i] = _frameTime;
        }
    }
    /// <summary>
    /// Creates an animation, a texture matches a frameTime
    /// </summary>
    /// <param name="_textures">The frames(in order) for the animation.</param>
    /// <param name="_frameTime">The time the corresponding frame lasts(in milliseconds).</param>
    public Animation(Texture2D[] _textures, float[] _frameTime){
        textures = _textures;
        frameTime = _frameTime;
    }
}