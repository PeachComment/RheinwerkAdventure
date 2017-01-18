using System;
using Microsoft.Xna.Framework;

namespace RheinwerkAdventure
{
	internal class SimulationComponent : GameComponent
	{
		private Game1 game;

		private Vector2 ballVelocity = new Vector2(0.3f, 0.2f);

		public Vector2 BallPosition
		{
			get;
			private set;
		}

		public float PlayerPosition
		{
			get;
			private set;
		}

		public float PlayerSize
		{
			get;
			private set;
		}

		public SimulationComponent(Game1 game) : base(game)
		{
			this.game = game;

			BallPosition = new Vector2(0.3f, 0.2f);
			PlayerPosition = 0.5f;
			PlayerSize = 0.2f;
		}

		public override void Update(GameTime gameTime)
		{
			BallPosition += ballVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
			PlayerPosition += game.Input.Direction.Y * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

			if (BallPosition.X < 0f)
			{
				if ((BallPosition.Y < (PlayerPosition - (PlayerSize / 2f)) || (BallPosition.Y > (PlayerPosition + (PlayerSize / 2f)))))
				{
					throw new Exception("Player hat verloren");
				}
				BallPosition = new Vector2(0f, BallPosition.Y);
				ballVelocity *= new Vector2(-1f, 1f);
			}
			if (BallPosition.Y < 0f)
			{
				BallPosition = new Vector2(BallPosition.X, 0f);
				ballVelocity *= new Vector2(1f, -1f);
			}
			if (BallPosition.X > 1f)
			{
				BallPosition = new Vector2(1f, BallPosition.Y);
				ballVelocity *= new Vector2(-1f, 1f);
			}
			if (BallPosition.Y > 1f)
			{
				BallPosition = new Vector2(BallPosition.X, 1f);
				ballVelocity *= new Vector2(1f, -1f);
			}

			base.Update(gameTime);
		}
	}
}
