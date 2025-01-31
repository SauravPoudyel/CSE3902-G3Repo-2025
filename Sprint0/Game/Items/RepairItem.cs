using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using System;
using System.Collections.Generic;

public class RepairItem : Item {
        public override void LoadContent(ContentManager content) {
            if (sprite == null)
            {
                throw new NullReferenceException("Sprite is not initialized. Call SetSprite before LoadContent.");
            }
            sprite.LoadContent(content, "2DTanksSprites", 464, 651, 110, 100, 1);
        }
}