using System;
using UnityEngine;
namespace DarkRoom.Core
{
	public class CGlobalExistComp : MonoBehaviour {
		void Start() {
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
}