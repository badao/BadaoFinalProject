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
    class BadaoMissFortuneHelper
    {
        // can use skill
        public static bool UseQCombo()
        {
            return BadaoMainVariables.Q.IsReady() && BadaoMissFortuneVariables.ComboQ.GetValue<bool>();
        }
        public static bool UseWCombo()
        {
            return BadaoMainVariables.W.IsReady() && BadaoMissFortuneVariables.ComboW.GetValue<bool>();
        }
        public static bool UseECombo()
        {
            return BadaoMainVariables.E.IsReady() && BadaoMissFortuneVariables.ComboE.GetValue<bool>();
        }
        public static bool UseRCombo()
        {
            return BadaoMainVariables.R.IsReady() && BadaoMissFortuneVariables.ComboR.GetValue<bool>();
        }
    }
}
