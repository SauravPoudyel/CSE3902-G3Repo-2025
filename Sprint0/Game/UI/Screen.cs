using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public interface IScreen
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void HandleClick(Point clickLocation); 
    }
}
