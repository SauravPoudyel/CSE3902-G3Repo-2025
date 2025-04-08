using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class ActionCommands_Logic
    {
        public static void HandlePlayerAction(Player player, GameManager gameManager, string actionType)
        {
            switch (actionType)
            {
                case "fire":
                    HandleFireAction(player);
                    break;
                
                case "interact":
                    player.Interact();
                    break;

                case "item1":
                case "item2":
                case "item3":
                case "item4":
                case "item5":
                    int slotIndex = GetItemSlotIndex(actionType);
                    HandleItemAction(player, gameManager, slotIndex);
                    break;

                default:
                    Console.WriteLine("Error: Invalid actionType.");
                    break;
            }
        }

        private static int GetItemSlotIndex(string actionType)
        {
            return actionType switch
            {
                "item1" => 0,
                "item2" => 1,
                "item3" => 2,
                "item4" => 3,
                "item5" => 4,
                _ => 0
            };
        }

        private static void HandleFireAction(Player player)
        {
            if (!player.CanFire)
                return;

            string projectileType = "Default";
            player.SetProjectileType(projectileType);

            // Ensure default ammo is decremented properly
            if (Globals.PlayerData.GetInt("AmmoDefault") > 0)
            {
                Globals.PlayerData.UpdateVariable("AmmoDefault", -1);
                player.FireProjectile();
            }
            else
            {
                Console.WriteLine("Out of default ammo!");
            }
        }

        private static void HandleItemAction(Player player, GameManager gameManager, int slotIndex)
        {
            if (!player.CanFire)
                return;

            InventorySlot slot = gameManager.screenManager.playerInventory.inventorySlots[slotIndex];
            string projectileType = slot.ProjectileType;

            if (slot.AmmoCount <= 0)
                return;

            // Decrement the correct ammo type
            switch (projectileType)
            {
                case "Sniper":
                    Globals.PlayerData.UpdateVariable("AmmoSniper", -1);
                    break;
                case "Rocket":
                    Globals.PlayerData.UpdateVariable("AmmoRocket", -1);
                    break;
                case "Shotgun":
                    Globals.PlayerData.UpdateVariable("AmmoShotgun", -1);
                    break;
                case "Mine":
                    Globals.PlayerData.UpdateVariable("AmmoMine", -1);
                    break;
                case "Teleporter":
                    Globals.PlayerData.UpdateVariable("AmmoTeleporter", -1);
                    break;
                case "Default":  // Ensure default ammo decrements properly here too
                    Globals.PlayerData.UpdateVariable("AmmoDefault", -1);
                    break;
                default:
                    Console.WriteLine("Error: Invalid projectile type.");
                    break;
            }

            player.SetProjectileType(projectileType);
            player.FireProjectile();

            if (slot.AmmoCount <= 0)
            {
                gameManager.screenManager.playerInventory.ShiftEmptySlot(slotIndex);
            }
        }

        public static void HandleCreateProjectileCommand(Dictionary<string, object> parameters, Character owner, string projectileType, Vector2 spawnPosition, float cannonRotation, GameManager gameManager)
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

        public static void HandleCreateEntityCommand(Dictionary<string, object> parameters, GameManager gameManager, string entityName, Entity entity, Vector2 position, Vector2 velocity)
        {
            gameManager.GetEntities().Add(entityName, entity);
            gameManager.GetEntities()[entityName].SetPosition(position);
            gameManager.GetEntities()[entityName].SetVelocity(velocity);

            if(parameters.ContainsKey("owner") && parameters["owner"] is Character owner)
            {
                gameManager.GetEntities()[entityName].Owner = owner;
            }
        }

        public static void HandleRequestPlayerPositionCommand(GameManager gameManager, Mob mob)
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

        public static void HandleHealRadiusCommand(GameManager gameManager, Vector2 origin, float healRadius, int healAmount)
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