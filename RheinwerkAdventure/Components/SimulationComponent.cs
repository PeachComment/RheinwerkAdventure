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

		static void initArea(Area area)
		{
			for (int x = 0; x < area.Width; x++)
			{
				for (int y = 0; y < area.Height; y++)
				{
					initLayers(area, x, y);
				}
			}
		}

		static void initLayers(Area area, int x, int y)
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
					character.Position += character.Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;

					foreach (var item in area.Items)
					{
						if (item.Equals(character))
						{
							continue;
						}

						Vector2 distance = item.Position - character.Position;
						float overlap = item.Radius + character.Radius - distance.Length();
						if (overlap > 0f)
						{
							Vector2 resolution = distance * (overlap / (distance.Length()));
							if (item.Fixed && !character.Fixed)
							{
								// Item fixiert
								character.Position -= resolution;
							}
							else if (!item.Fixed && character.Fixed)
							{
								// Character fixiert
								item.Position += resolution;
							}
							else if (!item.Fixed && !character.Fixed)
							{
								// keiner fixiert
								float totalMass = item.Mass + character.Mass;
								character.Position -= resolution * (item.Mass / totalMass);
								item.Position += resolution * (character.Mass / totalMass);
							}
						}
					}
						
				}
			}

			#endregion

			base.Update(gameTime);
		}
	}
}
