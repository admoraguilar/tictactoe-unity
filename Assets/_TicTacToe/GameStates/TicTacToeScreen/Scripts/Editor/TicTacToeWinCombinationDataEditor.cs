using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TicTacToeWinCombinationData), true)]
public class TicTacToeWinCombinationDataEditor : Editor {
	private SerializedProperty _combinationArrayProp;
	private SerializedProperty _boardSizeProp;


	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();

		serializedObject.Update();

		EditorGUILayout.BeginHorizontal(); {
			_boardSizeProp.intValue = EditorGUILayout.DelayedIntField(_boardSizeProp.displayName, _boardSizeProp.intValue);
			_combinationArrayProp.arraySize = _boardSizeProp.intValue * _boardSizeProp.intValue;
		} EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical(); {
			for(int i = 0; i < _boardSizeProp.intValue; i++) {
				EditorGUILayout.BeginHorizontal(); {
					for(int a = 0; a < _boardSizeProp.intValue; a++) {
						int index = a + (i * _boardSizeProp.intValue);
						SerializedProperty combinationProp = _combinationArrayProp.GetArrayElementAtIndex(index);
						combinationProp.boolValue =	EditorGUILayout.Toggle(combinationProp.boolValue);
					}
				} EditorGUILayout.EndHorizontal();
			}
		} EditorGUILayout.EndVertical();


		serializedObject.ApplyModifiedProperties();
	}

	private void OnEnable() {
		_combinationArrayProp = serializedObject.FindProperty("_combination");
		_boardSizeProp = serializedObject.FindProperty("_boardSize");
	}
}
