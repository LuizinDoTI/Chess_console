using ChessConsole.Models.Pieces;
using System.Drawing;

namespace ChessConsole.Models;

/// <summary>
/// Representa o tabuleiro de xadrez 8x8 e a lógica de movimentação de peças.
/// </summary>
public class Board
{
    private readonly ChessPiece?[,] _grid;

    public Board()
    {
        _grid = new ChessPiece[8, 8];
    }

    /// <summary>
    /// Configura o tabuleiro com a posição inicial das peças.
    /// </summary>
    public void InitializeBoard()
    {
        // Peões
        for (int i = 0; i < 8; i++)
        {
            _grid[1, i] = new Pawn(PlayerColor.Black, new Point(i, 1));
            _grid[6, i] = new Pawn(PlayerColor.White, new Point(i, 6));
        }

        // Torres
        _grid[0, 0] = new Rook(PlayerColor.Black, new Point(0, 0));
        _grid[0, 7] = new Rook(PlayerColor.Black, new Point(7, 0));
        _grid[7, 0] = new Rook(PlayerColor.White, new Point(0, 7));
        _grid[7, 7] = new Rook(PlayerColor.White, new Point(7, 7));

        // Cavalos
        _grid[0, 1] = new Knight(PlayerColor.Black, new Point(1, 0));
        _grid[0, 6] = new Knight(PlayerColor.Black, new Point(6, 0));
        _grid[7, 1] = new Knight(PlayerColor.White, new Point(1, 7));
        _grid[7, 6] = new Knight(PlayerColor.White, new Point(6, 7));

        // Bispos
        _grid[0, 2] = new Bishop(PlayerColor.Black, new Point(2, 0));
        _grid[0, 5] = new Bishop(PlayerColor.Black, new Point(5, 0));
        _grid[7, 2] = new Bishop(PlayerColor.White, new Point(2, 7));
        _grid[7, 5] = new Bishop(PlayerColor.White, new Point(5, 7));

        // Rainhas
        _grid[0, 3] = new Queen(PlayerColor.Black, new Point(3, 0));
        _grid[7, 3] = new Queen(PlayerColor.White, new Point(3, 7));

        // Reis
        _grid[0, 4] = new King(PlayerColor.Black, new Point(4, 0));
        _grid[7, 4] = new King(PlayerColor.White, new Point(4, 7));
    }

    public ChessPiece? GetPieceAt(Point p) => _grid[p.Y, p.X];

    public bool IsValidCoordinate(Point p) => p.X >= 0 && p.X < 8 && p.Y >= 0 && p.Y < 8;

    /// <summary>
    /// Move uma peça e atualiza o estado do jogo.
    /// </summary>
    public void MovePiece(Point from, Point to, GameState gameState)
    {
        ChessPiece? pieceToMove = GetPieceAt(from);
        if (pieceToMove == null) return;

        ChessPiece? capturedPiece = GetPieceAt(to);
        if (capturedPiece != null)
        {
            gameState.AddCapturedPiece(capturedPiece);
        }

        _grid[to.Y, to.X] = pieceToMove;
        _grid[from.Y, from.X] = null;
        pieceToMove.SetPosition(to);
        
        gameState.LastMove = $"{ToChessNotation(from)}-{ToChessNotation(to)}";
    }
    
    /// <summary>
    /// Desfaz uma jogada, usado para validação de xeque.
    /// </summary>
    public void UndoMovePiece(Point from, Point to, ChessPiece? capturedPiece)
    {
        ChessPiece? movedPiece = GetPieceAt(to);
        if (movedPiece == null) return;

        _grid[from.Y, from.X] = movedPiece;
        _grid[to.Y, to.X] = capturedPiece;
        movedPiece.SetPosition(from);
    }

    /// <summary>
    /// Verifica se um jogador está em xeque.
    /// </summary>
    public bool IsInCheck(PlayerColor playerColor)
    {
        Point kingPosition = FindKing(playerColor);
        PlayerColor opponentColor = playerColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;

        // Verifica se alguma peça adversária pode atacar o rei
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece? piece = GetPieceAt(new Point(col, row));
                if (piece != null && piece.Color == opponentColor)
                {
                    if (piece.GetValidMoves(this).Contains(kingPosition))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    /// <summary>
    /// Verifica se um jogador está em xeque-mate.
    /// </summary>
    public bool IsCheckmate(PlayerColor playerColor)
    {
        if (!IsInCheck(playerColor)) return false;

        // Verifica se existe alguma jogada legal que tira o jogador do xeque
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece? piece = GetPieceAt(new Point(col, row));
                if (piece != null && piece.Color == playerColor)
                {
                    var validMoves = piece.GetValidMoves(this);
                    foreach (var move in validMoves)
                    {
                        var originalPos = piece.Position;
                        var captured = GetPieceAt(move);
                        
                        MovePiece(originalPos, move, new GameState()); // Simula a jogada
                        
                        bool stillInCheck = IsInCheck(playerColor);
                        
                        UndoMovePiece(originalPos, move, captured); // Desfaz a simulação
                        
                        if (!stillInCheck)
                        {
                            return false; // Encontrou uma jogada que salva do xeque
                        }
                    }
                }
            }
        }

        return true; // Nenhuma jogada legal encontrada
    }
    
    /// <summary>
    /// Verifica se o jogo está em empate por afogamento (stalemate).
    /// </summary>
    public bool IsStalemate(PlayerColor playerColor)
    {
        if (IsInCheck(playerColor)) return false;

        // Verifica se o jogador não tem nenhuma jogada legal
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece? piece = GetPieceAt(new Point(col, row));
                if (piece != null && piece.Color == playerColor)
                {
                    var validMoves = piece.GetValidMoves(this);
                    foreach (var move in validMoves)
                    {
                        var originalPos = piece.Position;
                        var captured = GetPieceAt(move);
                        MovePiece(originalPos, move, new GameState());
                        
                        bool wouldBeInCheck = IsInCheck(playerColor);
                        
                        UndoMovePiece(originalPos, move, captured);
                        
                        if (!wouldBeInCheck)
                        {
                            return false; // Existe pelo menos uma jogada legal
                        }
                    }
                }
            }
        }

        return true; // Nenhuma jogada legal e não está em xeque
    }

    private Point FindKing(PlayerColor playerColor)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece? piece = GetPieceAt(new Point(col, row));
                if (piece is King && piece.Color == playerColor)
                {
                    return piece.Position;
                }
            }
        }
        throw new InvalidOperationException("Rei não encontrado no tabuleiro!");
    }
    
    public static string ToChessNotation(Point p)
    {
        char file = (char)('a' + p.X);
        char rank = (char)('8' - p.Y);
        return $"{file}{rank}";
    }
}