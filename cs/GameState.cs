namespace mask
{
  internal class GameState
  {
    public static readonly World theWorld = new World();

    // things at locations in the world
    public static Hydra hydra = new Hydra(new Mob(EMob.Hydra, 1, 1,2, 1, 0,0), [], []);
    public List<Mob> Mobs; // creatures roaming the world including our hydra
    public List<Mask> Items; // Things on the ground
    public int CurrentLevel;
    public Layer CurrentLayer;

    public void UpdateLayer()
    {
      CurrentLayer = new WorldLayer(theWorld, CurrentLevel,
        new HydraVision(hydra));
    }
    public IEnumerable<Mob> LayerMobs()
    {
      foreach (Mob mob in Mobs)
      {
        if (mob.Level == CurrentLevel) yield return mob;
      }
    }
    public void InitGame()
    {
      Mobs = new List<Mob>(theWorld.InitialMobs);
      Items = new List<Mask>(theWorld.InitialItems);
      CurrentLevel = 1;
      UpdateLayer();

    }

  }

}
