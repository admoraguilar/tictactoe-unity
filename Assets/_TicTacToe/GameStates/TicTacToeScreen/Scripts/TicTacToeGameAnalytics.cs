using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FoxCubTicTacToe {
	/// <summary>
	/// Tracks parts of TicTacToe game for analytical purposes.
	/// </summary>
	public class TicTacToeGameAnalytics : MonoBehaviour {
		[SerializeField] private TicTacToeGame _ticTacToeGame;

		[Header("Debug")]
		[SerializeField] private List<TicTacToeAnalyticsPlayDetails> _playHistory = new List<TicTacToeAnalyticsPlayDetails>();
		[SerializeField] private TicTacToeAnalyticsPlayDetails _currentPlay;

		#region EVENTS

		private void OnTileFilled(int tileIndex, int markerType) {
			_currentPlay.moveDetails.Add(new TicTacToeAnalyticsMoveDetail {
				markerType = markerType,
				tileIndex = tileIndex
			});
		}

		private void OnPlayStateEnter() {
			_currentPlay = new TicTacToeAnalyticsPlayDetails {
				gameType = _ticTacToeGame.gameName,
				moveDetails = new List<TicTacToeAnalyticsMoveDetail>()
			};

			_playHistory.Add(_currentPlay);
		}

		private void OnWinStateEnter() {
			_currentPlay.gameResult = string.Format("WIN: {0}", _ticTacToeGame.winnerPlayer.name);
		}

		private void OnDrawStateEnter() {
			_currentPlay.gameResult = "DRAW";
		}

		#endregion

		private void OnEnable() {
			_ticTacToeGame.OnTileFilled += OnTileFilled;

			_ticTacToeGame.playState.onEnter.AddListener(OnPlayStateEnter);
			_ticTacToeGame.winState.onEnter.AddListener(OnWinStateEnter);
			_ticTacToeGame.drawState.onEnter.AddListener(OnDrawStateEnter);
		}

		private void OnDisable() {
			_ticTacToeGame.OnTileFilled -= OnTileFilled;

			_ticTacToeGame.playState.onExit.RemoveListener(OnPlayStateEnter);
			_ticTacToeGame.winState.onEnter.RemoveListener(OnWinStateEnter);
			_ticTacToeGame.drawState.onEnter.RemoveListener(OnDrawStateEnter);
		}
	}


	[Serializable]
	public class TicTacToeAnalyticsPlayDetails {
		public string gameType;
		public string gameResult;
		public List<TicTacToeAnalyticsMoveDetail> moveDetails = new List<TicTacToeAnalyticsMoveDetail>();
	}


	[Serializable]
	public class TicTacToeAnalyticsMoveDetail {
		public int markerType;
		public int tileIndex;
	}
}