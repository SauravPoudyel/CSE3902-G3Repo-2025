using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class EventManager
    {
        private Dictionary<string, ICommand> commandMap;
        private List<CommandRequest> commandRequests;
        private GameManager gameManager;
        public EventManager(Game1 game, GameManager gameManager)
        {
            this.gameManager = gameManager;

            commandMap = new Dictionary<string, ICommand>
            {
                {"Quit", new GameCommands.QuitCommand()},
                {"Reset", new GameCommands.ResetCommand()},
                {"StartGame", new GameCommands.StartGameCommand()},
                {"ShowPauseMenu", new GameCommands.ShowPauseMenuCommand()},
                {"IncreaseLevel", new GameCommands.IncreaseLevelCommand()},
                {"DecreaseLevel", new GameCommands.DecreaseLevelCommand()},
                {"SetLevel", new GameCommands.SetLevelCommand()},

                {"Static", new GraphicCommands.DisplayStaticGameCommand()},
                {"Animated", new GraphicCommands.DisplayAnimatedGameCommand()},
                {"SetSprite", new GraphicCommands.SetSpriteCommand()},
                {"CycleBlockPrev", new GraphicCommands.CycleBlockPrevCommand()},
                {"CycleBlockNext", new GraphicCommands.CycleBlockNextCommand()},
                {"CycleItemPrev", new GraphicCommands.CycleItemPrevCommand()},
                {"CycleItemNext", new GraphicCommands.CycleItemNextCommand()},
                {"CycleEnemyPrev", new GraphicCommands.CycleEnemyPrevCommand()},
                {"CycleEnemyNext", new GraphicCommands.CycleEnemyNextCommand()},
                {"UpdateCannon", new GraphicCommands.UpdateCannonCommand()},
                {"SpawnEffect", new GraphicCommands.SpawnEffectCommand()},

                {"Move", new MovementCommands.MoveCommand()},
                {"MoveTo", new MovementCommands.MoveCommand()},
                {"ApplyFriction", new MovementCommands.ApplyFrictionCommand()},
                {"StopMove", new MovementCommands.MoveCommand()},

                {"CreateProjectile", new ActionCommands.CreateProjectileCommand()},
                {"CreateEntity", new ActionCommands.CreateEntityCommand()},
                {"DestroyEntity", new ActionCommands.DestroyEntityCommand()},
                {"PlayerAction", new ActionCommands.PlayerActionCommand()},
                {"RequestPlayerPosition", new ActionCommands.RequestPlayerPositionCommand() },
                {"Damage", new ActionCommands.DamageCommand()},
                {"HealRadius", new ActionCommands.HealRadiusCommand()},

                {"CollisionPush", new CollisionCommands.CollisionPushCommand()},
                {"CollisionStop", new CollisionCommands.CollisionStopCommand()},
                {"CollisionPickUp", new CollisionCommands.CollisionPickUpCommand()},
                {"CollisionHurt", new CollisionCommands.CollisionHurtCommand()},
                {"CollisionProjectileDestroy", new CollisionCommands.CollisionProjectileDestroyCommand()},
                {"CollisionProjectileReflect", new CollisionCommands.CollisionProjectileReflectCommand()},
                {"DestroyFlammableCommand", new CollisionCommands.DestroyFlammableCommand()},
                {"AutoDestroy", new ActionCommands.AutoDestroyCommand()}

            };

            commandRequests = new List<CommandRequest>();
        }

        public void CollectCommandRequests(Queue<CommandRequest> commandQueue)
        {
            while (commandQueue.Count > 0)
            {
                var commandRequest = commandQueue.Dequeue();
                commandRequest.Parameters.Add("gameManager", gameManager); 
                commandRequests.Add(commandRequest);
            }
        }

        public void ProcessCommandRequests()
        {
            foreach (var request in commandRequests)
            {
                if (!request.Parameters.ContainsKey("gameManager"))
                {
                    request.Parameters.Add("gameManager", this.gameManager);
                }
                ExecuteCommand(request.CommandKey, request.Parameters);
            }
            commandRequests.Clear();
        }

        public void ExecuteCommand(string commandKey, Dictionary<string, object> parameters)
        {
            if (commandMap.ContainsKey(commandKey))
            {
                commandMap[commandKey].Execute(parameters);
            }
            else
            {
                System.Console.WriteLine("Invalid Request for " + commandKey + "; check commandKey and or parameters entered");
            }
        }
    }
}
