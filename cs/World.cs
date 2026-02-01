namespace mask
{
  // The World and its levels are only constructed once
  // at the beginning of the game.
  public class Level
  {
    public readonly int X = 20, Y = 15;
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
    public List<Mob> InitialMobs = new List<Mob>();
    public List<Mask> InitialItems = new List<Mask>();
        public void AddRange(ETile type, int x0, int y0, int w, int h)
        {
            for (int i = 0; x0 + i < X; i++)
            {
                if (i == w)
                    break;
                int x = x0 + i;

                for (int j = 0; y0 + j < Y; j++)
                {
                    if (j == h)
                        break;
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
      Blocks[13, 13].Add(ETile.Ladder);
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
      Blocks[12, 12].Add(ETile.Ladder);

      Blocks[13, 13].Clear();
      Blocks[13, 13].Add(ETile.FloorLadder);

      InitialMobs.Add(GameState.hydra);
      InitialMobs.Add(new Mob(EMob.Slime, 1,/*x,y*/ 12, 12, 1, 1, 1));

      InitialItems.Add(new Mask(ETile.Bridge, MaskFeature.None));
    }
  }

  // very weak level design
  // but good for a stress test of behaviors.
  public class SecondLevel : Level
  {
    public SecondLevel() : base()
    {
      for (int i = 0; i < 20; i++)
      {
        for (int j = 0; j < 15; j++)
        {
          ETile type = Tile.RandomTileType();
          if (type != ETile.None && type != ETile.FloorLadder && type != ETile.Ladder)
            Blocks[i, j].Add(type);
        }
      }

      Blocks[13, 13].Add(ETile.FloorLadder);

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
      Levels[1] = new StartingLevel();
      Levels[0] = new BasementLevel();
      Levels[2] = new SecondLevel();
    }
    public IEnumerable<Mob> InitialMobs
    {
      get
      {
        foreach (Level l in Levels)
        {
          if (l != null)
          {
            foreach (Mob mob in l.InitialMobs)
              yield return mob;
          }
        }
      }
    }
    public IEnumerable<Mask> InitialItems
    {
      get
      {
        foreach (Level l in Levels)
        {
          if (l != null)
          {
            foreach (Mask mask in l.InitialItems)
              yield return mask;
          }
        }
      }
    }
  }
}
