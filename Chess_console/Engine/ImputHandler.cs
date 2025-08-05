using ChessConsole.Graphics;
using ChessConsole.Models;
using System.Drawing;

namespace ChessConsole.Engine;

/// <summary>
/// Responsável por ler e validar a entrada do usuário no formato de notação de xadrez.
/// </summary>
public class InputHandler
{
    /// <summary>
    /// Pede ao usuário uma coordenada e a converte para o formato do tabuleiro.
    /// </summary>
    public Point GetPlayerMove(string prompt, ConsoleRenderer renderer)
    {
        while (true)
        {
            renderer.PromptForInput(prompt);
            string? input = Console.ReadLine()?.ToLower().Trim();

            if (!string.IsNullOrEmpty(input) && input.Length == 2)
            {
                char fileChar = input[0];
                char rankChar = input[1];

                if (fileChar >= 'a' && fileChar <= 'h' && rankChar >= '1' && rankChar <= '8')
                {
                    int col = fileChar - 'a';
                    int row = 8 - (rankChar - '0');
                    return new Point(col, row);
                }
            }
            // Retorna uma coordenada inválida para o loop tentar novamente
            return new Point(-1, -1); 
        }
    }
}
