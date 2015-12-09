using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;

namespace Anti_Rito
{
    public static class OneTickOneSpell
    {
        public static Obj_AI_Hero Player { get{ return ObjectManager.Player; } }
        public static LastSpellCast LastSpell = new LastSpellCast();
        public static List<LastSpellCast> LastSpellsCast = new List<LastSpellCast>();
        public static int BlockedCount = 0;
        public static void Init()
        {
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Drawing.OnDraw += onDrawArgs =>
                {
                    if (Config.config.SubMenu(Config.OneSpell.Name).Item("Drawing").GetValue<bool>())
                    {
                        Drawing.DrawText(Drawing.Width - 180, 100, System.Drawing.Color.Lime, "Blocked " + BlockedCount + " Spells");
                    }
                };
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!Config.config.SubMenu(Config.OneSpell.Name).Item("Enable").GetValue<bool>())
                return;
            if (!sender.Owner.IsMe)
                return;
            if (!(new SpellSlot[] {SpellSlot.Q,SpellSlot.W,SpellSlot.E,SpellSlot.R,SpellSlot.Summoner1,SpellSlot.Summoner2
                ,SpellSlot.Item1,SpellSlot.Item2,SpellSlot.Item3,SpellSlot.Item4,SpellSlot.Item5,SpellSlot.Item6,SpellSlot.Trinket})
                .Contains(args.Slot))
                return;
            if (Utils.GameTimeTickCount - LastSpell.CastTick < 50)
            {
                args.Process = false;
                BlockedCount += 1;
            }
            else
            {
                LastSpell = new LastSpellCast() { Slot = args.Slot, CastTick = Utils.GameTimeTickCount};
            }
            if (LastSpellsCast.Any(x => x.Slot == args.Slot))
            {
                LastSpellCast spell = LastSpellsCast.FirstOrDefault(x => x.Slot == args.Slot);
                if (spell != null)
                {
                    if (Utils.GameTimeTickCount - spell.CastTick <= 250)
                    {
                        args.Process = false;
                        BlockedCount += 1;
                    }
                    else
                    {
                        LastSpellsCast.RemoveAll(x => x.Slot == args.Slot);
                        LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Utils.GameTimeTickCount });
                    }
                }
                else
                {
                    LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Utils.GameTimeTickCount });
                }
            }
            else
            {
                LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Utils.GameTimeTickCount });
            }
        }
        public class LastSpellCast
        {
            public SpellSlot Slot = SpellSlot.Unknown;
            public int CastTick = 0;
        }
    }
}
