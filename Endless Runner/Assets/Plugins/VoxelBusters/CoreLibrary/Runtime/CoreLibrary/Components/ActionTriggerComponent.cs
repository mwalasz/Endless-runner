using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public abstract class ActionTriggerComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private     ActionTriggerType       m_triggerOn;

        #endregion

        #region Properties

        public bool IsDone { get; protected set; }

        #endregion

        #region Abstract members

        public abstract void ExecuteAction();

        #endregion

        #region Unity methods

        private void Start()
        {
            TryExecuteAction(triggerType: ActionTriggerType.Start);
        }

        private void OnEnable()
        {       
            TryExecuteAction(triggerType: ActionTriggerType.OnEnable);
        }

        private void OnDisable()
        {
            TryExecuteAction(triggerType: ActionTriggerType.OnDisable);
        }

        private void OnDestroy()
        {
            TryExecuteAction(triggerType: ActionTriggerType.Destroy);
        }

        private void Update()
        {
            if (!IsDone)
            {
                TryExecuteAction(triggerType: ActionTriggerType.Update);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            TryExecuteAction(triggerType: ActionTriggerType.TriggerEnter);
        }

        private void OnTriggerExit(Collider other)
        {
            TryExecuteAction(triggerType: ActionTriggerType.TriggerExit);
        }

        private void OnCollisionEnter(Collision collision)
        {
            TryExecuteAction(triggerType: ActionTriggerType.CollisionEnter);
        }

        private void OnCollisionExit(Collision collision)
        {
            TryExecuteAction(triggerType: ActionTriggerType.CollisionExit);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TryExecuteAction(triggerType: ActionTriggerType.TriggerEnter2D);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            TryExecuteAction(triggerType: ActionTriggerType.TriggerExit2D);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryExecuteAction(triggerType: ActionTriggerType.CollisionEnter2D);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            TryExecuteAction(triggerType: ActionTriggerType.CollisionExit2D);
        }

        #endregion

        #region Public methods

        public virtual void Reset()
        {
            IsDone    = false;
        }

        #endregion

        #region Private methods

        private bool TryExecuteAction(ActionTriggerType triggerType)
        {
            if (!IsDone && (triggerType == m_triggerOn))
            {
                ExecuteAction();
                return true;
            }
            return false;
        }

        #endregion

        #region Nested types

        public enum ActionTriggerType
        {
            Start = 1,

            Destroy,

            OnEnable,

            OnDisable,

            Update,

            TriggerEnter,

            TriggerExit,

            CollisionEnter,

            CollisionExit,

            TriggerEnter2D,

            TriggerExit2D,

            CollisionEnter2D,

            CollisionExit2D,

            Custom,
        }

        #endregion
    }
}