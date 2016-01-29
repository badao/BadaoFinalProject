using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadaoKingdom.BadaoChampion.BadaoShen
{
    public static class BadaoShen
    {
        public static void BadaoActivate()
        {
            BadaoShenConfig.BadaoActivate();
            BadaoShenAuto.BadaoActivate();
            BadaoShenCombo.BadaoActive();
            BadaoShenJungleClear.BadaoActivate();
            BadaoShenLaneClear.BadaoActivate();
            BadaoShenSword.BadaoActivate();
        }
    }
}
