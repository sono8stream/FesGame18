using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateProcessor<T,TEnum> : MonoBehaviour
        where T : class where TEnum:System.IConvertible
    {
        protected List<State<T>> stateList = new List<State<T>>();
        protected State<T> currentState;

        // Update is called once per frame
        protected virtual void Update()
        {
            if (currentState != null)
            {
                currentState.Execute();
            }
        }

        public virtual void ChangeState(TEnum state)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = stateList[state.ToInt32(null)];
            currentState.Enter();
        }
    }
}