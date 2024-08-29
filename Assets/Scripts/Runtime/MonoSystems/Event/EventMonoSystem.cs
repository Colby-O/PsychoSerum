using PlazmaGames.Core;
using PsychoSerum.Ememy;
using PsychoSerum.Interactables;
using PsychoSerum.Task;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PsychoSerum.Puzzle;

namespace PsychoSerum.MonoSystem
{
	internal sealed class EventMonoSystem : MonoBehaviour, IEventMonoSystem
	{
		[SerializeField] private List<DialogueSO> _dialouges;
		private GameObject _spookyPrefab;

		private void Start()
		{
			_spookyPrefab = Resources.Load<GameObject>("Prefabs/EnemyNoLightReal");
		}

		public void Event1()
		{
			Door door = GameObject.FindWithTag("Door1").GetComponent<Door>();
			door.Lock();
			GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[0]);

			FindObjectOfType<Taskboard>().AddTask(new Task.Task(0, "Take Your Psycho-Serum Dose", false));
		}

		public void Event2()
		{
			Door door = GameObject.FindWithTag("Door1").GetComponent<Door>();
			door.Unlock();
			GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[1]);

			FindObjectOfType<Taskboard>().MarkTaskComplete(0);
			FindObjectOfType<Taskboard>().AddTask(new Task.Task(1, "Go To Laboratory", false));
		}

		public void Event3()
		{
			Jumpscare js = GameObject.FindWithTag("Jumpscare1").GetComponent<Jumpscare>();
			js.StartJumpScare(Random.Range(4f, 6f), Random.Range(5f, 6f),  false);
		}

		public void Event4()
		{
			GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[2]);
			FindObjectOfType<Taskboard>().MarkTaskComplete(1);
			FindObjectOfType<Taskboard>().AddTask(new Task.Task(2, "Complete The First Test at Laboratory Room 1", false));
		}

		public void Event5()
		{
			Door door1 = GameObject.FindWithTag("Door2").GetComponent<Door>();
			Door door2 = GameObject.FindWithTag("Door3").GetComponent<Door>();

			door1.Close();
			door2.Close();
			door2.Lock();
			GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[3]);
		}

		public void Event6()
		{
			GameObject.FindWithTag("Trigger4").GetComponent<Trigger>().isActive = true;
			Door door = GameObject.FindWithTag("Door3").GetComponent<Door>();
			Door door2 = GameObject.FindWithTag("Door4").GetComponent<Door>();
			door.Unlock();
			door2.Unlock();
			GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[4]);

			FindObjectOfType<Taskboard>().MarkTaskComplete(2);
			FindObjectOfType<Taskboard>().AddTask(new Task.Task(2, "Complete The Second Test at Laboratory Room 2", false));
		}

		public void Event7()
		{
			GameObject.FindWithTag("Trigger4").GetComponent<Trigger>().isActive = true;
			Door door = GameObject.FindWithTag("Door3").GetComponent<Door>();
			Door door2 = GameObject.FindWithTag("Door4").GetComponent<Door>();
			door.Unlock();
			door2.Unlock();
			GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[5]);

			FindObjectOfType<Taskboard>().MarkTaskComplete(2);
			FindObjectOfType<Taskboard>().AddTask(new Task.Task(2, "Complete The Second Test at Laboratory Room 2", false));
		}

		public void Event8()
		{
			Door door1 = GameObject.FindWithTag("Door2").GetComponent<Door>();
			Door door2 = GameObject.FindWithTag("Door3").GetComponent<Door>();
			Jumpscare js = GameObject.FindWithTag("Jumpscare2").GetComponent<Jumpscare>();

			door1.Close();
			door2.Close();
			door1.Lock();
			door2.Lock();


			js.onComplete = () => 
			{
				door1.Unlock();
				door2.Unlock();
			};
			js.StartJumpScare(Random.Range(1f, 2f), Random.Range(9f, 10f), false);
		}

		public void Event1001(GameObject from)
		{
			Vector3 pos = PsychoSerumGameManager.player.transform.position;
			float dir = Vector3.SignedAngle(
				PsychoSerumGameManager.player.transform.forward,
				Vector3.forward,
				Vector3.up
			);

			MazeController mc = GameObject.FindObjectOfType<MazeController>();
			Maze m = mc.GetMaze();
			Vector3 mPos = mc.GetMazePosition();
			float mScale = mc.GetMazeScale();

			Vector3 rel = pos - mPos;
			int tx = (int)Mathf.Floor(rel.z / mScale);
			int ty = (int)Mathf.Floor(rel.x / mScale);

			HashSet<Vector2Int> visited = new();

			List<Vector2Int> possible = new();

			void travel(int x, int y, int dist)
			{
				Vector2Int pos = new(x, y);
				if (m[x, y] != 1 || visited.Contains(pos)) return;
				visited.Add(pos);
				if (dist <= 0 && x != tx && y != ty)
				{
					possible.Add(pos);
					return;
				}
				travel(x+1, y, dist-1);
				travel(x-1, y, dist-1);
				travel(x, y+1, dist-1);
				travel(x, y-1, dist-1);
			}

			if (m[tx, ty] != 1)
			{
				if (m[tx+1, ty] == 1) tx += 1;
				else if (m[tx-1, ty] == 1) tx -= 1;
				else if (m[tx, ty+1] == 1) ty += 1;
				else if (m[tx, ty-1] == 1) ty -= 1;
			}
			travel(tx, ty, 5);

			if (possible.Count == 0) Debug.Log("NO SPAWN?????");
			Vector2Int spawnSpot = possible.OrderBy(p => Mathf.Abs(
				dir - Vector2.SignedAngle(new Vector2(tx, ty), (Vector2)p)
			)).First();

			Debug.Log(string.Format("[{0}, {1}] -> [{2}, {3}]", tx, ty, spawnSpot.x, spawnSpot.y));

			Vector3 spawnPos = new Vector3(
				mPos.x + (float)spawnSpot.y * mScale,
				mPos.y + 1.5f,
				mPos.z + (float)spawnSpot.x * mScale
			);

			GameObject c = GameObject.Instantiate(_spookyPrefab);
			c.transform.position = spawnPos;
		}

		public void RunEvent(int id, GameObject from = null)
		{
			if (id == 0) Event1();
			else if (id == 1) Event2();
			else if (id == 2) Event3();
			else if (id == 3) Event4();
			else if (id == 4) Event5();
			else if (id == 5) Event6();
			else if (id == 6) Event7();
			else if (id == 7) Event8();
			else if (id == 1001) Event1001(from);
		}
	}
}
