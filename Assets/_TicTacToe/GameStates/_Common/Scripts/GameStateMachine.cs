using UnityEngine;
using WishfulDroplet;


namespace FoxCubTicTacToe {
	/// <summary>
	/// The entry point of the game.
	/// Also handles the overall cycle of the game.
	/// </summary>
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
				main.LoadRootState();
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

		public void LoadRootState() {
			_gameStateMachine.SetState(GetMonoRunner(), new WaitForEndOfFrame(), _rootState);
		}

		public void SplashScreen() {
			_gameStateMachine.SetState(GetMonoRunner(), new WaitForEndOfFrame(), _splashScreenState);
		}

		public void MainScreen() {
			_gameStateMachine.SetState(GetMonoRunner(), new WaitForEndOfFrame(), _mainScreenState);
		}

		public void TicTacToe3x3() {
			_gameStateMachine.SetState(GetMonoRunner(), new WaitForEndOfFrame(), _ticTacToeScreenState3x3);
		}

		public void TicTacToe4x4() {
			_gameStateMachine.SetState(GetMonoRunner(), new WaitForEndOfFrame(), _ticTacToeScreenState4x4);
		}

		private MonoBehaviour GetMonoRunner() {
			if(!_monoRunner) {
				_monoRunner = new GameObject().AddComponent<MonoRunnerDummy>();
			}

			return _monoRunner;
		}

		private void OnEnterState(GameState toEnterState, GameState prevState) {
			toEnterState.OnEnter(prevState);
		}

		private void OnExitState(GameState toExitState, GameState nextState) {
			toExitState.OnExit(nextState);
		}

		private void OnEnable() {
			_gameStateMachine.OnEnter += OnEnterState;
			_gameStateMachine.OnExit += OnExitState;
		}

		private void OnDisable() {
			_gameStateMachine.OnEnter -= OnEnterState;
			_gameStateMachine.OnExit -= OnExitState;
		}
	}


	public abstract class GameState : ScriptableObject {
		public virtual void OnEnter(GameState prevState) { }
		public virtual void OnExit(GameState nextState) { }
	}


	public class MonoRunnerDummy : MonoBehaviour { }
}