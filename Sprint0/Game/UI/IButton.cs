using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0;

public interface IButton
{
    void Draw(SpriteBatch spriteBatch);
    void Update();
    bool IsClicked();
    bool IsMouseOver();
}