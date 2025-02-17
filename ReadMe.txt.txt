Tank You Very Much Group Members
Leo Chen (chen.11937)
Daoping Huang (huang.4808)
Harry Manzler (manzler.5)
Alexander Nistor (nistor.7)
Saurav Poudyel (poudyel.7)
Payton Yaffe (yaffe.14)

Submission:
** = above and beyond
Items
    - Cycling works as Intended AND **when cycling, the current item’s sprite is updated to reflect the new selection (retention of animation cycle)**.
    - Implemented number keys (1, 2, 3, etc.) to shoot different projecitles (1. Sniper, 2. Rocket, 3. Shotgun, 4. Bomb).
    - Used a standardized projectile factory to handle projectile creation and **deletion**

Player 
    - Implemented movement using both WASD and arrow keys.
    - Movement logic adjusts facing direction based on input.
    - **Implemented vector-based movement and friction**
    - Just 'z' triggers a default projectile 
    - **Projectile spawn is omnidirectional and based on a vector transformations and calcualtions of cannon rotation and pivot**
    - Pressing 'e' causes Link to take damage.

Enemy/NPC Behavior
    - Enemies move, animate, and fire projectiles **cannon animation is omnidirectional**
    - Implemented state machines for different enemy behaviors:
        - BossTank: Moves back and forth and fires a shotgun spread.
        - SmallEnemy: Moves in a square and has increased firing speedand cannon rotation speed.
        - ExplodingTank: Moves and then self-destructs (doesn't fire when exploding).
        - Turret: Stationary and fires in intervals.
        - TurningTank: Rotates and moves directionally in phases.
        - Plane: Flies in an orbit and shoots bullets.
    - **Multiple enemies feature extensive rotaional calcualtions of not just the cannons, but also the internal sprite bodies (plane, and turningtank in particular)**
    - Used 'o' and 'p' to cycle through available enemies and NPCs.
    - When cycling, the currently displayed enemy is removed and replaced using a mob factory that maintains an index of enemy types.
    - Implemented proper cycling logic that ensures we loop back at the start/end.

Blocks
    - Used 't' and 'y' to cycle through obstacles.

Start/Menu Screen
    - Implemented a menu screen that prompts the player to start the game.
    - Used an overlay system with a greyscale effect to visually distinguish the menu from gameplay.
    - **Restart and Quit buttons are also displayed**. 
    - **The Menu can be brought back up with escape**.
    - **The game state is paused during the menu screen**.

Going Above and Beyond Summary: 
    - Designed a screen state management system that allows transitions between menu and gameplay.
    - Ensured menus do not interfere with entity updates when active.
    - Extensive use of vector math for movement, aiming, and projectile behavior.
    - Used trigonometric calculations to ensure:
    - Cannon rotation follows correct pivot logic instead of orbiting.
    - Planes orbit properly in circular motion rather than just turning.
    - Cannon angles match sprite rotations.
    - When it comes to omnidirectional animation, there are a lot more issues that pop up, so debugging took a lot of time

Bugs: 
    - Pressing Escape multiple times makes the screen dim more and more
    - Sometimes a few projectiles, don't delete 
    - Very rarely the enemies stop shooting, just restart the game if that happens