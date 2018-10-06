using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class State<T>
    {
        protected T owner;

        public State(T owner)
        {
            this.owner = owner;
        }

        // このステートに遷移する時に一度だけ呼ばれる
        public virtual void Enter() { }

        // このステートである間、毎フレーム呼ばれる
        public virtual void Execute() { }

        // このステートから他のステートに遷移するときに一度だけ呼ばれる
        public virtual void Exit() { }
    }
}