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

    public ISprite sprite{get;}

    public void Update(GameTime gameTime);

    public void Draw(SpriteBatch spriteBatch);

    public void Destroy();

}