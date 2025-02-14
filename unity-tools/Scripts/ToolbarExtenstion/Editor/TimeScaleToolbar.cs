using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender {
	[InitializeOnLoad]
	public class TimeScaleToolbar {
		#region Styles

		private static GUIStyle _buttonStyle;
		private static GUIStyle _labelStyle;
		private static GUIStyle _sliderStyle;

		private static void LazyCreateStyles() {
			_buttonStyle ??= new GUIStyle("Command") {
				fontSize = 12,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fixedWidth = 45f,
				fontStyle = FontStyle.Bold
			};

			_labelStyle ??= new GUIStyle(GUI.skin.label) {
				fontSize = 11,
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				stretchHeight = true,
				margin = new RectOffset(0, 0, 4, 0),
			};

			_sliderStyle ??= new GUIStyle(GUI.skin.horizontalSlider) {
				margin = new RectOffset(0, 0, 6, 0),
			};
		}

		#endregion

		private static float _timeScale = 1f;

		static TimeScaleToolbar() {
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
		}

		private static void OnToolbarGUI() {
			LazyCreateStyles();

			GUILayout.Space(5f);

			GUILayout.Label("Time Scale ", _labelStyle);

			_timeScale = GUILayout.HorizontalSlider(
				_timeScale, 0f, 2f,
				_sliderStyle,
				GUI.skin.horizontalSliderThumb,
				GUILayout.Width(100f), GUILayout.ExpandHeight(true));

			GUILayout.Label($"{_timeScale:F2}", _labelStyle, GUILayout.Width(40f));

			if (Application.isPlaying) {
				Time.timeScale = _timeScale;
			}

			if (GUILayout.Button(new GUIContent("Reset", "Reset TimeScale"), _buttonStyle)) {
				_timeScale = 1f;
				Time.timeScale = _timeScale;
			}

			GUILayout.FlexibleSpace();
		}
	}
}