using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WinCombinationData", menuName = "TicTacToe/Data/WinCombinationData")]
public class TicTacToeWinCombinationData : ScriptableObject {
	[SerializeField] private bool[] _combination;
	[SerializeField] private int _boardSize;

	private HashSet<int> _combinationSet;


	public HashSet<int> GetCombinationSet() {
		if(_combinationSet == null) {
			_combinationSet = new HashSet<int>();
			for(int i = 0; i < _combination.Length; i++) {
				if(_combination[i]) {
					_combinationSet.Add(i);
				}
			}
		}

		return _combinationSet;
	}
}
