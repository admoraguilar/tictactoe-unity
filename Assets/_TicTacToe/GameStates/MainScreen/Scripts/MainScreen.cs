using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace WishfulDroplet {
	[CreateAssetMenu(fileName = "MainScreen", menuName = "TicTacToe/GameState/MainScreen")]
	public class MainScreen : GameState {
		[SerializeField] private SceneReference _mainScreenScene;


		public override void OnEnter(IState prevState) {
			SceneManager.LoadSceneAsync(_mainScreenScene.SceneName, LoadSceneMode.Single);
		}

		public override void OnExit(IState nextState) {
			//SceneManager.UnloadSceneAsync(_mainScreenScene.SceneName);
		}
	}
}