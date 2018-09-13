using UnityEngine;

namespace DarkRoom.Utility
{
	public sealed class CPlatformKit{
		public static string GetPlatformName(RuntimePlatform platform){
			string name = "Windows";
			switch (platform) {
			case RuntimePlatform.Android:
				name = "Android";
				break;

			case RuntimePlatform.IPhonePlayer:
				name = "iOS";
				break;

			case RuntimePlatform.WindowsPlayer:
				name = "Windows";
				break;

			case RuntimePlatform.OSXPlayer:
				name = "OSX";
				break;
			}

			return name;
		}

		//获取当前平台bundle的根目录
		public static string GetCurrentPlatformBundleRoot(){
			string platform = GetPlatformName(Application.platform);
			string root = string.Format("{0}/Bundles/{1}/", Application.streamingAssetsPath, platform);
			return root;
		}
	}

}