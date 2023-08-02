using MiniGame.Scene;
using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StateStartGame : State
    {
        public override void Init()
        {
        }

        public override async void Enter()
        {
            await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_Start.unity");
        }

        public override void Exit()
        {
        }
    }
}