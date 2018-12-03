using UnityEngine;
using UnityEditor;


namespace WishfulDroplet.Editor {
	/// <summary>
	/// Custom behaviours for the Editor.
	/// </summary>
	public static class EditorBehaviour {
		[InitializeOnLoadMethod]
		private static void HookBehaviours() {
			EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
		}

		private static void OnHierarchyWindowChanged() {
			EditorSettings editorSettings = EditorSingleton.Get<EditorSettings>();

			if (editorSettings.isObjUniqueNameOnDuplicate) return;

			GameObject[] gos = Selection.gameObjects;
			if (gos != null) {
				for (int i = 0; i < gos.Length; ++i) {
					GameObject go = gos[i];
					if (go.scene.name != "Null") {
						int index = go.name.IndexOf('(');
						if (index > 0) {
							go.name = go.name.Remove(index);
						}
					}
				}
			}
		}
	}
}

