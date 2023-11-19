using System;

namespace OrkadWeb.Domain.Exceptions
{
    public class DataNotFoundException<T>(object id) : Exception($"Impossible de trouver entité ({typeof(T)}) correspondant à l'identifiant ({id})")
    {
    }
}
