using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class CustomEditorStyles
    {
        #region Properties

        public static Color BorderColor => new Color(0.15f, 0.15f, 0.15f, 1f);

        #endregion

        #region Label styles

        public static GUIStyle Heading1Label()
        {
            return CreateLabel(baseStyle: EditorStyles.boldLabel,
                               fontSize: 18,
                               wordWrap: true,
                               richText: true,
                               alignment: TextAnchor.MiddleLeft);
        }

        public static GUIStyle Heading2Label()
        {
            return CreateLabel(baseStyle: EditorStyles.boldLabel,
                               fontSize: 16,
                               wordWrap: true,
                               richText: true,
                               alignment: TextAnchor.MiddleLeft);
        }

        public static GUIStyle Heading3Label()
        {
            return CreateLabel(baseStyle: EditorStyles.boldLabel,
                               fontSize: 14,
                               wordWrap: true,
                               richText: true,
                               alignment: TextAnchor.MiddleLeft);
        }

        public static GUIStyle NormalLabel()
        {
            return CreateLabel(baseStyle: EditorStyles.label,
                               fontSize: 14,
                               wordWrap: true,
                               richText: true);
        }

        public static GUIStyle OptionsLabel(bool wordWrap = true)
        {
            return CreateLabel(baseStyle: EditorStyles.label,
                               fontSize: 12,
                               wordWrap: wordWrap,
                               richText: true,
                               textClipping: TextClipping.Clip);
        }

        public static GUIStyle LinkLabel()
        {
            return CreateLabel(baseStyle: "LinkLabel",
                               fontSize: 12,
                               wordWrap: true,
                               richText: true);
        }

        public static GUIStyle SelectableLabel(FontStyle? fontStyle = null,
                                               int? fontSize = null,
                                               Color? textColor = null)
        {
            var     normal          = EditorStyles.label;
            var     baseStyle       = new GUIStyle()
            {
                border              = new RectOffset(0, 0, 0, 0),
                font                = normal.font,
                alignment           = normal.alignment,
                active              = normal.active,
                onActive            = normal.onActive,
                focused             = normal.focused,
                onFocused           = normal.onFocused,
                normal              = normal.normal,
                onNormal            = normal.onNormal,
                hover               = normal.hover,
                onHover             = normal.onHover,
            };
            return CreateLabel(baseStyle,
                               fontStyle: fontStyle,
                               fontSize: fontSize ?? 14,
                               textColor: textColor);
        }

        #endregion

        #region Button styles

        public static GUIStyle Button()
        {
            return CreateButton(baseStyle: "Button",
                                fontSize: 14);
        }

        public static GUIStyle InvisibleButton()
        {
            return CreateButton(baseStyle: "InvisibleButton");
        }

        #endregion

        #region Background styles

        public static GUIStyle ItemBackground()
        {
            return CreateBackground(baseStyle: "AnimItemBackground");
        }

        public static GUIStyle GroupBackground()
        {
            GUIStyle    baseStyle   = "FrameBox";
            var         baseMargin  = baseStyle.margin;
            return CreateBackground(baseStyle: baseStyle,
                                    border: new RectOffset(0, 0, 0, 0),
                                    margin: new RectOffset(baseMargin.left, baseMargin.right, baseMargin.top, 5));
        }

        #endregion

        #region Static methods

        public static GUIStyle CreateLabel(GUIStyle baseStyle,
                                           FontStyle? fontStyle = null,
                                           int? fontSize = null,
                                           Color? textColor = null,
                                           bool? wordWrap = null,
                                           bool? richText = null,
                                           TextAnchor? alignment = null,
                                           TextClipping? textClipping = null)
        {
            var     newStyle        = new GUIStyle(baseStyle);
            if (fontSize != null)
            {
                newStyle.fontSize   = fontSize.Value;
            }
            if (fontStyle != null)
            {
                newStyle.fontStyle  = fontStyle.Value;
            }
            if (textColor != null)
            {
                newStyle.normal.textColor       = textColor.Value;
                newStyle.onNormal.textColor     = textColor.Value;
                newStyle.active.textColor       = textColor.Value;
                newStyle.onActive.textColor     = textColor.Value;
                newStyle.focused.textColor      = textColor.Value;
                newStyle.onFocused.textColor    = textColor.Value;
                newStyle.hover.textColor        = textColor.Value;
                newStyle.onHover.textColor      = textColor.Value;
            }
            if (wordWrap != null)
            {
                newStyle.wordWrap   = wordWrap.Value;
            }
            if (richText != null)
            {
                newStyle.richText   = richText.Value;
            }
            if (alignment != null)
            {
                newStyle.alignment  = alignment.Value;
            }
            if (textClipping != null)
            {
                newStyle.clipping   = textClipping.Value;
            }
            return newStyle;
        }

        public static GUIStyle CreateButton(GUIStyle baseStyle,
                                            int? fontSize = null,
                                            int? fixedHeight = null,
                                            TextAnchor? alignment = null)
        {
            var     newStyle            = new GUIStyle(baseStyle);
            if (fontSize != null)
            {
                newStyle.fontSize       = fontSize.Value;
            }
            if (fixedHeight != null)
            {
                newStyle.fixedHeight    = fixedHeight.Value;
            }
            if (alignment != null)
            {
                newStyle.alignment      = alignment.Value;
            }
            return newStyle;
        }

        public static GUIStyle CreateBackground(GUIStyle baseStyle,
                                                RectOffset border = null,
                                                RectOffset margin = null)
        {
            var     newStyle        = new GUIStyle(baseStyle);
            if (border != null)
            {
                newStyle.border     = border;
            }
            if (margin != null)
            {
                newStyle.margin     = margin;
            }
            return newStyle;
        }

        #endregion
    }
}