using System;
namespace RheinwerkAdventure
{
	internal class Player : Character, IAttackable
	{
		public int Hitpoints
		{
			get;
		}

		public Player()
		{
		}
	}
}
