using UnityEngine;
using UnityEngine.SceneManagement;
using WishfulDroplet;


namespace FoxCubTicTacToe {
	[CreateAssetMenu(fileName = "TicTacToeScreen", menuName = "TicTacToe/GameState/TicTacToeScreen")]
	public class TicTacToeScreen : GameState {
		[SerializeField] private SceneReference _ticTacToeScene;


		public override void OnEnter(GameState prevState) {
			SceneManager.LoadSceneAsync(_ticTacToeScene.SceneName, LoadSceneMode.Single);
		}

		public override void OnExit(GameState nextState) {
			//SceneManager.UnloadSceneAsync(_ticTacToeScene.SceneName);
		}
	}
}