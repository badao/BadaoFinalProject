using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoHumanizedLib.BadaoOrbwalker
{
    public static class BadaoOrbwalker
    {
        public static void BadaoOrbwalkerInit()
        {
            BadaoOrbwalkerConfig.BadaoActivate();
            Orbwalking.Attack = false;
            Orbwalking.Move = false;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoOrbwalkerVariables.BadaoOrbwalk.ActiveMode == Orbwalking.OrbwalkingMode.None)
                return;
            if (ObjectManager.Player.IsCastingInterruptableSpell(true))
            {
                return;
            }
            var target = BadaoOrbwalkerVariables.BadaoOrbwalk.GetTarget();
            BadaoOrbwalkingMethods.BadaoBeginOrbwalk(target, BadaoOrbwalkerVariables.OrbwalkingPoint);
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && BadaoOrbwalkerVariables._spells.Contains(args.Slot))
            {
                BadaoOrbwalkerVariables._lastCastT = Utils.GameTimeTickCount;
            }
            if (!args.SData.IsAutoAttack())
            {
                return;
            }
            if (sender.IsMe)
            {
                BadaoOrbwalkerVariables._autoAttackCounter++;
            }
        }
    }
}
