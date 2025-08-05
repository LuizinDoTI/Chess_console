using ChessConsole.Models.Pieces;

namespace ChessConsole.Models;

/// <summary>
/// Armazena o estado dinâmico do jogo.
/// </summary>
public class GameState
{
    public PlayerColor CurrentPlayer { get; private set; }
    public GameStatus Status { get; set; }
    public string LastMove { get; set; }
    private readonly List<ChessPiece> _whiteCaptured;
    private readonly List<ChessPiece> _blackCaptured;

    public GameState()
    {
        CurrentPlayer = PlayerColor.White;
        Status = GameStatus.Ongoing;
        LastMove = "N/A";
        _whiteCaptured = new List<ChessPiece>();
        _blackCaptured = new List<ChessPiece>();
    }

    public void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == PlayerColor.White) ? PlayerColor.Black : PlayerColor.White;
    }

    public void AddCapturedPiece(ChessPiece piece)
    {
        if (piece.Color == PlayerColor.White)
        {
            _whiteCaptured.Add(piece);
        }
        else
        {
            _blackCaptured.Add(piece);
        }
    }

    public string GetCapturedPieces(PlayerColor playerColor)
    {
        var pieces = (playerColor == PlayerColor.White) ? _whiteCaptured : _blackCaptured;
        return string.Join(" ", pieces.Select(p => p.Symbol));
    }
}

// Enums para controlar o estado do jogo
public enum PlayerColor { White, Black }
public enum GameStatus { Ongoing, Check, Checkmate, Stalemate }
