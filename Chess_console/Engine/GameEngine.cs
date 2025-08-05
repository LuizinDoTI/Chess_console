using ChessConsole.Graphics;
using ChessConsole.Models;
using ChessConsole.Models.Pieces;
using System.Drawing;

namespace ChessConsole.Engine;

/// <summary>
/// Orquestra a lógica principal do jogo, o loop de renderização e a entrada do usuário.
/// </summary>
public class GameEngine
{
    private readonly Board _board;
    private readonly GameState _gameState;
    private readonly ConsoleRenderer _renderer;
    private readonly InputHandler _inputHandler;
    private bool _isRunning;

    public GameEngine()
    {
        _board = new Board();
        _gameState = new GameState();
        _renderer = new ConsoleRenderer(_board, _gameState);
        _inputHandler = new InputHandler();
        _isRunning = false;
    }

    /// <summary>
    /// Inicia o jogo.
    /// </summary>
    public void Start()
    {
        _isRunning = true;
        _board.InitializeBoard();
        GameLoop();
    }

    /// <summary>
    /// Loop principal da lógica do jogo, baseado em turnos.
    /// </summary>
    private void GameLoop()
    {
        while (_isRunning && _gameState.Status == GameStatus.Ongoing)
        {
            // 1. Renderiza o estado atual do jogo
            _renderer.Render();

            Point from, to;

            // Loop para obter uma jogada válida do usuário
            while (true)
            {
                from = _inputHandler.GetPlayerMove("Selecione a peça de origem (ex: e2): ", _renderer);
                if (!_board.IsValidCoordinate(from)) continue;

                var selectedPiece = _board.GetPieceAt(from);
                if (selectedPiece == null || selectedPiece.Color != _gameState.CurrentPlayer) continue;

                to = _inputHandler.GetPlayerMove($"Mover '{selectedPiece.Symbol}' de {Board.ToChessNotation(from)} para: ", _renderer);
                if (!_board.IsValidCoordinate(to)) continue;
                
                // Verifica se a jogada é válida (segue as regras da peça e não deixa o rei em xeque)
                if (IsMoveLegal(selectedPiece, from, to))
                {
                    break; // Se a jogada for legal, sai do loop de entrada
                }
            }

            // 2. Executa a jogada permanentemente e atualiza o estado
            lock (_gameState)
            {
                _board.MovePiece(from, to, _gameState);
                _gameState.SwitchPlayer();
                UpdateGameStatus();
            }
        }

        // Fim de jogo
        _renderer.Render();
        _renderer.ShowGameOverMessage();
    }

    /// <summary>
    /// Verifica se uma jogada é legal, simulando-a para checar se o rei fica em xeque.
    /// </summary>
    private bool IsMoveLegal(ChessPiece piece, Point from, Point to)
    {
        // Passo 1: A jogada segue as regras de movimento da peça?
        if (!piece.GetValidMoves(_board).Contains(to))
        {
            return false;
        }

        // Passo 2: A jogada deixa o próprio rei em xeque? (Simulação)
        var capturedPiece = _board.GetPieceAt(to);
        // Usamos um GameState temporário para não afetar o estado real durante a simulação
        _board.MovePiece(from, to, new GameState()); 

        bool leavesKingInCheck = _board.IsInCheck(piece.Color);

        // Desfaz a simulação no tabuleiro para restaurar o estado original
        _board.UndoMovePiece(from, to, capturedPiece);

        // A jogada só é legal se não deixar o rei em xeque.
        return !leavesKingInCheck;
    }

    /// <summary>
    /// Atualiza o status do jogo para verificar xeque ou xeque-mate.
    /// </summary>
    private void UpdateGameStatus()
    {
        var opponentColor = _gameState.CurrentPlayer;
        if (_board.IsInCheck(opponentColor))
        {
            _gameState.Status = _board.IsCheckmate(opponentColor) ? GameStatus.Checkmate : GameStatus.Check;
        }
        else
        {
            _gameState.Status = _board.IsStalemate(opponentColor) ? GameStatus.Stalemate : GameStatus.Ongoing;
        }
    }
}
