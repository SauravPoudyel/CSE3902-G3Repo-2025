Tank You Very Much
Group Members
Leo Chen (chen.11937)
Daoping Huang (huang.4808)
Harry Manzler (manzler.5)
Alexander Nistor (nistor.7)
Saurav Poudyel (poudyel.7)
Payton Yaffe (yaffe.14)

Submission Overview: 

Player:

Movement is controlled using W/S for forward/backward and A/D for turning.
Pressing Z fires the default projectile, while number keys fire the specialized ones as noted above.
Vector-based movement and friction have been implemented to provide smooth motion and realistic deceleration.
Pressing E causes the player to take damage for testing purposes.
Number keys trigger different projectiles:
1: Sniper
2: Rocket
3: Shotgun
4: Mine
5: Teleporter

Enemy / NPC Behavior:

Enemies move, animate, and fire projectiles.
State machines manage enemy behaviors:
BossTank: Moves back and forth while firing a shotgun spread.
SmallEnemy: Navigates in a square pattern with increased firing and cannon rotation speed.
ExplodingTank: Moves and then self-destructs (it doesn’t fire when exploding).
Turret: Remains stationary and fires at set intervals.
TurningTank: Rotates and moves directionally in phases.
Plane: Flies in an orbital pattern and shoots bullets.
Above and beyond:
Extensive rotational calculations ensure that not only the cannons but also the enemy body sprites (notably in the Plane and TurningTank) are properly oriented.
Ray tracing for enemy AI has been implemented so enemies can track the player effectively.
All characters (mobs and player) have a tracktrail that follows them around for more dynamic sprite animations

Collision Response:

Projectiles interact with blocks, mobs, and players. For instance, shooting an oil barrel (flammable block) triggers its explosion.
Explosions can damage both the player and enemies.
Pushable blocks (the box) get moved appropriately when collided with.
Above and beyond:
The sniper projectile reflects of the tree (there are some slight bugs, but it works pretty well to test)

Items:

A variety of pickups work as follows:
SpeedBoost: Temporarily increases player speed.
Shield: Activates a protective shield.
Ammo types: Increase the available ammo count for various projectile types (default, shotgun, sniper, rocket, laser, mine).
Magnet: Pulls items toward the player.
FireRateIncrease: Boosts the firing speed.
MedStrong/MedWeak: Restore health.
TimeSlow: Temporarily slows down time (adjusts frame time).
Fly & Cloak: Provide temporary abilities such as flight or invisibility. (fly not implemnted yet)
Tags (Silver/Gold): Increase coin totals.
Teleporter: Allows the player to teleport when fired.
A cyclable item is available on the left side for quick access, with its icon updating to reflect the current selection.
Above and beyond:
The cloak disables enemy visibility and ray tracing

CSV Parsing & Player Data:

The game uses CSV parsing to load both level data and player data:
Level CSV files are read to place entities (players, enemies, items, blocks) into the game world.
Player data CSV files store permanent and temporary player stats (health, ammo, shield, coins, etc.).
Data is loaded during initialization and can be saved, ensuring persistent player progress.
You can reset levels by just going back and forth through them

UI / Player Inventory:

The Player Inventory UI displays HUD elements (shield, health, ammo, coins) on the left side.
This UI is connected to player data, reflecting any pickups or changes in stats.
Inventory management is integrated into the overall game state and updates in real time.
Above and Beyond: 
The inventory slots shift and move accordingly once a slot is empty

Sounds:

We have sounds for: firing, explosions, picking up items, and background music


Areas in Progress / Future Work
Collision Interactions:

Further smoothing out of some smaller collision interactions.
Refinement of collision resolution for a more natural “stopping” effect.

Enemy AI:

Continued improvements to enemy behavior for more complex decision-making.
Tuning of tracking and shooting patterns.

Block Types & Level Design:

Addition of more varied block types.
Improved level cycling and level design.

UI Enhancements:

Expansion of inventory slots and further integration with the player inventory system.
Refinement of the UI to handle dynamic data from player pickups and progress.
Going Above and Beyond

Screen State Management:

Developed a robust system for handling transitions between menu and gameplay.
Menus pause entity updates and have an overlay that clearly distinguishes them from gameplay.

Detailed CSV Parsing:

Our CSV parser efficiently loads level configurations and player data, ensuring a seamless integration of external data.

Known Bugs:
The projectile reflection may pass through the blocks sometimes
At certain angles and rarely collisions may not work as intended, that's just because the precise angle
offsets for adjustments gets miscalculated at certain angles
THE SOUND HAS ISSUES ON MAC. We are trying to debug it but, the program itself may not run correctly on Mac, we just figured it out close to submission date, so 
we didn't have time to debug it

All Relavent Documents are in the DOCS folder