using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;


public class TicTacToeGame : MonoBehaviour {
	[Header("TicTacToe")]
	[SerializeField] private TicTacToe _ticTacToe;
	[SerializeField] private int _boardSize;

	[Header("Data")]
	[SerializeField] private Player[] _players;

	[Header("References")]
	[SerializeField] private Button _buttonPrefab;
	[SerializeField] private RectTransform _buttonContainer;

	[Header("Debug")]
	[SerializeField] private Player _winnerPlayer;
	[SerializeField] private int _currentPlayerIndex;


	private void EndCurrentPlayerTurn() {
		if(_currentPlayerIndex < _players.Length - 1) _currentPlayerIndex++;
		else _currentPlayerIndex = 0;
	}

	private void ProgressPlay(int tileIndex, Button button) {
		if(_winnerPlayer != null) return;

		int playerMarker = _currentPlayerIndex;

		_ticTacToe.SetTileContent(tileIndex, playerMarker);
		button.GetComponentInChildren<TextMeshProUGUI>().SetText("{0}", playerMarker);

		if(_ticTacToe.CheckIfWon(playerMarker)) {
			_winnerPlayer = _players[playerMarker];
			Debug.Log(string.Format("Player {0} won!", playerMarker.ToString()));
		}

		EndCurrentPlayerTurn();
	}

	private void Start() {
		_ticTacToe.Init(_boardSize);

		for(int i = 0; i < _ticTacToe.tilesSize; i++) {
			Button button = Instantiate(_buttonPrefab);

			int catchTileIndex = i;
			button.onClick.AddListener(() => {
				ProgressPlay(catchTileIndex, button);
			});

			RectTransform buttonRT = button.GetComponent<RectTransform>();
			buttonRT.localPosition = Vector3.zero;
			buttonRT.SetParent(_buttonContainer, false);
		}
	}
}


[Serializable]
public class TicTacToe {
	[SerializeField] private TicTacToeWinCombinationData[] _winCombinationData;

	private Dictionary<int, HashSet<int>> _markerCombinationData;
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

		_rowSize = size;
		_columnSize = size;
		_tiles = new int[_rowSize * _columnSize];
	}

	/// <summary>
	/// Resets the values of the board.
	/// </summary>
	public void Reset() {
		_markerCombinationData.Clear();

		for(int i = 0; i < _tiles.Length; i++) {
			_tiles[i] = 0;
		}
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

	private string HashSetIntToString(HashSet<int> hashSet) {
		string values = "";
		foreach(int value in hashSet) {
			values += value.ToString() + ",";
		}
		return values;
	}
}
