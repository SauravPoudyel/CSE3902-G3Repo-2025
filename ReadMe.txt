Tank You Very Much
Group Members:
Leo Chen (chen.11937)
Daoping Huang (huang.4808)
Harry Manzler (manzler.5)
Alexander Nistor (nistor.7)
Saurav Poudyel (poudyel.7)
Payton Yaffe (yaffe.14)

SUBMISSION OVERVIEW

PLAYER CONTROLS
- Movement: W/S to move forward/backward, A/D to rotate.
- Firing: Press Z to shoot the default projectile.
- Press 1–5 to switch special projectiles:
  1: Sniper — bounces off trees, pierces enemies.
  2: Rocket — explodes on impact.
  3: Shotgun — fires a spread of projectiles.
  4: Mine — deploys and detonates on contact.
  5: Teleporter — teleports player to impact location.
- Press H to test self-damage (there's a death animation)
- J should toggle invincibility, but it might not be working 100% (we gave you more than enough health to play test)
- ESC to access the Pause Menu.
- Press Shift for a player boost (there's a boost stamina meter)
- We gave you a bunch of health to be able to tets things out for now

ENEMY AI
- Enemies constantly track the player between agressive, passive, and neutral states
- BossTank: Moves back and forth while firing a shotgun spread.
- SmallEnemy: Navigates with increased firing and cannon rotation speed.
- SwarmingTank: Moves and then self-destructs (it doesn’t fire).
- Turret: Remains stationary and fires at set intervals.
- HoveringTank: Hovers over blocks and fires.
- ShieldTank: Blocks some damage from the front. 
- Ship: Matches the player X or Y-axis values but not both, and also fires
- HealerTank: Stays passive and heals other tanks
- StealthTank: Occasionally goes stealth and goes aggresive, stays passive otherwise
- Plane: Flies in an orbital pattern and shoots bullets.
- Trail visuals follow enemies and players.

COLLISION + WORLD INTERACTIONS
- Projectiles interact with players, mobs, and environment.
- Oil barrels explode when shot.
- Sniper bullets reflect off trees.
- Explosions damage nearby entities.
- Pushable boxes move on collision.
- THERE IS A "Z-AXIS implementation" on level 3-ish and others. You should be able to walk up on to the top of the cliff
and shoot down on the enemies without moving through

ITEM SYSTEM (PICKUPS)
- SpeedBoost: Increases movement speed.
- Shield: Grants a temporary protective shield.
- Ammo Pickups (Default, Shotgun, Sniper, Rocket, Laser, Mine): Refills corresponding ammo.
- Magnet: Attracts nearby pickups.
- FireRateIncrease: Boosts fire rate.
- MedWeak/MedStrong: Restore health.
- TimeSlow: Temporarily slows game time.
- Cloak: Grants temporary invisibility (avoids enemy detection).
- Silver/Gold Tags: Add to coin total.

UI + INVENTORY
- Player inventory HUD appears on the bottom of the screen.
- Displays current health, shield, coins, and ammo.
- Special item inventory rotates when an item is finished.
- Dialogue is added to appear upon certain events, click to skip dialogue
- XP is also incremented and increases per enemy
- Map on the bottom right for navigation

SHOP SYSTEM
- Access the shop by clicking the icon in the top-right corner.
- Buy items using coins.
- Categories include Weapons, Consumables, and more.
- Click icons or text to purchase.
- Purchases update player stats immediately.
- WE NOW HAVE LOGIC TO INTERACT WITH THE SHOP building, this is unlcoked after you defeat the first Boss
in the grass biome, but you can tets it by going into PLayerData.csv and changing HasShopKeys to 1. The shop is the
top right building, and you can interact with it by pressing 3. This is how it will function in the final game

MENUS + STATES
- Start Menu: Loads on game boot. Press ENTER to begin.
- Pause Menu: Accessed via ESC. Click “Resume” or press ENTER to unpause.
- Achievement Screen ans Stat screen in the top right of the game. 
- There are also some other options to mess with in PauseMenu
- Screen transitions are managed to avoid input issues.
- We have a Day and Night Cycle, it works with shaders and you can see it's effects after
a couple minutes (we accelerated it for now)
- There is also a stat screen that you can look at for each mob type killed

CSV-BASED DESIGN + DATA
- Levels are loaded from CSV files.
- Player data (ammo, health, coins, etc.) is stored persistently in CSV format.
- Player inventory and progress update based on CSV values.
- Moving off screen edges triggers level transitions based on connected map data.
- PROCEDURAL GENERATION is now in the game. You can access it by pressing e on the purple portal in the base level
a second portal appears once you defeat all enemies within the procedural portal.

SOUND
- Sounds include: shooting, explosions, pickup, and menu background music.
- Known Issue: Engine/hum sound may continue during pause.
- Known Issue: Mac compatibility issues with audio playback.

KNOWN BUGS
- Some projectiles may pass through targets at extreme angles.
- Audio persistence bug: Driving sound sometimes continues when game is paused.
- We think the sound issue might be fixed on MAC, but there still might be a small change it doesn't run on MAC
- We have raised ground in hub and level 1 but their collisions don't work right now (we're trying to figure out 
how to best do tile collisions)
- AFTER YOUR FIRST DEATH. some of the items may not magnetize to you. this has to do with player references, and we are working
on a fix

DOCUMENTATION
- All relevant CSV files, assets, and developer documentation are located in the DOCS folder.