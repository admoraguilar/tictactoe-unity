using System;
using UnityEngine;


namespace WishfulDroplet {
	/// <summary>
	/// Use this attribute to display a better header in the inspector.
	/// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class InspectorNoteAttribute : PropertyAttribute {
        public readonly string header;
        public readonly string message;


        public InspectorNoteAttribute(string header, string message = "") {
            this.header = header;
            this.message = message;
        }
    }

	
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class InterfaceFieldAttribute : PropertyAttribute {
		public readonly Type type;


		public InterfaceFieldAttribute(Type type) {
			this.type = type;	
		}
	}
}