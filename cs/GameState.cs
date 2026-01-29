namespace mask
{
  internal class GameState
  {
    public static readonly World theWorld = new World();

    // things at locations in the world
    public Hydra hydra;
    public List<Mob> Mobs;
    public List<Mask> masks;

  }

}
