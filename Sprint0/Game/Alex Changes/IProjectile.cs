
interface IProjectile : IObject{

    public Vector2 Velocity{ get;}
    public float Speed {get;}
    public void Collide(IObject entity);
}