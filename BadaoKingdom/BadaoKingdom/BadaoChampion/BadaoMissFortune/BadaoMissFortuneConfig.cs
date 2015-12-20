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
    public static class BadaoMissFortuneConfig
    {
        public static Menu config;
        public static void BadaoActivate()
        {
            // spells init
            BadaoMainVariables.Q = new Spell(SpellSlot.Q, 650);
            BadaoMainVariables.Q.SetTargetted(0.25f, 1400);
            BadaoMainVariables.W = new Spell(SpellSlot.W); 
            BadaoMainVariables.W = new Spell(SpellSlot.E, 1000); // radius 200
            BadaoMainVariables.W = new Spell(SpellSlot.R, 1400); // chua biet goc

            // main menu
            config = new Menu("BadaoKingdom " + ObjectManager.Player.ChampionName, ObjectManager.Player.ChampionName, true);
            config.SetFontStyle(System.Drawing.FontStyle.Bold, SharpDX.Color.YellowGreen);

            // orbwalker menu
            Menu orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            BadaoMainVariables.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            config.AddSubMenu(orbwalkerMenu);

            // TS
            Menu ts = config.AddSubMenu(new Menu("Target Selector", "Target Selector")); ;
            TargetSelector.AddToMenu(ts);

            // Combo
            Menu Combo = config.AddSubMenu(new Menu("Combo", "Combo"));
            BadaoMissFortuneVariables.ComboQ = Combo.AddItem(new MenuItem("ComboQ","Q")).SetValue(true);
            BadaoMissFortuneVariables.ComboW = Combo.AddItem(new MenuItem("ComboW", "W")).SetValue(true);
            BadaoMissFortuneVariables.ComboE = Combo.AddItem(new MenuItem("ComboE", "E")).SetValue(true);
            BadaoMissFortuneVariables.ComboR = Combo.AddItem(new MenuItem("ComboR", "R")).SetValue(true);

            // attach to mainmenu
            config.AddToMainMenu();
        }
    }
}
