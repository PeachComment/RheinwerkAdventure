using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RheinwerkAdventure
{
	internal class InputComponent : GameComponent
	{
		public Vector2 Direction
		{
			get;
			private set;
		}

		public InputComponent(Game1 game) : base(game)
		{
		}

		public override void Update(GameTime gameTime)
		{
			GamePadState state = GamePad.GetState(PlayerIndex.One);

			Direction = state.ThumbSticks.Left * new Vector2(1f, -1f);

			//if (Keyboard.GetState().IsKeyDown(Keys.Left))
			//{
			//	pos += new Vector2(-1f, 0f);
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.Right))
			//{
			//	pos += new Vector2(1f, 0f);
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.Up))
			//{
			//	pos += new Vector2(0f, -1f);
			//}
			//if (Keyboard.GetState().IsKeyDown(Keys.Down))
			//{
			//	pos += new Vector2(0f, 1f);
			//}

			//if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			//{
			//	int x = Mouse.GetState().X;
			//	int y = Mouse.GetState().Y;
			//	pos = new Vector2(x, y);
			//}

			base.Update(gameTime);
		}
	}
}
