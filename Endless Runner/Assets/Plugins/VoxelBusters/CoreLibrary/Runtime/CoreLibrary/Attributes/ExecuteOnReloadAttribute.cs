using System;
// Credits: https://github.com/joshcamas/unity-domain-reload-helper

namespace VoxelBusters.CoreLibrary
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteOnReloadAttribute : Attribute
    {
        /// <summary>
        /// Marks method to be executed on reload.
        /// </summary>
        public ExecuteOnReloadAttribute()
        { }
    }
}