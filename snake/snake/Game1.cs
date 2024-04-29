#region File Description
//-----------------------------------------------------------------------------
// snakeGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

#endregion

namespace snake
{
	/// <summary>
	/// Default Project Template
	/// </summary>
	/// 
	enum SnakeDirection {
		UP = 1,
		RIGHT = 2,
		DOWN = 3,
		LEFT = 4
	}
	public class Game1 : Game
	{

		#region Fields

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D SnakeGraphic;
		SnakeBody Snakey;
		FoodBlock Food = new FoodBlock ();

		Boolean GameOver;
		Color GameColor;
		int CurrentScore = 0;

		Rectangle SnakeHead;
		Rectangle FoodPosition;
		private SpriteFont font;

		SnakeDirection direction;
		SnakeDirection newDirection;
		int[] movement = new int[2];

		List<SnakeBody> BodyParts = new List<SnakeBody>();

		private int xPos;
		private int yPos;



		Texture2D pixel;

		#endregion

		#region Initialization

		public Game1 ()
		{

			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Assets";

			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = 600;
			graphics.PreferredBackBufferHeight = 600;

			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromMilliseconds(80); // 20 milliseconds, or 50 FPS.

		}

		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize ()
		{
			BodyParts.Clear ();
			Snakey = new SnakeBody(90,300);
			BodyParts.Add (Snakey);
			Snakey.GenerateSnake (BodyParts);

			//First Food Rect.
			FoodPosition = Food.GenerateFood (BodyParts);

			direction = SnakeDirection.RIGHT;
			newDirection = SnakeDirection.RIGHT;

			GameColor = Color.White;

			base.Initialize ();

		}


		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			
			// TODO: use this.Content to load your game content here eg.
			SnakeGraphic = Content.Load<Texture2D> ("Snake");

			pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it



			// Load game font
			//gameFont = Content.Load<SpriteFont> ("font");
			font = Content.Load<SpriteFont> ("Arial");
		}

		#endregion

		#region Update and Draw

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{	
			KeyboardState KBS = Keyboard.GetState();

			if (KBS.IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			if (KBS.IsKeyDown (Keys.R)) { //TODO: Make an actual reset function that works
				BodyParts.Clear ();
				Snakey = new SnakeBody (90, 300);
				BodyParts.Add (Snakey);
				Snakey.GenerateSnake (BodyParts);
				direction = SnakeDirection.RIGHT;
				newDirection = SnakeDirection.RIGHT;
				GameOver = false;
				GameColor = Color.White;
				CurrentScore = 0;
			}
			if ((KBS.IsKeyDown (Keys.Up) || KBS.IsKeyDown (Keys.W)) && direction != SnakeDirection.DOWN) {
				newDirection = SnakeDirection.UP;
			}
			if ((KBS.IsKeyDown (Keys.Down) || KBS.IsKeyDown (Keys.S)) && direction != SnakeDirection.UP) {
				newDirection = SnakeDirection.DOWN;
			}
			if ((KBS.IsKeyDown (Keys.Left) || KBS.IsKeyDown (Keys.A)) && direction != SnakeDirection.RIGHT) {
				newDirection = SnakeDirection.LEFT;
			}
			if ((KBS.IsKeyDown (Keys.Right) || KBS.IsKeyDown (Keys.D)) && direction != SnakeDirection.LEFT) {
				newDirection = SnakeDirection.RIGHT;
			}
				
			if (Food.EatFood (SnakeHead, FoodPosition) == true) { //if snake collides with a food, eat it
				FoodPosition = Food.GenerateFood (BodyParts); //eat function
				Snakey.GrowSnake (BodyParts);				  //grow function
				CurrentScore = Snakey.IncreaseScore (CurrentScore);
			}

			xPos = movement [0];
			yPos = movement [1];

			if (Snakey.XPosition % 30 == 0 && Snakey.YPosition % 30 == 0) {  // This saves the position you moved, 
				direction = newDirection;									 // but only allows movement on the 30 block grid
				Snakey.LastXPosition = Snakey.XPosition;
				Snakey.LastYPosition = Snakey.YPosition;
			}

			if (GameOver == false) { //Only move if GameOver is not true
				movement = Snakey.MoveHead ((int)direction);
				GameOver = Snakey.SnakeCollision (BodyParts); //TODO: Make a real game over state

				Snakey.LastXPosition = Snakey.XPosition; //Saving the last position of the head to the list
				Snakey.LastYPosition = Snakey.YPosition;

				Snakey.MoveBody (BodyParts);
			} else {
				GameColor = Color.Red; //TODO: gameover function
			}
			base.Update (gameTime);
		}

		private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
		{
			// Draw top line
			spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + 30, rectangleToDraw.Width, thicknessOfBorder), borderColor);

			// Draw left line
			spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + 30, thicknessOfBorder, rectangleToDraw.Height), borderColor);

			// Draw right line
			spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
				rectangleToDraw.Y + 30,
				thicknessOfBorder,
				rectangleToDraw.Height), borderColor);
			// Draw bottom line
			spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
				rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
				rectangleToDraw.Width,
				thicknessOfBorder), borderColor);
		}

		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbufferG
			graphics.GraphicsDevice.Clear (Color.Black);

			spriteBatch.Begin ();

			//borders
			Rectangle titleSafeRectangle = GraphicsDevice.Viewport.TitleSafeArea;

			// Call our method (also defined in this blog-post)
			DrawBorder(titleSafeRectangle, 2, GameColor);

			Rectangle SnakeRectangle = new Rectangle (0, 0, 30, 30); // The sprite for Snake Parts
			Rectangle FoodRectangle = new Rectangle (30, 0, 30, 30); // The sprite for food

			SnakeHead = new Rectangle (BodyParts[0].XPosition, BodyParts[0].YPosition, 30, 30); // The snake head is it's own rectangle
			spriteBatch.Draw (SnakeGraphic, SnakeHead, SnakeRectangle, GameColor);				// for collision purposes.

			for (int i = 1; i < BodyParts.Count; i++) { //Printing the whole snake body
				Rectangle BodySquare = new Rectangle (BodyParts [i].XPosition, BodyParts [i].YPosition, 30, 30);
				spriteBatch.Draw (SnakeGraphic, BodySquare, SnakeRectangle, GameColor);
			}

			spriteBatch.Draw (SnakeGraphic, FoodPosition, FoodRectangle, GameColor); //drawing food

			spriteBatch.DrawString(font, "Score: " + CurrentScore, new Vector2(400, 0), Color.White); //Drawing the player's score
			spriteBatch.End ();
			base.Draw (gameTime);
		}

		#endregion
	}
}
