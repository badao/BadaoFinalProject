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
    public static class BadaoMissFortuneCombo
    {
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
        }

        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (unit.IsMe)
            {
                BadaoMissFortuneVariables.TapTarget = target as Obj_AI_Hero;
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
                BadaoMissFortuneVariables.TapTarget = null;
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            // Q2 logic
            if (BadaoMissFortuneHelper.UseQCombo())
            {
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
                                if(BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted)
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
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted)
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
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted)
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
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted)
                                    goto abc;
                            }
                        }
                    abc:;
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
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(hero) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
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
                                if (BadaoMainVariables.Q.Cast(minion) == Spell.CastStates.SuccessfullyCasted)
                                    goto xyz;
                            }
                        }
                    xyz:;
                    }
                }
            }

        }

    }
}
