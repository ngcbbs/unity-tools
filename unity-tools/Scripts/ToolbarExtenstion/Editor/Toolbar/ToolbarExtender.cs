using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender {
	[InitializeOnLoad]
	public static class ToolbarExtender {
		public static readonly List<Action> LeftToolbarGUI = new();
		public static readonly List<Action> RightToolbarGUI = new();

		static ToolbarExtender() {
			ToolbarCallback.onToolbarGUILeft = GUILeft;
			ToolbarCallback.onToolbarGUIRight = GUIRight;
		}

		private static void GUILeft() {
			GUILayout.BeginHorizontal();
			foreach (var handler in LeftToolbarGUI)
				handler?.Invoke();
			GUILayout.EndHorizontal();
		}

		private static void GUIRight() {
			GUILayout.BeginHorizontal();
			foreach (var handler in RightToolbarGUI)
				handler?.Invoke();
			GUILayout.EndHorizontal();
		}
	}
}