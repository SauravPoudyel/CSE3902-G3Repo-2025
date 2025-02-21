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
            responseMap = new Dictionary<Tuple<Type, Type>, string>();
            responseMap.Add(new Tuple<Type, Type>(typeof(Player), typeof(Blocks)), "CollisionStop");
            responseMap.Add(new Tuple<Type, Type>(typeof(Player), typeof(Blocks)), "CollisionPush");
            responseMap.Add(new Tuple<Type, Type>(typeof(Projectile), typeof(Blocks)), "CollisionReflect");
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
