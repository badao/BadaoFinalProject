﻿using System;
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
    public static class BadaoMissFortuneCombo
    {
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate; // Q,E
            Orbwalking.AfterAttack += Orbwalking_AfterAttack; // R
            Orbwalking.OnAttack += Orbwalking_OnAttack; // W
        }

        private static void Orbwalking_OnAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe && BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (BadaoMissFortuneHelper.UseWCombo() && target.BadaoIsValidTarget() && target is Obj_AI_Hero)
            {
                BadaoMainVariables.W.Cast();
            }
        }

        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (BadaoMissFortuneHelper.UseRCombo() && unit.IsMe 
                && target.BadaoIsValidTarget() &&
                target is Obj_AI_Hero && BadaoMissFortuneHelper.Rdamepior() 
                && target.Health <= 0.6f*BadaoMissFortuneHelper.RDamage(target as Obj_AI_Base))
            {
                var PredTarget = Prediction.GetPrediction(target as Obj_AI_Base, 0.25f);
                Vector2 x1 = new Vector2();
                Vector2 x2 = new Vector2();
                Vector2 CenterPolar = new Vector2();
                Vector2 CenterEnd = new Vector2();
                BadaoMissFortuneHelper.RPrediction(PredTarget.UnitPosition.To2D(), target as Obj_AI_Base,
                    out CenterPolar, out CenterEnd, out x1, out x2);
                float dis1 = PredTarget.UnitPosition.To2D().Distance(x1);
                float dis2 = PredTarget.UnitPosition.To2D().Distance(x2);
                Obj_AI_Hero Target = target as Obj_AI_Hero;
                if (Target.Position.To2D().Distance(ObjectManager.Player.Position.To2D()) >= 250 &&
                    (Target.HasBuffOfType(BuffType.Stun) || Target.HasBuffOfType(BuffType.Snare) ||
                    (dis1 >= dis2 && dis1 / Target.MoveSpeed >= 0.6 * 3)))
                {
                    BadaoMainVariables.R.Cast(PredTarget.UnitPosition.To2D());
                    BadaoMissFortuneVariables.TargetRChanneling = target as Obj_AI_Hero;
                    BadaoMissFortuneVariables.CenterPolar = CenterPolar;
                    BadaoMissFortuneVariables.CenterEnd = CenterEnd;
                }
                else if (Target.Position.To2D().Distance(ObjectManager.Player.Position.To2D()) >= 250 &&
                        (Target.HasBuffOfType(BuffType.Stun) || Target.HasBuffOfType(BuffType.Snare) ||
                        (dis2 >= dis1 && dis2 / Target.MoveSpeed >= 0.6 * 3)))
                {
                    BadaoMainVariables.R.Cast(PredTarget.UnitPosition.To2D());
                    BadaoMissFortuneVariables.TargetRChanneling = target as Obj_AI_Hero;
                    BadaoMissFortuneVariables.CenterPolar = CenterPolar;
                    BadaoMissFortuneVariables.CenterEnd = CenterEnd;
                }
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            // stop from canceling R
            if (ObjectManager.Player.IsChannelingImportantSpell() && !BadaoMissFortuneVariables.TargetRChanneling.IsDead &&
                BadaoChecker.BadaoInTheCone(BadaoMissFortuneVariables.TargetRChanneling.Position.To2D(),
                BadaoMissFortuneVariables.CenterPolar,BadaoMissFortuneVariables.CenterEnd, 36))
                return;
            // Q logic
            if (BadaoMissFortuneHelper.UseQCombo() && Orbwalking.CanMove(80))
            {
                // Q2 logic
                var targetQ = TargetSelector.GetTarget(BadaoMainVariables.Q.Range + 600, TargetSelector.DamageType.Physical);
                if (targetQ.BadaoIsValidTarget())
                {
                    if (BadaoMissFortuneVariables.TapTarget.BadaoIsValidTarget() && 
                        targetQ.NetworkId == BadaoMissFortuneVariables.TapTarget.NetworkId)
                    {
                        foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.NetworkId != targetQ.NetworkId &&
                                    x.BadaoIsValidTarget(BadaoMainVariables.Q.Range)))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(hero);
                            var PredHero = Prediction.GetPrediction(hero, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ,0.25f + 
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D())/1400 + Game.Ping/1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredHero.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredHero.UnitPosition.To2D()) + 500);
                            if (ObjectManager.Player.GetSpellDamage(hero, SpellSlot.Q) >= hero.Health &&
                                BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredHero.UnitPosition.To2D(), endpos, 40))
                            {
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Minion minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(minion);
                            var PredMinion = Prediction.GetPrediction(minion, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredMinion.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredMinion.UnitPosition.To2D()) + 500);
                            if (ObjectManager.Player.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health &&
                                BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredMinion.UnitPosition.To2D(), endpos, 40))
                            {
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.NetworkId != targetQ.NetworkId &&
                                                                                x.BadaoIsValidTarget(BadaoMainVariables.Q.Range)))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(hero);
                            var PredHero = Prediction.GetPrediction(hero, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredHero.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredHero.UnitPosition.To2D()) + 500);
                            if (BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredHero.UnitPosition.To2D(), endpos, 40))
                            {
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Minion minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(minion);
                            var PredMinion = Prediction.GetPrediction(minion, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredMinion.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredMinion.UnitPosition.To2D()) + 500);
                            if (BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredMinion.UnitPosition.To2D(), endpos, 40))
                            {
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                    }
                    else if (!BadaoMissFortuneVariables.TapTarget.IsValidTarget() || 
                        targetQ.NetworkId != BadaoMissFortuneVariables.TapTarget.NetworkId)
                    {
                        //20
                        foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.NetworkId != targetQ.NetworkId &&
                                                                                x.BadaoIsValidTarget(BadaoMainVariables.Q.Range)))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(hero);
                            var PredHero = Prediction.GetPrediction(hero, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredHero.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredHero.UnitPosition.To2D()) + 500);
                            if (ObjectManager.Player.GetSpellDamage(hero, SpellSlot.Q) >= hero.Health &&
                                BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredHero.UnitPosition.To2D(), endpos, 20) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 600).Any(x =>
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f + 
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredHero.UnitPosition.To2D(), endpos, 20)))
                            {
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Minion minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(minion);
                            var PredMinion = Prediction.GetPrediction(minion, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredMinion.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredMinion.UnitPosition.To2D()) + 500);
                            if (ObjectManager.Player.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health &&
                                BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredMinion.UnitPosition.To2D(), endpos, 20) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 500).Any(x =>
                                x.NetworkId != minion.NetworkId &&
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredMinion.UnitPosition.To2D(), endpos, 20)))
                            {
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.NetworkId != targetQ.NetworkId &&
                                                                                x.BadaoIsValidTarget(BadaoMainVariables.Q.Range)))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(hero);
                            var PredHero = Prediction.GetPrediction(hero, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredHero.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredHero.UnitPosition.To2D()) + 500);
                            if (BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredHero.UnitPosition.To2D(), endpos, 20) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 600).Any(x =>
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredHero.UnitPosition.To2D(), endpos, 20)))
                            {
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Minion minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(minion);
                            var PredMinion = Prediction.GetPrediction(minion, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredMinion.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredMinion.UnitPosition.To2D()) + 500);
                            if (BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredMinion.UnitPosition.To2D(), endpos, 20) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 500).Any(x =>
                                x.NetworkId != minion.NetworkId &&
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredMinion.UnitPosition.To2D(), endpos, 20)))
                            {
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        //40
                        foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.NetworkId != targetQ.NetworkId &&
                                                                                x.BadaoIsValidTarget(BadaoMainVariables.Q.Range)))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(hero);
                            var PredHero = Prediction.GetPrediction(hero, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredHero.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredHero.UnitPosition.To2D()) + 500);
                            if (ObjectManager.Player.GetSpellDamage(hero, SpellSlot.Q) >= hero.Health &&
                                BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredHero.UnitPosition.To2D(), endpos, 40) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 600).Any(x =>
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredHero.UnitPosition.To2D(), endpos, 40)))
                            {
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Minion minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(minion);
                            var PredMinion = Prediction.GetPrediction(minion, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredMinion.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredMinion.UnitPosition.To2D()) + 500);
                            if (ObjectManager.Player.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health &&
                                BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredMinion.UnitPosition.To2D(), endpos, 40) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 500).Any(x =>
                                x.NetworkId != minion.NetworkId &&
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredMinion.UnitPosition.To2D(), endpos, 40)))
                            {
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.NetworkId != targetQ.NetworkId &&
                                                                                x.BadaoIsValidTarget(BadaoMainVariables.Q.Range)))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(hero);
                            var PredHero = Prediction.GetPrediction(hero, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredHero.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredHero.UnitPosition.To2D()) + 500);
                            if (BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredHero.UnitPosition.To2D(), endpos, 40) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 600).Any(x =>
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredHero.UnitPosition.To2D(), endpos, 40)))
                            {
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                        foreach (Obj_AI_Minion minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range))
                        {
                            var Qpred = BadaoMainVariables.Q.GetPrediction(minion);
                            var PredMinion = Prediction.GetPrediction(minion, 0.25f + ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                            1400 + Game.Ping / 1000));
                            var PredTargetQ = Prediction.GetPrediction(targetQ, 0.25f +
                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D()) / 1400 + Game.Ping / 1000);
                            Vector2 endpos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredMinion.UnitPosition.To2D(),
                                ObjectManager.Player.Position.To2D().Distance(PredMinion.UnitPosition.To2D()) + 500);
                            if (BadaoChecker.BadaoInTheCone(PredTargetQ.UnitPosition.To2D(), PredMinion.UnitPosition.To2D(), endpos, 40) &&
                                !MinionManager.GetMinions(BadaoMainVariables.Q.Range + 500).Any(x =>
                                x.NetworkId != minion.NetworkId &&
                                BadaoChecker.BadaoInTheCone(Prediction.GetPrediction(x, 0.25f +
                                                                                    ObjectManager.Player.Position.To2D().Distance(Qpred.UnitPosition.To2D() /
                                                                                    1400 + Game.Ping / 1000)).UnitPosition.To2D(),
                                                                                    PredMinion.UnitPosition.To2D(), endpos, 40)))
                            {
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted) ;
                                goto abc;
                            }
                        }
                    }
                }
                // Q1 logic
                var targetQ1 = TargetSelector.GetTarget(BadaoMainVariables.Q.Range, TargetSelector.DamageType.Physical);
                if (targetQ1.BadaoIsValidTarget())
                {
                    if (BadaoMainVariables.Q.Cast(targetQ1) == Spell.CastStates.SuccessfullyCasted) ;
                    goto abc;
                }
            abc:;
            }
            // E logic
            if (BadaoMissFortuneHelper.UseECombo() && Orbwalking.CanMove(80))
            {
                Game.PrintChat("E");
                var targetE = TargetSelector.GetTarget(BadaoMainVariables.E.Range + 200, TargetSelector.DamageType.Physical);
                if (targetE.BadaoIsValidTarget())
                {
                    var PredTargetE = Prediction.GetPrediction(targetE, 0.25f);
                    if (PredTargetE.UnitPosition.To2D().Distance(ObjectManager.Player.Position.To2D()) <= BadaoMainVariables.E.Range)
                    {
                        BadaoMainVariables.E.Cast(PredTargetE.UnitPosition);
                        goto xyz;
                    }
                }
            xyz:;
            }
        }

    }
}