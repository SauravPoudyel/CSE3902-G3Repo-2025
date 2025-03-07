using Microsoft.Xna.Framework;

namespace Sprint0
{
    public interface IRigid { }        // Immovable blocks.
    public interface IPushable 
    {
        void Push(Vector2 direction);
    }     // Blocks that can be pushed.
    public interface IObtuse { }   // Blocks that block line-of-sight.
    public interface IDestructible 
    {
        bool IsDestroyed { get; }
        void Destroy();
    }
    public interface IFlammable 
    {
        bool IsIgnited { get; }
        void Ignite();
        void Destroy();
    }
}
