using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Sprint0
{
    public class GameManager
    {
        private Dictionary<string, ICommand> commandMap;
        private Dictionary<string, Entity> entities;
        private List<CommandRequest> commandRequests;
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
                {"SetSprite", new GraphicCommands.SetSpriteCommand()},
                {"CycleBlockPrev", new GraphicCommands.CycleBlockPrevCommand()},
                {"CycleBlockNext", new GraphicCommands.CycleBlockNextCommand()},
                {"CycleItemPrev", new GraphicCommands.CycleItemPrevCommand()},
                {"CycleItemNext", new GraphicCommands.CycleItemNextCommand()},
                {"CycleEnemyPrev", new GraphicCommands.CycleEnemyPrevCommand()},
                {"CycleEnemyNext", new GraphicCommands.CycleEnemyNextCommand()},

                {"Move", new MovementCommands.MoveCommand()},
                {"StopMove", new MovementCommands.MoveCommand()},

                {"Attack", new ActionCommands.AttackCommand()},
                {"CreateEntity", new ActionCommands.CreateEntityCommand()}, 
                {"UseItem1", new ActionCommands.UseItemCommand(1)},
                {"UseItem2", new ActionCommands.UseItemCommand(2)},
                {"Damage", new ActionCommands.DamageCommand()}
            };

            entities = new Dictionary<string, Entity>(); 
            physicsManager = new PhysicsManager();
            spriteManager = new SpriteManager();
            commandRequests = new List<CommandRequest>(); 
        }

        public ContentManager GetContent()
        {
            return content; 
        }

        public Dictionary<string, Entity> GetEntities()
        {
            return entities; 
        }

        public Entity GetEntity(string entityKey)
        {
            return entities[entityKey]; 
        }

        public void LoadContent(ContentManager contentManager)
        {
            content = contentManager;
            InitializeEntities();
        }
        private void InitializeEntities()
        {
            Entity player = new Player(content);
            player.SetPosition(new Vector2(Globals.SCREENWIDTH/2, 300)); 
            entities.Add("player", player);

            Entity mob = new Mob(content);
            mob.SetPosition(new Vector2(Globals.SCREENWIDTH/2 + 100, 400)); 
            entities.Add("mob", mob);

            PickupItem pickupItem = new PickupItem(content);
            pickupItem.SetPosition(new Vector2(Globals.SCREENWIDTH/2 + 50, 400)); 
            entities.Add("pickupItem", pickupItem);

            Blocks blocks = new Blocks(content);
            blocks.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 256, 400));
            entities.Add("blocks", blocks);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                entity.Update(gameTime);
                CollectCommandQueues(entity.GetCommandQueue());
            }
            physicsManager.Update(gameTime, entities);
            spriteManager.Update(gameTime);
            ProcessCommandRequests();
        }

        private void CollectCommandQueues(Queue<CommandRequest> commandQueue)
        {
            while (commandQueue.Count > 0)
            {
                var commandRequest = commandQueue.Dequeue();
                commandRequest.Parameters.Add("gameManager", this);
                this.commandRequests.Add(commandRequest);
            }
        }

        private void ProcessCommandRequests()
        {
            foreach (var command in commandRequests)
            {
                ExecuteCommand(command.CommandKey, command.Parameters);
            }
            commandRequests.Clear();
        }

        public void ExecuteCommand(string commandKey, Dictionary<string, object> parameters)
        {
            if (commandMap.ContainsKey(commandKey))
            {
                commandMap[commandKey].Execute(parameters);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities.Values)
            {
                entity.Draw(spriteBatch);
            }

            spriteManager.Draw(spriteBatch);
        }

        public void SetSprite(ISprite sprite)
        {
            spriteManager.SetSprite(sprite);
        }
    }
}