using System;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
	[CustomPropertyDrawer(typeof(EnumMaskFieldAttribute))]
	public class EnumMaskFieldDrawer : PropertyDrawer 
	{
		#region Drawer Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label	= EditorGUI.BeginProperty(position, label, property);
			if (IsEnum())
			{
				UnityEditorUtility.EnumFlagsField(position, label, property, GetEnumType());
			}
			else
			{
				base.OnGUI(position, property, label);
			}

			EditorGUI.EndProperty();
		}
		
		#endregion

		#region Private methods

		private Type GetEnumType()
		{
			return ((EnumMaskFieldAttribute)attribute).EnumType; 
		}

		private bool IsEnum()
		{
			Type type = GetEnumType();
			#if NETFX_CORE
			return type.GetTypeInfo().IsEnum;
			#else 
			return type.IsEnum;
			#endif
		}

		#endregion
	}
}