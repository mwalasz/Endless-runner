using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class RateMyAppDefaultControllerSettings
    {
        #region Fields

        [SerializeField]
        [Tooltip("The number of hours elapsed since first launch,  to show ratings window for the first time.")]
        private     PromptConstraints       m_initialPromptConstraints;

        [SerializeField]
        [Tooltip("The number of times the user must launch the app to show ratings window for the first time.")]
        private     PromptConstraints       m_repeatPromptConstraints;

        #endregion

        #region Properties

        public PromptConstraints InitialPromptConstraints => m_initialPromptConstraints;

        public PromptConstraints RepeatPromptConstraints => m_repeatPromptConstraints;

        #endregion

        #region Constructors

        public RateMyAppDefaultControllerSettings()
            : this(new PromptConstraints(minHours: 2, minLaunches: 0), new PromptConstraints(minHours: 6, minLaunches: 5))
        { }

        public RateMyAppDefaultControllerSettings(PromptConstraints initialPromptConstraints, PromptConstraints repeatPromptConstraints)
        {
            // set properties
            m_initialPromptConstraints      = initialPromptConstraints;
            m_repeatPromptConstraints       = repeatPromptConstraints;
        }

        #endregion

        #region Nested types

        [Serializable]
        public class PromptConstraints
        {
            [SerializeField]
            [Tooltip("Minimum hours elapsed.")]
            private     int     m_minHours;

            [SerializeField]
            [Tooltip("Minimum app launches count.")]
            private     int     m_minLaunches;

            public int MinHours { get { return m_minHours; } }

            public int MinLaunches { get { return m_minLaunches; } }

            public PromptConstraints(int minHours, int minLaunches)
            {
                // set properties
                m_minHours      = minHours;
                m_minLaunches   = minLaunches;
            }
        }

        #endregion
    }
}