namespace Orbital.Models.Domain
{
  public class Club
  {
    public Club(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
  }
}
