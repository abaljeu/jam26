using mask;

namespace mask
{
  public record Layer(int MapX, int MapY)
  {
    public Layer(Level level) : this(level.X, level.Y)
    {
    }
    public Tile[,] tiles = new Tile[MapX, MapY];
  }
  // Layer l = new ExampleLayer();
  public record ExampleLayer : Layer
  {
    public ExampleLayer() : base(20, 20)
    {
      for (int i = 0; i < 20; i++)
      {
        for (int j = 0; j < 20; j++)
        {
          tiles[i, j] = new Tile(PickTile(i, j));
        }
      }
    }
    public static ETile PickTile(int i, int j)
    {
      if (i == 18 && j == 16)
        return ETile.Ladder;
      if (Math.Abs(i - j) > 10)
        return ETile.Rock;
      if (Math.Abs(i + j) < 25)
        return ETile.Dirt;
      return ETile.Snow;
    }
  }
}

// 
public record WorldLayer : Layer
{
  public WorldLayer(World w, int level, HydraVision vision) : base(w.Level(level))
  {
    Level lev = w.Level(level);
    for (int i = 0; i < lev.X; i++)
    {
      for (int j = 0; j < lev.Y; j++)
      {
        ETile eTile = vision.Filter(lev.BlockAt(i, j));
        tiles[i, j] = new Tile(eTile);
      }
    }
  }
}