using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Managers;
using System.Collections.Generic;

namespace Sprint0
{
    public class GameManager
    {
        private Dictionary<string, ICommand> commandMap;
        private List<Entity> entities;
        private PhysicsManager physicsManager;
        private SpriteManager spriteManager;
        private ContentManager content;

        public GameManager(Game1 game)
        {
            commandMap = new Dictionary<string, ICommand>
            {
                {"Quit", new GameCommands.QuitCommand(game)},
                {"Reset", new GameCommands.ResetCommand(game)}, 

                {"Static", new GraphicCommands.DisplayStaticGameCommand()},
                {"Animated", new GraphicCommands.DisplayAnimatedGameCommand()},
                {"CycleBlockPrev", new GraphicCommands.CycleBlockPrevCommand()},
                {"CycleBlockNext", new GraphicCommands.CycleBlockNextCommand()},
                {"CycleItemPrev", new GraphicCommands.CycleItemPrevCommand()},
                {"CycleItemNext", new GraphicCommands.CycleItemNextCommand()},
                {"CycleEnemyPrev", new GraphicCommands.CycleEnemyPrevCommand()},
                {"CycleEnemyNext", new GraphicCommands.CycleEnemyNextCommand()},

                {"MoveUp", new MovementCommands.MoveUpCommand()},
                {"MoveDown", new MovementCommands.MoveDownCommand()},
                {"MoveLeft", new MovementCommands.MoveLeftCommand()},
                {"MoveRight", new MovementCommands.MoveRightCommand()},

                {"Attack", new ActionCommands.AttackCommand()},
                {"UseItem1", new ActionCommands.UseItemCommand(1)},
                {"UseItem2", new ActionCommands.UseItemCommand(2)},
                {"Damage", new ActionCommands.DamageCommand()}
            };

            entities = new List<Entity>();
            physicsManager = new PhysicsManager();
            spriteManager = new SpriteManager();
        }

        public ContentManager GetContent()
        {
            return content; 
        }

        public List<Entity> GetEntities()
        {
            return entities; 
        }

        public Entity GetEntity(int entityPos)
        {
            return entities[entityPos]; 
        }

        public void LoadContent(ContentManager contentManager)
        {
            content = contentManager;
            InitializeEntities();
        }
        private void InitializeEntities()
        {
            Entity player = new Player();
            player.SetPosition(new Vector2(Globals.SCREENWIDTH/2, 300)); 
            player.SetSprite(new AnimatedSprite(10f));
            player.LoadContent(content, "LinkSpriteSheet", 0, 0, 64, 64, 1); 
            entities.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }

            physicsManager.Update(gameTime, entities);
            spriteManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                entity.Draw(spriteBatch);
            }

            spriteManager.Draw(spriteBatch);
        }

        public void ExecuteCommand(string commandKey, Dictionary<string, object> parameters)
        {
            if (commandMap.ContainsKey(commandKey))
            {
                commandMap[commandKey].Execute(parameters);
            }
        }

        public void SetSprite(ISprite sprite)
        {
            spriteManager.SetSprite(sprite);
        }
    }
}