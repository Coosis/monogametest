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
        xin = Math.WithinRange(pos_self.X, pos_target.X, pos_target.X + c.bounds.X);
        xendin = Math.WithinRange(pos_self.X + bounds.X, pos_target.X, pos_target.X + c.bounds.X);
        yin = Math.WithinRange(pos_self.Y, pos_target.Y, pos_target.Y + c.bounds.Y);
        yendin = Math.WithinRange(pos_self.Y + bounds.Y, pos_target.Y, pos_target.Y + c.bounds.Y);
        xoverlapped = 0;
        yoverlapped = 0;
        
        if((pos_self.X-pos_target.X)*((pos_self.X+bounds.X)-(pos_target.X+c.bounds.X)) < 0){
            XOverlap[c] = true;
            xoverlapped = Math.min(bounds.X, c.bounds.X);
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
            yoverlapped = Math.min(bounds.Y, c.bounds.Y);
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

public class Raycast{
    public static RayResult raycast(Vector2 p_start, Vector2 p_end){
        RayResult result = new RayResult();
        foreach(Collider c in Collider.colliders){
            Vector2 p1 = c.node.position + c.UpperLeft;
            Vector2 p2 = p1, p3 = p1, p4 = p1;
            p2.X += c.bounds.X;
            p3.Y += c.bounds.Y;
            p4 += c.bounds;

            if(Math.WithinRange(p_start.X, p1.X, p2.X)&&Math.WithinRange(p_start.Y, p1.Y, p3.Y))
            {
                result.hit = true;
                result.point = p_start;
                if(!result.colliders.Contains(c))
                    result.colliders.Add(c);
            }
            else{
                float[] distance = {-1, -1, -1, -1};
                bool side1crossed = Math.CrossedLine(p1, p2, p_start, p_end);
                bool side2crossed = Math.CrossedLine(p1, p3, p_start, p_end);
                bool side3crossed = Math.CrossedLine(p3, p4, p_start, p_end);
                bool side4crossed = Math.CrossedLine(p2, p4, p_start, p_end);
                float _x = 0, _y = 0;
                Vector2 vec;
                if(side1crossed){
                    _x = ((p1.X-p2.X)*(p_end.X*p_start.Y-p_start.X*p_end.Y)-(p_start.X-p_end.X)*(p2.X*p1.Y-p1.X*p2.Y))/((p1.X-p2.X)*(p_start.Y-p_end.Y)-(p_start.X-p_end.X)*(p1.Y-p2.Y));
                    _y = ((p1.Y-p2.Y)*(p_end.Y*p_start.X-p_start.Y*p_end.X)-(p_start.Y-p_end.Y)*(p2.Y*p1.X-p1.Y*p2.X))/((p1.Y-p2.Y)*(p_start.X-p_end.X)-(p_start.Y-p_end.Y)*(p1.X-p2.X));
                    vec = new Vector2(_x, _y);
                    distance[0] = Math.distance(p_start, vec);
                }  
                if(side2crossed){
                    _x = ((p1.X-p3.X)*(p_end.X*p_start.Y-p_start.X*p_end.Y)-(p_start.X-p_end.X)*(p3.X*p1.Y-p1.X*p3.Y))/((p1.X-p3.X)*(p_start.Y-p_end.Y)-(p_start.X-p_end.X)*(p1.Y-p3.Y));
                    _y = ((p1.Y-p3.Y)*(p_end.Y*p_start.X-p_start.Y*p_end.X)-(p_start.Y-p_end.Y)*(p3.Y*p1.X-p1.Y*p3.X))/((p1.Y-p3.Y)*(p_start.X-p_end.X)-(p_start.Y-p_end.Y)*(p1.X-p3.X));
                    vec = new Vector2(_x, _y);
                    distance[1] = Math.distance(p_start, vec);
                }
                if(side3crossed){
                    _x = ((p3.X-p4.X)*(p_end.X*p_start.Y-p_start.X*p_end.Y)-(p_start.X-p_end.X)*(p4.X*p3.Y-p3.X*p4.Y))/((p3.X-p4.X)*(p_start.Y-p_end.Y)-(p_start.X-p_end.X)*(p3.Y-p4.Y));
                    _y = ((p3.Y-p4.Y)*(p_end.Y*p_start.X-p_start.Y*p_end.X)-(p_start.Y-p_end.Y)*(p4.Y*p3.X-p3.Y*p4.X))/((p3.Y-p4.Y)*(p_start.X-p_end.X)-(p_start.Y-p_end.Y)*(p3.X-p4.X));
                    vec = new Vector2(_x, _y);
                    distance[2] = Math.distance(p_start, vec);
                }
                if(side4crossed){
                    _x = ((p2.X-p4.X)*(p_end.X*p_start.Y-p_start.X*p_end.Y)-(p_start.X-p_end.X)*(p4.X*p2.Y-p2.X*p4.Y))/((p2.X-p4.X)*(p_start.Y-p_end.Y)-(p_start.X-p_end.X)*(p2.Y-p4.Y));
                    _y = ((p2.Y-p4.Y)*(p_end.Y*p_start.X-p_start.Y*p_end.X)-(p_start.Y-p_end.Y)*(p4.Y*p2.X-p2.Y*p4.X))/((p2.Y-p4.Y)*(p_start.X-p_end.X)-(p_start.Y-p_end.Y)*(p2.X-p4.X));
                    vec = new Vector2(_x, _y);
                    distance[3] = Math.distance(p_start, vec);
                }
                if(side1crossed||side2crossed||side3crossed||side4crossed){
                    result.hit = true;
                    int index = -1;
                    float d = distance[0];
                    bool found = false;
                    for(int i = 0; i < 4; ++i){
                        if(distance[i] != -1){
                            if(!found){
                                d = distance[i];
                                found = true;
                                index = i;
                            }
                            else if(distance[i] <= d){
                                d = distance[i];
                                index = i;
                            }
                        }
                    }
                }
                else{
                    result.hit = false;
                }
            }
        }
        return result;
    }
}

public class RayResult{
    public List<Collider> colliders = new List<Collider>();
    public bool hit = false;
    public Vector2 point = new Vector2(0, 0);
}