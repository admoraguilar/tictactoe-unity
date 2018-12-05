using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using WishfulDroplet;


namespace FoxCubTicTacToe {
	public class TicTacToeGame : MonoBehaviour {
		public event Action OnResetBoard = delegate { };
		public event Action<int, int> OnTileFilled = delegate { };

		[Header("TicTacToe")]
		[SerializeField] private string _gameName;
		[SerializeField] private TicTacToe _tictacToe;
		[SerializeField] private int _boardSize;

		[Header("Data")]
		[SerializeField] private Player[] _players;

		[Header("Debug")]
		[SerializeField] private Player _winnerPlayer;
		[SerializeField] private int _prevPlayerIndex;
		[SerializeField] private int _currentPlayerIndex;

		private StackStateMachine<TicTacToeGameState> _ticTacToeStateMachine = new StackStateMachine<TicTacToeGameState>();

		private PlayState _playState = new PlayState();
		private WinState _winState = new WinState();
		private DrawState _drawState = new DrawState();


		public string gameName {
			get { return _gameName; }
		}

		public int tileSize {
			get { return _tictacToe.tilesSize; }
		}

		public int columnSize {
			get { return _tictacToe.columnSize; }
		}

		public int rowSize {
			get { return _tictacToe.rowSize; }
		}

		public Player currentPlayer {
			get { return _players[_currentPlayerIndex]; }
		}

		public int currentPlayerIndex {
			get { return _currentPlayerIndex; }
		}

		public Player prevPlayer {
			get { return _players[_prevPlayerIndex]; }
		}

		public int prevPlayerIndex {
			get { return _prevPlayerIndex; }
		}

		public Player winnerPlayer {
			get { return _winnerPlayer; }
		}

		public TicTacToeGameState currentState {
			get { return _ticTacToeStateMachine.currentState; }
		}

		public PlayState playState {
			get { return _playState; }
		}

		public WinState winState {
			get { return _winState; }
		}

		public DrawState drawState {
			get { return _drawState; }
		}

		public void PlayGame() {
			ResetBoard();
			_ticTacToeStateMachine.SetState(this, new WaitForEndOfFrame(), _playState);
		}

		public void EndGame() {
			_ticTacToeStateMachine.SetState(this, new WaitForEndOfFrame(), _winState);
		}

		public void DrawGame() {
			_ticTacToeStateMachine.SetState(this, new WaitForEndOfFrame(), _drawState);
		}

		public bool ProgressPlay(int tileIndex) {
			if(_winnerPlayer != null) return false;
			if(_tictacToe.CheckTileContent(tileIndex) != -1) {
				Debug.Log("Tile is already marked.");
				return false;
			}

			_tictacToe.SetTileContent(tileIndex, _currentPlayerIndex);
			OnTileFilled(tileIndex, _currentPlayerIndex);

			if(_tictacToe.CheckIfWon(_currentPlayerIndex)) {
				_winnerPlayer = _players[_currentPlayerIndex];
				_ticTacToeStateMachine.SetState(_winState);
				Debug.Log(string.Format("Player {0} won!", _currentPlayerIndex.ToString()));
			} else if(_tictacToe.IsAllTileFilled() && _winnerPlayer == null) {
				DrawGame();
				Debug.Log("DRAW");
			}

			EndCurrentPlayerTurn();
			return true;
		}

		public void ResetBoard() {
			_tictacToe.Reset();
			_winnerPlayer = null;
			_currentPlayerIndex = 0;
			OnResetBoard();
		}

		private void EndCurrentPlayerTurn() {
			_prevPlayerIndex = _currentPlayerIndex;

			if(_currentPlayerIndex < _players.Length - 1) _currentPlayerIndex++;
			else _currentPlayerIndex = 0;
		}

		#region EVENTS

		private void OnEnterState(TicTacToeGameState toEnterState, TicTacToeGameState prevState) {
			toEnterState.onEnter.Invoke();
		}

		private void OnExitState(TicTacToeGameState toExitState, TicTacToeGameState nextState) {
			toExitState.onExit.Invoke();
		}

		#endregion

		private void OnEnable() {
			_ticTacToeStateMachine.OnEnter += OnEnterState;
			_ticTacToeStateMachine.OnExit += OnExitState;
		}

		private void OnDisable() {
			_ticTacToeStateMachine.OnEnter -= OnEnterState;
			_ticTacToeStateMachine.OnExit -= OnExitState;
		}

		private void Awake() {
			_tictacToe.Init(_boardSize);
		}

