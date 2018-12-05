using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace FoxCubTicTacToe {
	/// <summary>
	/// Represents a tile in a TicTacToe board.
	/// </summary>
	public class TicTacToeBoardTile : MonoBehaviour {
		public Button button;
		public TextMeshProUGUI text;
		public Image image;
	}
}