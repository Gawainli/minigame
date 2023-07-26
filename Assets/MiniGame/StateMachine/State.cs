namespace MiniGame.StateMachine
{
    public abstract class State
    {
        public float age;
        public float fixedAge;
        
        protected StateMachine fsm;

        public State(){}

        public abstract void Init();
        public abstract void Enter();
        public abstract void Exit();

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

        public void ChangeState<T>() where T : State
        {
            if (fsm != null)
            {
                fsm.ChangeState<T>();
            }
        }
    }
}