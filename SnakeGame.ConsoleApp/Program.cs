using System;
using System.Collections.Generic;

namespace SnakeGame.ConsoleApp
{
	static class Program
	{
		// Melhorar o código e fazer com que não fique 'piscando' quando movimentar
		// Cada classe deve ter seu proprio arquivo, isto ajuda na organização.

		// Todos os métodos aqui, são do jogo. Então, uma classe de jogo faz sentido, certo?
		static void Main(string[] args)
		{
			// Quando instancio a classe jogo eu defino o seu tamanho
			new Game(40, 20).Run();
		}
	}
}
