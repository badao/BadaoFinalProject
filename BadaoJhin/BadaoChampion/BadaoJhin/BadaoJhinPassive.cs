using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoJhin
{
    public static class BadaoJhinPassive
    {
        public static List<GameObject> JhinPassive
        {
            get
            {
                return ObjectManager.Get<GameObject>().Where(x => x.Name.Contains("Jhin_Base_E_passive")).ToList();
            }
        }
        public static List<Obj_AI_Minion> JhinTrap
        {
            get 
            {
                return ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsAlly && x.CharData.BaseSkinName == "jhintrap").ToList();
            }
        }
        public static void BadaoActiavte()
        {
            Game.OnUpdate += Game_OnUpdate;
        }
        private static void Game_OnUpdate(EventArgs args)
        {

        }
    }
}
