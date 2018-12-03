using System;
using UnityEngine;


namespace WishfulDroplet {
	[Serializable]
	public class Timer {
		public event Action OnElapsed = delegate { };

		[SerializeField] private float _duration;
		

		public float timer {
			get;
			private set;
		}

		public bool isElapsed {
			get;
			private set;
		}

		public void Update(float updateRate) {
			if(timer >= _duration) {
				if(!isElapsed) {
					OnElapsed();
					isElapsed = true;
				}
			} else {
				timer += updateRate;
			}
		}

		public void Reset() {
			timer = 0f;
			isElapsed = false;
		}
	}
}