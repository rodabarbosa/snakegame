using System;
namespace SnakeGame.ConsoleApp
{
	public class Position
	{
		public int PosX { get; set; }
		public int PosY { get; set; }

		public Position() {}

		public Position(int posX, int posY)
		{
			PosX = posX;
			PosY = posY;
		}

		public void RandomPosition(int limitX = int.MaxValue, int limitY = int.MaxValue)
		{
			Random rnd = new Random();
			do
			{
				PosX = rnd.Next(limitX);
			} while (PosX == 0);

			do
			{
				PosY = rnd.Next(limitY);
			} while (PosY == 0);
		}
	}
}
