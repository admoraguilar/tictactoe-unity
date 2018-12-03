using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace WishfulDroplet {
	namespace Editor {
		[CustomPropertyDrawer(typeof(InterfaceFieldAttribute), true)]
		public class InterfaceFieldPropertyDrawer : PropertyDrawer {
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
				return base.GetPropertyHeight(property, label);
			}

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				InterfaceFieldAttribute attr = (InterfaceFieldAttribute)attribute;
				Object obj = EditorGUI.ObjectField(position,
												   property.displayName,
												   property.objectReferenceValue,
												   attr.type,
												   true);
				if(obj) {
					property.objectReferenceValue = obj;
				}
			}
		}
	}
}