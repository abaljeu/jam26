namespace mask
{
    public class GameState
    {
        public static GameState theGame = new GameState();
        public GameState()
        { 
            hydra = new Hydra(new Mob(EMob.Hydra, 1, 1, 2, 1, 0, 0));
        }
        public static readonly World theWorld = new World();

        // things at locations in the world
        public Hydra hydra;
        public List<Mob> Mobs; // creatures roaming the world including our hydra
        public List<MaskOnGround> Items; // Things on the ground
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
                if (mob != null && mob.Level == CurrentLevel) 
                    yield return mob;
            }
        }
        public void InitGame()
        {
            Mobs = new List<Mob>(theWorld.InitialMobs);
            Items = new List<MaskOnGround>(theWorld.InitialItems);
            CurrentLevel = 1;
            UpdateLayer();
        }

    }

}
