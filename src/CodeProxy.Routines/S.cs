using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProxy.Routines
{
    public static class S
    {
        private static Dictionary<Type, Dictionary<string,object?>> Storage = new Dictionary<Type, Dictionary<string, object?>>();

        public static void Set<T>(string key, T? value)
        {
            if (!Storage.ContainsKey(typeof(T)))
            {
                Storage.Add(typeof(T), new Dictionary<string, object?>());
            }
            Storage[typeof(T)][key] = value;
        }

        public static T? Get<T>(string key)
        {
            if (!Storage.ContainsKey(typeof(T)))
            {
                return default;
            }
            if (!Storage[typeof(T)].ContainsKey(key))
            {
                return default;
            }
            return (T)Storage[typeof(T)][key];
        }
    }
}
