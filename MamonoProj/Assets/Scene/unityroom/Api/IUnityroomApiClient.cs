namespace unityroom.Api
{
    public interface IUnityroomApiClient
    {
        void SendPoint(
            int boardNo
            , float score
            , PointboardWriteMode mode
        );
    }
}