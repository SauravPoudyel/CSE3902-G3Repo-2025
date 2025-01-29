/*
    This is the normal tank that fires bouncing shells.
    The tank body can only face in 8 direction, while the tank turret always points towards it's target.
    The tank can only take 1 hit of damage before being destroyed.
    The tank can only fire 5 projectiles at a time, if there are already 5 projectiles then this tank cannot fire again until one has been destroyed.
*/
class ShellTank : ICharacter{

    public ISprite Sprite{ get;}
    public ISprite TurretSprite;

}