namespace MiniGame.Pool
{
    public interface IPoolObj
    {
        void Init(params System.Object[] userDatas);
        void Reset();
    }
}