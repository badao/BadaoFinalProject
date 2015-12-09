using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;

namespace Anti_Rito
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            OneTickOneSpell.Init();
            Game.PrintChat("Anti-Riot by Sebby and Badao Loaded!");
        }
    }
}
