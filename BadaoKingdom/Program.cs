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
            "MissFortune","Poppy","Jhin"
        };
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (!SupportedChampion.Contains(ObjectManager.Player.ChampionName))
            {
                Game.PrintChat("<font color=\"#24ff24\">BadaoKingdom </font>" + "<font color=\"#ff8d1a\">" +
                    ObjectManager.Player.ChampionName + "</font>" + "<font color=\"#24ff24\"> not supported!</font>");
                return;
            }
            Game.PrintChat("<font color=\"#24ff24\">BadaoKingdom </font>" + "<font color=\"#ff8d1a\">" + 
                ObjectManager.Player.ChampionName + "</font>" + "<font color=\"#24ff24\"> loaded!</font>");
            BadaoChampionActivate();
            BadaoUtility.BadaoActivator.BadaoActivator.BadaoActivate();
        }

        private static void BadaoChampionActivate()
        {
            var ChampionName = ObjectManager.Player.ChampionName;
            if (ChampionName == "MissFortune")
                BadaoChampion.BadaoMissFortune.BadaoMissFortune.BadaoActivate();
            else if (ChampionName == "Poppy")
                BadaoChampion.BadaoPoppy.BadaoPoppy.BadaoActivate();
            else if (ChampionName == "Jhin")
                BadaoChampion.BadaoJhin.BadaoJhin.BadaoActivate();
            ;
        }
    }
}
