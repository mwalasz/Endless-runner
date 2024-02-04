using System.IO;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
	[System.Serializable]
	public class PBXFile
	{
		#region Fields

		[SerializeField]
		private     Object			m_reference;

		[SerializeField]
		private	    string			m_compileFlags;

		#endregion

		#region Properties

		public string RelativePath => AssetDatabase.GetAssetPath(m_reference);

        public string AbsoultePath => Path.GetFullPath(RelativePath);

		public string[] CompileFlags
		{
			get
			{
				return m_compileFlags.Split(',');
			}
			set
			{
				m_compileFlags = string.Join(",", value);
			}
		}

		#endregion
	}
}