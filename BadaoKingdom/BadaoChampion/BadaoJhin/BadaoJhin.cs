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
    public static class BadaoJhin
    {
        //Jhin_Base_E_passive_mark.troy
        //jhinpassiveattackbuff, JhinPassiveReload
        //qrange = 600 ,  Qbound 500
        //W rang = 2500, W rad = 100
        //E range = 750, Ebound = 260
        // R range = 3500
        public static void BadaoActivate()
        {
            BadaoJhinAuto.BadaoActiavate();
            BadaoJhinCombo.BadaoActivate();
            BadaoJhinConfig.BadaoActivate();
            BadaoJhinHarass.BadaoActivate();
            BadaoJhinJungleClear.BadaoActivate();
            BadaoJhinLaneClear.BadaoActivate();
            BadaoJhinPassive.BadaoActiavte();
            //Drawing.OnDraw += Drawing_OnDraw;
        }

        //private static void Drawing_OnDraw(EventArgs args)
        //{
        //    var info = BadaoJhinHelper.GetQInfo();
        //    var target = info.Where(x => x.BounceTargets.LastOrDefault(y => y.Target is Obj_AI_Hero) != null)
        //        .OrderBy(x => x.BounceTargets.LastOrDefault(y => y.Target is Obj_AI_Hero).DeathCount)
        //        .ThenByDescending(x => x.BounceTargets.IndexOf(x.BounceTargets.LastOrDefault(y => y.Target is Obj_AI_Hero)))
        //        .LastOrDefault();
        //    if (target != null)
        //    { 
        //        foreach (var minion in target.BounceTargets)
        //        {
        //            if (target.BounceTargets.IndexOf(minion) == 0)
        //                Render.Circle.DrawCircle(minion.Target.Position, 100, Color.Yellow);
        //            if (target.BounceTargets.IndexOf(minion) == 1)
        //                Render.Circle.DrawCircle(minion.Target.Position, 100, Color.Green);
        //            if (target.BounceTargets.IndexOf(minion) == 2)
        //                Render.Circle.DrawCircle(minion.Target.Position, 100, Color.Red);
        //            if (target.BounceTargets.IndexOf(minion) == 3)
        //                Render.Circle.DrawCircle(minion.Target.Position, 100, Color.Pink);
        //        }
        //    }
        //}
    }
}
