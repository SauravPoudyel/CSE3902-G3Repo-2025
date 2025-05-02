using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ActionCommandsLogic_PlayerAction
    {
        public static void HandlePlayerAction(Player player, GameManager gameManager, string actionType)
        {
            if (actionType == "fire")
                HandleFireAction(player);
            else if (actionType == "interact")
                player.Interact();
            else if (actionType.StartsWith("item"))
                HandleItemAction(player, gameManager, GetItemSlotIndex(actionType));
            else
                Console.WriteLine("Error: Invalid actionType.");
        }

        private static void HandleFireAction(Player player)
        {
            if (!player.CanFire)
                return;

            player.SetProjectileType("Default");

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

            var slot = gameManager.screenManager.playerInventory.inventorySlots[slotIndex];
            if (slot.AmmoCount <= 0)
                return;

            DecrementAmmo(slot.ProjectileType);
            player.SetProjectileType(slot.ProjectileType);
            player.FireProjectile();

            if (slot.AmmoCount <= 0)
                gameManager.screenManager.playerInventory.ShiftEmptySlot(slotIndex);
        }

        private static void DecrementAmmo(string type)
        {
            switch (type)
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
                default:
                    Globals.PlayerData.UpdateVariable("AmmoDefault", -1);
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
                _       => 0
            };
        }
    }
}
