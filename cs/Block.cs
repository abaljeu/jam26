
namespace mask
{
  public record Block(EnableFlag EnableMode = EnableFlag.EnabledBy,
    EFeature Feature = EFeature.None)
  {
    public static List<Tile> Stack { get; } = new List<Tile>();
    public static Block Snow() => new Block().Add(ETile.Snow);
    public static Block Rock() => new Block().Add(ETile.Rock);
    public static Block Dirt() => new Block().Add(ETile.Dirt);
    public static Block Ladder() => new Block().Add(ETile.Ladder);

    public Block Add(ETile e) { Stack.Add(new Tile(e)); return this; }

    internal void Clear()
    {
      Stack.Clear();
    }
  }



  public record HydraVision(Hydra h)
  {
    public bool DefaultSees(ETile t)
    {
      switch (t)
      {
        case ETile.Snow:
        case ETile.Rock:
        case ETile.Dirt:
        case ETile.Ladder:
          return true;
        case ETile.Bridge:
        default: return false;
      }
    }
    public bool Sees(Tile t)
    {
      foreach (var m in h.masks)
      {
        if (m.tileToggle == t.tileType)
          return !DefaultSees(t.tileType);
      }
      return DefaultSees(t.tileType);
    }
  }
}