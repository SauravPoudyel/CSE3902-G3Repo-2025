namespace Sprint0
{
    public interface IRigid { }        // Immovable blocks.
    public interface IPushable { }     // Blocks that can be pushed.
    public interface IObtuse { }   // Blocks that block line-of-sight.
    public interface IDestructible 
    {
        bool IsDestroyed { get; }
        void Destroy();
    }
    public interface IFlammable 
    {
        void Ignite();
    }
}
