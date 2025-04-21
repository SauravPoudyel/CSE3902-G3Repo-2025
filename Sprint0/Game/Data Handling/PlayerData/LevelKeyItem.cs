using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class KeyItem
    {
        private bool hasKeyItem;
        private bool dropped;
        private EntityKeys.ItemType type;

        public bool Exists => hasKeyItem;
        public bool Dropped => dropped;
        public EntityKeys.ItemType Type => type;

        public void Set(EntityKeys.ItemType itemType)
        {
            type = itemType;
            hasKeyItem = true;
            dropped = false;
        }

        public void TryDrop(ContentManager content, bool levelComplete, LevelEntities entityManager)
        {
            if (!hasKeyItem || dropped || !levelComplete)
                return;

            entityManager.AddItem(content, new Vector2(8, 5), type); // Consider extracting Vector2(8,5) as a constant
            dropped = true;
        }
    }
}
