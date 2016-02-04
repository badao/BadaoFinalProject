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
    public static class BadaoJhinDrawing
    {
        public static void BadaoActivate()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (BadaoJhinHelper.DrawWMiniMap())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, BadaoMainVariables.W.Range, Color.Green, 1, 23, true);
            }
        }
    }
}
