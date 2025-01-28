
/*
    This is the projectile type that normal tanks fire.
    This projectile travels at an average speed and can bounce off of walls once before being destroyed.
    When this projectile hits another projectile, both projectiles will be destroyed.
*/
class ShellProjectile : IProjectile{
    public Vector2 Position{ get;}
    public Vector2 Velocity{ get;}
    public float speed{ get;}
    public Rectangle CollisionBox{ get;}

    public ISprite sprite{ get;}

    /*
        Initiates the projectile.
        This should load in the projectile's sprite.
    */
    public ShellProjectile(Vector2 position, Vector2 velocity){
        Position = position;
        Velocity = velocity;
    }


    /* 
        This is the function responsible for allowing Shells to bounce off of obstacles. 
        The projectile should be able to bounce off of either horizontal or vertical walls,
        but it should not be expected to bounce off of diagonal walls
        (the parameter int axis is just a placeholder, feel free to implement the direction of reflection however you think is best)
    */ 
    public void Reflect(int axis){}
    /*
        When this projectile collides with another projectile, both projectiles should be destroyed.
        When this projectile collides with an obstacle for the FIRST time, it should be reflected.
        When this projectile collides with an obstacle for the SECOND time, it should be destroyed.
        When this projectile collides with a character, it should deal damage to the character and then be destroyed.
    */
    public void Collide(IObject entity){}
    /*
        This should update the projectiles position based on its velocity as well as any other of the projectiles fields
    */
    public void Update(GameTime gameTime){}
    /*
        This should draw the projectile's sprite at the projectile's Position.
    */
    public void Draw(SpriteBatch spriteBatch){}
}