using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoFaultFixerUI.Services
{
    public class StateHandlerService<T>
    {
        private Stack<T> undoStates;
        private Stack<T> redoStates;
        private T currentState;

        public StateHandlerService(T startingState)
        {
            currentState = startingState;
            undoStates = new Stack<T>();
            redoStates = new Stack<T>();
        }

        public bool CanRedo()
        {
            return (redoStates.Count > 0);
        }

        public bool CanUndo()
        {
            return (undoStates.Count > 0);
        }

        public void Clear()
        {
            undoStates.Clear();
            redoStates.Clear();
        }

        public T SetNewState(T newState)
        {
            redoStates.Clear();
            undoStates.Push(currentState);
            currentState = newState;
            return currentState;
        }

        public T GetCurrentState()
        {
            return currentState;
        }

        public T Redo()
        {
            undoStates.Push(currentState);
            currentState = redoStates.Pop();
            return currentState;
        }

        public T Undo()
        {
            redoStates.Push(currentState);
            currentState = undoStates.Pop();
            return currentState;
        }
    }
}
