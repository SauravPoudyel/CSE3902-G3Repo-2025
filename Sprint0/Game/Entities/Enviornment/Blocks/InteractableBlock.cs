using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class InteractableBlock : BaseBlock, IRigid, IObtuse, IInteractable
    {
        private bool canInteract;
        private EntityKeys.BlockType blockType;
        public bool CanInteract 
        { 
            get { return canInteract; } 
            private set { canInteract = value; } 
        }
        public InteractableBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime = 0.3f)
        {
            LoadBlockContent(content, blockType);
            SetFrameTime(frameTime);
            this.blockType = blockType;
        }

        public override void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType)
        {
            animatedSprite = new AnimatedSprite(frameTime);
            var (x, y, width, height, scale) = GetSpriteCoords(blockType);
            Scale = scale; // this gets aplied in draw
            animatedSprite.LoadContent(content, "TDTanksAllSprites", x, y, width, height, 1);
            spriteWidth = width;
            spriteHeight = height;
            UpdateBounds();
        }
        public void IsInRange(Vector2 playerPosition)
        {
            float interactionRange = 300f;

            float distance = Vector2.Distance(playerPosition, this.position);
            if (distance <= interactionRange)
                CanInteract = true;
            else
                CanInteract = false;
        }

        public void Interact() {
            Dictionary<string, object> parameters2 = new Dictionary<string, object>{};

            switch (blockType)
            {
                case EntityKeys.BlockType.Shop:
                    if(Globals.PlayerData.GetInt("HasShopKeys") == 1)
                        commandQueue.Enqueue(new CommandRequest("OpenShop", parameters2));
                    break;
                default:
                    throw new System.ArgumentException($"Invalid BlockType for interaction: {blockType}");
            }
        }

        private (int x, int y, int width, int height, float scale) GetSpriteCoords(EntityKeys.BlockType blockType)
        {
            return blockType switch
            {
                EntityKeys.BlockType.Shop => (766, 1768, 104, 96, 1.5f),
            
                _ => throw new System.ArgumentException($"Invalid BlockSpriteKey: {blockType}")
            };
        }
    }
}
