using FotoFaultFixerUI.Services.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FotoFaultFixerUI.Services
{
    /// <summary>
    /// A Manager for invoking Commands against an image, and keeping track of current/previous states.
    /// Allows undo/redo of commands.
    /// </summary>
    /// <remarks>Command Pattern</remarks>
    public class ImageStateManager
    {
        private Stack<ICommandBMP> _undoCommands;
        private Stack<ICommandBMP> _redoCommands;
        private Bitmap _currentState;

        public ImageStateManager(Bitmap img)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            _currentState = img;
            _undoCommands = new Stack<ICommandBMP>();
            _redoCommands = new Stack<ICommandBMP>();
        }

        /// <summary>
        /// Returns the current state of the Image
        /// </summary>
        /// <returns></returns>
        public Bitmap GetCurrentState()
        {
            return _currentState;
        }

        /// <summary>
        /// Executes a command, updating the current image
        /// </summary>
        /// <param name="cmd"></param>
        public void Invoke(ICommandBMP cmd)
        {
            _currentState = cmd.Execute(_currentState);
            _undoCommands.Push(cmd);
        }

        public bool CanRedo()
        {
            return (_redoCommands.Count > 0);
        }

        /// <summary>
        /// Re-executes a command (after an undo)
        /// </summary>
        public void Redo()
        {
            if (CanRedo())
            {
                ICommandBMP cmd = _redoCommands.Pop();
                _currentState = cmd.Execute(_currentState);
                _undoCommands.Push(cmd);
            }
        }

        public bool CanUndo()
        {
            return (_undoCommands.Count > 0);
        }

        /// <summary>
        /// Undoes the effects of the last executed command
        /// </summary>
        public void Undo()
        {
            if (CanUndo())
            {
                ICommandBMP cmd = _undoCommands.Pop();
                _currentState = cmd.UnExecute(_currentState);
                _redoCommands.Push(cmd);
            }
        }

        public void Clear()
        {
            _undoCommands.Clear();
            _redoCommands.Clear();
        }
    }
}
