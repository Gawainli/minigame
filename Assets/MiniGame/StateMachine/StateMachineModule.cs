using System.Collections.Generic;
using MiniGame.Base;

namespace MiniGame.StateMachine
{
    public class StateMachineModule : GameModule
    {
        private static readonly Dictionary<string, List<StateMachine>> StateMachineDict = new Dictionary<string, List<StateMachine>>();
        
        public static List<StateMachine> GetStateMachine(string name)
        {
            if (StateMachineDict.TryGetValue(name, out var machines))
            {
                return machines;
            }
            return null;
        }
        
        public static void RegisterStateMachine(string name, StateMachine stateMachine)
        {
            if (StateMachineDict.TryGetValue(name, out var machines))
            {
                machines.Add(stateMachine);
            }
            StateMachineDict.Add(name, new List<StateMachine>(){stateMachine});
        }
        
        public static void UnRegisterStateMachine(string name, StateMachine stateMachine)
        {
            if (StateMachineDict.TryGetValue(name, out var machines))
            {
                machines.Remove(stateMachine);
            }
        }
        
        public override void Tick(float deltaTime, float unscaledDeltaTime)
        {
            foreach (var machines in StateMachineDict.Values)
            {
                foreach (var machine in machines)
                {
                   machine.Tick(deltaTime); 
                }
            }
        }

        public override void Shutdown()
        {
            StateMachineDict.Clear();
        }
    }
}