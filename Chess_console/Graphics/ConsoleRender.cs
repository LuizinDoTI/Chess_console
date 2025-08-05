using ChessConsole.Models;
using ChessConsole.Models.Pieces;
using System.Drawing;

namespace ChessConsole.Graphics;

/// <summary>
/// Responsável por desenhar todos os elementos do jogo no console.
/// </summary>
public class ConsoleRenderer
{
    private readonly Board _board;
    private readonly GameState _gameState;

    public ConsoleRenderer(Board board, GameState gameState)
    {
        _board = board;
        _gameState = gameState;
    }

    /// <summary>
    /// Renderiza o quadro completo do jogo com um layout vertical.
    /// </summary>
    public void Render()
    {
        Console.Clear();
        Console.CursorVisible = false;

        DrawBoard();
        DrawInfoPanel();
    }

    /// <summary>
    /// Desenha o tabuleiro de xadrez e as peças.
    /// </summary>
    private void DrawBoard()
    {
        Console.WriteLine("    a b c d e f g h");
        Console.WriteLine("  +-----------------+");

        for (int row = 0; row < 8; row++)
        {
            Console.Write($"{8 - row} |");
            for (int col = 0; col < 8; col++)
            {
                bool isLightSquare = (row + col) % 2 == 0;
                Console.BackgroundColor = isLightSquare ? ConsoleColor.DarkGray : ConsoleColor.Black;

                ChessPiece? piece = _board.GetPieceAt(new Point(col, row));
                Console.ForegroundColor = piece?.Color == PlayerColor.White ? ConsoleColor.White : ConsoleColor.Yellow;

                Console.Write($"{piece?.Symbol ?? ' '}");
                
                Console.ResetColor();
                Console.Write("|");
            }
            Console.WriteLine($" {8 - row}");
        }
        Console.WriteLine("  +-----------------+");
        Console.WriteLine("    a b c d e f g h");
    }

    /// <summary>
    /// Desenha os painéis de informação e performance abaixo do tabuleiro.
    /// </summary>
    private void DrawInfoPanel()
    {
        Console.WriteLine(); // Linha em branco para separar

        // Painel de Status do Jogo
        string player = _gameState.CurrentPlayer == PlayerColor.White ? "Brancas" : "Pretas";
        string status = GetGameStatusMessage();
        Console.WriteLine($"Turno: {player} | Status: {status}");
        Console.WriteLine($"Última Jogada: {_gameState.LastMove}");
        Console.WriteLine($"Capturadas (pelas Brancas): {_gameState.GetCapturedPieces(PlayerColor.Black)}");
        Console.WriteLine($"Capturadas (pelas Pretas):  {_gameState.GetCapturedPieces(PlayerColor.White)}");
        
        Console.WriteLine("------------------------------------------");

        // Painel de Performance
        var perf = ThreadMonitor.GetThreadInfo();
        Console.WriteLine($"📊 Perf: Threads: {perf.TotalThreads} | CPU: {perf.CpuTime.TotalSeconds:F1}s | Mem: {perf.MemoryUsageMB}MB");
        
        Console.WriteLine("------------------------------------------");
    }

    /// <summary>
    /// Exibe a mensagem de entrada do usuário.
    /// </summary>
    public void PromptForInput(string prompt)
    {
        Console.Write(prompt);
        Console.CursorVisible = true;
    }
    
    /// <summary>
    /// Exibe a mensagem de fim de jogo.
    /// </summary>
    public void ShowGameOverMessage()
    {
        Console.WriteLine();
        Console.WriteLine($"FIM DE JOGO: {GetGameStatusMessage()}");
        Console.WriteLine("Pressione qualquer tecla para sair.");
        Console.ReadKey();
    }

    private string GetGameStatusMessage()
    {
        return _gameState.Status switch
        {
            GameStatus.Ongoing => "Em andamento",
            GameStatus.Check => "XEQUE!",
            GameStatus.Checkmate => $"XEQUE-MATE! {(_gameState.CurrentPlayer == PlayerColor.White ? "Pretas" : "Brancas")} venceram!",
            GameStatus.Stalemate => "Empate por afogamento!",
            _ => ""
        };
    }
}
