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

		private float gap = 0.00001f;

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

				bool collision = false;
				int loops = 0;

				do
				{
					Vector2 position = item.Position + item.move;
					int minCellX = (int)(position.X - item.Radius);
					int maxCellX = (int)(position.X + item.Radius);
					int minCellY = (int)(position.Y - item.Radius);
					int maxCellY = (int)(position.Y + item.Radius);

					// Console.WriteLine(minCellX);
					// Console.WriteLine(maxCellX);
					// Console.WriteLine(minCellY);
					// Console.WriteLine(maxCellY);

					collision = false;
					float minImpact = 2f;
					int minAxis = 0;

					for (int x = minCellX; x <= maxCellX; x++)
					{
						for (int y = minCellY; y <= maxCellY; y++)
						{
							// ist die Zelle blockiert?
							if (!area.IsCellBlocked(x, y))
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

							collision = true;

							float diffX = float.MaxValue;
							if (item.move.X > 0)
							{
								diffX = (position.X + item.Radius) - x + gap;
							}
							if (item.move.X < 0)
							{
								diffX = (position.X - item.Radius) - (x + 1) - gap;
							}

							float impactX = 1f - (diffX / item.move.X);

							float diffY = float.MaxValue;
							if (item.move.Y > 0)
							{
								diffY = (position.Y + item.Radius) - y + gap;
							}
							if (item.move.Y < 0)
							{
								diffY = (position.Y - item.Radius) - (y + 1) - gap;
							}

							float impactY = 1f - (diffY / item.move.Y);

							// 1 = x, 2 = y
							int axis = 0;
							float impact = 0f;

							if (impactX > impactY)
							{
								axis = 1;
								impact = impactX;
							}
							else
							{
								axis = 2;
								impact = impactY;
							}

							// Ist diese Kollision eher als die bisher erkannten
							if (impact < minImpact)
							{
								minImpact = impact;
								minAxis = axis;
							}
						}
					}

					if (collision)
					{
						if (minAxis == 1)
						{
							item.move *= new Vector2(minImpact, 1f);
						}
						if (minAxis == 2)
						{
							item.move *= new Vector2(1f, minImpact);
						}
					}

					loops++;

				} while (collision && loops < 2);

				item.Position += item.move;
				item.move = Vector2.Zero;
			}
		}
	}

}
