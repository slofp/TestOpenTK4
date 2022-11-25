using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK {
	internal class Prog {

		public static void Main(string[] args) {
			using var game = new Game(1600, 900, "TestOpenTKKKKKK");

			game.Run();
		}
	}
}
