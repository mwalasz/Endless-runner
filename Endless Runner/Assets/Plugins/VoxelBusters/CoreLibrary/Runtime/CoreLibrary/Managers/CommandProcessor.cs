using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VoxelBusters.CoreLibrary
{
    public class CommandProcessor
    {
        #region Fields

        private     List<ICommand>          m_inprogressCommands;

        private     List<ICommand>          m_pendingCommands;

        #endregion

        #region Properties

        public CommandExecutionOrder ExecutionOrder { get; private set; }

        #endregion

        #region Events

        public event Callback<ICommand> OnCompletion;

        #endregion

        #region Constructors

        public CommandProcessor(CommandExecutionOrder order)
        {
            // set properties
            m_inprogressCommands    = new List<ICommand>();
            m_pendingCommands       = new List<ICommand>();
            ExecutionOrder          = order;
        }

        #endregion

        #region Command methods

        public void AddCommand(ICommand command)
        {
            Assert.IsArgNotNull(command, nameof(command));

            if (CommandExecutionOrder.None == ExecutionOrder)
            {
                ProcessCommandInternal(command);
            }
            else if (CommandExecutionOrder.Sequential == ExecutionOrder)
            {
                // add command to queue
                m_pendingCommands.Add(command);
            }
        }

        public void InvalidateAll()
        {
            m_pendingCommands.Clear();
        }

        #endregion

        #region Public methods

        public void Update()
        {
            if (CommandExecutionOrder.None == ExecutionOrder)
            {
                ProcessInprogressCommands();
            }
            else if (CommandExecutionOrder.Sequential == ExecutionOrder)
            {
                ProcessCommandsInSequentialOrder();
            }
        }

        #endregion

        #region Private methods

        private void ProcessCommandInternal(ICommand command)
        {
            // add item to the tracker
            m_inprogressCommands.Add(command);

            // execute action associated with the command
            command.Execute();
        }

        private void ProcessInprogressCommands()
        { 
            // parse through the inprogress commands and remove the completed ones
            for (int iter = 0; iter < m_inprogressCommands.Count; iter++)
            {
                var     command     = m_inprogressCommands[iter];
                if (command.IsDone)
                {
                    m_inprogressCommands.RemoveAt(iter);
                    iter--;
                    
                    // send completion event
                    PostCommandCompleteEvent(command);
                }
            }
        }

        private void ProcessCommandsInSequentialOrder()
        {
            // check whether active command is processed
            if (m_inprogressCommands.Count > 0)
            {
                var     command     = m_inprogressCommands[0];
                if (!command.IsDone)
                {
                    return;
                }

                // notify that command is completed
                m_inprogressCommands.RemoveAt(0);
                PostCommandCompleteEvent(command);
            }

            // execute command available in the pool
            if (m_pendingCommands.Count > 0)
            {
                var     newCommand  = m_pendingCommands[0];
                m_pendingCommands.RemoveAt(0);

                ProcessCommandInternal(newCommand);
            }
        }

        private void PostCommandCompleteEvent(ICommand command)
        {
            OnCompletion?.Invoke(command);
        }

        #endregion

        #region Nested types

        [Serializable]
        public class CommandEvent : UnityEvent<ICommand>
        { }

        public enum CommandExecutionOrder
        {
            None,
            
            Sequential,
        }

        #endregion
    }
}