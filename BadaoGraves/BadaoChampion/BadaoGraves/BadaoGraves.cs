﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoGraves
{
    using static BadaoGravesVariables;
    public static class BadaoGraves
    {
        public static void BadaoActivate()
        {
            BadaoGravesConfig.BadaoActivate();
            BadaoGravesCombo.BadaoActivate();
            BadaoGravesBurst.BadaoActivate();
            BadaoGravesAuto.BadaoActivate();
            BadaoGravesJungle.BadaoActivate();
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Obj_AI_Base.OnIssueOrder += Obj_AI_Base_OnIssueOrder;
        }

        private static void Obj_AI_Base_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None && !BurstKey.GetValue<KeyBind>().Active)
                return;
            if (!sender.IsMe)
                return;
            if (args.Order != GameObjectOrder.AttackUnit)
                return;
            if (args.Target == null)
                return;
            if (!(args.Target is Obj_AI_Base))
                return;
            if (Player.Distance(args.Target.Position) > Player.BoundingRadius + Player.AttackRange + args.Target.BoundingRadius - 20)
            {
                args.Process = false;
                return;
            }
            if (!BadaoGravesHelper.CanAttack())
            {
                args.Process = false;
                return;
            }
            BadaoGravesCombo.Obj_AI_Base_OnIssueOrder(sender, args);
            BadaoGravesBurst.Obj_AI_Base_OnIssueOrder(sender, args);
            BadaoGravesJungle.Obj_AI_Base_OnIssueOrder(sender, args);
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.E)
            {
                Utility.DelayAction.Add(0, () => Orbwalking.ResetAutoAttackTimer());
            }
        }
    }
}
