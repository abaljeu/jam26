
namespace mask
{
    public class GameState
    {
        public static readonly GameState theGame = new GameState();
        public GameState()
        {
            InitGame();

        }
        public World theWorld;

        // things at locations in the world
        public Hydra hydra;
        public List<Mob> Mobs; // creatures roaming the world including our hydra
        public List<MaskOnGround> Items; // Things on the ground
        public int CurrentLevel;
        public Layer CurrentLayer;



        public bool isWalkingRight;
        public bool isWalkingLeft;
        public bool isWalkingUp;
        public bool isWalkingDown;
        public Mob mobHydraIsFighting;
        public int enemyHealth;

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
        private void InitGame()
        {
            isWalkingDown = isWalkingRight = isWalkingLeft = isWalkingUp = false;
            mobHydraIsFighting = null;
            enemyHealth = 0;

            hydra = new Hydra(new Mob(EMob.Hydra, 1, 1, 2, 1, 0, 0));
            theWorld = new World(hydra);

            Mobs = new List<Mob>(theWorld.InitialMobs);
            Items = new List<MaskOnGround>(theWorld.InitialItems);
            CurrentLevel = 1;
            UpdateLayer();
        }

        public void Reset()
        {
            InitGame();
        }
    }

}
