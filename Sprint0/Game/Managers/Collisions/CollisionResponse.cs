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
            responseMap = new Dictionary<Tuple<Type, Type>, string>(){
                {new Tuple<Type, Type>(typeof(Player), typeof(Blocks)), "CollisionStop"},
                {new Tuple<Type, Type>(typeof(Projectile), typeof(Blocks)), "CollisionReflect"}, 

                {new Tuple<Type, Type>(typeof(Player), typeof(PickupItem)), "CollisionPickUp"}, 
                {new Tuple<Type, Type>(typeof(Projectile), typeof(Mob)), "CollisionProjectileToMob"}
            };
        }

        public static string GetResponseCommand(Type subject, Type target)
        {
            Tuple<Type, Type> key = new Tuple<Type, Type>(subject, target);
            if (responseMap.ContainsKey(key))
                return responseMap[key];
            key = new Tuple<Type, Type>(target, subject);
            if (responseMap.ContainsKey(key))
                return responseMap[key];
            return null;
        }
    }
}
