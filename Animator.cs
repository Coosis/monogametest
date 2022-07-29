using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;

public class Animator{
    /// <summary>
    /// The node the animator is on.
    /// </summary>
    protected Node node;
    /// <summary>
    /// Different animations, sorted by name.
    /// </summary>
    public Dictionary<string, Animation> AnimationSet;
    /// <summary>
    /// The current playing animation.
    /// </summary>
    protected Animation current;
    /// <summary>
    /// A timer for changing frames(in milliseconds).
    /// </summary>
    protected float Timer;
    /// <summary>
    /// The index of the frame in the current animation.
    /// </summary>
    protected int index{
        get{ return _index; }
        set{ 
            _index = value; 
            node.texture = current.textures[_index];
        }
    }
    protected int _index;
    /// <summary>
    /// Whether the animation is playing or not.
    /// </summary>
    protected bool play = true;
    /// <summary>
    /// Update the animator.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="_node">The node this animator is on.</param>
    public void Update(GameTime gameTime){
        if(play){
            Timer += gameTime.ElapsedGameTime.Milliseconds;
            if(Timer >= current.frameTime[index]){
                if(index < current.textures.Length - 1){
                    index += 1;
                }
                else{
                    if(current.loop)
                        index = 0;
                }
                Timer = 0;
            }
        }
    }
    /// <summary>
    /// Set the current animation, by name.
    /// </summary>
    /// <param name="name">The name of the animation.</param>
    public void SetAnimation(string name){
        current = AnimationSet[name];
        index = 0;
        Timer = 0;
    }
    /// <summary>
    /// Play the animation.
    /// </summary>
    public void Play(){
        play = true;
    }
    /// <summary>
    /// Pause the animation.
    /// </summary>
    public void Pause(){
        play = false;
    }
    /// <summary>
    /// Set the animations for animator.
    /// </summary>
    /// <param name="_AnimationSet">The animation set to be used.</param>
    public void SetAnimationSet(Dictionary<string, Animation> _AnimationSet){
        AnimationSet = _AnimationSet;
    }
    /// <param name="node">The node this animator is attached to.</param>
    public Animator(Node node){
        AnimationSet = new Dictionary<string, Animation>();
        this.node = node;
    }
}