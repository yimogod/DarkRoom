using UnityEngine;

namespace DarkRoom.Utility
{
	public class CCameraSimpleMoveComp : MonoBehaviour {
        public float Speed = 0.1f;
        public Transform Tran;

        private void Start()
        {
            if (Tran == null) Tran = transform;
        }

        void Update() {
			Vector3 pos = Tran.position;

			if (Input.GetKey(KeyCode.A)) {
				pos.x -= Speed;
			}

			if (Input.GetKey(KeyCode.D)) {
				pos.x += Speed;
			}

			if (Input.GetKey(KeyCode.W)) {
				pos.z += Speed;
			}

			if (Input.GetKey(KeyCode.S)) {
				pos.z -= Speed;
			}

			if (Input.GetKey(KeyCode.Z)) {
				pos.y += Speed;
			}

			if (Input.GetKey(KeyCode.X)) {
				pos.y -= Speed;
			}

			Tran.position = pos;
		}
	}

}