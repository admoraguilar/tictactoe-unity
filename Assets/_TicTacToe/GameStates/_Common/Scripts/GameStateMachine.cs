using UnityEngine;


namespace WishfulDroplet {
	[CreateAssetMenu(fileName = "GameStateMachine", menuName = "TicTacToe/GameStateMachine")]
	public class GameStateMachine : ScriptableObject {
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


		[Header("States")]
		[SerializeField] private GameState _splashScreenState;
		[SerializeField] private GameState _mainScreenState;
		[SerializeField] private GameState _ticTacToeScreenState3x3;
		[SerializeField] private GameState _ticTacToeScreenState4x4;

		[Header("Settings")]
		[SerializeField] private GameState _rootState;
		[SerializeField] private bool _isLoadRootAtStart;

		private StackStateMachine<GameState> _gameStateMachine = new StackStateMachine<GameState>();
		private MonoBehaviour _monoRunner;


		public GameState splashScreenState {
			get { return _splashScreenState; }
		}

		public GameState mainScreenState {
			get { return _mainScreenState; }
		}

		public GameState ticTacToeScreenState3x3 {
			get { return _ticTacToeScreenState3x3; }
		}

		public GameState ticTacToeScreenState4x4 {
			get { return _ticTacToeScreenState4x4; }
		}

		public void SplashScreen() {
			SetState(splashScreenState);
		}

		public void MainScreen() {
			SetState(mainScreenState);
		}

		public void TicTacToe3x3() {
			SetState(ticTacToeScreenState3x3);
		}

		public void TicTacToe4x4() {
			SetState(ticTacToeScreenState4x4);
		}

		protected void SetState(GameState state, YieldInstruction yield = null) {
			if(yield != null) _gameStateMachine.SetState(_monoRunner, yield, state);
			else _gameStateMachine.SetState(state);

			_gameStateMachine.SetState(state);
		}

		protected void PushState(GameState state, YieldInstruction yield = null) {
			if(yield != null) _gameStateMachine.PushState(_monoRunner, yield, state);
			else _gameStateMachine.PushState(state);
		}

		protected void PopState(YieldInstruction yield = null) {
			if(yield != null) _gameStateMachine.PopState(_monoRunner, yield);
			else _gameStateMachine.PopState();
		}

		protected void PopAllStates(YieldInstruction yield = null) {
			if(yield != null) _gameStateMachine.PopAllStates(_monoRunner, yield);
			else _gameStateMachine.PopAllStates();
		}

		private void OnEnterState(GameState toEnterState, GameState prevState) {
			toEnterState.OnEnter(prevState);
		}

		private void OnExitState(GameState toExitState, GameState nextState) {
			toExitState.OnExit(nextState);
		}

		private void OnResumeState(GameState toResumeState, GameState prevState) {
			toResumeState.OnResume(prevState);
		}

		private void OnOverrideState(GameState toOverrideState, GameState nextState) {
			toOverrideState.OnOverride(nextState);
		}

		private void OnEnable() {
			_gameStateMachine.OnEnter += OnEnterState;
			_gameStateMachine.OnExit += OnExitState;
			_gameStateMachine.OnResume += OnResumeState;
			_gameStateMachine.OnOverride += OnOverrideState;

			if(Application.isPlaying) {
				_monoRunner = new GameObject().AddComponent<MonoRunnerDummy>();
			}
		}

		private void OnDisable() {
			_gameStateMachine.OnEnter -= OnEnterState;
			_gameStateMachine.OnExit -= OnExitState;
			_gameStateMachine.OnResume -= OnResumeState;
			_gameStateMachine.OnOverride -= OnOverrideState;
		}
	}


	public abstract class GameState : ScriptableObject {
		public virtual void OnEnter(GameState prevState) { }
		public virtual void OnExit(GameState nextState) { }
		public virtual void OnResume(GameState prevState) { }
		public virtual void OnOverride(GameState nextState) { }
	}


	public class MonoRunnerDummy : MonoBehaviour { }
}