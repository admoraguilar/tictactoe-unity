using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WishfulDroplet;


namespace FoxCubTicTacToe {
	/// <summary>
	/// Use for small actions that needs delay.
	/// </summary>
	public class DelayedAction : MonoBehaviour {
		[SerializeField] private UnityEvent _action;
		[SerializeField] private Timer _time;


		private void OnEnable() {
			_time.OnElapsed += _action.Invoke;
		}

		private void OnDisable() {
			_time.OnElapsed -= _action.Invoke;
		}

		private void Update() {
			_time.Update(Time.deltaTime);
		}
	}
}