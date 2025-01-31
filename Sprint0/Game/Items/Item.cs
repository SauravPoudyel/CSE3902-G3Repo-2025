using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using System;
using System.Collections.Generic;

public abstract class Item : Entity {
        public override Vector2 GetVelocity() { 
                // Items do not move so do not have velocity
                return new Vector2(0,0);
        }
        public override void SetVelocity(Vector2 velocity) { 
                // Items do not move so do not have velocity
        }
}