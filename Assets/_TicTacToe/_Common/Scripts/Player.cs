using UnityEngine;


namespace FoxCubTicTacToe {
	/// <summary>
	/// Used for identifying players across the game.
	/// </summary>
	[CreateAssetMenu(fileName = "Player", menuName = "TicTacToe/Data/Player")]
	public class Player : ScriptableObject {
		[SerializeField] private string _name;


		public new string name {
			get { return _name; }
		}
	}
}