using UnityEngine;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Game;

namespace SystemClockMod
{
    public class SystemClockMod : MonoBehaviour
    {
        public static Mod Mod;
        private static GUIStyle style;
        private static Rect rect;
        private static Vector2 rectSize;
        private static Vector2 rectPosition;

        // Mod options
        static int horizontalPositionPercent;
        static int verticalPositionPercent;
        static string format;  // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
        static Color fontColor;
        static int fontSize;
        static int guiDepth;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            Mod = initParams.Mod;
            new GameObject(Mod.Title).AddComponent<SystemClockMod>();
            Mod.LoadSettingsCallback = LoadSettings;
            Mod.IsReady = true;
        }

        void Start()
        {
            style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            Mod.LoadSettings();
        }

        void OnGUI()
        {
            GUI.depth = guiDepth;
            GUI.Label(
                rect,
                System.DateTime.Now.ToString(format),
                style
            );
        }

        static void LoadSettings(ModSettings modSettings, ModSettingsChange change)
        {
            if (modSettings != null)
            {
                format = modSettings.GetString("Settings", "Format");
                guiDepth = modSettings.GetValue<int>("Settings", "RenderOrder"); ;
                horizontalPositionPercent = modSettings.GetValue<int>("Settings", "HorizontalPositionPercent");
                verticalPositionPercent = modSettings.GetValue<int>("Settings", "VerticalPositionPercent");
                fontSize = modSettings.GetValue<int>("Settings", "Size");
                fontColor = modSettings.GetColor("Settings", "Color");

                style.normal.textColor = fontColor;
                style.fontSize = fontSize;
                rectSize = style.CalcSize(new GUIContent(System.DateTime.Now.ToString(format)));
                rectPosition.x = (Screen.width - rectSize.x) * (horizontalPositionPercent / 100f);
                rectPosition.y = (Screen.height - rectSize.y) * (verticalPositionPercent / 100f);
                rect = new Rect(rectPosition.x, rectPosition.y, rectSize.x, rectSize.y);
            }
        }

    }
}
