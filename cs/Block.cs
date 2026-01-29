namespace mask
{
  public record Block(ETile type, 
    EnableFlag EnableMode=EnableFlag.EnabledBy,
    EFeature Feature = EFeature.None);



  public static class Blocks
  {
    public static Block Space() => new Block(ETile.Space);
    public static Block Rock() => new Block(ETile.Rock);
    public static Block Dirt() => new Block(ETile.Dirt);
    public static Block Ladder() => new Block(ETile.Ladder);
  }
}
