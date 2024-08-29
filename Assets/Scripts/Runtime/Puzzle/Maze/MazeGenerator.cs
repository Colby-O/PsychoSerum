using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.Puzzle
{
	public class Maze
	{
		public int width;
		public int height;
		private int[,] _data;

		public Maze(int w, int h)
		{
			_data = new int[w, h];
			width = w;
			height = h;
		}

		public int this[int x, int y]
		{
			get { return _data[x, y]; }
			set { _data[x, y] = value; }
		}
	}


	public class MazeGenerator : MonoBehaviour
	{
		private static Maze _maze = null;

		private static void Shuffle<T>(T[] arr)
		{
			System.Random r = new System.Random();
			int n = arr.Length;
			while (n > 1) 
			{
				int k = r.Next(n--);
				T tmp = arr[n];
				arr[n] = arr[k];
				arr[k] = tmp;
			}
		}

		public static void ResetGenerator()
		{
			_maze = null;
		}

		public static Maze Generate(int width, int height)
		{
			Maze m = new Maze(width, height);

			bool touching(int x, int y) {
				if (x < 0 || x >= m.width || y < 0 || y >= m.height) return true;
				int total = 0;
				if (x+1 == m.width  || m[x+1, y] == 1) total++;
				if (x-1 == -1       || m[x-1, y] == 1) total++;
				if (y+1 == m.height || m[x,   y+1] == 1) total++;
				if (y-1 == -1       || m[x,   y-1] == 1) total++;
				return total > 1;
			}

			void visit(int x, int y) {
				m[x, y] = 1;
				Vector2Int[] dirs = {
					Vector2Int.up,
					Vector2Int.down,
					Vector2Int.left,
					Vector2Int.right,
				};
				Shuffle(dirs);

				foreach (Vector2Int dir in dirs)
				{
					int nx = x + dir.x;
					int ny = y + dir.y;
					if (!touching(nx, ny)) visit(nx, ny);
				}
			}

			visit(2, 1);
			m[2, 0] = 1;

			_maze = m;

			return m;
		}
	}
}
