using UnityEngine;
using System.Runtime.InteropServices;

public class Link : MonoBehaviour {
	public void OpenLinkJSPlugin() {
		#if !UNITY_EDITOR
		openWindow("https://sdgs4all.rs/");
		#endif
	}

	[DllImport("__Internal")]
	private static extern void openWindow(string url);
}