using SPATextRPG;

public class Inventory
{
    private Dictionary<Item, int> items = new Dictionary<Item, int>();
    public Dictionary<Item, int> GetItems() => new Dictionary<Item, int>(items);
    public int Coins { get; private set; }
    public int ItemsCount => items.Count;
    public Inventory(int initialCoins) => Coins = initialCoins;

    public void AddItem(Item item)
    {
        if (items.ContainsKey(item)) items[item]++;
        else items[item] = 1;
        Console.WriteLine();
    }
    public void ShowItems()
    {
        Console.WriteLine("인벤토리:");
        var itemList = new List<Item>(items.Keys);

        for (int i = 0; i < itemList.Count; i++)
        {
            string status = (itemList[i] is Equipment equipment) ? (equipment.IsEquipped ? "(착용 중)" : "(미 착용 중)") : "";
            Console.WriteLine($"{i + 1}. {itemList[i].Name} {status} (수량: {items[itemList[i]]})");
        }
    }

    public void UseItem(int index, MyChar character)
    {
        var itemList = new List<Item>(items.Keys);

        if (index >= 0 && index < itemList.Count)
        {
            var item = itemList[index];
            item.Use(character);

            if (!(item is Equipment))
            {
                items[item]--;
                if (items[item] == 0)
                    items.Remove(item);
            }

            Console.WriteLine($"{item.Name}을(를) 착용(사용)했습니다.");
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }
    public void RemoveItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0) items.Remove(item);
        }
    }

    public void SpendCoins(int amount)
    {
        if (Coins >= amount) Coins -= amount;
        else Console.WriteLine("코인이 부족합니다.");
    }

    public void AddCoins(int amount) => Coins += amount;
}