using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
	[RequireComponent(typeof(Button))]
	public class DemoBackButton : MonoBehaviour 
	{
		#region Unity methods

		private void Awake()
		{
			// set button
			GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("DemoMenu"));
		}

		#endregion
	}
}
