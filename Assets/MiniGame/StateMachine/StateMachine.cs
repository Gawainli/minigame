using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame.StateMachine
{
    public class StateMachine
    {
        public bool isRunning = false;
        public string name;
        
        private object _owner;
        public object Owner => _owner;

        private readonly Dictionary<Type, State> _states;
        private State _currentState;
        private State _lastState;

        private StateMachine()
        {
            _owner = null;
            _states = new Dictionary<Type, State>();
        }
        
        ~StateMachine()
        {
            _owner = null;
            _states.Clear();
            StateMachineModule.UnRegisterStateMachine(name, this);
        }

        public State GetState(Type stateType)
        {
            if (stateType == null)
            {
                return null;
            }

            State state = null;
            if (_states.TryGetValue(stateType, out state))
            {
                return state;
            }
            return null;
        }


        public void Tick(float delta)
        {
            if (!isRunning)
            {
                return;
            }

            if (_currentState != null)
            {
                _currentState.Tick(delta);
            }
        }

        public void FixedTick(float delta)
        {
            if (!isRunning)
            {
                return;
            }

            if (_currentState != null)
            {
                _currentState.FixedTick(delta);
            }
        }

        public void ChangeState<T>() where T : State
        {
            if (!isRunning)
            {
                return;
            }
            var state = GetState(typeof(T));
            if (state == null)
            {
                return;
            }
            
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _lastState = _currentState;
            _currentState = state;
            _currentState.SetParent(this);
            _currentState.Enter();
        }
        
        public static StateMachine Create<T>(T owner, params State[] states) where T : class
        {
            if (owner == null)
            {
                return null;
            }

            var stateMachine = new StateMachine();
            stateMachine._owner = owner;
            stateMachine.name = owner.GetType().Name + ".FSM";
            
            foreach (var state in states)
            {
                if (state == null)
                {
                    continue;
                }

                var type = state.GetType();
                if (stateMachine._states.ContainsKey(type))
                {
                    continue;
                }
                stateMachine._states.Add(type, state);
                state.Init();
            }
            
            StateMachineModule.RegisterStateMachine(stateMachine.name, stateMachine);
            return stateMachine;
        }
    }
}