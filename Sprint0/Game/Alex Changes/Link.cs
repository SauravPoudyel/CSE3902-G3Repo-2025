/*
    This class defines the playable character's abilities.
*/
class Link : ICharacter{
    public Vector2 Position {get;}
    public Rectangle CollisionBox {get;}
    public ISprite Sprite{get;}
    private IProjectile projectiles[6];
    private int equippedProjectile;
    private int walkSpeed;

    public Link(Vector2 position){
        Position = position;

        equippedProjectile = 0;
        projectiles[0] = new Arrow();
        projectiles[1] = new SilverArrow();
        projectiles[2] = new Boomerang();
        projectiles[3] = new MagicBoomerang();
        projectiles[4] = new Bomb();
        projectiles[5] = new FireProjectile();

        walkSpeed = 5;
    }
    /*
        This makes Link shoot whichever projectile is currently equipped
    */
    public void ShootProjectile(){}
    /*
        This switches Link's equipped projectile
    */
    public void SwitchWeapon(int weapon){}
    /*
        This moves Link's position.
        Link can only move in 4 directions. 
        The parameter direction only provides the direction of the movement, not the magnitude.
        The magnitude of movement is defined by Link's walkSpeed.
    */
    public void Move(Vector2 direction){}
    /*
        This is Link's sword swing attack
    */
    public void Attack(){}

    public void Update(GameTime gameTime){}
    public void Draw(SpriteBatch spriteBatch){}

    public void Destroy(){}

}