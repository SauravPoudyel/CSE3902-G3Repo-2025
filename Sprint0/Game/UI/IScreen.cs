using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public interface IScreen
    {
        bool BlocksInput { get; }
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
