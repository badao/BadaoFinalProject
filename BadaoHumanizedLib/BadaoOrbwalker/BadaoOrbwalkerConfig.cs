using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoHumanizedLib.BadaoOrbwalker
{
    public static class BadaoOrbwalkerConfig
    {
        public static Menu Config;
        public static void BadaoActivate()
        {
            Config = new Menu("BadaoOrbwalk" , "Badao Orbwalker", true);
            Config.SetFontStyle(System.Drawing.FontStyle.Bold, SharpDX.Color.YellowGreen);

            BadaoOrbwalkerVariables.BadaoOrbwalk = new Orbwalking.Orbwalker(Config);

            Config.AddToMainMenu();
        }
    }
}
