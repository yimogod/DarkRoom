using UnityEngine;
namespace DarkRoom.Game
{
	public class CShowFPS : MonoBehaviour {
		public static GUIStyle guiStyle = null;

		public float UpdateInterval = 1.0f;
		public int fontSize = 30;

		private float lastInterval;
		private float fps;
		private int frames;
		private Rect showRect;

		private void Start() {
			InitStyle();

			showRect = new Rect(0, Screen.height - fontSize, 200, fontSize);
			lastInterval = Time.realtimeSinceStartup;
			frames = 0;
		}

		private void InitStyle() {
			if (guiStyle == null) {
				guiStyle = new GUIStyle();
				guiStyle.fontStyle = FontStyle.Normal;
				guiStyle.fontSize = fontSize;
				guiStyle.normal.textColor = Color.white;
			}
		}

		private void Update() {
			++frames;
			float timeNow = Time.realtimeSinceStartup;
			if (timeNow > lastInterval + UpdateInterval) {
				fps = frames / (timeNow - lastInterval);
				frames = 0;
				lastInterval = timeNow;

				showRect = new Rect(0, Screen.height - fontSize, 200, fontSize);
			}
		}

		private void OnGUI() {
			if (guiStyle == null) {
				InitStyle();
			}
			GUI.Label(showRect, string.Format("{0:f2}fps", fps), guiStyle);
		}
	}
}