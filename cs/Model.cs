using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mask
{
    public enum EFeature
    {
        None = 0,
        SuppressMask,
        TrapVision,
        Life,
        Construction,
        Clown,
        Party
    }
    public enum Style : int { FullMask, PartyHat, ColoredGlasses }

    public record MaskOnGround(Mask mask, int Level, int X, int Y)
    {
    }
    public class Mask
    {
        public EFeature WhichEffect;
        public ETile tileToggle;

        public Mask(ETile t, EFeature m)
        {
            Style style = Rand.GetRandomEnumValue<Style>();
            Color color = Color.FromArgb(
              Rand.Value(0, 255),
              Rand.Value(0, 255),
              Rand.Value(0, 255));

            WhichEffect = m;
            tileToggle = t;
        }
    }

    public enum ETile : int
    {
        None = 0,
        Snow,
        Rock,
        Dirt,
        Ladder,
        FloorLadder,
        Bridge,
        Wall
    }
    public enum EMob
    {
        None = 0,
        Hydra,
        Slime,
        Hero,
        Skeleton,
        Ghost,
    }
    public enum LocationProperty
    {
        Hollow,
        Floor,
        Wall,
        UpLadder,
        DownLadder,
        Trapped,

    }
    public enum EnableFlag { EnabledBy, DisabledBy }
    public class GameObjects
    {
        public Block TrapBlock
        { get => new Block(
          EnableFlag.EnabledBy,
          EFeature.TrapVision).Add(ETile.Dirt);
        }
    }
    public class Mob
    {
        public EMob type;
        public int Level;
        public int X;
        public int Y;
        public int Health;
        public int Attack;
        public int Def;

        public Mob(EMob _type, int _Level, int _X, int _Y, int _Health,
            int _Attack, int _Def)
        {
            type = _type;
            Level = _Level;
                
            X = _X;
            Y = _Y;
            Health = _Health;
            Attack = _Attack;
            Def = _Def;
        }
    }
    public record Head(Mask? mask);

    // Egg state has 0 heads.
    public class Hydra : Mob
    {
        public List<Mask> masks = new List<Mask>(); // carried, not worn
        public List<Head> heads = new List<Head>();
        public Hydra(Mob m)
            : base(m.type, m.Level, m.X, m.Y, m.Health, m.Attack, m.Def) 
        {
            heads.Add(new Head(null));
        }
        public IEnumerable<Mask> EquippedMasks
        {
            get
            {
                foreach (var h in heads)
                {
                    if (h.mask != null)
                        yield return h.mask;
                }
            }
        }

        public int HP;
        public EFeature? CurrentlyWornMask;
    }

    public record Item(int Level, int X, int Y, Mask? mask);
}
