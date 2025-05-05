using System;

namespace StateSystem
{
    public class State
    {
        private Action start, loop, end;

        public virtual void Start() => start?.Invoke();
        public virtual void Loop() => loop?.Invoke();
        public virtual void End() => end?.Invoke();

        public State SetStart(Action action)
        {
            start = action;
            return this;
        }

        public State SetLoop(Action action)
        {
            loop = action;
            return this;
        }

        public State SetEnd(Action action)
        {
            end = action;
            return this;
        }
    }
}