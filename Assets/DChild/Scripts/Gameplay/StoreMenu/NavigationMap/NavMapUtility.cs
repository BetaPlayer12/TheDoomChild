namespace DChild.Gameplay.NavigationMap
{
    public static class NavMapUtility
    {
        public static string CreateFogOfWarVariableName(Environment.Location sceneLocation, int sceneIndex, int index) => $"{sceneLocation}_{sceneIndex}_FOW_{index}";
        public static string GetObjectNameFromFogOfWarVariable(string varName)
        {
            var splitVarName = varName.Split('_');
            return $"Fog Of War ({splitVarName[3]})";
        }
    }
}
