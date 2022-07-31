using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;

public class Node{
    public int NodeID;
    public string NodeTag = "";
    public static List<Node> NodePool = new List<Node>();
    public Texture2D texture;
    /// <summary>
    /// The position of the node.
    /// </summary>
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

            foreach(Node node in children){
                node.position += position;
            }
        }
    }
    protected Vector2 _position = new Vector2(0, 0);
    public Vector2 center = new Vector2(0, 0);
    public int width = 0, height = 0, depth = 0;
    public float angle = 0, layer = 0;
    public Color color = new Color(255, 255, 255, 255);
    public SpriteEffects flip = SpriteEffects.None;
    
    //special classes that enables the node to do great things
    public Animator animator;
    public Collider collider;
    public PhysicalBody physicalBody;

    //node tree properties
    public Node parent;
    public List<Node> children = new List<Node>();

    /// <summary>
    /// Update the node.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime){
        animator?.Update(gameTime);
        physicalBody?.Update(gameTime);
        collider?.Update(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch){
        spriteBatch.Begin(samplerState: SamplerState.PointWrap);
        spriteBatch.Draw(texture, new Rectangle((int)(position.X - center.X), (int)(position.Y - center.Y), width, height), new Rectangle(0, 0, texture.Width, texture.Height), color, angle, center, flip, depth);
        spriteBatch.End();
    }
    public void SetBoundToTexture(int multiplier){
        width = texture.Width * multiplier;
        height = texture.Height * multiplier;
    }
    public void SetParent(Node _parent){
        Vector2 pos = position;
        if(parent != null)
            parent.children.Remove(this);
        parent = _parent;
        if(!parent.children.Contains(this))
            parent.children.Add(this);
        this.position = pos;
    }
    public void AddChild(Node Child){
        if(!children.Contains(Child)){
            Vector2 pos = Child.position;
            children.Add(Child);
            if(Child.parent != null)
                 Child.parent.children.Remove(Child);
            Child.parent = this;
            Child.position = pos;
        }
    }
    public void AddAnimator(){
        animator = new Animator(this);
    }
    public void AddCollider(Vector2 Bounds){
        collider = new Collider(this, Bounds);
    }
    public void AddPhysicalBody(){
        physicalBody = new PhysicalBody(this);
    }
    public Node(){
        if(!NodePool.Contains(this))
            NodePool.Add(this);
        NodeID = NodePool.IndexOf(this);
        children = new List<Node>();
    }
}