namespace DevOpen.Domain
{
    public struct EventType
    {
        public EventType(string eventName, int latestVersion = 1)
        {
            EventName = eventName;
            LatestVersion = latestVersion;
        }

        public string EventName { get; }

        public int LatestVersion { get; }

        public static implicit operator string(EventType type)
        {
            return type.EventName;
        }
    }
}