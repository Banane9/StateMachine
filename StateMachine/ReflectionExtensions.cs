using System;
using System.Linq;
using System.Collections.Generic;

namespace StateMachine
{
    internal static class ReflectionExtensions
    {
        private static readonly Type objType = typeof(object);

        public static bool DerivesFrom(this Type type, Type baseType, out Type baseTypeDef)
        {
            var genericDefinition = baseType.IsGenericTypeDefinition;

            while (type != objType)
            {
                if ((!genericDefinition && type.BaseType == baseType)
                 || (genericDefinition && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == baseType))
                {
                    baseTypeDef = type.BaseType;
                    return true;
                }

                type = type.BaseType;
            }

            baseTypeDef = null;
            return false;
        }

        public static bool DerivesFrom(this Type type, Type baseType)
        {
            return type.DerivesFrom(baseType, out var _);
        }

        public static bool DerivesFromOrIs(this Type type, Type baseType, out Type baseTypeDef)
        {
            var genericDefinition = baseType.IsGenericTypeDefinition;
            if ((!genericDefinition && type == baseType)
             || (genericDefinition && type.IsGenericType && type.GetGenericTypeDefinition() == baseType))
            {
                baseTypeDef = type;
                return true;
            }

            return type.DerivesFrom(baseType, out baseTypeDef);
        }

        public static bool DerivesFromOrIs(this Type type, Type baseType)
        {
            return type.DerivesFromOrIs(baseType, out var _);
        }
    }
}