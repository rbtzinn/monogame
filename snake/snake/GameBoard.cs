using System;

namespace snake
{
	public class GameBoard
	{

		Boolean holds;

		public Boolean HoldsSnake {
			get {
				return holds;
			}
			set {
				holds = value;
			}
		}

		public GameBoard (){
			this.HoldsSnake = false;
		}

		public GameBoard[,] CreateGameBoard (int width, int height)
		{
			GameBoard[,] board = new GameBoard[width,height];

			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					board [i, j].HoldsSnake = false;
				}
			}
			return board;
		}
	}
}

