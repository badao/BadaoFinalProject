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
    public static class Config
    {
        public static Menu config, OneSpell;
        public static void Init()
        {
            config = new Menu("Anti-Rito","Anti-Rito", true);
            OneSpell = config.AddSubMenu(new Menu("OneSpellOneTick", "OneSpellOneTick"));
            OneSpell.AddItem(new MenuItem("Enable","Enable").SetValue(true));
            OneSpell.AddItem(new MenuItem("Drawing", "Draw Block Count").SetValue(true));
            //OneSpell.AddItem(new MenuItem("Recast", "Re-cast blocked spell after a delay?").SetValue(true));
            config.AddToMainMenu();
        }
    }
}
