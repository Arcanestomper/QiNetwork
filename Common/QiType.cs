namespace QiNetwork.Common
{
    public enum QiType
    {
        Fire,
        Wood,
        Metal,
        Water,
        Earth,
        YinYang,
    }

    public static class QiTypeCollections
    {
        public static QiType[] AllTypes => [QiType.Fire, QiType.Wood, QiType.Metal, QiType.Water, QiType.Earth, QiType.YinYang];

        public static QiType[] ElementalTypes => [QiType.Fire, QiType.Wood, QiType.Metal, QiType.Water, QiType.Earth];

        public static QiType[] OtherTypes => [QiType.YinYang];
    }
}
