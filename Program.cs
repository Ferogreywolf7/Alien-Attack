internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new Alien_Attack.Game1();
        game.Run();
    }
}