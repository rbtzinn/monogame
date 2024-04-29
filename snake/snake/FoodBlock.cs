using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace snake
{
	public class FoodBlock
	{
		Random r = new Random ();
		int x;
		int y;

		public int XPosition {
			get {
				return x;
			}
			set {
				x = value;
			}
		}
		public int YPosition {
			get {
				return y;
			}
			set {
				y = value;
			}
		}

		public FoodBlock ()
		{
		}

		public Rectangle GenerateFood (List<SnakeBody> BodyParts)
		{
			Boolean FreeSquare = false;
			while (!FreeSquare) {
				x = r.Next(1,19) * 30;
				y = r.Next(1,19) * 30;
				if (BodyParts.Exists (part => (part.XPosition == x && part.YPosition ==y))) {
				} else {
					FreeSquare = true;
					break;
				}
			}
			return new Rectangle (x, y, 30, 30);
		}

		public Boolean EatFood (Rectangle Snake, Rectangle Food) 
		{
			if (Snake.Contains(Food)) {
				return true;
			} else {
				return false;
			}
		}
	}
}

