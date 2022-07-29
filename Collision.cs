using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;

public class Collider{
    /// <summary>
    /// The node this collider is on.
    /// </summary>
    public Node node;
    /// <summary>
    /// All the colliders in the game.
    /// </summary>
    public static List<Collider> colliders{
        get { return _colliders; }
    }
    private static List<Collider> _colliders = new List<Collider>();
    /// <summary>
    /// The collision layer of this collider.
    /// </summary>
    public int collision_layer = 0;
    /// <summary>
    /// A property used to describe the ability to give force based on strain. The greater the value, the further a node needs to go in order to stay still.
    /// </summary>
    public float elasticity = 0;
    /// <summary>
    /// The position of the upper-left point of this collider(relative to the node's center, in pixels).
    /// </summary>
    public Vector2 UpperLeft = new Vector2(0, 0);
    /// <summary>
    /// The width and height of the collider.
    /// </summary>
    public Vector2 bounds = new Vector2(0, 0);
    /// <summary>
    /// Whether a specified collider is overlaped in x/y axis.
    /// </summary>
    public Dictionary<Collider, bool> XOverlap, YOverlap;
    /// <summary>
    /// Updates the collider.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime){
        foreach(Collider c in colliders){
            if(c.collision_layer == this.collision_layer && c != this){
                XOverlap[c] = false;
                YOverlap[c] = false;
                //get the upper-left point's position(in pixels)
                Vector2 pos_self = node.position - node.center + UpperLeft;
                Vector2 pos_target = c.node.position - c.node.center + c.UpperLeft;
                float xoverlapped, yoverlapped;
                calculateOverlap(pos_self, pos_target, c, out xoverlapped, out yoverlapped);
                if(XOverlap[c] && YOverlap[c]){
                    Vector2 dir = (pos_target+(c.bounds/2))-(pos_self+(bounds/2));
                    if(xoverlapped <= yoverlapped)
                        dir.Y = 0;
                    else dir.X = 0;
                    dir.Normalize();

                    dir.X *= xoverlapped;
                    dir.Y *= yoverlapped;

                    if(node.physicalBody == null){
                        c.node.position += dir;
                    }

                    PhysicsCollide(c, dir);
                }
                else{
                    //Take off the force when not colliding.
                    c.node.physicalBody?.ClearCollideForce(this);
                    node.physicalBody?.ClearSelfCollideForce(this);
                }
            }
        }
    }
    /// <summary>
    /// Alters the physical properties for the target collider.
    /// </summary>
    /// <param name="c">The target collider.</param>
    /// <param name="normal">A vector, perpendicular to the colliding surface.</param>
    private void PhysicsCollide(Collider c, Vector2 normal){
        if(c.node.physicalBody != null){
            Vector2 counterForce = c.node.physicalBody.force;
            counterForce.X = counterForce.X*normal.X/(1+elasticity);
            counterForce.Y = counterForce.Y*normal.Y/(1+elasticity);
            c.node.physicalBody.SetCollideForce(this, counterForce);
            if(node.physicalBody != null)
                node.physicalBody.SetSelfCollideForce(this, -counterForce);
            /*
            if(node.staticNode){
                normal.Normalize();
                Vector2 velocity = c.node.physicalBody.velocity;
                if(normal.X == 0){
                    velocity.Y = 0;
                }
                if(normal.Y == 0){
                    velocity.X = 0;
                }
                c.node.physicalBody.velocity = velocity;
            }*/
        }
    }
    private float min(float v1, float v2){
        if(v1 <= v2)
            return v1;
        else return v2;
    }
    /// <summary>
    /// Check whether a value is within a given range.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns></returns>
    private bool WithinRange(float value, float min, float max)
    {
        return (value >= min) && (value <= max);
    }
    /// <summary>
    /// Calculate the x/y overlap(in pixels)
    /// </summary>
    /// <param name="pos_self">The upper-left point's position of this collider.</param>
    /// <param name="pos_target">The upper-left point's position of the target collider.</param>
    /// <param name="c">The target collider.</param>
    /// <param name="xoverlapped">X-overlap.</param>
    /// <param name="yoverlapped">Y-overlap.</param>
    private void calculateOverlap(Vector2 pos_self, Vector2 pos_target, Collider c, out float xoverlapped, out float yoverlapped){
        bool xin, xendin, yin, yendin;
        xin = WithinRange(pos_self.X, pos_target.X, pos_target.X + c.bounds.X);
        xendin = WithinRange(pos_self.X + bounds.X, pos_target.X, pos_target.X + c.bounds.X);
        yin = WithinRange(pos_self.Y, pos_target.Y, pos_target.Y + c.bounds.Y);
        yendin = WithinRange(pos_self.Y + bounds.Y, pos_target.Y, pos_target.Y + c.bounds.Y);
        xoverlapped = 0;
        yoverlapped = 0;
        
        if((pos_self.X-pos_target.X)*((pos_self.X+bounds.X)-(pos_target.X+c.bounds.X)) < 0){
            XOverlap[c] = true;
            xoverlapped = min(bounds.X, c.bounds.X);
        }
        else if(xin || xendin){
            XOverlap[c] = true;
            if(xin)
                xoverlapped = (pos_target.X + c.bounds.X - pos_self.X);
            else if(xendin)
                xoverlapped = (pos_self.X + bounds.X - pos_target.X);
        }
        if((pos_self.Y-pos_target.Y)*((pos_self.Y+bounds.Y)-(pos_target.Y+c.bounds.Y)) < 0){
            YOverlap[c] = true;
            yoverlapped = min(bounds.Y, c.bounds.Y);
        }
        else if(yin || yendin){
            YOverlap[c] = true;
            if(yin)
                yoverlapped = (pos_target.Y + c.bounds.Y - pos_self.Y);
            else if(yendin)
                yoverlapped = (pos_self.Y + bounds.Y - pos_target.Y);
        }
    }
    //Constructor
    public Collider(Node _node, Vector2 _bounds){
        node = _node;
        if(!colliders.Contains(this))
            colliders.Add(this);
        XOverlap = new Dictionary<Collider, bool>();
        YOverlap = new Dictionary<Collider, bool>();
        foreach(Collider c in colliders){
            if(!c.XOverlap.ContainsKey(this)){
                c.XOverlap.Add(this, false);
            }
            if(!c.YOverlap.ContainsKey(this)){
                c.YOverlap.Add(this, false);
            }
        }
        bounds = _bounds;
    }
}