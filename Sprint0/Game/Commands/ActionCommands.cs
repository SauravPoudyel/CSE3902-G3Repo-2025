using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ActionCommands
    {

        public class PlayerActionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                    parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("actionType") && parameters["actionType"] is string actionType)
                {
                    switch (actionType)
                    {
                        case "fire":
                            if (!player.CanFire) return;
                            player.SetProjectileType("Default");
                            player.FireProjectile();
                            break;
                        case "item1":
                        case "item2":
                        case "item3":
                        case "item4":
                        case "item5":
                            int slotIndex = actionType switch
                            {
                                "item1" => 0,
                                "item2" => 1,
                                "item3" => 2,
                                "item4" => 3,
                                "item5" => 4,
                                _ => 0
                            };

                            if (!player.CanFire) return;

                            InventorySlot slot = gameManager.playerInventory.inventorySlots[slotIndex];
                            string projectileType = slot.ProjectileType;
                            if (slot.AmmoCount <= 0) return;

                            // Decrement ammo based on the projectile type.
                            switch (projectileType)
                            {
                                case "Sniper":
                                    Globals.PlayerData.TemporaryAmmoSniper--;
                                    break;
                                case "Rocket":
                                    Globals.PlayerData.TemporaryAmmoRocket--;
                                    break;
                                case "Shotgun":
                                    Globals.PlayerData.TemporaryAmmoShotgun--;
                                    break;
                                case "Mine":
                                    Globals.PlayerData.TemporaryAmmoMine--;
                                    break;
                                case "Teleporter":
                                    Globals.PlayerData.TemporaryAmmoTeleporter--;
                                    break;
                                default:
                                    Globals.PlayerData.TemporaryAmmoDefault--;
                                    break;
                            }

                            player.SetProjectileType(projectileType);
                            player.FireProjectile();

                            if (slot.AmmoCount <= 0)
                            {
                                gameManager.playerInventory.ShiftEmptySlot(slotIndex);
                            }
                            break;
                        default:
                            System.Console.WriteLine("Error: invalid actionType.");
                            break;
                    }
                }
            }
        }

    
        public class DamageCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    // Implement the logic for the entity to take damage
                    if (gameManager.GetEntity("player") is Character player)
                    {
                        player.ChangeHealth(-20);
                    }
                }
            }
        }
        
    public class CreateProjectileCommand : ICommand
    {
        public void Execute(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                parameters.ContainsKey("projectileType") && parameters["projectileType"] is string projectileType &&
                parameters.ContainsKey("spawnPosition") && parameters["spawnPosition"] is Vector2 spawnPosition &&
                parameters.ContainsKey("cannonRotation") && parameters["cannonRotation"] is float cannonRotation &&
                parameters.ContainsKey("owner") && parameters["owner"] is Character owner)
            {
                int numberOfProjectiles = 1;
                float spreadAngle = 0f;
                float speedModifer = 0f;
                if (parameters.ContainsKey("numberOfProjectiles") && parameters["numberOfProjectiles"] is int num)
                    numberOfProjectiles = num;
                if (parameters.ContainsKey("spreadAngle") && parameters["spreadAngle"] is float angle)
                    spreadAngle = angle;
                if (parameters.ContainsKey("speedModifier") && parameters["speedModifier"] is float speedMod)
                    speedModifer = speedMod;
                ProjectileFactory.CalculateProjectiles(owner, projectileType, spawnPosition, cannonRotation, spreadAngle, numberOfProjectiles, speedModifer);
                ProjectileFactory.SpawnProjectiles(gameManager, owner);
            }
        }
    }

        public class CreateEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("create") && parameters["create"] is Projectile entity &&
                    parameters.ContainsKey("entityName") && parameters["entityName"] is string entityName &&
                    parameters.ContainsKey("position") && parameters["position"] is Vector2 position &&
                    parameters.ContainsKey("velocity") && parameters["velocity"] is Vector2 velocity &&
                    parameters.ContainsKey("owner") && parameters["owner"] is Character owner)
                {
                    gameManager.GetEntities().Add(entityName, entity);
                    gameManager.GetEntities()[entityName].SetPosition(position);
                    gameManager.GetEntities()[entityName].SetVelocity(velocity);

                    gameManager.GetEntities()[entityName].Owner = owner;
                }
            }
        }

        public class DestroyEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("destroyEntity") && parameters["destroyEntity"] is string entityName)
                {
                    gameManager.GetEntities().Remove(entityName);
                }
            }
        }

        public class RequestPlayerPositionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("mob") && parameters["mob"] is Mob mob)
                {
                    if (gameManager.GetEntity("player") is Player player)
                    {
                        if(player.isInvis)
                            return; 
                        
                        Vector2 playerPos = player.GetPosition();
                        Rectangle playerBounds = player.GetBounds();

                        List<Entity> blocks = new List<Entity>();
                        foreach (var entity in gameManager.GetEntities().Values)
                        {
                            if (entity is IObtuse)
                                blocks.Add(entity);
                        }

                        // Use the new RayTracer function to determine full exposure
                        bool fullyVisible = RayTracer.IsPlayerFullyExposed(mob.GetPosition(), playerBounds, blocks);

                        if (fullyVisible)
                        {
                            mob.UpdateKnownPlayerPosition(playerPos);
                        }
                    }
                }
            }
        }

        public class AutoDestroyCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["targetKey"] is string key &&
                    parameters["gameManager"] is GameManager gm)
                {
                    gm.RemoveEntity(key);
                }
            }
        }

        public class HealRadiusCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("healOrigin") && parameters["healOrigin"] is Vector2 origin &&
                    parameters.ContainsKey("healRadius") && parameters["healRadius"] is float healRadius &&
                    parameters.ContainsKey("healAmount") && parameters["healAmount"] is int healAmount)
                {
                    foreach (var entity in gameManager.GetEntities().Values)
                    {
                        if (entity is Mob mob)
                        {
                            if (Vector2.Distance(origin, mob.GetPosition()) <= healRadius)
                            {
                                mob.ChangeHealth(healAmount);
                            }
                        }
                        // for testing only, not actually used in the game
                        // if (entity is Player player)
                        // {
                        //     System.Console.WriteLine("Healing player");
                        //     if (Vector2.Distance(origin, player.GetPosition()) <= healRadius)
                        //     {
                        //         System.Console.WriteLine("Healed player");
                        //         player.ChangeHealth(healAmount);
                        //     }
                        // }
                    }
                }
            }
        }
    }
}
