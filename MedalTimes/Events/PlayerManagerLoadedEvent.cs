namespace TNRD.Zeepkist.MedalTimes.Events
{
    public readonly struct PlayerManagerLoadedEvent
    {
        public readonly PlayerManager PlayerManager;

        public PlayerManagerLoadedEvent(PlayerManager playerManager)
        {
            PlayerManager = playerManager;
        }
    }
}
