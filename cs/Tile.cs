using System;

namespace mask
{
  public record Tile(ETile tileType, Mob? mob = null)
  {
    public string tileGraphic { get; set; }
    public bool walkable { get => tileType != ETile.None && tileType != ETile.Wall; }

    public int trap { get; set; } = 0; // dmg amount

    public static ETile RandomTileType()
    {
      return (ETile)new System.Random().Next((int)(ETile.None + 1), (int)ETile.Wall);
    }
  }
}
