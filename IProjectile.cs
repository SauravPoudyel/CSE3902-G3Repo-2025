
interface IProjectile : IObject{

    public Vector2 Velocity{ get;}
    public float speed {get;}
    public void Collide(IObject entity);
}