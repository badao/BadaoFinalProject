using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using SharpDX;
using Color = System.Drawing.Color;
using LeagueSharp.Common;

namespace BadaoActionsLimiter
{
    public static class AttackBlock
    {
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public static int AttackBlockCount;
        public static int LastAutoAttack;
        public static void BadaoActivate()
        {
            Spellbook.OnStopCast += Spellbook_OnStopCast;
            Obj_AI_Base.OnIssueOrder += Obj_AI_Base_OnIssueOrder;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Spellbook_OnStopCast(Spellbook spellbook, SpellbookStopCastEventArgs args)
        {
            if (spellbook.Owner.IsValid && spellbook.Owner.IsMe && args.DestroyMissile && args.StopAnimation)
            {
                LastAutoAttack = 0;
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;
            if (args.SData.IsAutoAttack())
            {
                LastAutoAttack = 0;
            }
            if (Orbwalking.IsAutoAttackReset(args.SData.Name))
            {
                LastAutoAttack = 0;
            }

        }

        private static void Obj_AI_Base_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (!sender.IsMe)
                return;
            if (args.Order != GameObjectOrder.AttackUnit)
                return;
            int limitTick = 1f / Player.AttackDelay > 4.5f ? 6 : 5;
            if (Environment.TickCount - LastAutoAttack < 100)
            {
                args.Process = false;
                AttackBlockCount += 1;
            }
            else
            {
                LastAutoAttack = Environment.TickCount;
            }
        }
    }
}
