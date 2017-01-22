﻿using System;
using System.Collections.Generic;

namespace RheinwerkAdventure
{
	internal class World
	{
		public List<Area> Areas
		{
			get;
			private set;
		}

		public World()
		{
			Areas = new List<Area>();
		}
	}
}
