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
  public enum Style : int { FullMask, PartyHat, ColoredGlasses }
  public record MaskFeature(EFeature e)
  {
    public static MaskFeature None { get => new (EFeature.None); }
  }
  public record LifeGainFeature(int amount) : MaskFeature(EFeature.Life);
  public record Mask(
      ETile tileToggle, MaskFeature m)
  {
    Style style = Rand.GetRandomEnumValue<Style>();
    Color color = Color.FromArgb(
      Rand.Value(0,255), 
      Rand.Value(0,255), 
      Rand.Value(0,255));
  }

  public enum ETile : int
  {
    None=0,
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
    Hero
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
  public record Mob(EMob type, int Level, int X, int Y, int Health, int Attack, int Def);

  public record Head(Mask? mask);

  // Egg state has 0 heads.
  public record Hydra(Mob m, Mask[] masks, Head[] heads) : Mob(m);
  public record Item(int Level, int X, int Y, Mask? mask);
}
