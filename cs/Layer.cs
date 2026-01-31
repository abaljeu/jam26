namespace mask
{
  public class Layer
  {
    public Tile[,] tiles;

    public static Layer FromWorld(int level, Mask[] masks)
    {
      return new Layer();
    }
    public static ETile PickTile(int i, int j)
    {
      if (i == 18 && j == 16)
        return ETile.Ladder;
      if (Math.Abs(i - j) > 10)
        return ETile.Rock;
      if (Math.Abs(i + j) < 25)
        return ETile.Dirt;
      return ETile.Space;
    }
    public static Layer ExampleLayer()
    {
      Layer lay = new Layer();
      lay.tiles = new Tile[20, 20];
      for (int i = 0; i < 20; i++)
      {
        for (int j = 0; j < 20; j++)
        {
          lay.tiles[i, j] = new Tile(PickTile(i, j));
        }
      }
      return lay;
    }
  }
}
