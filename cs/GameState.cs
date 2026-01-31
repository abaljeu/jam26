namespace mask
{
  internal class GameState
  {
    public static readonly World theWorld = new World();

    // things at locations in the world
    public Hydra hydra = new Hydra(new Mob("Monty", 1, 1,2, 1, 0,0), [], []);
    public List<Mob> Mobs;
    public List<Mask> masks;
    public int CurrentLevel = 1;
    public Layer CurrentLayer;

    public void UpdateLayer()
    {
      CurrentLayer = new WorldLayer(theWorld, CurrentLevel,
        new HydraVision(hydra));
    }
  }

}
