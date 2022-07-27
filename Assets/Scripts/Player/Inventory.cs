using System.Collections.Generic;

public class Inventory
{
    public bool IsChanged { get; private set; }
    private List<ItemInfo> _contents = new();
    public IReadOnlyList<ItemInfo> Contents => _contents;
    public int Count => _contents.Count;

    public void AddItem(ItemInfo item, byte amount = 1)
    {
        if (item == null) return;
        if (!_contents.Contains(item))
        {
            _contents.Add(item);
            item.Amount = 0;
        }

        item.Amount += (int)amount;
        IsChanged = true;
    }

    public ItemInfo GetItem(int index)
    {
        return _contents[index];
    }

    public void SubItem(ItemInfo item, int amount = 1)
    {
        item.Amount -= amount;
        if (item.Amount <= 0)
        {
            _contents.Remove(item);
        }

        IsChanged = true;
    }

    public void ChangeIndex(ItemInfo item, int index)
    {
        if (_contents.Contains(item))
        {
            _contents.Remove(item);
            _contents.Insert(index, item);
            IsChanged = true;
        }
    }

    public int GetAmount(ItemInfo item)
    {
        if (!_contents.Contains(item)) item.Amount = 0;
        return item.Amount;
    }

    public void Updated()
    {
        IsChanged = false;
    }
}
