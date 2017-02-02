﻿using System;
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

		public Layer[] Layers
		{
			get;
			private set;
		}

		public List<Item> Items
		{
			get;
			private set;
		}

		public Area(int layers, int width, int height)
		{
			if (width < 5)
			{
				throw new ArgumentException("Spielbereich muss mindestens 5 Zellen breit sein.");
			}

			if (height < 5)
			{
				throw new ArgumentException("Spielbereich muss mindestens 5 Zellen hoch sein.");
			}

			Width = width;
			Height = height;

			Layers = new Layer[layers];
			for (int l = 0; l < layers; l++)
			{
				Layers[l] = new Layer(width, height);
			}

			Items = new List<Item>();
		}

		public bool IsCellBlocked(int x, int y)
		{
			// Sonderfall außerhalb des Spielfeldes
			if (x == 0 || y == 0 || x > Width - 1 || y > Height - 1)
			{
				return true;
			}

			for (int l = 0; l < Layers.Length; l++)
			{
				if (Layers[l].Tiles[x, y].Blocked)
				{
					return true;
				}
			}
			return false;
		}
	}
}
