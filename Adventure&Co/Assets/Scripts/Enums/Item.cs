namespace Enums
{
    public enum ItemCategory
    {
        Consumable,
        Equipment,
        QuestItem,
        Readable,
        Material
    }
    // NOTE: values are serialized as ints in item assets. Do not renumber without migrating assets.
    public enum ItemRarity
    {
        Common=1255000000,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Ascended

    }
}
