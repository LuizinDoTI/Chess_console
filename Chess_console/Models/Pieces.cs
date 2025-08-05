using System.Drawing;

namespace ChessConsole.Models.Pieces;

/// <summary>
/// Classe base abstrata para todas as peças de xadrez.
/// </summary>
public abstract class ChessPiece
{
    public PlayerColor Color { get; }
    public Point Position { get; private set; }
    public abstract char Symbol { get; }

    protected ChessPiece(PlayerColor color, Point position)
    {
        Color = color;
        Position = position;
    }

    public void SetPosition(Point newPosition)
    {
        Position = newPosition;
    }

    /// <summary>
    /// Retorna uma lista de todas as jogadas válidas para esta peça.
    /// </summary>
    public abstract List<Point> GetValidMoves(Board board);

    /// <summary>
    /// Helper para adicionar movimentos em linha reta (Torre, Bispo, Rainha).
    /// </summary>
    protected void AddLineMoves(Board board, List<Point> moves, int dx, int dy)
    {
        Point nextPos = new Point(Position.X + dx, Position.Y + dy);
        while (board.IsValidCoordinate(nextPos))
        {
            var pieceAtNext = board.GetPieceAt(nextPos);
            if (pieceAtNext == null)
            {
                moves.Add(nextPos);
            }
            else
            {
                if (pieceAtNext.Color != this.Color)
                {
                    moves.Add(nextPos); // Pode capturar
                }
                break; // Bloqueado
            }
            nextPos = new Point(nextPos.X + dx, nextPos.Y + dy);
        }
    }
}

// --- Implementações Concretas das Peças ---

public class Pawn : ChessPiece
{
    // Peças brancas em maiúsculas, pretas em minúsculas
    public override char Symbol => Color == PlayerColor.White ? 'P' : 'p';
    private bool _hasMoved = false;

    public Pawn(PlayerColor color, Point position) : base(color, position) { }

    public override List<Point> GetValidMoves(Board board)
    {
        var moves = new List<Point>();
        int direction = Color == PlayerColor.White ? -1 : 1;

        // Movimento para frente
        Point oneStep = new Point(Position.X, Position.Y + direction);
        if (board.IsValidCoordinate(oneStep) && board.GetPieceAt(oneStep) == null)
        {
            moves.Add(oneStep);
            // Movimento duplo inicial
            if (!_hasMoved)
            {
                Point twoSteps = new Point(Position.X, Position.Y + 2 * direction);
                if (board.IsValidCoordinate(twoSteps) && board.GetPieceAt(twoSteps) == null)
                {
                    moves.Add(twoSteps);
                }
            }
        }

        // Captura diagonal
        Point[] captureMoves = {
            new Point(Position.X - 1, Position.Y + direction),
            new Point(Position.X + 1, Position.Y + direction)
        };
        foreach (var move in captureMoves)
        {
            if (board.IsValidCoordinate(move))
            {
                var piece = board.GetPieceAt(move);
                if (piece != null && piece.Color != this.Color)
                {
                    moves.Add(move);
                }
            }
        }
        
        if (Position.Y != (Color == PlayerColor.White ? 6 : 1)) _hasMoved = true;

        return moves;
    }
}

public class Rook : ChessPiece
{
    public override char Symbol => Color == PlayerColor.White ? 'R' : 'r';
    public Rook(PlayerColor color, Point position) : base(color, position) { }

    public override List<Point> GetValidMoves(Board board)
    {
        var moves = new List<Point>();
        AddLineMoves(board, moves, 1, 0);
        AddLineMoves(board, moves, -1, 0);
        AddLineMoves(board, moves, 0, 1);
        AddLineMoves(board, moves, 0, -1);
        return moves;
    }
}

public class Knight : ChessPiece
{
    public override char Symbol => Color == PlayerColor.White ? 'N' : 'n';
    public Knight(PlayerColor color, Point position) : base(color, position) { }

    public override List<Point> GetValidMoves(Board board)
    {
        var moves = new List<Point>();
        int[] dx = { 1, 1, 2, 2, -1, -1, -2, -2 };
        int[] dy = { 2, -2, 1, -1, 2, -2, 1, -1 };

        for (int i = 0; i < 8; i++)
        {
            Point nextPos = new Point(Position.X + dx[i], Position.Y + dy[i]);
            if (board.IsValidCoordinate(nextPos))
            {
                var piece = board.GetPieceAt(nextPos);
                if (piece == null || piece.Color != this.Color)
                {
                    moves.Add(nextPos);
                }
            }
        }
        return moves;
    }
}

public class Bishop : ChessPiece
{
    public override char Symbol => Color == PlayerColor.White ? 'B' : 'b';
    public Bishop(PlayerColor color, Point position) : base(color, position) { }

    public override List<Point> GetValidMoves(Board board)
    {
        var moves = new List<Point>();
        AddLineMoves(board, moves, 1, 1);
        AddLineMoves(board, moves, 1, -1);
        AddLineMoves(board, moves, -1, 1);
        AddLineMoves(board, moves, -1, -1);
        return moves;
    }
}

public class Queen : ChessPiece
{
    public override char Symbol => Color == PlayerColor.White ? 'Q' : 'q';
    public Queen(PlayerColor color, Point position) : base(color, position) { }

    public override List<Point> GetValidMoves(Board board)
    {
        var moves = new List<Point>();
        AddLineMoves(board, moves, 1, 0);
        AddLineMoves(board, moves, -1, 0);
        AddLineMoves(board, moves, 0, 1);
        AddLineMoves(board, moves, 0, -1);
        AddLineMoves(board, moves, 1, 1);
        AddLineMoves(board, moves, 1, -1);
        AddLineMoves(board, moves, -1, 1);
        AddLineMoves(board, moves, -1, -1);
        return moves;
    }
}

public class King : ChessPiece
{
    public override char Symbol => Color == PlayerColor.White ? 'K' : 'k';
    public King(PlayerColor color, Point position) : base(color, position) { }

    public override List<Point> GetValidMoves(Board board)
    {
        var moves = new List<Point>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Point nextPos = new Point(Position.X + dx, Position.Y + dy);
                if (board.IsValidCoordinate(nextPos))
                {
                    var piece = board.GetPieceAt(nextPos);
                    if (piece == null || piece.Color != this.Color)
                    {
                        moves.Add(nextPos);
                    }
                }
            }
        }
        return moves;
    }
}
