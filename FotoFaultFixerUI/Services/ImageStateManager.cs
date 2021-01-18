using FotoFaultFixerLib.ImageProcessing;
using FotoFaultFixerUI.Services.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FotoFaultFixerUI.Services
{
    /// <summary>
    /// A Manager for invoking Commands against an image, and keeping track of current/previous states.
    /// Allows undo/redo of commands.
    /// </summary>
    /// <remarks>Command Pattern</remarks>
    public class ImageStateManager
    {
        private Stack<ICommandCImage> _undoCommands;
        private Stack<ICommandCImage> _redoCommands;
        private CImage _currentState;

        public ImageStateManager(CImage img)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            _currentState = img;
            _undoCommands = new Stack<ICommandCImage>();
            _redoCommands = new Stack<ICommandCImage>();
        }

        /// <summary>
        /// Returns the current state of the Image
        /// </summary>
        /// <returns></returns>
        public CImage GetCurrentState()
        {
            return _currentState;
        }

        /// <summary>
        /// Executes a command, updating the current image
        /// </summary>
        /// <param name="cmd"></param>
        public async Task Invoke(ICommandCImage cmd, IProgress<int> progressReporter)
        {
            await Task.Run(() =>
            {
                _currentState = cmd.Execute(_currentState, progressReporter);
                _undoCommands.Push(cmd);
            });
        }

        public bool CanRedo()
        {
            return (_redoCommands.Count > 0);
        }

        /// <summary>
        /// Re-executes a command (after an undo)
        /// </summary>
        public async Task Redo(IProgress<int> progressReporter)
        {
            if (CanRedo())
            {
                ICommandCImage cmd = _redoCommands.Pop();
                await Invoke(cmd, progressReporter);
            }
        }

        public bool CanUndo()
        {
            return (_undoCommands.Count > 0);
        }

        /// <summary>
        /// Undoes the effects of the last executed command
        /// </summary>
        public async Task Undo()
        {
            if (CanUndo())
            {
                ICommandCImage cmd = _undoCommands.Pop();
                await Task.Run(() =>
                {
                    _currentState = cmd.UnExecute(_currentState);
                    _redoCommands.Push(cmd);
                });
            }
        }

        public void Clear()
        {
            _undoCommands.Clear();
            _redoCommands.Clear();
        }
    }
}
