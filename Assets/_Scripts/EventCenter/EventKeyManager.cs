namespace Event
{
    public static class EventKeyManager
    {
        public static readonly EventKey Move = new("Move");
        public static readonly EventKey StopMove = new("StopMove");

        public static readonly EventKey Fire = new("Fire");
        public static readonly EventKey StopFire = new("StopFire");
        public static readonly EventKey LaunchMissile = new("LaunchMissile");

        public static readonly EventKey Dodge = new("Dodge");

        public static readonly EventKey InputOverDriveOn = new("InputOverDriveOn");
        public static readonly EventKey PlayerOverDriveOn = new("PlayerOverDriveOn");
        public static readonly EventKey OverDriveOff = new("OverDriveOff");

        public static readonly EventKey AddOption = new("AddOption");

        public static readonly EventKey EnemyManagementInit = new("Initialize");
    }
}
