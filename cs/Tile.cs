namespace mask
{
  public record Tile(ETile tileType)
  {
    public string tileGraphic { get; set; }
    public bool walkable { get; set; }

    public int trap { get; set; } = 0; // dmg amount
  }
}
