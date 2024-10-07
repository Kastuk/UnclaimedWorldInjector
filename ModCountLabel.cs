using HarmonyLib;
using System.Reflection;

namespace UnclaimedWorldInjector
{
    public class ModCountLabel
    {
        public static void Patch(Assembly UnclaimedWorld, Harmony harmony)
        {
            _game = UnclaimedWorld;
            harmony.Patch(TargetMethod(), new HarmonyMethod(SymbolExtensions.GetMethodInfo(() => ModCountLabel.Prefix())));
        }
        private static Assembly _game;

        public static void Initialize()
        {
            GM.Logger.Info(MethodBase.GetCurrentMethod().DeclaringType.Name + " initialized!");
        }
        static MethodBase TargetMethod()
        {
            return _game.GetType("UnclaimedWorld.Main").GetMethod("DrawVersionNumber", BindingFlags.NonPublic | BindingFlags.Static);
        }
        static void Prefix()
        {
            string version = (string)_game.GetType("UnclaimedWorld.Main").GetField("versionNumber").GetValue(null);

            if (!version.Contains("Modded!"))
            {
                version += " - Modded! (" + GM.ModCount + ")";
                _game.GetType("UnclaimedWorld.Main").GetField("versionNumber").SetValue(null, version);
            }
        }
    }
}
