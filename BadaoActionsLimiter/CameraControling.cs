using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using SharpDX;
using Color = System.Drawing.Color;
using ClipperLib;
using LeagueSharp.Common;

namespace BadaoActionsLimiter
{
    public static class CameraControling
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);
        public static void BadaoActivate()
        {

        }

        public static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!Program.Config.Item("CameraControl").GetValue<bool>())
                return;
            if (!args.StartPosition.IsOnScreen())
            {
                var pos = Camera.Position;
                Vector3 NewCameraPos = new Vector3(args.StartPosition.X + _random.Next(400), args.StartPosition.Y + _random.Next(400),Camera.Position.Z);
                Camera.Position = NewCameraPos;
                Utility.DelayAction.Add(300, () => Camera.Position = pos);
            }
        }
    }
}
