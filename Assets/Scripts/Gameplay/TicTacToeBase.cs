using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Gameplay
{
    public enum GameState
    {
        Player,
        Opponent
    }

    public enum ResultType
    {
        PlayerWin,
        AIWin,
        Draw
    }

    [Serializable]
    public class Move
    {
        [field: SerializeField] public int Row { get; set; }
        [field: SerializeField] public int Col { get; set; }
    };

    public abstract class TicTacToeBase : MonoBehaviour
    {
        protected const char Player = 'X';

        protected const char Opponent = 'O';

        // Initial values of
        // Alpha and Beta
        protected const int MAX = 1000;
        protected const int MIN = -1000;

        private char[,] _board =
        {
            { '_', '_', '_' },
            { '_', '_', '_' },
            { '_', '_', '_' }
        };

        private Box[,] boxes = new Box[3, 3];

        private GameController _gameController;
        private UIManager _uiManager;

        private void Start()
        {
            _gameController = GameController.Instance;
            _uiManager = UIManager.Instance;
            SetMoveToChildren();

            DebugBoard();
        }

        // Set row and col to boxes
        private void SetMoveToChildren()
        {
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Box boxChild = transform.GetChild(index).GetComponent<Box>();
                    Move moveChild = boxChild.Move();
                    moveChild.Row = i;
                    moveChild.Col = j;

                    boxChild.OnClick += OnClickBox;

                    boxes[i, j] = boxChild;

                    index += 1;
                }
            }
        }

        private void UpdateUI()
        {
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                if (_board[i, j] != '_')
                    boxes[i, j].UpdateBoxUI(_board[i, j] == Player ? GameState.Player : GameState.Opponent);
            }
        }

        // Call when player click box
        private void OnClickBox(int row, int col)
        {
            if (_gameController.EndGame)
                return;
            if (_board[row, col] != '_')
                return;
            _board[row, col] = Player;
            UpdateUI();
            DebugBoard();

            CheckEndGame();
            _gameController.SwitchState();
            OpponentTurn();
        }

        private async void OpponentTurn()
        {
            await Task.Delay(1000);
            if (_gameController.EndGame)
                return;

            Move bestMove = FindBestMove(_board);

            _board[bestMove.Row, bestMove.Col] = Opponent;
            UpdateUI();
            DebugBoard();

            CheckEndGame();
            _gameController.SwitchState();

            Debug.Log("The AI Move is :\n" +
                      $"ROW: {bestMove.Row} COL: {bestMove.Col}");
        }

        private void CheckEndGame()
        {
            if (IsMovesLeft(_board))
            {
                if (Evaluate(_board) > 0)
                {
                    _uiManager.ShowResult(ResultType.PlayerWin);
                    Debug.Log($"<color=yellow> Player Win </color>");
                }
                else if (Evaluate(_board) < 0)
                {
                    _uiManager.ShowResult(ResultType.AIWin);
                    Debug.Log($"<color=yellow> AI Win </color>");
                }
                else
                {
                    _gameController.SetEndGame(false);
                    return;
                }
            }
            else
            {
                _uiManager.ShowResult(ResultType.Draw);
                Debug.Log($"<color=green> Draw </color>");
            }

            _gameController.SetEndGame(true);
            Evaluate(_board);
        }

        private void DebugBoard()
        {
            Debug.Log($"{_board[0, 0]} | {_board[0, 1]} | {_board[0, 2]}\n" +
                      $"{_board[1, 0]} | {_board[1, 1]} | {_board[1, 2]}\n" +
                      $"{_board[2, 0]} | {_board[2, 1]} | {_board[2, 2]}");
        }

        // If no way to go return false
        protected Boolean IsMovesLeft(char[,] board)
        {
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == '_')
                    return true;
            return false;
        }


        protected int Evaluate(char[,] b)
        {
            // Checking for Rows for X or O victory.
            for (int row = 0; row < 3; row++)
            {
                if (b[row, 0] == b[row, 1] &&
                    b[row, 1] == b[row, 2])
                {
                    if (_gameController.EndGame)
                        _gameController.ShowLine(boxes[row, 0].transform, boxes[row, 1].transform,
                            boxes[row, 2].transform);
                    if (b[row, 0] == Player)
                        return +10;
                    else if (b[row, 0] == Opponent)
                        return -10;
                }
            }

            // Checking for Columns for X or O victory.
            for (int col = 0; col < 3; col++)
            {
                if (b[0, col] == b[1, col] &&
                    b[1, col] == b[2, col])
                {
                    if (_gameController.EndGame)
                        _gameController.ShowLine(boxes[0, col].transform, boxes[1, col].transform,
                            boxes[2, col].transform);
                    if (b[0, col] == Player)
                        return +10;
                    else if (b[0, col] == Opponent)
                        return -10;
                }
            }

            // Checking for Diagonals for X or O victory.
            if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
            {
                if (_gameController.EndGame)
                    _gameController.ShowLine(boxes[0, 0].transform, boxes[1, 1].transform, boxes[2, 2].transform);
                if (b[0, 0] == Player)
                    return +10;
                else if (b[0, 0] == Opponent)
                    return -10;
            }

            if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
            {
                if (_gameController.EndGame)
                    _gameController.ShowLine(boxes[0, 2].transform, boxes[1, 1].transform, boxes[2, 0].transform);
                if (b[0, 2] == Player)
                    return +10;
                else if (b[0, 2] == Opponent)
                    return -10;
            }

            // Else if none of them have won then return 0
            return 0;
        }

        // This is the minimax function. It considers all
        // the possible ways the game can go and returns
        // the value of the board
        protected virtual int Minimax(char[,] board,
            int depth, bool isMax, int alpha, int beta)
        {
            int score = Evaluate(board);

            // If Maximizer has won the game 
            // return his/her evaluated score
            if (score == 10)
                return score;

            // If Minimizer has won the game 
            // return his/her evaluated score
            if (score == -10)
                return score;

            // If there are no more moves and 
            // no winner then it is a tie
            if (IsMovesLeft(board) == false)
                return 0;

            // If this maximizer's move
            if (isMax)
            {
                int best = -1000;

                // Traverse all cells
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Check if cell is empty
                        if (board[i, j] == '_')
                        {
                            // Make the move
                            board[i, j] = Player;

                            // Call minimax recursively and choose
                            // the maximum value
                            best = Math.Max(best, Minimax(board,
                                depth + 1, !isMax, alpha, beta));

                            alpha = Math.Max(alpha, best);

                            // Undo the move
                            board[i, j] = '_';

                            // Alpha Beta Pruning
                            if (beta <= alpha)
                                break;
                        }
                    }
                }

                return best;
            }

            // If this minimizer's move
            else
            {
                int best = 1000;

                // Traverse all cells
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Check if cell is empty
                        if (board[i, j] == '_')
                        {
                            // Make the move
                            board[i, j] = Opponent;

                            // Call minimax recursively and choose
                            // the minimum value
                            best = Math.Min(best, Minimax(board,
                                depth + 1, !isMax, alpha, beta));

                            beta = Math.Min(beta, best);

                            // Undo the move
                            board[i, j] = '_';

                            // Alpha Beta Pruning
                            if (beta <= alpha)
                                break;
                        }
                    }
                }

                return best;
            }
        }

        // This will return the best possible
        // move for the player
        protected abstract Move FindBestMove(char[,] board);
    }
}