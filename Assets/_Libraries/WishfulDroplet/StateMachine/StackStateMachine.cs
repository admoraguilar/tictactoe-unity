using System;
using System.Collections.Generic;
using UnityEngine;


namespace WishfulDroplet {
	/// <summary>
	/// A push-down automaton state machine.
	/// </summary>
	/// <typeparam name="TState">The type of states this machine accepts.</typeparam>
	public class StackStateMachine<TState> where TState : class {
		private bool DEBUG_MODE = false;

		/// <summary>
		/// Occurs when a state is entered.
		/// First param: The state we're entering.
		/// Second param: The previous state before this.
		/// </summary>
		public event Action<TState, TState> OnEnter = delegate { };

		/// <summary>
		/// Occurs when a state is exited.
		/// First param: The state we're exiting.
		/// Second param: The next state after this.
		/// </summary>
		public event Action<TState, TState> OnExit = delegate { };

		/// <summary>
		/// Occurs when a state is resumed.
		/// First param: The state we're resuming.
		/// Second param: The previous state before this.
		/// </summary>
		public event Action<TState, TState> OnResume = delegate { };

		/// <summary>
		/// Occurs when a state is overriden.
		/// First param: The state we're overriding.
		/// Second param: The next state after this.
		/// </summary>
		public event Action<TState, TState> OnOverride = delegate { };

		private Stack<TState> _stateStack = new Stack<TState>();


		/// <summary>
		/// The state on the top of the stack.
		/// </summary>
		/// <value>The state of the current.</value>
		public TState currentState {
			get { return _stateStack.Count > 0 ? _stateStack.Peek() : null; }
		}

		/// <summary>
		/// The states the stack contains.
		/// </summary>
		public TState[] states {
			get { return _stateStack.ToArray(); }
		}

		/// <summary>
		/// How many states do we have in the stack?
		/// </summary>
		public int stateStackCount {
			get { return _stateStack.Count; }
		}

		/// <summary>
		/// Pops all state and replaces the last.
		/// </summary>
		/// <param name="state"></param>
		public void SetState(TState state) {
			if(IsStateNull(state)) return;
			if(IsStateDuplicate(state)) return;

			TState nextState = state;
			TState prevState = currentState;

			PopAllStates();

			// Exit the last state
			if(_stateStack.Count > 0) {
				if(DEBUG_MODE) Debug.Log(string.Format("Exitting: {0}", prevState.GetType().Name));
				OnExit(prevState, nextState);

				_stateStack.Pop();
			}

			// Replace the last state with the new
			_stateStack.Push(nextState);
			if(DEBUG_MODE) Debug.Log(string.Format("Entering: {0}", nextState.GetType().Name));
			OnEnter(nextState, prevState);
		}

		/// <summary>
		/// Push a state on top of the current.
		/// </summary>
		/// <param name="state"></param>
		public void PushState(TState state) {
			if(IsStateNull(state)) return;
			if(IsStateDuplicate(state)) return;

			TState nextState = state;
			TState prevState = currentState;

			// Override the top state
			if(_stateStack.Count > 0) {
				if(DEBUG_MODE) Debug.Log(string.Format("Overriding: {0}", prevState.GetType().Name));
				OnOverride(prevState, nextState);
			}

			// Push the new state
			_stateStack.Push(nextState);
			if(DEBUG_MODE) Debug.Log(string.Format("Entering: {0}", nextState.GetType().Name));
			OnEnter(nextState, prevState);
		}

		/// <summary>
		/// Pops the top state. If the stack only contains one state then it just returns that.
		/// </summary>
		/// <returns></returns>
		public TState PopState() {
			TState nextState = null;
			TState prevState = null;

			if(_stateStack.Count > 1) {
				// Pop the top state
				prevState = _stateStack.Pop();
				nextState = currentState;
				if(DEBUG_MODE) Debug.Log(string.Format("Exitting: {0}", prevState.GetType().Name));
				OnExit(prevState, nextState);

				// Resume the overriden state
				if(DEBUG_MODE) Debug.Log(string.Format("Resuming: {0}", nextState.GetType().Name));
				OnResume(nextState, prevState);
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

		private bool IsStateNull(TState state) {
			return state == null;
		}

		private bool IsStateDuplicate(TState state) {
			if(IsStateStackEmpty()) return false;
			return currentState.Equals(state);
		}

		private bool IsStateStackEmpty() {
			return _stateStack.Count <= 0;
		}
	}


	public static class StackStateMachineExtensions {
		/// <summary>
		/// Sets state after yield instruction. 
		/// Useful for delayed actions.
		/// </summary>
		/// <typeparam name="TState"></typeparam>
		/// <param name="stackStateMachine"></param>
		/// <param name="runner"></param>
		/// <param name="yield"></param>
		/// <param name="state"></param>
		public static void SetState<TState>(this StackStateMachine<TState> stackStateMachine,
												 MonoBehaviour runner,
												 YieldInstruction yield,
												 TState state) where TState : class {
			Utilities.RunOnYield(runner, yield, () => stackStateMachine.SetState(state));
		}

		/// <summary>
		/// Pushes state after yield instruction. 
		/// Useful for delayed actions.
		/// </summary>
		/// <typeparam name="TState"></typeparam>
		/// <param name="stackStateMachine"></param>
		/// <param name="runner"></param>
		/// <param name="yield"></param>
		/// <param name="state"></param>
		public static void PushState<TState>(this StackStateMachine<TState> stackStateMachine,
												  MonoBehaviour runner,
												  YieldInstruction yield,
												  TState state) where TState : class {
			Utilities.RunOnYield(runner, yield, () => stackStateMachine.PushState(state));
		}

		/// <summary>
		/// Pops state after yield instruction. 
		/// Useful for delayed actions.
		/// </summary>
		/// <typeparam name="TState"></typeparam>
		/// <param name="stackStateMachine"></param>
		/// <param name="runner"></param>
		/// <param name="yield"></param>
		/// <param name="state"></param>
		public static void PopState<TState>(this StackStateMachine<TState> stackStateMachine,
												 MonoBehaviour runner,
												 YieldInstruction yield) where TState : class {
			Utilities.RunOnYield(runner, yield, () => stackStateMachine.PopState());
		}

		/// <summary>
		/// Pops all state after yield instruction. 
		/// Useful for delayed actions.
		/// </summary>
		/// <typeparam name="TState"></typeparam>
		/// <param name="stackStateMachine"></param>
		/// <param name="runner"></param>
		/// <param name="yield"></param>
		/// <param name="state"></param>
		public static void PopAllStates<TState>(this StackStateMachine<TState> stackStateMachine,
													 MonoBehaviour runner,
													 YieldInstruction yield) where TState : class {
			Utilities.RunOnYield(runner, yield, () => stackStateMachine.PopAllStates());
		}
	}
}