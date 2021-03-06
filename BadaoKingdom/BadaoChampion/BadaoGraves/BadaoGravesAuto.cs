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
    public static class BadaoGravesAuto
    {
        private static Obj_AI_Hero Player {get { return ObjectManager.Player; } }
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoGravesVariables.AutoSmite.GetValue<bool>() && BadaoMainVariables.Smite != SpellSlot.Unknown && BadaoMainVariables.Smite.IsReady())
            {
                var creep = MinionManager.GetMinions(800, MinionTypes.All, MinionTeam.Neutral)
                    .Where(x => x.CharData.BaseSkinName.Contains("SRU_Dragon") || x.CharData.BaseSkinName.Contains("SRU_Baron"));
                foreach (var x in creep.Where(y => Player.Distance(y.Position) <= Player.BoundingRadius + 500 + y.BoundingRadius))
                {
                    if (x != null && x.Health <= BadaoChecker.BadaoGetSmiteDamage())
                        Player.Spellbook.CastSpell(BadaoMainVariables.Smite, x);
                }
            }
            if (BadaoMainVariables.R.IsReady() && BadaoGravesVariables.AutoRKS.GetValue<bool>())
            {
                foreach (var hero in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget(BadaoMainVariables.R.Range)))
                {
                    if (BadaoMainVariables.R.GetDamage(hero) >= hero.Health)
                    {
                        BadaoMainVariables.R.Cast(hero);
                    }
                }
            }
        }
    }
}
