using UnityEngine;


namespace WishfulDroplet {
	public class ScriptableStackStateMachine : ScriptableObject, IStackStateMachine {
		private StackStateMachine _stateMachine = new StackStateMachine();


		public IState currentState {
			get { return _stateMachine.currentState; }
		}

		public IState[] states {
			get { return _stateMachine.states; }
		}

		public int stateStackCount {
			get { return _stateMachine.stateStackCount; }
		}

		public void SetState(IState state) {
			_stateMachine.SetState(state);
		}

		public void PushState(IState state) {
			_stateMachine.PushState(state);
		}

		public IState PopState() {
			return _stateMachine.PopState();
		}

		public void PopAllStates() {
			_stateMachine.PopAllStates();
		}
	}
}
