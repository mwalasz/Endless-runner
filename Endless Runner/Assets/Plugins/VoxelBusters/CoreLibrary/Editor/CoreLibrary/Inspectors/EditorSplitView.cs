using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
	public class EditorSplitView
	{
        #region Fields

        private		SplitDirection		m_direction;

		private		float				m_splitRatio;

		private		bool				m_resize;

		private		Vector2				m_firstContainerScrollPos;

		private		Rect				m_availableRect;

        #endregion

        #region Constructors

        private EditorSplitView(SplitDirection direction, float splitRatio)
		{
			m_splitRatio	= splitRatio;
			m_direction		= direction;
		}

		#endregion

		#region Static methods

		public static EditorSplitView CreateHorizontalSplitView(float splitRatio = 0.3f)
		{
			return new EditorSplitView(SplitDirection.Horizontal, splitRatio);
		}

		public static EditorSplitView CreateVerticalSplitView(float splitRatio = 0.3f)
		{
			return new EditorSplitView(SplitDirection.Vertical, splitRatio);
		}

        #endregion

        #region Private methods

        public void BeginArea()
		{
			Rect	tempRect		= Rect.zero;
			if (m_direction == SplitDirection.Horizontal)
			{
				tempRect			= EditorGUILayout.BeginHorizontal();
			}
			else if (m_direction == SplitDirection.Vertical)
			{
				tempRect			= EditorGUILayout.BeginVertical();
			}
			if (tempRect.width > 0.0f)
			{
				m_availableRect		= tempRect;
			}

			BeginContainer();
			if (m_direction == SplitDirection.Horizontal)
			{
				m_firstContainerScrollPos	= EditorGUILayout.BeginScrollView(m_firstContainerScrollPos, GUILayout.Width(m_availableRect.width * m_splitRatio));
			}
			else if (m_direction == SplitDirection.Vertical)
			{
				m_firstContainerScrollPos	= EditorGUILayout.BeginScrollView(m_firstContainerScrollPos, GUILayout.Height(m_availableRect.height * m_splitRatio));
			}
		}

		public void Split()
		{
			// firstly mark that first container has ended
			EditorGUILayout.EndScrollView();
			EndContainer();
			ResizeFirstContainer();

			// initiate rendering second container
			BeginContainer();
		}

		public void EndArea()
		{
			EndContainer();

			if (m_direction == SplitDirection.Horizontal)
			{
				EditorGUILayout.EndHorizontal();
			}
			else if (m_direction == SplitDirection.Vertical)
			{
				EditorGUILayout.EndVertical();
			}
		}

		private Rect BeginContainer()
		{
			if (m_direction == SplitDirection.Horizontal)
			{
				return EditorGUILayout.BeginVertical();
			}
			else if (m_direction == SplitDirection.Vertical)
			{
				return EditorGUILayout.BeginHorizontal();
			}
			else
			{
				return Rect.zero;
			}
		}

		private void EndContainer()
		{
			if (m_direction == SplitDirection.Horizontal)
			{
				EditorGUILayout.EndVertical();
			}
			else if (m_direction == SplitDirection.Vertical)
			{
				EditorGUILayout.EndHorizontal();
			}
		}

		private void ResizeFirstContainer()
		{
			Rect resizeHandleRect;
			if (m_direction == SplitDirection.Horizontal)
			{
				resizeHandleRect	= new Rect(m_availableRect.width * m_splitRatio, m_availableRect.y, 1f, m_availableRect.height);
			}
			else
			{
				resizeHandleRect	= new Rect(m_availableRect.x, m_availableRect.height * m_splitRatio, m_availableRect.width, 1f);
			}
			EditorGUI.DrawRect(resizeHandleRect, CustomEditorStyles.BorderColor);
			if (m_direction == SplitDirection.Horizontal)
			{
				EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);
			}
			else
			{
				EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeVertical);
			}

			if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
			{
				m_resize	= true;
			}
			if (m_resize)
			{
				if (m_direction == SplitDirection.Horizontal)
				{
					m_splitRatio	= Event.current.mousePosition.x / m_availableRect.width;
				}
				else
				{
					m_splitRatio	= Event.current.mousePosition.y / m_availableRect.height;
				}
			}
			if (Event.current.type == EventType.MouseUp)
			{
				m_resize	= false;
			}
		}

        #endregion

        #region Nested types

		private enum SplitDirection
		{
			Horizontal,

			Vertical
		}

        #endregion
    }
}