using UnityEngine;
using UnityEditor;


namespace WishfulDroplet.Editor {
	/// <summary>
	/// Settings for WishfulDroplet's editor functionalities.
	/// </summary>
    public class EditorSettings : ScriptableObject {
		[MenuItem("Tools/WishfulDropet/Editor/Editor Settings")]
		private static void ShowEditorResources() {
			EditorSettings editorResources = EditorSingleton.Get<EditorSettings>();
			Selection.activeObject = editorResources;
		}


		[Header("Editor Behaviour")]
		public bool isObjUniqueNameOnDuplicate;

		[Header("Skins")]
		public GUISkin inspectorNoteSkin;
    }
}
