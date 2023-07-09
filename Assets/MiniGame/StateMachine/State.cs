namespace MiniGame.StateMachine
{
    public class State
    {
        public float age;
        public float fixedAge;
        
        protected StateMachine fsm;

        public State(){}
        
        public virtual void Init(){}
        public virtual void Enter(){}
        public virtual void Exit(){}

        public virtual void Tick(float delta)
        {
            age += delta;
        }

        public virtual void FixedTick(float delta)
        {
            fixedAge += delta;
        }

        public void SetParent(StateMachine p)
        {
            fsm = p;
        }
    }
}