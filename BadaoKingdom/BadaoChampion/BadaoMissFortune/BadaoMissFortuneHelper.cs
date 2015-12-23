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
    class BadaoMissFortuneHelper
    {
        // can use skill
        public static bool UseQCombo()
        {
            return BadaoMainVariables.Q.IsReady() && BadaoMissFortuneVariables.ComboQ.GetValue<bool>();
        }
        public static bool UseWCombo()
        {
            return BadaoMainVariables.W.IsReady() && BadaoMissFortuneVariables.ComboW.GetValue<bool>();
        }
        public static bool UseECombo()
        {
            return BadaoMainVariables.E.IsReady() && BadaoMissFortuneVariables.ComboE.GetValue<bool>();
        }
        public static bool UseRCombo()
        {
            return BadaoMainVariables.R.IsReady() && BadaoMissFortuneVariables.ComboR.GetValue<bool>();
        }
        // damage caculation
        public static float GetAADamage(Obj_AI_Hero target)
        {
            if (BadaoMissFortuneVariables.TapTarget.BadaoIsValidTarget() && target.BadaoIsValidTarget() &&
                target.NetworkId == BadaoMissFortuneVariables.TapTarget.NetworkId)
                return (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                                 ObjectManager.Player.TotalAttackDamage);
            else
                return (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                                     ObjectManager.Player.TotalAttackDamage)
                       + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                         (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                         [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                         * ObjectManager.Player.TotalAttackDamage));
        }
        public static float Q1Damage(Obj_AI_Base target)
        {
            if (target.BadaoIsValidTarget())
            {
                if (BadaoMissFortuneVariables.TapTarget.BadaoIsValidTarget() &&
                    target.NetworkId == BadaoMissFortuneVariables.TapTarget.NetworkId)
                {
                    return BadaoMainVariables.Q.GetDamage(target);
                }
                if (target is Obj_AI_Minion)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target)
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage * 0.5f));
                }
                if (target is Obj_AI_Hero)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target)
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage));
                }
                return BadaoMainVariables.Q.GetDamage(target);
            }
            return 0;
        }
        public static float Q2Damage(Obj_AI_Base target, bool dead = false)
        {
            if (!dead)
            {
                if (target is Obj_AI_Minion)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target,1)
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage * 0.5f));
                }
                if (target is Obj_AI_Hero)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target,1)
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage));
                }
            }
            if (dead)
            {
                if (target is Obj_AI_Minion)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target, 1) * 0.5f
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage * 0.5f));
                }
                if (target is Obj_AI_Hero)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target, 1) * 0.5f
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage));
                }
            }
            return BadaoMainVariables.Q.GetDamage(target, 1);
        }
        public static float RDamage(Obj_AI_Base target)
        {
            return (float)(new double[] { 12, 14, 16 }[BadaoMainVariables.R.Instance.Level - 1] * BadaoMainVariables.R.GetDamage(target)
                                  * (1 + ObjectManager.Player.Crit * 0.2));
        }
        public static void RPrediction(Vector2 CastPos, Obj_AI_Base TargetToCheck, out Vector2 CenterPolar, out Vector2 CenterEnd, out Vector2 x1, out Vector2 x2)
        {
            //changeable
            float goc = 36f;
            //process
            float goc1rad = (float)Math.PI * (90f - goc / 2f) / 180f;
            float backward = TargetToCheck.BoundingRadius / (float)Math.Cos(goc1rad);
            CenterPolar = ObjectManager.Player.Position.To2D().Extend(CastPos, -backward);
            CenterEnd = ObjectManager.Player.Position.To2D().Extend(CastPos, 1400);
            Vector2 Rangestraight = ObjectManager.Player.Position.To2D().Extend(CastPos, ObjectManager.Player.BoundingRadius
                                                                                + ObjectManager.Player.AttackRange + TargetToCheck.BoundingRadius);
            float goc2rad = (float)Math.PI * (goc / 2f + 90f) / 180f - (float)Math.Acos(TargetToCheck.BoundingRadius /
                (ObjectManager.Player.BoundingRadius + ObjectManager.Player.AttackRange + TargetToCheck.BoundingRadius));
            x1 = BadaoChecker.BadaoRotateAround(Rangestraight, ObjectManager.Player.Position.To2D(), goc2rad);
            x2 = BadaoChecker.BadaoRotateAround(Rangestraight, ObjectManager.Player.Position.To2D(), -goc2rad);
        }
        //compare damage 
        public static bool Rdamepior()
        {
            float Rdame = (float)(new double[] { 12, 14, 16 }[BadaoMainVariables.R.Instance.Level - 1] * BadaoMainVariables.R.GetDamage(ObjectManager.Player)
                                  * (1 + ObjectManager.Player.Crit * 0.2));
            float Playerdame = (float)Damage.CalcDamage(ObjectManager.Player, ObjectManager.Player, Damage.DamageType.Physical,
                                                 ObjectManager.Player.TotalAttackDamage * 3 / ObjectManager.Player.AttackDelay)
                                                 * (1 + ObjectManager.Player.Crit)
                               + (BadaoMainVariables.Q.IsReady() ? BadaoMainVariables.Q.GetDamage(ObjectManager.Player) : 0);
            return Rdame > Playerdame;
        }
    }
}
