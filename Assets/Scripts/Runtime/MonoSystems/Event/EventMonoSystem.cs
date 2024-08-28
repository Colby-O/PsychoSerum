using PlazmaGames.Core;
using PsychoSerum.Ememy;
using PsychoSerum.Interactables;
using PsychoSerum.Task;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
    internal sealed class EventMonoSystem : MonoBehaviour, IEventMonoSystem
    {
        [SerializeField] private List<DialogueSO> _dialouges;

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

        public void RunEvent(int id)
        {
            if (id == 0) Event1();
            else if (id == 1) Event2();
            else if (id == 2) Event3();
            else if (id == 3) Event4();
            else if (id == 4) Event5();
            else if (id == 5) Event6();
            else if (id == 6) Event7();
            else if (id == 7) Event8();
        }
    }
}
