using PlazmaGames.Core;
using PsychoSerum.Ememy;
using PsychoSerum.Enemy;
using PsychoSerum.Interactables;
using PsychoSerum.Puzzle;
using PsychoSerum.Task;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal sealed class EventMonoSystem : MonoBehaviour, IEventMonoSystem
    {
        [SerializeField] private List<DialogueSO> _dialouges;
	    private GameObject _spookyPrefab;

        private GameObject _blood;
        private GameObject _group1;
        private GameObject _bf;

        public void Event1()
        {
            // Intro
            Door door = GameObject.FindWithTag("Door1").GetComponent<Door>();
            door.Lock();
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[0]);

            FindObjectOfType<Taskboard>().AddTask(new Task.Task(0, "Take Your Psycho-Serum Dose", false));
        }

        public void Event2()
        {
            // gather items
            Door door = GameObject.FindWithTag("Door1").GetComponent<Door>();
            door.Unlock();
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[1]);

            FindObjectOfType<Taskboard>().MarkTaskComplete(0);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(1, "Go To Laboratory", false));
        }

        public void Event3()
        {
            // first jump scare
            Jumpscare js = GameObject.FindWithTag("Jumpscare1").GetComponent<Jumpscare>();
            js.StartJumpScare(UnityEngine.Random.Range(4f, 6f), UnityEngine.Random.Range(5f, 6f),  false);
        }

        public void Event4()
        {
            // on enter lab
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[2]);
            FindObjectOfType<Taskboard>().MarkTaskComplete(1);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(2, "Complete Retention Test", false));
        }

        public void Event5()
        {
            // on enter first test room
            Door door1 = GameObject.FindWithTag("Door2").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door3").GetComponent<Door>();

            door1.Close();
            door2.Close();
            door2.Lock();
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[3]);
        }

        public void Event6()
        {
            // on first test completed sucess
            GameObject.FindWithTag("Trigger4").GetComponent<Trigger>().isActive = true;
            Door door = GameObject.FindWithTag("Door3").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door4").GetComponent<Door>();
            door.Unlock();
            door2.Unlock();
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[4]);

            FindObjectOfType<Taskboard>().MarkTaskComplete(2);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(3, "Complete Perception Test", false));
        }

        public void Event7()
        {
            // on first test completed fail
            GameObject.FindWithTag("Trigger4").GetComponent<Trigger>().isActive = true;
            Door door = GameObject.FindWithTag("Door3").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door4").GetComponent<Door>();
            door.Unlock();
            door2.Unlock();
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[5]);

            FindObjectOfType<Taskboard>().MarkTaskComplete(2);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(3, "Complete Perception Test", false));
        }

        public void Event8()
        {
            // second jumpscare
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
            js.StartJumpScare(UnityEngine.Random.Range(4f, 6f), UnityEngine.Random.Range(5f, 6f), false);
        }

        public void Event9()
        {
            // on enter second test room
            Door door1 = GameObject.FindWithTag("Door4").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door5").GetComponent<Door>();
            door1.Close();
            door2.Close();
            door1.Lock();
            door2.Lock();

            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[6]);
        }

        public void Event10()
        {
            // test 2 hint
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[7]);
        }

        public void Event11()
        {
            // test 2 sucess
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[8]);

            GameObject.FindWithTag("Trigger6").GetComponent<Trigger>().isActive = true;
            Door door1 = GameObject.FindWithTag("Door4").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door5").GetComponent<Door>();
            Door door3 = GameObject.FindWithTag("Door6").GetComponent<Door>();
            door1.Unlock();
            door2.Unlock();
            door3.Unlock();

            FindObjectOfType<Taskboard>().MarkTaskComplete(3);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(4, "Complete Perception Test", false));
        }
        public void Event12()
        {
            // test 2 fail
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[9]);

            GameObject.FindWithTag("Trigger6").GetComponent<Trigger>().isActive = true;
            Door door1 = GameObject.FindWithTag("Door4").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door5").GetComponent<Door>();
            Door door3 = GameObject.FindWithTag("Door6").GetComponent<Door>();
            door1.Unlock();
            door2.Unlock();
            door3.Unlock();

            FindObjectOfType<Taskboard>().MarkTaskComplete(3);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(4, "Complete Spatial Aawarenesses Test", false));
        }

        public void Event13()
        {
            // fake jumpscare
            Jumpscare js = GameObject.FindWithTag("Jumpscare3").GetComponent<Jumpscare>();
            js.StartJumpScare(UnityEngine.Random.Range(4f, 6f), UnityEngine.Random.Range(5f, 6f), true);
        }

        public void Event14()
        {
            // last puzzle start
            Door door1 = GameObject.FindWithTag("Door6").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door7").GetComponent<Door>();

            door1.Close();
            door2.Close();
            door1.Lock();
            door2.Lock();

            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[10]);

        }

        public void Event15()
        {
            // last puzzle end bad
            Door door1 = GameObject.FindWithTag("Door6").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door7").GetComponent<Door>();

            GameObject.FindWithTag("Trigger 12").GetComponent<Trigger>().isActive = true;
            GameObject.FindWithTag("Trigger18").GetComponent<Trigger>().isActive = true;

            door1.Unlock();
            door2.Unlock();

            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[11]);


            FindObjectOfType<Taskboard>().MarkTaskComplete(4);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(5, "Go back to your cell", false));
        }

        public void Event16()
        {
            // last puzzle end good
            Door door1 = GameObject.FindWithTag("Door6").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("Door7").GetComponent<Door>();

            GameObject.FindWithTag("Trigger 12").GetComponent<Trigger>().isActive = true;
            GameObject.FindWithTag("Trigger18").GetComponent<Trigger>().isActive = true;

            door1.Unlock();
            door2.Unlock();

            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[12]);

            FindObjectOfType<Taskboard>().MarkTaskComplete(4);
            FindObjectOfType<Taskboard>().AddTask(new Task.Task(5, "Go back to your cell", false));
        }

        public void Event17()
        {
            FindObjectOfType<MazeController>().CloseGate();
        }

        public void Event18()
        {
            _bf.SetActive(true);
            Door door1 = GameObject.FindWithTag("SubjectDoors1").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("SubjectDoors2").GetComponent<Door>();
            Door door3 = GameObject.FindWithTag("SubjectDoors3").GetComponent<Door>();
            Door door4 = GameObject.FindWithTag("SubjectDoors4").GetComponent<Door>();
            Door door5 = GameObject.FindWithTag("SubjectDoors5").GetComponent<Door>();

            door1.Close();
            door2.Close();
            door3.Close();
            door4.Close();
            door5.Close();

            door1.Lock();
            door2.Lock();
            door3.Lock();
            door4.Lock();
            door5.Lock();
        }

        public void Event19()
        {
            _bf.SetActive(false);
            FindObjectOfType<Taskboard>().MarkTaskComplete(5);
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialouges[13]);
        }

        public void Event100()
        {
            _blood.SetActive(true);

            GameObject.FindWithTag("Trigger9").GetComponent<Trigger>().isActive = true;
            GameObject.FindWithTag("Trigger10").GetComponent<Trigger>().isActive = true;

            Door door1 = GameObject.FindWithTag("SubjectDoors1").GetComponent<Door>();
            Door door2 = GameObject.FindWithTag("SubjectDoors2").GetComponent<Door>();
            Door door3 = GameObject.FindWithTag("SubjectDoors3").GetComponent<Door>();
            Door door4 = GameObject.FindWithTag("SubjectDoors4").GetComponent<Door>();
            Door door5 = GameObject.FindWithTag("SubjectDoors5").GetComponent<Door>();

            door1.PlayBang();
            door2.PlayBang();
            door3.PlayBang();
            door4.PlayBang();
            door5.PlayBang();
        }

        public void Event101()
        {
            _group1.SetActive(true);
            _group1.transform.GetChild(0).GetComponent<EnemyMove>().StartMovement();
            _group1.transform.GetChild(1).GetComponent<EnemyMove>().StartMovement();
            _group1.transform.GetChild(2).GetComponent<EnemyMove>().StartMovement();
        }

        private IEnumerator End()
        {
            GameObject.FindWithTag("L4").SetActive(false);
            GameObject.FindWithTag("L1").SetActive(false);
            yield return new WaitForSeconds(1);
            GameObject.FindWithTag("L2").SetActive(false);
            yield return new WaitForSeconds(1);
            GameObject.FindWithTag("L3").SetActive(false);
            PsychoSerumGameManager.player.Final();
        }

        public void Event102()
        {
            PsychoSerumGameManager.allowInput = false;
            StartCoroutine(End());
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
            else if (id == 8) Event9();
            else if (id == 9) Event10();
            else if (id == 10) Event11();
            else if (id == 11) Event12();
            else if (id == 12) Event13();
            else if (id == 13) Event14();
            else if (id == 100) Event100();
            else if (id == 101) Event101();
            else if (id == 102) Event102();
	        else if (id == 1001) Event1001(from);
            else if (id == 14) Event15();
            else if (id == 15) Event16();
            else if (id == 16) Event17();
            else if (id == 17) Event18();
            else if (id == 18) Event19();
        }

        private void Start()
        {
            _blood = GameObject.FindWithTag("Blood");
            _blood.SetActive(false);
            _group1 = GameObject.FindWithTag("EnemyGroup1");
            _group1.SetActive(false);
            _bf = GameObject.FindWithTag("BeforeFinal");
            _bf.SetActive(false);
            _spookyPrefab = Resources.Load<GameObject>("Prefabs/EnemyNoLightReal");
        }
    }
}
