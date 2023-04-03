using System;

namespace Gameplay.Level
{
    public class MediumLevel : TicTacToeBase
    {
        protected override int Minimax(char[,] board, int depth, bool isMax, int alpha, int beta)
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
                            board[i, j] = Opponent;

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
                            board[i, j] = Player;

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

        protected override Move FindBestMove(char[,] board)
        {
            int bestVal = 1000;
            Move bestMove = new Move();
            bestMove.Row = -1;
            bestMove.Col = -1;

            // Traverse all cells, evaluate minimax function 
            // for all empty cells. And return the cell 
            // with optimal value.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == '_')
                    {
                        // Make the move
                        board[i, j] = Opponent;

                        // compute evaluation function for this
                        // move.
                        int moveVal = Minimax(board, 0, false, MIN, MAX);

                        // Undo the move
                        board[i, j] = '_';

                        // If the value of the current move is
                        // more than the best value, then update
                        // best/
                        if (moveVal < bestVal)
                        {
                            bestMove.Row = i;
                            bestMove.Col = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }

            return bestMove;
        }
    }
}