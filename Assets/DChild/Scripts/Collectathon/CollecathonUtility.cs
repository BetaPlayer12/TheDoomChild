using DChild.UI;
using System.Collections.Generic;
using Location = DChild.Gameplay.Environment.Location;

public static class CollecathonUtility
{
    private static Dictionary<Location, string> locationSuffixPair = new Dictionary<Location, string>();

    static CollecathonUtility()
    {
        locationSuffixPair.Add(Location.City_Of_The_Dead, "COTD");
        locationSuffixPair.Add(Location.Graveyard, "GY");
        locationSuffixPair.Add(Location.Unholy_Forest, "UF");
        locationSuffixPair.Add(Location.Garden, "GD");
        locationSuffixPair.Add(Location.Laboratory, "LAB");
        locationSuffixPair.Add(Location.Library, "LIB");
        locationSuffixPair.Add(Location.Prison, "PR");
        locationSuffixPair.Add(Location.Throne_Room, "TR");
        locationSuffixPair.Add(Location.Realm_Of_Nightmare, "RON");
        locationSuffixPair.Add(Location.Temple_Of_The_One, "TOTO");
    }

    public static string GenerateCurrentCountVariableName(CollectathonTypes type, Location location)
    {
        return $"Collectathon_{type}_Count_{locationSuffixPair[location]}";
    }


    public static string GenerateCurrentTotalVariableName(CollectathonTypes type, Location location)
    {
        return $"Collectathon_{type}_Total_{locationSuffixPair[location]}";
    }

    public static Dictionary<Location, string> AccessLocationDictionary()
    {
        return locationSuffixPair;
    }
}
