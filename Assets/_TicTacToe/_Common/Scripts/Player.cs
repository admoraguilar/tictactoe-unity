using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "TicTacToe/Data/Player")]
public class Player : ScriptableObject {
	[SerializeField] private string _name;


	public new string name {
		get { return _name; }
	}
}
