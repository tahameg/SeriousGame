using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Common
{
    public abstract class StateMachineBase : MonoBehaviour
    {
        public IState CurrentState { get; private set; }
        private IState _previousState;
        bool _isInTransition;

        public void ChangeState(IState newState)
        {
            if(_isInTransition || CurrentState == newState)
            {
                return;
            }
            else
            {
                
            }
        }

        public void ChangeStateRoutine(IState newState)
        {
            if(_isInTransition || CurrentState == newState)
            {
                return;
            }
            else
            {
                _isInTransition = true;
                if (CurrentState != null)
                {
                    CurrentState.Exit();
                }

                _previousState = CurrentState;
                CurrentState = newState;
                if(CurrentState != null)
                {
                    CurrentState.Enter();
                }
                _isInTransition = false;
            }
            
        }
        public void RevertState()
        {
            if( _previousState != null)
            {
                ChangeStateRoutine(_previousState);
            }
        }

        public void Update()
        {
            if(CurrentState != null && !_isInTransition)
            {
                CurrentState.Tick();
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState != null && !_isInTransition)
            {
                CurrentState.FixedTick();
            }
        }

        public void LateUpdate()
        {
            if (CurrentState != null && !_isInTransition)
            {
                CurrentState.LateTick();
            }
        }
    }
}

