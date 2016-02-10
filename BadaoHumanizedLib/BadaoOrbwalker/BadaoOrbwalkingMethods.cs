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
    public static class BadaoOrbwalkingMethods
    {
        private static Obj_AI_Hero Player { get{ return ObjectManager.Player; } }
        public static void BadaoMoveTo(Vector3 position)
        {
            if (BadaoOrbwalkerVariables._lastCastT >= Orbwalking.LastMoveCommandT)
                goto ABC;
            if (Utils.GameTimeTickCount - Orbwalking.LastAttackCommandT <= 400)
                return;
            if (Utils.GameTimeTickCount - Orbwalking.LastMoveCommandT < 200 + BadaoOrbwalkerVariables._random.Next(0, 100))
                return;
            if (Player.Position.Distance(position) <= 70)
                return;
            ABC:
            Player.IssueOrder(GameObjectOrder.MoveTo, position);
            Orbwalking.LastMoveCommandPosition = position;
            Orbwalking.LastMoveCommandT = Utils.GameTimeTickCount;
        }
        public static void BadaoBeginOrbwalk(AttackableUnit target, Vector3 position)
        {
            if (Utils.GameTimeTickCount - Orbwalking.LastAttackCommandT < 70 + Math.Min(Player.AttackCastDelay * 1000 , Game.Ping))
            {
                return;
            }
            if (target.IsValidTarget() && Orbwalking.InAutoAttackRange(target) && Orbwalking.CanAttack())
            {
                float randomTime = BadaoOrbwalkerVariables._random.NextFloat(0f, Player.AttackDelay);
                if ((Player.AttackDelay < 1f / 1.5f) &&
                    Utils.GameTimeTickCount + Game.Ping / 2 + 25 < Orbwalking.LastAATick + (Player.AttackDelay + randomTime) * 1000)
                    return;
                if (Player.IssueOrder(GameObjectOrder.AttackUnit, target))
                {
                    Orbwalking.LastAttackCommandT = Utils.GameTimeTickCount;
                    BadaoOrbwalkerVariables.LastTarget = target;
                }
                
                return;
            }
            if (Orbwalking.CanMove(80,true))
            {
                int count = Player.ChampionName == "Kalista" ? 4 : 3;
                if ((Player.AttackDelay < 1f / 1.5f) && BadaoOrbwalkerVariables._autoAttackCounter % count == 0
                    && target.IsValidTarget() && Orbwalking.InAutoAttackRange(target) )
                {
                    return;
                }

                BadaoMoveTo(position);
            }
        }
    }
}
