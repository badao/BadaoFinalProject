using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using ItemData = LeagueSharp.Common.Data.ItemData;

namespace BadaoKingdom.BadaoChampion.BadaoKatarina
{
    using static BadaoMainVariables;
    using static BadaoKatarinaVariables;
    using static BadaoKatarinaHelper;
    public static class BadaoKatarinaCombo
    {
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            // blocking AA
            if (ComboDontAttack.GetValue<bool>() && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (Daggers.Any(x => x.Dagger.Position.Distance(Player.Position) <= 150 + ComboDontAttackRange.GetValue<Slider>().Value)
                    && !Daggers.Any(x => x.Dagger.Position.Distance(Player.Position) <= 150))
                    Orbwalking.Attack = false;
                else
                    Orbwalking.Attack = true;

            }
            else
            {
                Orbwalking.Attack = true;
            }

            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;

            // cancel R
            if (ComboCancelRNoTarget.GetValue<bool>() && Player.IsChannelingImportantSpell() 
                && Player.CountEnemiesInRange(R.Range) == 0 && Environment.TickCount >= LastRMis)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
            // ks
            if (ComboCancelRForKS.GetValue<bool>() || !Player.IsChannelingImportantSpell())
            {
                var targetQ = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (targetQ.IsValidTarget() && Q.IsReady() && GetQDamage(targetQ) >= targetQ.Health)
                {
                    Q.Cast(targetQ);
                }

                var targetE = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (targetE.IsValidTarget() && E.IsReady() && GetEDamage(targetE) >= targetE.Health)
                {
                    E.Cast(targetE);
                }

                var targetEQ = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (targetEQ.IsValidTarget() && E.IsReady() && Q.IsReady() && GetEDamage(targetEQ) + GetQDamage(targetEQ) >= targetEQ.Health)
                {
                    E.Cast(targetEQ);
                }

                if (Q.IsReady() && E.IsReady())
                {
                    foreach (var hero in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range + Q.Range + 150) && GetQDamage(x) >= x.Health))
                    {
                        var nearest = GetEVinasun().MinOrDefault(x => x.Position.Distance(hero.Position));
                        if (nearest != null && nearest.Position.Distance(hero.Position) <= 150 + Q.Range)
                        {
                            var pos = nearest.Position.To2D().Extend(hero.Position.To2D(), 150);
                            E.Cast(pos);
                        }
                    }
                }
            }
            // normal
            if (Player.IsChannelingImportantSpell())
                return;
            //hextechgunblade
            if (ItemData.Hextech_Gunblade.GetItem().IsReady())
            {
                var target = TargetSelector.GetTarget(700, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget())
                {
                    ItemData.Hextech_Gunblade.GetItem().Cast(target);
                }
            }
            //cutlass
            if (ItemData.Bilgewater_Cutlass.GetItem().IsReady())
            {
                var target = TargetSelector.GetTarget(550, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget())
                {
                    ItemData.Bilgewater_Cutlass.GetItem().Cast(target);
                }
            }
            // W
            if (W.IsReady() && !E.IsReady())
            {
                var target = TargetSelector.GetTarget(300, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget())
                {
                    W.Cast();
                }
            }
            // Q
            if (Q.IsReady())
            {
                if (!(W.IsReady() && E.IsReady()))
                {
                    var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                    if (target.IsValidTarget())
                    {
                        Q.Cast(target);
                    }
                }
            }
            //E
            if (E.IsReady())
            {
                var EdaggerTarget = TargetSelector.GetTarget(E.Range + 200, TargetSelector.DamageType.Magical);
                var ETarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                var EdaggerOthers = HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range + 200) && IsDaggerFixed(x));
                if (EdaggerTarget != null && IsDaggerFixed(EdaggerTarget))
                {
                    var EdaggerTargetdagger = GetFixedDagger(EdaggerTarget);
                    if (EdaggerTargetdagger.Dagger != null)
                    {
                        CastEFixedDagger(EdaggerTargetdagger, EdaggerTarget);
                    }
                }
                else if (EdaggerOthers.Any())
                {
                    var target = EdaggerOthers.MinOrDefault(x => x.Health);
                    var targetdagger = GetFixedDagger(target);
                    if (targetdagger.Dagger != null)
                    {
                        CastEFixedDagger(targetdagger, target);
                    }
                }
                else if (ETarget != null && !(Q.IsReady() && !W.IsReady()))
                {
                    var pos = Player.Position.To2D().Extend(ETarget.Position.To2D(),Player.Distance(ETarget) + 150);
                    E.Cast(pos);
                }
                // gapclose 
                else if (Q.IsReady() || W.IsReady() || R.IsReady())
                {
                    var Vinasun = PickableDaggers.Where(x => x.Dagger.Position.CountEnemiesInRange(E.Range) > 0).MaxOrDefault(x => x.Dagger.Position.CountEnemiesInRange(E.Range));
                    if (Vinasun != null)
                        E.Cast(Vinasun.Dagger.Position);
                }
            }
            // R
            if (R.Level >= 1 && R.Instance.CooldownExpires <= Game.Time)
            {
                //Game.PrintChat("1");
                {
                    var expires = (E.Instance.CooldownExpires - Game.Time) * 1000;
                    if (E.IsReady() || expires < 50)
                    {
                        //Game.PrintChat("2");
                        var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                        if (target.IsValidTarget())
                        {
                            E.Cast(target.Position);
                        }

                    }
                    else if (R.IsReady())
                    {
                        //Game.PrintChat("3");
                        var target = TargetSelector.GetTarget(R.Range - 200, TargetSelector.DamageType.Magical);
                        if (target.IsValidTarget())
                        {
                            if (W.IsReady())
                                W.Cast();
                            else if (Q.IsReady())
                                Q.Cast(target);
                            else
                                R.Cast();
                        }
                    }
                }
            }

        }
    }
}
