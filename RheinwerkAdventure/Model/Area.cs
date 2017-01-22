using System;
using System.Collections.Generic;

namespace RheinwerkAdventure
{
	internal class Area
	{
		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}

		public Tile[,] Tiles
		{
			get;
			private set;
		}

		public List<Item> Items
		{
			get;
			private set;
		}

		public Area(int width, int height)
		{
			Width = width;
			Height = height;
			Tiles = new Tile[width, height];
			Items = new List<Item>();
		}
	}
}
