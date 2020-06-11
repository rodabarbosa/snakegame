using System;
using System.Collections.Generic;

namespace SnakeGame.ConsoleApp
{
	public class Game
	{
		private readonly Position _canvasLimit;
		private readonly List<Position> _playerPositions;
		private readonly Position _fruit;

		private readonly Dictionary<ConsoleKey, Action> _keyAction;

		// Ao invés de colocar onde o jogador inicia o sistema irá gerar automaticamente
		// Como é posição, então será um método em position
		public Game(int limitX, int limitY)
		{
			// Readonly só pode ser inicializado no construtor da classe
			_canvasLimit = new Position(limitX, limitY);
			_fruit = new Position();
			_playerPositions = new List<Position>();
			_keyAction = new Dictionary<ConsoleKey, Action>();
			Init();
		}

		private void Init()
		{
			var firstPostion = new Position();
			firstPostion.RandomPosition(_canvasLimit.PosX, _canvasLimit.PosY);
			_playerPositions.Add(firstPostion);

			_keyAction.Add(ConsoleKey.LeftArrow, MoveLeft);
			_keyAction.Add(ConsoleKey.UpArrow, MoveUp);
			_keyAction.Add(ConsoleKey.RightArrow, MoveRight);
			_keyAction.Add(ConsoleKey.DownArrow, MoveDown);
		}

		private void NewFruit() => _fruit.RandomPosition(_canvasLimit.PosX, _canvasLimit.PosY);

		// Colocar o jogador
		private void PlacePlayer()
		{
			string player = "O";
			for (int i = 0; i < _playerPositions.Count; i++)
			{
				Console.SetCursorPosition(_playerPositions[i].PosX, _playerPositions[i].PosY);
				Console.Write(player);
				player = player.ToLower();
			}
		}
		private void CreateCanvas()
		{
			// y = 0
			for (int i = 0; i < _canvasLimit.PosX; i++)
			{
				Console.SetCursorPosition(i, 0);
				Console.Write("*");
			}

			// y > 0 e y < limitY
			for (int i = 1; i < _canvasLimit.PosY; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write("*");

				Console.SetCursorPosition(_canvasLimit.PosX - 1, i);
				Console.Write("*");
			}

			// y = limitY
			for (int i = 0; i < _canvasLimit.PosX; i++)
			{
				Console.SetCursorPosition(i, _canvasLimit.PosY);
				Console.Write("*");
			}
			Console.WriteLine(string.Empty);
		}

		private void GenerateFruit()
		{
			if (_fruit.PosX == 0)
				_fruit.RandomPosition(_canvasLimit.PosX, _canvasLimit.PosY);

			Console.SetCursorPosition(_fruit.PosX, _fruit.PosY);
			Console.Write("F");
		}

		// tratar para não voltar para o mesmo lugar do qual veio
		private void MovePlayer(ConsoleKey keyPressed)
		{
			if (!_keyAction.ContainsKey(keyPressed))
				return;

			_keyAction[keyPressed].Invoke();
		}

		private void MoveUp()
		{
			if (_playerPositions[0].PosY < 2)
				return;

			if (_playerPositions.Count > 1 && _playerPositions[1].PosY == _playerPositions[0].PosY - 1)
				return;

			CleanPlayerPositions();
			TransferPositions();

			_playerPositions[0].PosY--;
			CollectFruit(true, -1);
		}

		private void MoveDown()
		{
			if (_playerPositions[0].PosY > _canvasLimit.PosY - 2)
				return;

			if (_playerPositions.Count > 1 && _playerPositions[1].PosY == _playerPositions[0].PosY + 1)
				return;

			CleanPlayerPositions();
			TransferPositions();

			_playerPositions[0].PosY++;
			CollectFruit(true, 1);
		}

		private void MoveLeft()
		{
			if (_playerPositions[0].PosX < 2)
				return;

			if (_playerPositions.Count > 1 && _playerPositions[1].PosX == _playerPositions[0].PosX - 1)
				return;

			CleanPlayerPositions();
			TransferPositions();

			_playerPositions[0].PosX--;
			CollectFruit(false, -1);
		}

		private void MoveRight()
		{
			if (_playerPositions[0].PosX > _canvasLimit.PosX - 2)
				return;

			if (_playerPositions.Count > 1 && _playerPositions[1].PosX == _playerPositions[0].PosX + 1)
				return;

			CleanPlayerPositions();
			TransferPositions();

			_playerPositions[0].PosX++;
			CollectFruit(false, 1);
		}

		private void TransferPositions()
		{
			for (int i = _playerPositions.Count - 1; i > 0; i--)
			{
				_playerPositions[i].PosX = _playerPositions[i - 1].PosX;
				_playerPositions[i].PosY = _playerPositions[i - 1].PosY;
			}
		}

		private void CleanPlayerPositions()
		{
			for (int i = 0; i < _playerPositions.Count; i++)
				CleanCanvasPosition(_playerPositions[i]);
		}

		private void CleanCanvasPosition(Position position)
		{
			Console.SetCursorPosition(position.PosX, position.PosY);
			Console.Write(" ");
		}

		private void CollectFruit(bool vertical, int value) // +1 / -1 
		{
			if (!HasFruit())
				return;

			ClearFruit();
			int posX = _playerPositions[0].PosX;
			int posY = _playerPositions[0].PosY;

			if (vertical)
				posY += value;
			else
				posX += value;

			var newPosition = new Position(posX, posY);
			_playerPositions.Insert(0, newPosition); // falta definir a direção do crescimento
			// as posições estão ficando iguais
		}
		private bool HasFruit() => _fruit.PosX == _playerPositions[0].PosX && _fruit.PosY == _playerPositions[0].PosY;

		private void ClearFruit()
		{
			_fruit.PosX = 0;
			_fruit.PosY = 0;
		}
		public void Run()
		{
			Console.Clear();
			ConsoleKeyInfo keyPressed = new ConsoleKeyInfo();
			do
			{
				GenerateFruit();
				CreateCanvas();
				PlacePlayer();

				ShowDetailDebug(keyPressed.Key);
				Console.SetCursorPosition(0, _canvasLimit.PosY + 2);
				Console.WriteLine("SNAKE GAME");
				Console.WriteLine("Presione ESC para sair.");

				keyPressed = Console.ReadKey();
				MovePlayer(keyPressed.Key);
			} while (keyPressed.Key != ConsoleKey.Escape);
			Console.WriteLine(" Fim de jogo!");
		}

		private void ShowDetailDebug(ConsoleKey keyPressed)
		{
			int posX = _canvasLimit.PosX + 5;
			Console.SetCursorPosition(posX, 0);
			Console.Write("Tamanho: {0}", _playerPositions.Count);

			for (int i = 0; i < _playerPositions.Count; i++)
			{
				Console.SetCursorPosition(posX, i + 2);
				Console.Write("Posição {0}: {1}, {2};        ", i + 1, _playerPositions[i].PosX, _playerPositions[i].PosY);
			}

			Console.SetCursorPosition(posX, 5 + _playerPositions.Count - 1);
			Console.Write("                                              ");

			Console.SetCursorPosition(posX, 5 + _playerPositions.Count);
			Console.Write("Tecla pressionada: {0}", keyPressed.ToString());
		}
	}
}
