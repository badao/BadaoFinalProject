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
    public static class BadaoMissFortuneVariables
    {
        // menu
        public static MenuItem ComboQ;
        public static MenuItem ComboW;
        public static MenuItem ComboE;
        public static MenuItem ComboR;

        // #
        public static Obj_AI_Base TapTarget = null;
        public static Obj_AI_Hero TargetRChanneling = null;
        public static Vector2 CenterPolar = new Vector2();
        public static Vector2 CenterEnd = new Vector2();
    }
}