		private void Start() {
			PlayGame();
		}

		#region STATES

		public class PlayState : TicTacToeGameState { }
		public class WinState : TicTacToeGameState { }
		public class DrawState : TicTacToeGameState { }


		public abstract class TicTacToeGameState {
			private UnityEvent _onEnter = new UnityEvent();
			private UnityEvent _onExit = new UnityEvent();


			public UnityEvent onEnter {
				get { return _onEnter; }
			}

			public UnityEvent onExit {
				get { return _onExit; }
			}
		}

		#endregion
	}


	[Serializable]
	public class TicTacToe {
		[SerializeField] private TicTacToeWinCombinationData[] _winCombinationData;

		private Dictionary<int, HashSet<int>> _markerCombinationData;
		private HashSet<int> _filledTiles;
		private int[] _tiles;
		private int _rowSize;
		private int _columnSize;


		public int tilesSize {
			get { return _tiles.Length; }
		}

		public int rowSize {
			get { return _rowSize; }
		}

		public int columnSize {
			get { return _columnSize; }
		}

		/// <summary>
		/// Initialize the size of the board. Row and column equates to size.
		/// </summary>
		/// <param name="size"></param>
		public void Init(int size) {
			_markerCombinationData = new Dictionary<int, HashSet<int>>();
			_filledTiles = new HashSet<int>();

			_rowSize = size;
			_columnSize = size;
			_tiles = new int[_rowSize * _columnSize];

			Reset();
		}

		/// <summary>
		/// Resets the values of the board.
		/// </summary>
		public void Reset() {
			_markerCombinationData.Clear();
			_filledTiles.Clear();

			for(int i = 0; i < _tiles.Length; i++) {
				_tiles[i] = -1;
			}
		}

		public bool IsAllTileFilled() {
			return _filledTiles.Count >= _tiles.Length;
		}

		/// <summary>
		/// Check tile content located in (x, y).
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int CheckTileContent(int x, int y) {
			return CheckTileContent(GetTileNumberByPosition(x, y));
		}

		/// <summary>
		/// Check tile content of index.
		/// </summary>
		/// <param name="tileNumber"></param>
		/// <returns></returns>
		public int CheckTileContent(int tileNumber) {
			return _tiles[tileNumber];
		}

		/// <summary>
		/// Check if the player with "marker" has a winning combination.
		/// </summary>
		/// <param name="marker"></param>
		/// <returns></returns>
		public bool CheckIfWon(int marker) {
			HashSet<int> markerCombinationData = GetMarkerCombinationData(marker);
			bool isWin = false;

			foreach(TicTacToeWinCombinationData data in _winCombinationData) {
				HashSet<int> winCombinationData = data.GetCombinationSet();
				if(winCombinationData.IsSubsetOf(markerCombinationData)) {
					isWin = true;
				}

				//Debug.Log(string.Format("WIN combination: {0} {1}MYY combination: {2}",
				//							 HashSetIntToString(winCombinationData),
				//							 Environment.NewLine,
				//							 HashSetIntToString(markerCombinationData)));
			}

			return isWin;
		}

		/// <summary>
		/// Sets the content of a tile located in (x, y).
		/// </summary>
		/// <param name="tileNumber"></param>
		/// <param name="marker"></param>
		public void SetTileContent(int x, int y, int marker) {
			SetTileContent(GetTileNumberByPosition(x, y), marker);
		}

		/// <summary>
		/// Sets the content of a tile by number.
		/// </summary>
		/// <param name="tileNumber"></param>
		/// <param name="marker"></param>
		public void SetTileContent(int tileNumber, int marker) {
			Assert.IsTrue(_tiles != null, "Board should be initialized first.");

			HashSet<int> markerCombinationData = GetMarkerCombinationData(marker);

			_tiles[tileNumber] = marker;
			markerCombinationData.Add(tileNumber);

			if(!_filledTiles.Contains(tileNumber)) {
				_filledTiles.Add(tileNumber);
			}
		}

		private int GetTileNumberByPosition(int x, int y) {
			Assert.IsTrue(x < _rowSize, "X shouldn't be larger than the board row size.");
			Assert.IsTrue(y < _columnSize, "Y shouldn't be larger than the board column size.");

			return x + (y * _columnSize);
		}

		private HashSet<int> GetMarkerCombinationData(int marker) {
			HashSet<int> data = null;

			if(!_markerCombinationData.TryGetValue(marker, out data)) {
				data = _markerCombinationData[marker] = new HashSet<int>();
			}

			return data;
		}
	}
}