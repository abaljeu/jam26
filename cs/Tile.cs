namespace mask
{
  public record Tile(ETile tileType, Mob? mob = null)
  {
    public string tileGraphic { get; set; }
    public bool walkable { get; set; }

    public int trap { get; set; } = 0; // dmg amount
  }
}
