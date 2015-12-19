using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom
{
    static class Program
    {
        public static readonly List<string> SupportedChampion = new List<string>()
        {
            "MissFortune"
        };
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (!SupportedChampion.Contains(ObjectManager.Player.ChampionName))
                return;
            BadaoChampionActivate();
            BadaoUtility.BadaoActivator.BadaoActivator.BadaoActivate();
        }
        private static void BadaoChampionActivate()
        {
            var ChampionName = ObjectManager.Player.ChampionName;
            if (ChampionName == "MissFortune")
                BadaoChampion.BadaoMissFortune.BadaoMissFortune.BadaoActivate();
            else
                ;
        }
    }
}
