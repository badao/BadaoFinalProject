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
    public static class BadaoOrbwalkerVariables
    {
        public static List<SpellSlot> _spells = new List<SpellSlot>
        {
            SpellSlot.Q, SpellSlot.W,SpellSlot.E,SpellSlot.R,SpellSlot.Summoner1,SpellSlot.Summoner2
        };
        public static int _lastCastT;
        public static int _autoAttackCounter;
        public static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public static Orbwalking.Orbwalker BadaoOrbwalk;
        public static AttackableUnit LastTarget;
        public static Vector3 _orbwalkingPoint;
        public static Vector3 OrbwalkingPoint
        {
            get 
            {
                return
                    _orbwalkingPoint.To2D().IsValid() ?
                    _orbwalkingPoint :
                    Game.CursorPos;
            }
            set
            {
                _orbwalkingPoint = value;
            }
        }
    }
}
