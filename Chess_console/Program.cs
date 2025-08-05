using ChessConsole.Engine;
using System.Text;

namespace ChessConsole;

/// <summary>
/// Ponto de entrada principal da aplicação.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Xadrez de Console com Monitor de Performance";
        var engine = new GameEngine();
        engine.Start();
    }
}
