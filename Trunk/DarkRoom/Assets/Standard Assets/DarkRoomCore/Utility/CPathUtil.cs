using System;

namespace DarkRoom.Core {
	public class CPathUtil
	{

		//没有后缀名
		public static string GetFileNameFromPath(string path){
			string[] stra = path.Split('/');
			string fileName = stra[stra.Length - 1].Split('.')[0];
			return fileName;
		}

		public static string GetFolderNameFromPath(string path){
			string[] stra = path.Split('/');
			string fileName = stra[stra.Length - 2];
			return fileName;
		}
	}
}

