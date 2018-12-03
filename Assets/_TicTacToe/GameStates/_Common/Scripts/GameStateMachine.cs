using System;
using UnityEngine;


namespace WishfulDroplet {
	[CreateAssetMenu(fileName = "GameStateMachine", menuName = "TicTacToe/GameStateMachine")]
	public class GameStateMachine : ScriptableStackStateMachine {
		public static GameStateMachine main {
			get;
			private set;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RunOnStartupBeforeSceneLoad() {
			main = Resources.LoadAll<GameStateMachine>("")[0];

			// Set the first element as the default state
			if(main._isLoadRootAtStart) {
				main.SetState(main._rootState);
			}
		}


		[SerializeField] private GameState _rootState;
		[SerializeField] private bool _isLoadRootAtStart;


		public void SetState(MonoState state) {
			base.SetState(state);
		}

		public void SetState(ScriptableState state) {
			base.SetState(state);
		}

		public void PushState(MonoState state) {
			base.PushState(state);
		}

		public void PushState(ScriptableState state) {
			base.PushState(state);
		}

		public new void PopState() {
			base.PopState();
		}
	}


	public abstract class GameState : ScriptableState { }
}