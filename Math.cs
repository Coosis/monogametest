using System;
using Microsoft.Xna.Framework;
using MonoGame;

public class Math{
    public static float max(float v1, float v2){
        if(v1 >= v2)
            return v1;
        else return v2;
    }
    public static float min(float v1, float v2){
        if(v1 <= v2)
            return v1;
        else return v2;
    }
    public static bool WithinRange(float value, float min, float max)
    {
        return (value >= min) && (value <= max);
    }
    public static Vector2 Ratio(Vector2 v1, Vector2 v2){
        Vector2 vec = new Vector2(0, 0);
        if(v2.X != 0 && v2.Y != 0)
            vec = new Vector2(v1.X/v2.X, v1.Y/v2.Y);
        else{
            if(v2.X == 0)
                vec.Y = v1.Y/v2.Y;
            if(v2.Y == 0)
                vec.X = v1.X/v2.X;
        }
        return vec;
    }
    public static bool OnLine(Vector2 l1, Vector2 l2, Vector2 p){
        Vector2 l = vabs(l2 - l1);
        Vector2 _l = vabs(p - l1);
        l.Normalize();
        _l.Normalize();
        return l == _l && (Ratio(l2-l1, p-l1).X>=0) && (Ratio(l2-l1, p-l1).Y>=0);
    }
    public static float distance(Vector2 from, Vector2 to){
        return MathF.Sqrt(square((from.X-to.X))+square((from.Y-to.Y)));
    }
    public static float square(float x){
        return x*x;
    }
    public static bool CrossedLine(Vector2 m1, Vector2 m2, Vector2 n1, Vector2 n2){
        if(max(m1.X, m2.X)>=min(n1.X, n2.X) && min(m1.X, m2.X)<=max(n1.X, n2.X) 
        && max(m1.Y, m2.Y)>=min(n1.Y, n2.Y) && min(m1.Y, m2.Y)<=max(n1.Y, n2.Y)){
            if(((n1.X-m1.X)*(m2.Y-m1.Y)-(n1.Y-m1.Y)*(m2.X-m1.X))*((n2.X-m1.X)*(m2.Y-m1.Y)-(n2.Y-m1.Y)*(m2.X-m1.X))<=0 && 
                ((m1.X-n1.X)*(n2.Y-n1.Y)-(m1.Y-n1.Y)*(n2.X-n1.X))*((m2.X-n1.X)*(n2.Y-n1.Y)-(m2.Y-n1.Y)*(n2.X-n1.X))<=0)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
    public static Vector2 vabs(Vector2 v){
        Vector2 _v = v;
        if(_v.X < 0)
            _v.X *= -1;
        if(_v.Y < 0)
            _v.Y *= -1;
        return _v;
    }
}