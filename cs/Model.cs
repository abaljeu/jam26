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
  public record Mask(Color color, Style style,
      ETile tileToggle, MaskFeature m);

  public enum ETile
  {
    None=0,
    Snow,
    Rock,
    Dirt,
    Ladder,
    FloorLadder,
    Bridge
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
    public Block TrapBlock 
    { get => new Block(
      EnableFlag.EnabledBy,
      EFeature.TrapVision).Add(ETile.Dirt);
    }
  }
  public record Mob(string Name, int Level, int X, int Y, int Health, int Attack, int Def);

  public record Head(Mask? mask);

  // Egg state has 0 heads.
  public record Hydra(Mob m, Mask[] masks, Head[] heads) : Mob(m);
  public record Item(int Level, int X, int Y, Mask? mask);
}
