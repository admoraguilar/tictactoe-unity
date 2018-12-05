using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace FoxCubTicTacToe {
	/// <summary>
	/// Handles the UI for a TicTacToe game.
	/// </summary>
	public class TicTacToeUI : MonoBehaviour {
		[Header("Board")]
		[SerializeField] private RectTransform _boardTilesContainer;
		[SerializeField] private TicTacToeBoardTile _boardTilePrefab;
		[SerializeField] private Sprite[] _markerFillers;

		[Header("Overlay")]
		[SerializeField] private RectTransform _overlay;
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private Button _playAgainButton;
		[SerializeField] private Button _backToMenuButton;

		[Header("References")]
		[SerializeField] private TicTacToeGame _ticTacToeGame;

		private List<TicTacToeBoardTile> _boardTiles = new List<TicTacToeBoardTile>();


		#region EVENTS

		private void OnPlayButtonClicked() {
			_ticTacToeGame.PlayGame();
		}

		private void OnBackToMenuButtonClicked() {
			GameStateMachine.main.MainScreen();
		}

		private void OnResetBoard() {
			foreach(TicTacToeBoardTile tile in _boardTiles) {
				tile.image.sprite = null;
				tile.text.SetText(string.Empty);
			}
		}

		private void OnWinStateEnter() {
			_text.SetText(string.Format("{0} wins!", _ticTacToeGame.winnerPlayer.name));
			_overlay.gameObject.SetActive(true);
		}

		private void OnWinStateExit() {
			_overlay.gameObject.SetActive(false);
		}

		private void OnDrawStateEnter() {
			_text.SetText("Draw!");
			_overlay.gameObject.SetActive(true);
		}

		private void OnDrawStateExit() {
			_overlay.gameObject.SetActive(false);
		}

		#endregion

		private void OnEnable() {
			_playAgainButton.onClick.AddListener(OnPlayButtonClicked);
			_backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);

			_ticTacToeGame.OnResetBoard += OnResetBoard;

			_ticTacToeGame.winState.onEnter.AddListener(OnWinStateEnter);
			_ticTacToeGame.winState.onExit.AddListener(OnWinStateExit);
			_ticTacToeGame.drawState.onEnter.AddListener(OnDrawStateEnter);
			_ticTacToeGame.drawState.onExit.AddListener(OnDrawStateExit);

			_overlay.gameObject.SetActive(_ticTacToeGame.currentState == _ticTacToeGame.winState ||
										  _ticTacToeGame.currentState == _ticTacToeGame.drawState);
		}

		private void OnDisable() {
			_playAgainButton.onClick.RemoveListener(OnPlayButtonClicked);
			_backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);

			_ticTacToeGame.OnResetBoard -= OnResetBoard;

			_ticTacToeGame.winState.onEnter.RemoveListener(OnWinStateEnter);
			_ticTacToeGame.winState.onExit.RemoveListener(OnWinStateExit);
			_ticTacToeGame.drawState.onEnter.RemoveListener(OnDrawStateEnter);
			_ticTacToeGame.drawState.onExit.RemoveListener(OnDrawStateExit);
		}

		private void Start() {
			for(int i = 0; i < _ticTacToeGame.tileSize; i++) {
				TicTacToeBoardTile boardTile = Instantiate(_boardTilePrefab);
				_boardTiles.Add(boardTile);

				int catchTileIndex = i;
				boardTile.button.onClick.AddListener(() => {
					if(_ticTacToeGame.ProgressPlay(catchTileIndex)) {
						//boardTile.text.SetText("{0}", _ticTacToeGame.prevPlayerIndex);
						boardTile.image.sprite = _markerFillers[_ticTacToeGame.prevPlayerIndex];
					}
				});

				RectTransform buttonRT = boardTile.GetComponent<RectTransform>();
				buttonRT.localPosition = Vector3.zero;
				buttonRT.SetParent(_boardTilesContainer, false);
			}
		}
	}
}
