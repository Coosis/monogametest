using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;

public class PhysicalBody{
    /// <summary>
    /// The node this physicalBody is on.
    /// </summary>
    public Node node;
    public static List<PhysicalBody> physicalBodies = new List<PhysicalBody>();
    /// <summary>
    /// The total velocity of the body.
    /// </summary>
    /// <returns></returns>
    public Vector2 velocity = new Vector2(0, 0);
    /// <summary>
    /// The velocity created by force.
    /// </summary>
    /// <returns></returns>
    private Vector2 forceVelocity = new Vector2(0, 0);
    /// <summary>
    /// Other velocities, used for creating effects and others.
    /// </summary>
    /// <typeparam name="string">The name/tag of the velocity.</typeparam>
    /// <typeparam name="Vector2">The velocity.</typeparam>
    /// <returns></returns>
    public Dictionary<string, Vector2> externalVelocity = new Dictionary<string, Vector2>();
    /// <summary>
    /// The gravity.
    /// </summary>
    /// <returns></returns>
    public Vector2 gravity_acceleration = new Vector2(0, 10);
    /// <summary>
    /// The total force, combining externalForces and collideForces.
    /// </summary>
    /// <value></value>
    public Vector2 force{
        get { return _force; }
    }
    private Vector2 _force = new Vector2(0, 0);
    /// <summary>
    /// External forces.
    /// </summary>
    /// <typeparam name="string">The name/tag of the force.</typeparam>
    /// <typeparam name="Vector2">The force.</typeparam>
    /// <returns></returns>
    public Dictionary<string, Vector2> externalForce = new Dictionary<string, Vector2>();
    /// <summary>
    /// The forces other colliders give.
    /// </summary>
    /// <typeparam name="Collider">The name/tag of the collider.</typeparam>
    /// <typeparam name="Vector2">The force.</typeparam>
    /// <returns></returns>
    public Dictionary<Collider, Vector2> collideForce = new Dictionary<Collider, Vector2>();
    /// <summary>
    /// The forces given by this node's collider.
    /// </summary>
    /// <typeparam name="Collider">The name/tag of the collider.</typeparam>
    /// <typeparam name="Vector2">The force.</typeparam>
    /// <returns></returns>
    public Dictionary<Collider, Vector2> selfCollideForce = new Dictionary<Collider, Vector2>();
    public float mass{
        get { return _mass; }
        set { 
            if(value != 0)
                _mass = value;
        }
    }
    private float _mass = 1;
    /// <summary>
    /// Updates the physicalBody.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="c"></param>
    public void Update(GameTime gameTime){
        Vector2 gravity = gravity_acceleration * mass;
        Vector2 _collideForce = new Vector2(0, 0);
        Vector2 _selfCollideForce = new Vector2(0, 0);
        Vector2 _externalVelocity = new Vector2(0, 0);
        //force
        _force = new Vector2(0, 0);
        foreach(Vector2 v in externalForce.Values)
            _force += v;
        foreach(Vector2 v in collideForce.Values)
            _collideForce += v;
        foreach(Vector2 v in selfCollideForce.Values)
            _selfCollideForce += v;
        _force += gravity;
        
        forceVelocity += gameTime.ElapsedGameTime.Milliseconds/1000f*(force+_collideForce+_selfCollideForce)/mass;
        
        //velocity
        foreach(Vector2 v in externalVelocity.Values)
            _externalVelocity += v;
        velocity = forceVelocity + _externalVelocity;
    }
    public void Move(){
        node.position += velocity;
    }
    //Velocity manipulation
    public void SetExternalVelocity(string name, Vector2 _velocity){
        if(!externalVelocity.ContainsKey(name))
            externalVelocity.Add(name, Vector2.Zero);
        externalVelocity[name] = _velocity;
    }
    public void ClearExternalVelocity(string name){
        if(externalVelocity.ContainsKey(name))
            externalVelocity.Remove(name);
    }
    public void ClearAllExternalVelocity()
    {
        externalVelocity = new Dictionary<string, Vector2>();
    }
    //Force manipulation
    public void SetExternalForce(string name, Vector2 _force){
        if(!externalForce.ContainsKey(name))
            externalForce.Add(name, Vector2.Zero);
        externalForce[name] = _force;
    }
    public void ClearExternalForce(string name){
        if(externalForce.ContainsKey(name))
            externalForce.Remove(name);
    }
    public void ClearAllExternalForce()
    {
        externalForce = new Dictionary<string, Vector2>();
    }
    public void SetCollideForce(Collider c, Vector2 _velocity){
        if(!collideForce.ContainsKey(c))
            collideForce.Add(c, Vector2.Zero);
        collideForce[c] = _velocity;
    }
    public void ClearCollideForce(Collider c){
        if(collideForce.ContainsKey(c))
            collideForce.Remove(c);
    }
    public void SetSelfCollideForce(Collider c, Vector2 _velocity){
        if(!selfCollideForce.ContainsKey(c))
            selfCollideForce.Add(c, Vector2.Zero);
        selfCollideForce[c] = _velocity;
    }
    public void ClearSelfCollideForce(Collider c){
        if(selfCollideForce.ContainsKey(c))
            selfCollideForce.Remove(c);
    }
    //Constructor
    public PhysicalBody(Node node){
        this.node = node;
        physicalBodies.Add(this);
        velocity = new Vector2(0, 0);
        forceVelocity = new Vector2(0, 0);
        externalVelocity = new Dictionary<string, Vector2>();
        _force = new Vector2(0, 0);
        externalForce = new Dictionary<string, Vector2>();
        collideForce = new Dictionary<Collider, Vector2>();
        selfCollideForce = new Dictionary<Collider, Vector2>();
    }
}