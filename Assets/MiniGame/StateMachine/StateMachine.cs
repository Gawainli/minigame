using System;
using System.Collections.Generic;
using MiniGame.Logger;
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

        public StateMachine(object owner)
        {
            _owner = owner;
            name = owner.GetType().Name + ".FSM";
            _states = new Dictionary<Type, State>();
        }
        
        ~StateMachine()
        {
            _owner = null;
            _states.Clear();
            StateMachineModule.UnRegisterStateMachine(name, this);
        }

        public void Start<T>() where T : State
        {
            _currentState = GetState(typeof(T));
            _lastState = _currentState;
            if (_currentState == null)
            {
                LogModule.Error("Start StateMachine Failed, State is Null");
                return;
            }

            isRunning = true;
            _currentState.SetParent(this);
            _currentState.Enter();
        }

        public State GetState(Type stateType)
        {
            if (stateType == null)
            {
                return null;
            }

            if (_states.TryGetValue(stateType, out var state))
            {
                return state;
            }
            return null;
        }

        public void AddState<T>() where T : State
        {
            var type = typeof(T);
            var state = Activator.CreateInstance<T>();
            if (state == null)
            {
                return;
            }
            AddState(state);
        }
        
        
        public void AddState(State state)
{
            if (state == null)
            {
                return;
            }

            if (_states.ContainsKey(state.GetType()))
            {
                return;
            }

            _states.Add(state.GetType(), state);
            state.Init();
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
    }
}