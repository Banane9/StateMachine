using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public static Type GetNextDerivative<TCutOff>(this Type type)
        {
            return type != typeof(TCutOff) ? type.BaseType : null;
        }

        public static Func<object> MakeConstructor(this Type type)
        {
            var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(ctor => ctor.GetParameters().Length == 0);

            return Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(constructor), objType)).Compile();
        }

        public static Action<object, object> MakePropertySetter(this PropertyInfo property)
        {
            var target = Expression.Parameter(objType);
            var value = Expression.Parameter(objType);
            var body = Expression.Call(
                Expression.Convert(target, property.DeclaringType),
                property.SetMethod,
                Expression.Convert(value, property.PropertyType));

            return Expression.Lambda<Action<object, object>>(body, target, value).Compile();
        }
    }
}