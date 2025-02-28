using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprint0
{
    public static class CollisionResponse
    {
        private static Dictionary<Tuple<Type, Type>, string> responseMap;

        static CollisionResponse()
        {

            responseMap = new Dictionary<Tuple<Type, Type>, string>()
            {
                // Stop Interactions
                { new Tuple<Type, Type>(typeof(Player), typeof(Blocks)), "CollisionStop" },
                { new Tuple<Type, Type>(typeof(Player), typeof(RigidBlock)), "CollisionStop" },
                { new Tuple<Type, Type>(typeof(Player), typeof(Mob)), "CollisionStop" },
                { new Tuple<Type, Type>(typeof(PushableBlock), typeof(RigidBlock)), "CollisionStop" },
                { new Tuple<Type, Type>(typeof(Mob), typeof(RigidBlock)), "CollisionStop" },

                { new Tuple<Type, Type>(typeof(Player), typeof(PushableBlock)), "CollisionPush" },

                { new Tuple<Type, Type>(typeof(Projectile), typeof(FlammableBlock)), "DestroyFlammableCommand" },


                { new Tuple<Type, Type>(typeof(Projectile), typeof(Blocks)), "CollisionReflect" },
                { new Tuple<Type, Type>(typeof(Projectile), typeof(RigidBlock)), "CollisionProjectileDestroy" },
                {new Tuple<Type, Type>(typeof(Projectile), typeof(Mob)), "CollisionProjectileDestroy"},

                { new Tuple<Type, Type>(typeof(Player), typeof(PickupItem)), "CollisionPickUp" },

            };
        }

        public static string GetResponseCommand(Type subject, Type target)
        {
            // First, try to find an exact or assignable match
            foreach (var kvp in responseMap)
            {
                if (kvp.Key.Item1.IsAssignableFrom(subject) && kvp.Key.Item2.IsAssignableFrom(target))
                    return kvp.Value;
            }
            // Try reversed order.
            foreach (var kvp in responseMap)
            {
                if (kvp.Key.Item1.IsAssignableFrom(target) && kvp.Key.Item2.IsAssignableFrom(subject))
                    return kvp.Value;
            }
            return null;
        }
    }
}
