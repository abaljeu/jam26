namespace mask
{
  public record Block(EnableFlag EnableMode = EnableFlag.EnabledBy,
    EFeature Feature = EFeature.None)
  {
    List<ETile> Tiles = [];
    public static TileStack Stack { get; } = new TileStack();
    public static Block Snow() => new Block().Add(ETile.Snow);
    public static Block Rock() => new Block().Add(ETile.Rock);
    public static Block Dirt() => new Block().Add(ETile.Dirt);
    public static Block Ladder() => new Block().Add(ETile.Ladder);

    public Block Add(ETile e) { Tiles.Add(e); return this; }
  }

  public class TileStack : List<ETile>
  {
    public TileStack() { }
    public TileStack Add(ETile t)
    {
      base.Add(t);
      return this;
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