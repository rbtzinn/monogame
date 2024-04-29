using System;
using System.Collections.Generic;

namespace snake
{
	public class SnakeBody
	{
		int x;
		int y;
		Boolean collide = false;

		//SnakeBody Snake_Body;
		public int XPosition {
			get;
			set;
		}
		public int LastXPosition {
			get;
			set;
		}
		public int YPosition {
			get;
			set;
		}
		public int LastYPosition {
			get;
			set;
		}
		public int BodyLength {
			get;
			set;
		}

//		public SnakeBody ()
//		{
//			this.x = 0;
//			this.y = 0;
//			this.length = 1;
//		}
		public SnakeBody (int xPos, int yPos)
		{
			this.XPosition = xPos;
			this.YPosition = yPos;
		}

		public void GenerateSnake(List<SnakeBody> BodyParts) { //makes a new snake.
			int x = BodyParts [0].XPosition;
			int y = BodyParts [0].YPosition;
			for (int i = 0; i <= 3; i++) {
				SnakeBody TempPart = new SnakeBody (x - (30 * i), y - (30 * 1));
				BodyParts.Add (TempPart);
			}
		}

		public int[] MoveHead(int direction) //returns a new int for the head's location based on direction
		{
			int[] movement = new int[2];
			if (direction == 1) { //UP
				movement [0] = this.XPosition -= 0;
				movement [1] = this.YPosition -= 30;
				return movement;
			} else if (direction == 2) { //RIGHT
				movement [0] = this.XPosition += 30;
				movement [1] = this.YPosition -= 0;
				return movement;
			} else if (direction == 3) { //DOWN
				movement [0] = this.XPosition -= 0;
				movement [1] = this.YPosition += 30;
				return movement;
			} else if (direction == 4) { //LEFT
				movement [0] = this.XPosition -= 30;
				movement [1] = this.YPosition -= 0;
				return movement;
			}
			return movement;

		}

		public void MoveBody(List<SnakeBody> BodyParts) //cycles through each segment, updating current and previous position 
		{												// based off the position of the previous segment in line (except the head)
			for (int i = 1; i < BodyParts.Count; i++) { //i = 1 to skip the head
				BodyParts [i].LastXPosition = BodyParts[i].XPosition;
				BodyParts [i].LastYPosition = BodyParts[i].YPosition;
				BodyParts [i].XPosition = BodyParts [i - 1].LastXPosition;
				BodyParts [i].YPosition = BodyParts [i - 1].LastYPosition;
			}
		}

		public Boolean SnakeCollision(List<SnakeBody> BodyParts) { //Checks for collisions with the body or walls
			for (int i = 1; i < BodyParts.Count; i++) { //i = 1 to skip the head (including the head always make collision true)
				Boolean SnakeCollide = (BodyParts [0].XPosition == BodyParts [i].XPosition) && (BodyParts [0].YPosition == BodyParts [i].YPosition);
				Boolean WallCollide = (BodyParts [0].XPosition < 0 || BodyParts [0].XPosition > 570 || BodyParts[0].YPosition < 30 || BodyParts [0].YPosition > 570);
				if (SnakeCollide || WallCollide) {
					collide = true;
					break;
				} else {
					collide = false;
				}
			}
			return collide;
		}

		public void GrowSnake (List<SnakeBody> BodyParts) //Adds a new snake object to the list
		{												  //assigns the position based off the butt-end of the snake
			int length = BodyParts.Count - 1;
			int x = BodyParts [length].XPosition;
			int y = BodyParts [length].YPosition;

			BodyParts.Add(new SnakeBody(x, y));
		}
		public int IncreaseScore(int CurrentScore) {
			return CurrentScore += 10;
		}
	}
}

