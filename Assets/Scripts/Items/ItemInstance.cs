public class ItemInstance
{
    public IItem instance;
    public ItemData data;

    public ItemInstance(IItem instance, ItemData data)
    {
        this.instance = instance;
        this.data = data;
    }
}