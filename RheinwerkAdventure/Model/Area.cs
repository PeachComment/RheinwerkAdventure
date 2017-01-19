using System;
using System.Collections.Generic;

namespace RheinwerkAdventure
{
	public class Area
	{
		public List<Tile> Tiles
		{
			get;
			private set;
		}

		public List<Item> Items
		{
			get;
			private set;
		}

		public Area()
		{
			Tiles = new List<Tile>();
			Items = new List<Item>();
		}
	}
}
