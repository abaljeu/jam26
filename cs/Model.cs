using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mask
{
  public enum Property
  { 
    None = 0,
    SuppressMask,
    TrapVision
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
  public record Block(string Name, Property AffectedBy, EnableFlag EnableMode);

  public record Tile(Block sourceBlock);
  class GameObjects
  {
    public Block TrapBlock { get => new Block("Trap", Property.TrapVision, EnableFlag.EnabledBy); }
  }
  public record Mob(string Name, int Level, int X, int Y, int Health, int Attack, int Def);

  public record Head(Mask? mask);
  public record Hydra(Mob mob, Mask[] masks, Head[] heads);
  public record Item(int Level, int X, int Y, Mask? mask);
  public record Mask(Property property);
}
