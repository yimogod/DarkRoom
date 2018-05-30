using UnityEngine;

namespace DarkRoom.Utility
{
	public class CCameraSimpleMoveComp : MonoBehaviour {
		public float speed = 0.1f;
		public Transform tran;

		void Update() {
			Vector3 pos = tran.position;

			if (Input.GetKey(KeyCode.A)) {
				pos.x -= speed;
			}

			if (Input.GetKey(KeyCode.D)) {
				pos.x += speed;
			}

			if (Input.GetKey(KeyCode.W)) {
				pos.z += speed;
			}

			if (Input.GetKey(KeyCode.S)) {
				pos.z -= speed;
			}

			if (Input.GetKey(KeyCode.Z)) {
				pos.y += speed;
			}

			if (Input.GetKey(KeyCode.X)) {
				pos.y -= speed;
			}

			tran.position = pos;
		}
	}

}