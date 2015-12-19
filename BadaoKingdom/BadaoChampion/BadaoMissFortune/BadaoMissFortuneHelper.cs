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
        public static float Q1Damage(Obj_AI_Base target)
        {
            if(BadaoMissFortuneVariables.TapTarget.BadaoIsValidTarget() && target.BadaoIsValidTarget() &&
                target.NetworkId == BadaoMissFortuneVariables.TapTarget.NetworkId)
            {
                if (target is Obj_AI_Minion)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target)
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical,
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage * 0.5f));
                }
                if(target is Obj_AI_Hero)
                {
                    return
                        BadaoMainVariables.Q.GetDamage(target)
                        + (float)Damage.CalcDamage(ObjectManager.Player, target, Damage.DamageType.Physical, 
                        (new double[] { 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 0.9, 1 }
                        [ObjectManager.Player.Level > 11 ? 10 : ObjectManager.Player.Level - 1]
                        * ObjectManager.Player.TotalAttackDamage));
                }
            }
            return BadaoMainVariables.Q.GetDamage(target);
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
    }
}
