namespace Gameplay.Level
{
    public class EasyLevel : TicTacToeBase
    {
        protected override Move FindBestMove(char[,] board)
        {
            int bestVal = -1000;
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
                        int moveVal = Minimax(board, 0, true, MIN, MAX);

                        // Undo the move
                        board[i, j] = '_';

                        // If the value of the current move is
                        // more than the best value, then update
                        // best/
                        if (moveVal > bestVal)
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