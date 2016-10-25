using System;
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
    public static class BadaoGravesJungle
    {
        public static Obj_AI_Hero Player { get{ return ObjectManager.Player; } }
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear)
                return;
            if (Player.ManaPercent < BadaoGravesVariables.ManaJungle.GetValue<Slider>().Value)
                return;
            if (!sender.IsMe)
                return;
            if (args.SData.IsAutoAttack() && args.Target != null && args.Target.Team == GameObjectTeam.Neutral)
            {
                int delay2 = 200;
                Utility.DelayAction.Add(Game.Ping - Game.Ping, () =>
                {
                    if (BadaoMainVariables.E.IsReady() && BadaoGravesVariables.JungleE.GetValue<bool>())
                    {
                        var position = Player.Position.To2D().Extend(Game.CursorPos.To2D(), BadaoMainVariables.E.Range);
                        if (args.Target.Position.To2D().Distance(position) <= -250 + Player.AttackRange + Player.BoundingRadius
                            /*&& !Utility.UnderTurret(position.To3D(), true)*/)
                        {
                            BadaoMainVariables.E.Cast(position);
                            for (int i = 0; i < delay2; i = i + 5)
                            {
                                Game.SendEmote(Emote.Dance); Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                Utility.DelayAction.Add(i, () => {
                                    Game.SendEmote(Emote.Dance);
                                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                    Player.IssueOrder(GameObjectOrder.AttackUnit, args.Target);
                                });
                            }

                        }
                        else
                        {
                            var points = Geometry.CircleCircleIntersection(Player.Position.To2D(), args.Target.Position.To2D(), 425,
                                -250 + Player.AttackRange + Player.BoundingRadius);
                            var pos = points.Where(x => !NavMesh.GetCollisionFlags(x.X, x.Y).HasFlag(CollisionFlags.Wall)
                                && !NavMesh.GetCollisionFlags(x.X, x.Y).HasFlag(CollisionFlags.Building)/* && !Utility.UnderTurret(x.To3D(), true)*/)
                                .OrderBy(x => x.Distance(Game.CursorPos)/*x.To3D().CountEnemiesInRange(1000)*/).FirstOrDefault();
                            if (pos != null)
                            {
                                BadaoMainVariables.E.Cast(pos);
                                for (int i = 0; i < delay2; i = i + 5)
                                {
                                    Game.SendEmote(Emote.Dance); Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                    Utility.DelayAction.Add(i, () => {
                                        Game.SendEmote(Emote.Dance);
                                        Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                        Player.IssueOrder(GameObjectOrder.AttackUnit, args.Target);
                                    });
                                }
                            }
                        }
                    }
                }
                );
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear)
                return;
            if (Player.ManaPercent < BadaoGravesVariables.ManaJungle.GetValue<Slider>().Value)
                return;
            if (BadaoMainVariables.Q.IsReady() && BadaoGravesVariables.JungleQ.GetValue<bool>())
            {
                var target  = MinionManager.GetMinions(BadaoMainVariables.Q.Range, MinionTypes.All, MinionTeam.Neutral
                                        , MinionOrderTypes.MaxHealth).FirstOrDefault();
                if (target.BadaoIsValidTarget() && BadaoMath.GetFirstWallPoint(Player.Position.To2D(), target.Position.To2D()) == null)
                {
                    BadaoMainVariables.Q.Cast(target);
                }
            }
        }
    }
}
