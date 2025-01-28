/*
    Generic object interface
    Every other game object will inherit from this interface
*/
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

interface IObject{

    public Vector2 Position {get;}
    public Rectangle CollisionBox {get;}

    public void Move(Vector2 displacement);

    public void Draw(SpriteBatch spriteBatch);

}