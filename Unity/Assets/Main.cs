using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

public class Main : MonoBehaviour
{
		[DllImport("CppPlugin")]
		private static extern int Extern1(int n);

		void Awake()
		{
			Debug.Log(Extern1(42));
		}
}
