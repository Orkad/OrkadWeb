using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Contexts
{
    [Binding]
    public class LastContext
    {
        private readonly Dictionary<Type, object> Lasts = new();

        public T Last<T>()
        {
            return (T)Lasts[typeof(T)];
        }

        /// <summary>
        /// Define the last object mentionned in steps
        /// </summary>
        public void Mention<T>(T last)
        {
            if (last == null)
            {
                throw new ArgumentNullException(nameof(last));
            }
            var type = typeof(T);
            if (Lasts.ContainsKey(type))
            {
                Lasts[type] = last;
            }
            else
            {
                Lasts.Add(type, last);
            }
        }
    }
}
