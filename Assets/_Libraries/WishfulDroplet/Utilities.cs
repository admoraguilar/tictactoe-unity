using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WishfulDroplet {
	public static class Utilities {
		/// <summary>
		/// Run an action after yield instruction.
		/// </summary>
		/// <param name="runner"></param>
		/// <param name="yield"></param>
		/// <param name="action"></param>
		public static void RunOnYield(MonoBehaviour runner, YieldInstruction yield, Action action) {
			runner.StartCoroutine(RunOnYield(yield, action));
		}

		/// <summary>
		/// Run an action after yield instruction.
		/// </summary>
		/// <param name="yield"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static IEnumerator RunOnYield(YieldInstruction yield, Action action) {
			yield return yield;
			action();
		}

#if UNITY_EDITOR

		public static void SetExecutionOrder(Type type, int order) {
			string scriptName = type.Name;

			foreach(MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts()) {
				if(monoScript.name == scriptName) {
					if(MonoImporter.GetExecutionOrder(monoScript) != order) {
						MonoImporter.SetExecutionOrder(monoScript, order);
					}
					break;
				}
			}
		}

#endif
	}
}