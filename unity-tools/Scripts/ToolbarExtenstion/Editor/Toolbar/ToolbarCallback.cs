using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

// UNITY_2019_1_OR_NEWER

namespace UnityToolbarExtender {
	public static class ToolbarCallback {
		private const BindingFlags kRootBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
		private const string kToolbarLeftZone = "ToolbarZoneLeftAlign";
		private const string kToolbarRightZone = "ToolbarZoneRightAlign";

		private static readonly Type ToolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
		private static ScriptableObject _toolbar;

		public static Action onToolbarGUILeft;
		public static Action onToolbarGUIRight;

		static ToolbarCallback() {
			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate() {
			if (_toolbar is not null)
				return;

			var toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);
			_toolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;

			var root = _toolbar?.GetType().GetField("m_Root", kRootBindingFlags);

			var rawRoot = root?.GetValue(_toolbar);
			if (rawRoot is not VisualElement visualRoot)
				return;

			RegisterCallback(visualRoot, kToolbarLeftZone, onToolbarGUILeft);
			RegisterCallback(visualRoot, kToolbarRightZone, onToolbarGUIRight);
		}

		private static void RegisterCallback(VisualElement root, string name, Action cb) {
			var toolbarZone = root.Q(name);
			var parent = new VisualElement() {
				style = {
					flexGrow = 1,
					flexDirection = FlexDirection.Row,
				}
			};
			var container = new IMGUIContainer();
			container.style.flexGrow = 1;
			container.onGUIHandler += () => { cb?.Invoke(); };
			parent.Add(container);
			toolbarZone.Add(parent);
		}
	}
}