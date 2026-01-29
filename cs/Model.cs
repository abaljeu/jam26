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
    Life
  }
  public enum Style { FullMask, PartyHat, ColoredGlasses }
  public record MaskFeature(EFeature e);
  public record LifeGainFeature(int amount) : MaskFeature(EFeature.Life);
  public record Mask(Color color, Style style,MaskFeature m);

  public enum ETile
  {
    Space,
    Rock,
    Dirt,
    Ladder
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
  class GameObjects
  {
    public Block TrapBlock { get => new Block(ETile.Dirt,
      EnableFlag.EnabledBy,
      EFeature.TrapVision);
    }
  }
  public record Mob(string Name, int Level, int X, int Y, int Health, int Attack, int Def);

  public record Head(Mask? mask);
  public record Hydra(Mob mob, Mask[] masks, Head[] heads);
  public record Item(int Level, int X, int Y, Mask? mask);
}
