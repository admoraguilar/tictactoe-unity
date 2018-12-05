using UnityEngine;
using UnityEngine.SceneManagement;
using WishfulDroplet;


namespace FoxCubTicTacToe {
	[CreateAssetMenu(fileName = "SplashScreen", menuName = "TicTacToe/GameState/SplashScreen")]
	public class SplashScreen : GameState {
		[SerializeField] private SceneReference _splashScreenScene;


		public override void OnEnter(GameState prevState) {
			SceneManager.LoadSceneAsync(_splashScreenScene.SceneName, LoadSceneMode.Single);
		}

		public override void OnExit(GameState nextState) {
			//SceneManager.UnloadSceneAsync(_splashScreenScene.SceneName);
		}
	}
}