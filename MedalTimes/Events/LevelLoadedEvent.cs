namespace TNRD.Zeepkist.MedalTimes.Events
{
    public readonly struct LevelLoadedEvent
    {
        public readonly GameMode Mode;

        public LevelLoadedEvent(GameMode mode)
        {
            Mode = mode;
        }
    }
}
