using PlazmaGames.Core.MonoSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychoSerum.MonoSystem
{
   internal interface IEventMonoSystem : IMonoSystem
	{
		public void RunEvent(int id, GameObject from = null);
	}

}
