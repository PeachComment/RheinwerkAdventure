using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RheinwerkAdventure
{
	internal class SimulationComponent : GameComponent
	{
		public const int AREA_LAYERS = 2;
		public const int AREA_WIDTH = 30;
		public const int AREA_HEIGHT = 20;

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

			Area area = new Area(AREA_LAYERS, AREA_WIDTH, AREA_HEIGHT);
			initArea(area);

			Player = new Player() { Position = new Vector2(25, 10), Radius = 0.25f };
			Diamond diamond = new Diamond() { Position = new Vector2(10, 10), Radius = 0.25f };

			area.Items.Add(Player);
			area.Items.Add(diamond);

			World.Areas.Add(area);
		}

		private void initArea(Area area)
		{
			for (int x = 0; x < area.Width; x++)
			{
				for (int y = 0; y < area.Height; y++)
				{
					initLayers(area, x, y);
				}
			}
		}

		private void initLayers(Area area, int x, int y)
		{
			for (int l = 0; l < area.Layers.Length; l++)
			{
				area.Layers[l].Tiles[x, y] = new Tile();
			}

			if (x == 0 || y == 0 || x == area.Width - 1 || y == area.Height - 1)
			{
				for (int l = 0; l < area.Layers.Length; l++)
				{
					area.Layers[l].Tiles[x, y].Blocked = true;
				}
			}
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
					character.move += character.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
					calculateCharacterCollisionsWithItems(area, character);
					calculateItemCollisionsWithCells(area);
				}
			}

			#endregion

			base.Update(gameTime);
		}

		private void calculateCharacterCollisionsWithItems(Area area, Character character)
		{
			foreach (var item in area.Items)
			{
				if (item.Equals(character))
				{
					continue;
				}

				Vector2 distance = (item.Position + item.move) - (character.Position + character.move);
				float overlap = item.Radius + character.Radius - distance.Length();
				if (overlap > 0f)
				{
					Vector2 resolution = distance * (overlap / (distance.Length()));
					if (item.Fixed && !character.Fixed)
					{
						// Item fixiert
						character.move -= resolution;
					}
					else if (!item.Fixed && character.Fixed)
					{
						// Character fixiert
						item.move += resolution;
					}
					else if (!item.Fixed && !character.Fixed)
					{
						// keiner fixiert
						float totalMass = item.Mass + character.Mass;
						character.move -= resolution * (item.Mass / totalMass);
						item.move += resolution * (character.Mass / totalMass);
					}
				}
			}
		}

		private void calculateItemCollisionsWithCells(Area area)
		{
			foreach (var item in area.Items)
			{
				Vector2 position = item.Position + item.move;
				int minCellX = (int)(position.X - item.Radius);
				int maxCellX = (int)(position.X + item.Radius);
				int minCellY = (int)(position.Y - item.Radius);
				int maxCellY = (int)(position.Y + item.Radius);

				for (int x = minCellX; x <= maxCellX; x++)
				{
					for (int y = minCellY; y <= maxCellY; y++)
					{
						// ist die Zelle blockiert?
						if (!area.isCellBlocked(x, y))
						{
							continue;
						}

						if (position.X - item.Radius > x + 1 ||
						   position.X + item.Radius < x ||
						   position.Y - item.Radius > y + 1 ||
						   position.Y + item.Radius < y)
						{
							continue;
						}
					}
				}

				item.Position += item.move;
				item.move = Vector2.Zero;
			}
		}
}
}
