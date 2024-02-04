using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
	[Serializable]
	public class PBXFramework
	{
		#region Fields

		[SerializeField, PBXFrameworkName]
		private	    string			    m_name;

        [SerializeField, EnumMaskField(typeof(PBXTargetMembership))]
		private     PBXTargetMembership m_target;
        
        [SerializeField, FormerlySerializedAs("m_isWeak")]
		private	    bool			    m_isOptional;

		#endregion

		#region Properties

		public string Name
        {
            get => m_name;
            private set => m_name = value;
        }

        public PBXTargetMembership Target
        {
            get => m_target;
            private set => m_target = value;
        }

        public bool IsOptional
        {
            get => m_isOptional;
            private set => m_isOptional = value;
        }

        #endregion

        #region Constructors

        public PBXFramework(string name, PBXTargetMembership target = (PBXTargetMembership)0,
            bool isOptional = false)
        {
            // Set properties
            Name            = name;
            Target          = target;
            IsOptional      = isOptional;
        }

        #endregion

        #region Base class methods

        public override bool Equals(object obj)
        {
            if (obj is PBXFramework)
            {
                return string.Equals(m_name, ((PBXFramework)obj).m_name);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}