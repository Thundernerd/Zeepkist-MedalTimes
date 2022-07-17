namespace TNRD.Zeepkist.MedalTimes.Events
{
    public readonly struct GameMasterLoadedEvent
    {
        public readonly GameMaster GameMaster;

        public GameMasterLoadedEvent(GameMaster gameMaster)
        {
            GameMaster = gameMaster;
        }
    }
}
