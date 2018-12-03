using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace WishfulDroplet {
	public class StackStateMachine : IStackStateMachine {
		private Stack<IState> _stateStack = new Stack<IState>();


		public IState currentState {
			get { return _stateStack.Count > 0 ? _stateStack.Peek() : null; }
		}

		public IState[] states {
			get { return _stateStack.ToArray(); }
		}

		public int stateStackCount {
			get { return _stateStack.Count; }
		}

		/// <summary>
		/// Pops all state and replaces the last.
		/// </summary>
		/// <param name="state"></param>
		public void SetState(IState state) {
			if(_stateStack.Count > 0) {
				if(currentState.Equals(states)) return;
			}

			PopAllStates();

			IState nextState = state;
			IState prevState = currentState;

			if(_stateStack.Count > 0) {
				prevState.OnExit(nextState);
				_stateStack.Pop();
			}

			_stateStack.Push(nextState);
			nextState.OnEnter(prevState);
		}

		/// <summary>
		/// Push a state on top of the current.
		/// </summary>
		/// <param name="state"></param>
		public void PushState(IState state) {
			IState nextState = state;
			IState prevState = currentState;

			if(nextState != null) {
				// Override the top state
				if(_stateStack.Count > 0) {
					if(prevState.Equals(nextState)) return;
					prevState.OnOverride(nextState);
				}

				// Push the new state
				_stateStack.Push(nextState);
				nextState.OnEnter(prevState);
			}
		}

		/// <summary>
		/// Pops the top state. If the stack only contains one state then it just returns that.
		/// </summary>
		/// <returns></returns>
		public IState PopState() {
			IState nextState = null;
			IState prevState = null;

			if(_stateStack.Count > 1) {
				// Pop the top state
				prevState = _stateStack.Pop();
				nextState = currentState;
				prevState.OnExit(nextState);

				// Resume the overriden state
				nextState.OnResume(prevState);
			} else {
				nextState = currentState;
			}

			return nextState;
		}
		
		/// <summary>
		/// Pops all state except the last.
		/// </summary>
		public void PopAllStates() {
			while(_stateStack.Count > 1) {
				PopState();
			}
		}
	}


	public interface IStackStateMachine {
		IState currentState { get; }
		IState[] states { get; }
		int stateStackCount { get; }

		void SetState(IState state);
		void PushState(IState state);
		IState PopState();
		void PopAllStates();
	}


	public interface IState {
		void OnEnter(IState prevState);
		void OnExit(IState nextState);

		void OnOverride(IState nextState);
		void OnResume(IState prevState);
	}
}