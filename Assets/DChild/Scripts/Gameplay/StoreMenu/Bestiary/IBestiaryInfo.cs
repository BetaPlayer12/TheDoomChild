using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Menu.Bestiary
{
    public interface IBestiaryInfo
    {
        string description { get; }
        int id { get; }
        Sprite indexImage { get; }
        Location[] locatedIn { get; }
        string creatureName { get; }
    }
}