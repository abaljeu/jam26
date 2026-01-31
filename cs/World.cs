namespace mask
{
  public class Level
  {
    public const int X = 20, Y = 20;
    protected Block[,] Blocks = new Block[X, Y];
    public Level() 
    {
      for (int i = 0; i < X; i++)
      {
        for (int j = 0; j < Y; j++)
        {
          Blocks[i, j] = new Block();
        }
      }
    }
  }
  public class BasementLevel : Level
  {
    public BasementLevel() : base()
    {
      Blocks[18, 18].Add(ETile.Ladder);
      for (int i = 0; i < X; i++)
      {
        for (int j = 0; j < Y; j++)
        {
          Blocks[i, j].Add(ETile.Snow);
        }
      }
    }
  }

  public class StartingLevel : Level
  {
    public StartingLevel() : base()
    {
      Blocks[18, 18].Add(ETile.Ladder);
      for (int i = 0; i < X; i++)
      {
        for (int j = 0; j < Y; j++)
        {
          Blocks[i, j].Add(ETile.Snow);
        }
      }
    }
  }


  public class World
  {
    private Level[] Levels = new Level[5];
    public World()
    {
      Levels[0] = new BasementLevel();
      Levels[1] = new StartingLevel();

    }
  }

}
