using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoMissFortune
{
    public static class BadaoMissFortune
    {
        private static int createtime;
        private static int processtime;

        public static void BadaoActivate()
        {
            BadaoMissFortuneConfig.BadaoActivate();
            BadaoMissFortuneCombo.BadaoActivate();
            
        }
    }
}
