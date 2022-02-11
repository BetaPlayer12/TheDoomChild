namespace DChild.Gameplay.NavigationMap
{
    public static class NavMapUtility
    {
        public static string CreateFogOfWarVariableName(Environment.Location sceneLocation, int sceneIndex, int index) => $"{sceneLocation}_{sceneIndex}_FOW_{index}";
        public static string CreatePointOfInterestVarableName(Environment.Location sceneLocation, int sceneIndex, string type, int Index) => $"{sceneLocation}_{sceneIndex}_POI_{type}_{Index}";
        public static string GetObjectNameFromFogOfWarVariable(string varName)
        {
            var splitVarName = varName.Split('_');
            return $"Fog Of War ({splitVarName[3]})";
        }
        public static string GetObjectNameFromPointOfInterestVariable(string varName)
        {
            var splitVarName = varName.Split('_');
            return $"Point Of Interest ({splitVarName[4]})";
        }
    }
}
