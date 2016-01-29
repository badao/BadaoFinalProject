using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoShen
{
    public static class BadaoShenCombo
    {
        public static void BadaoActive()
        {
            Orbwalking.OnAttack += Orbwalking_OnAttack;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (BadaoMainVariables.E.IsReady() && Orbwalking.CanMove(80) && BadaoShenVariables.ComboEIfHit.GetValue<bool>())
            {
                foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget() 
                && x.Position.To2D().Distance(ObjectManager.Player.Position.To2D()) <= BadaoMainVariables.E.Range))
                {
                    List<Obj_AI_Hero> a = new List<Obj_AI_Hero>();
                    var EPred = BadaoMainVariables.E.GetPrediction(hero);
                    var EndPos = Geometry.Extend(ObjectManager.Player.Position.To2D(), EPred.CastPosition.To2D(), 650);
                    foreach (Obj_AI_Hero hero2 in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget() &&
                    x.NetworkId != hero.NetworkId &&
                    x.Position.To2D().Distance(ObjectManager.Player.Position.To2D()) <= BadaoMainVariables.E.Range))
                    {
                        var PredHero2 = Prediction.GetPrediction(hero2, (hero2.Position.To2D().Distance(ObjectManager.Player.Position.To2D()) /
                            BadaoMainVariables.E.Range) * 0.3f);
                        if (hero2.Position.To2D().Distance(ObjectManager.Player.Position.To2D(), EndPos, true)
                            <= 50 + hero.BoundingRadius &&
                            PredHero2.UnitPosition.To2D().Distance(ObjectManager.Player.Position.To2D(), EndPos, true)
                            <= 50 + hero.BoundingRadius)
                        {
                            a.Add(hero2);
                        }
                    }
                    if (a.Count() >= BadaoShenVariables.ComboEIfWillHit.GetValue<Slider>().Value + 1)
                    {
                        BadaoMainVariables.E.Cast(EndPos);
                    }
                }
            }
            foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget() &&
                     x.Position.To2D().Distance(ObjectManager.Player.Position.To2D()) <= BadaoMainVariables.E.Range))
            {
                if (BadaoShenHelper.UseEComboToTarget(hero) && Orbwalking.CanMove(80))
                {
                    var PredPos = BadaoMainVariables.E.GetPrediction(hero).CastPosition.To2D();
                    Vector2 CastPos = new Vector2();
                    if (PredPos.Distance(ObjectManager.Player.Position.To2D()) <= 450)
                    {
                        CastPos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredPos, PredPos.Distance(ObjectManager.Player.Position.To2D()) + 200);
                    }
                    else
                    {
                        CastPos = Geometry.Extend(ObjectManager.Player.Position.To2D(), PredPos, 650);
                    }
                    if (PredPos.Distance(ObjectManager.Player.Position.To2D()) <= 650)
                    {
                        BadaoMainVariables.E.Cast(hero);
                        break;
                    }
                }
            }
            if (BadaoShenHelper.UseQCombo())
            {
                foreach (Obj_AI_Hero hero in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget()))
                {
                    var PredHero = Prediction.GetPrediction(hero, 0.25f);
                    if (hero.Position.To2D().Distance(ObjectManager.Player.Position.To2D(), BadaoShenVariables.SwordPos, true)
                        <= 50 + hero.BoundingRadius &&
                        PredHero.UnitPosition.To2D().Distance(ObjectManager.Player.Position.To2D(), BadaoShenVariables.SwordPos, true)
                        <= 50 + hero.BoundingRadius)
                    {
                        BadaoMainVariables.Q.Cast();
                    }
                }
            }
        }

        private static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (BadaoShenHelper.UseQCombo())
            {
                BadaoMainVariables.Q.Cast();
            }
        }
        private static void Orbwalking_OnAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (unit is Obj_AI_Hero && unit.IsEnemy && target is Obj_AI_Hero && target.IsAlly && target.BadaoIsValidTarget(float.MaxValue, false)
                && target.Position.To2D().Distance(BadaoShenVariables.SwordPos) <= BadaoMainVariables.W.Range
                && BadaoShenHelper.UseWCombo(target as Obj_AI_Hero))
            {
                BadaoMainVariables.W.Cast();
            }
        }
    }
}
