namespace TNRD.Zeepkist.MedalTimes.Events
{
    public readonly struct SetupGameLoadedEvent
    {
        public readonly SetupGame SetupGame;

        public SetupGameLoadedEvent(SetupGame setupGame)
        {
            SetupGame = setupGame;
        }
    }
}
