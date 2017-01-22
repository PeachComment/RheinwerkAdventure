using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RheinwerkAdventure
{
	internal class SimulationComponent : GameComponent
	{
		private readonly RheinwerkGame game;

		public World World
		{
			get;
			private set;
		}

		public Player Player
		{
			get;
			private set;
		}

		public SimulationComponent(RheinwerkGame game) : base(game)
		{
			this.game = game;
			NewGame();
		}

		public void NewGame()
		{
			World = new World();

			Area area = new Area(30, 20);

			for (int x = 0; x < area.Width; x++)
			{
				for (int y = 0; y < area.Height; y++)
				{
					area.Tiles[x, y] = new Tile();
				}
			}

			Player = new Player() { Position = new Vector2(25, 10), Radius = 0.25f };
			Diamond diamond = new Diamond() { Position = new Vector2(10, 10), Radius = 0.25f };

			area.Items.Add(Player);
			area.Items.Add(diamond);

			World.Areas.Add(area);
		}

		public override void Update(GameTime gameTime)
		{
			#region Player Input

			Player.Velocity = game.Input.Movement * 10f;

			#endregion

			#region Character Movement

			foreach (var area in World.Areas)
			{
				foreach (var character in area.Items.OfType<Character>())
				{
					character.Position += character.Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
				}
			}

			#endregion

			base.Update(gameTime);
		}
	}
}
