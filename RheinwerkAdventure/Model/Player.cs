using System;
namespace RheinwerkAdventure
{
	public class Player : Character, IAttackable
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
