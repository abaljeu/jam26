namespace mask
{
  public class Level
  {
    public readonly int X = 20, Y = 20;
    protected Block[,] Blocks;
    public Level()
    {
      Blocks = new Block[X, Y];
      for (int i = 0; i < X; i++)
      {
        for (int j = 0; j < Y; j++)
        {
          Blocks[i, j] = new Block();
        }
      }
    }
    public void AddRange(ETile type, int x0, int y0, int w, int h)
    {
      for (int i = 0; i <= X; i++)
      {
        if (i == w)
          return;
        for (int j = 0; j <= Y; j++)
        {
          if (j == h)
            return;
          int x = x0 + i;
          int y = y0 + j;
          Blocks[x, y].Add(type);
        }
      }
    }
    public Block BlockAt(int x, int y)
    {
      if (x < 0 || y < 0 || x >= X || y >= Y)
        return new Block();

      return Blocks[x, y];
    }
  }
    public class BasementLevel : Level
    {
      public BasementLevel() : base()
      {
        Blocks[18, 18].Add(ETile.Ladder);
        AddRange(ETile.Snow, 0, 0, X, Y);
      }
    }

    public class StartingLevel : Level
    {
      public StartingLevel() : base()
      {
        AddRange(ETile.Dirt, 0, 0, 5, 5);
        AddRange(ETile.Bridge, 5, 3, 4, 2);
        AddRange(ETile.Rock, 9, 0, 20, 20);
        AddRange(ETile.Rock, 0, 9, 20, 20);
        Blocks[12, 16].Add(ETile.Ladder);

        Blocks[18, 18].Clear();
        Blocks[18, 18].Add(ETile.FloorLadder);
      }
    }


    public class World
    {
      private Level[] Levels = new Level[5];
      public Level Level(int z)
      {
        if (z > 0 && z < Levels.Length)
          return Levels[z];
        return Levels[1];
      }
      public World()
      {
        Levels[0] = new BasementLevel();
        Levels[1] = new StartingLevel();

      }
    }
  }
