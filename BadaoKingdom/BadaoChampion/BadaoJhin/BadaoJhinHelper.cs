using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoJhin
{
    public static class BadaoJhinHelper
    {
        public static bool UseAutoKS()
        {
            return BadaoJhinVariables.AutoKS.GetValue<bool>();
        }
        public static bool UseWAuto()
        {
            return BadaoMainVariables.W.IsReady() && BadaoJhinVariables.AutoW.GetValue<bool>();
        }
        public static bool UseRAuto()
        {
            return BadaoMainVariables.R.IsReady() && BadaoJhinVariables.AutoR.GetValue<bool>();
        }
        public static bool UseQJungle()
        {
            return BadaoMainVariables.Q.IsReady() && BadaoJhinVariables.JungleClearQ.GetValue<bool>();
        }
        public static bool UseWJungle()
        {
            return BadaoMainVariables.W.IsReady() && BadaoJhinVariables.JungleClearW.GetValue<bool>();
        }
        public static bool UseEJungle()
        {
            return BadaoMainVariables.E.IsReady() && BadaoJhinVariables.JungleClearE.GetValue<bool>();
        }
        public static bool UseQLane()
        {
            return BadaoMainVariables.Q.IsReady() && BadaoJhinVariables.LaneClearQ.GetValue<bool>();
        }
        public static bool UseQHarass()
        {
            return BadaoMainVariables.Q.IsReady() && BadaoJhinVariables.HarassQ.GetValue<bool>();
        }
        public static bool UseWHarass()
        {
            return BadaoMainVariables.W.IsReady() && BadaoJhinVariables.HarassW.GetValue<bool>();
        }
        public static bool UseEHarass()
        {
            return BadaoMainVariables.E.IsReady() && BadaoJhinVariables.HarassE.GetValue<bool>();
        }
        public static bool UseECombo()
        {
            return BadaoMainVariables.E.IsReady() && BadaoJhinVariables.ComboE.GetValue<bool>();
        }
        public static bool UseWCombo()
        {
            return BadaoMainVariables.W.IsReady() && BadaoJhinVariables.ComboW.GetValue<bool>();
        }
        public static bool UseQCombo()
        {
            return BadaoMainVariables.Q.IsReady() && BadaoJhinVariables.ComboQ.GetValue<bool>();
        }
        public static bool CanHarassMana()
        {
            return ObjectManager.Player.Mana / ObjectManager.Player.MaxMana * 100 >= BadaoJhinVariables.HarassMana.GetValue<Slider>().Value;
        }
        public static bool CanLaneClearMana()
        {
            return ObjectManager.Player.Mana / ObjectManager.Player.MaxMana * 100 >= BadaoJhinVariables.LaneClearMana.GetValue<Slider>().Value;
        }
        public static bool CanJungleClearMana()
        {
            return ObjectManager.Player.Mana / ObjectManager.Player.MaxMana * 100 >= BadaoJhinVariables.JungleClearMana.GetValue<Slider>().Value;
        }
        public static bool CanAutoMana()
        {
            return ObjectManager.Player.Mana / ObjectManager.Player.MaxMana * 100 >= BadaoJhinVariables.AutoMana.GetValue<Slider>().Value;
        }
        public static bool HasJhinPassive(Obj_AI_Hero target)
        {
            return BadaoJhinPassive.JhinPassive.Any(x => Geometry.Distance(x.Position.To2D(),(target.Position.To2D())) <= 50);
        }
        public static List<QInfo> GetQInfo()
        {
            List<QInfo> QInfo = new List<QInfo>();
            List<Obj_AI_Base> Targets = new List<Obj_AI_Base>();
            var heroes = HeroManager.Enemies.Where(x => x != null && x.IsValid && !x.IsDead);
            var laneMinions = MinionManager.GetMinions(ObjectManager.Player.Position, 2100);
            var jungleMinions = MinionManager.GetMinions(ObjectManager.Player.Position, 2100, MinionTypes.All, MinionTeam.Neutral);
            Targets.AddRange(heroes);
            Targets.AddRange(laneMinions);
            Targets.AddRange(jungleMinions);
            var TargetsQ = Targets.Where(x => x.Distance(ObjectManager.Player.Position) <= 600);
            foreach (var target in TargetsQ)
            {
                List<int> usedID = new List<int>();
                int deathCount = 0;
                int deathCount1 = 0;
                int deathCount2 = 0;
                int deathCount3 = 0;
                int deathCount4 = 0;
                usedID.Add(target.NetworkId);
                if (BadaoMainVariables.Q.GetDamage(target)* (1f + deathCount * 0.35f) > target.Health)
                    deathCount += 1;
                deathCount1 = deathCount;
                //target2
                var target2 = Targets.OrderBy(x => x.Distance(target))
                    .Where(x => !usedID.Contains(x.NetworkId) && x.Distance(target) <= 500).FirstOrDefault();
                if (target2 ==  null)
                {
                    QInfo.Add(new QInfo()
                    {
                        QTarget = target,
                        BounceTargets = new List<TargetInfo>()
                            {
                                new TargetInfo() { Target = target, DeathCount = 0}
                            },
                        DeathCount = deathCount
                    });
                    continue;
                }
                usedID.Add(target2.NetworkId);
                if (BadaoMainVariables.Q.GetDamage(target2)*(1f + deathCount * 0.35f) > target2.Health)
                    deathCount += 1;
                deathCount2 = deathCount;
                //target3
                var target3 = Targets.OrderBy(x => x.Distance(target))
                    .Where(x => !usedID.Contains(x.NetworkId) && x.Distance(target2) <= 500).FirstOrDefault();
                if (target3 == null)
                {
                    QInfo.Add(new QInfo()
                    {
                        QTarget = target,
                        BounceTargets = new List<TargetInfo>()
                            {
                                new TargetInfo() { Target = target, DeathCount = 0 },
                                new TargetInfo() { Target = target2, DeathCount = deathCount1 }
                            },
                        DeathCount = deathCount
                    });
                    continue;
                }
                usedID.Add(target3.NetworkId);
                if (BadaoMainVariables.Q.GetDamage(target3)* (1f + deathCount * 0.35f) > target3.Health)
                    deathCount += 1;
                deathCount3 = deathCount;
                //target4
                var target4 = Targets.OrderBy(x => x.Distance(target))
                    .Where(x => !usedID.Contains(x.NetworkId) && x.Distance(target3) <= 500).FirstOrDefault();
                if (target4 == null)
                {
                    QInfo.Add(new QInfo()
                    {
                        QTarget = target,
                        BounceTargets = new List<TargetInfo>()
                            {
                                new TargetInfo() { Target = target, DeathCount = 0 },
                                new TargetInfo() { Target = target2, DeathCount = deathCount1 },
                                new TargetInfo() { Target = target3, DeathCount = deathCount2 }
                            },
                        DeathCount = deathCount
                    });
                    continue;
                }
                if (BadaoMainVariables.Q.GetDamage(target4) * (1f + deathCount * 0.35f) > target4.Health)
                    deathCount += 1;
                deathCount4 = deathCount;
                QInfo.Add(new QInfo()
                {
                    QTarget = target,
                    BounceTargets = new List<TargetInfo>()
                        {
                            new TargetInfo() { Target = target, DeathCount = 0 },
                            new TargetInfo() { Target = target2, DeathCount = deathCount1 },
                            new TargetInfo() { Target = target3, DeathCount = deathCount2 },
                            new TargetInfo() { Target = target4, DeathCount = deathCount3 }
                        },
                    DeathCount = deathCount
                });
            }
            return QInfo;
        }
        public class QInfo
        {
            public Obj_AI_Base QTarget = null;
            public List<TargetInfo> BounceTargets = new List<TargetInfo>();
            public int DeathCount = 0;
        }
        public class TargetInfo
        {
            public Obj_AI_Base Target = null;
            public int DeathCount = 0;

        }
    }
}
