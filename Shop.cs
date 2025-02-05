using SPATextRPG;

public class Shop
{
    private List<Item> potions = new List<Item> { new HealthPotion(), new StrengthPotion() };
    private Inventory inventory;
    public Shop(Inventory inventory) => this.inventory = inventory;
    private List<Item> equipments = new List<Item> { new Dagger(), new IronSword(), new LongSword(), new Excalibur(), new Shield(), new Armor() };
    public void EnterShop(MyChar character)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("-상점-");
            Thread.Sleep(500);
            Console.WriteLine("◢ ◥ ▦ ◣");
            Thread.Sleep(50);
            Console.WriteLine("│田│田│\\");
            Thread.Sleep(1000);
            Console.WriteLine("상점에 오신 것을 환영합니다!");
            Console.WriteLine($"현재 보유 코인: {inventory.Coins}");
            Console.WriteLine("1. 물약 상점");
            Console.WriteLine("2. 무기 업그레이드");
            Console.WriteLine("3. 방어구 업그레이드");
            Console.WriteLine("4. 장비 상점");
            Console.WriteLine("5. 아이템 판매");
            Console.WriteLine("9. 상점 종료");

            int choice = InputHelper.GetValidNumber("-입력-", 1, 9);
            switch (choice)
            {
                case 1: BuyPotion(); break;
                case 2: UpgradeWeapon(character); break;
                case 3: UpgradeArmor(character); break;
                case 4: BuyEquipment(); break;
                case 5: SellItem(); break;
                case 9: Console.WriteLine("상점을 나갑니다."); return;
                default: Console.WriteLine("잘못된 입력입니다."); break;
            }
        }
    }
    // 장비 상점
    private void BuyEquipment()
    {
        Console.WriteLine();
        Console.WriteLine("-장비 목록-");
        Thread.Sleep(500);
        for (int i = 0; i < equipments.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {equipments[i].Name}");
            Console.WriteLine($"가격: {equipments[i].Price}");
            Console.WriteLine($"공격력: {equipments[i].AttackBonus}");
            Console.WriteLine($"방어력: {equipments[i].DefenseBonus}");
            Console.WriteLine();
        }
        Console.WriteLine("9. 돌아가기");

        int choice = InputHelper.GetValidNumber("-입력-", 1, 9);

        switch (choice)
        {
            case 9:
                Console.WriteLine("돌아갑니다.");
                break;
            default:
                int selectedPrice = equipments[choice - 1].Price; // 선택한 장비의 가격을 가져옵니다.
                if (inventory.Coins >= selectedPrice)
                {
                    inventory.SpendCoins(selectedPrice); // 선택한 장비의 가격만큼 코인을 차감합니다.
                    inventory.AddItem(equipments[choice - 1]);
                    Console.WriteLine($"{equipments[choice - 1].Name}을(를) 구매했습니다.");
                }
                else
                {
                    Console.WriteLine("코인이 부족합니다.");
                }
                break;
        }
    }

    // 아이템 판매
    public void SellItem()
    {
        Console.WriteLine();
        Console.WriteLine("-판매할 아이템 선택-");
        Thread.Sleep(500);
        var itemList = new List<Item>(inventory.GetItems().Keys);

        for (int i = 0; i < itemList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {itemList[i].Name} (수량: {inventory.GetItems()[itemList[i]]})");
        }
        Console.WriteLine("9. 돌아가기");

        int choice = InputHelper.GetValidNumber("-입력-", 1, 9);
        if (choice != 9)
        {
            var item = itemList[choice - 1];
            int sellPrice = item is Equipment ? 50 : 15;
            inventory.AddCoins(sellPrice);
            inventory.RemoveItem(item);

            Console.WriteLine($"{item.Name}을(를) 판매했습니다. ({sellPrice} 코인 획득)");
        }
    }

    // 무기 강화
    private void UpgradeWeapon(MyChar character)
    {
        const int upgradeCost = 50;
        if (inventory.Coins >= upgradeCost)
        {
            inventory.SpendCoins(upgradeCost);
            character.AttackPower(10);
            Console.WriteLine($"무기 업그레이드 완료! {upgradeCost}코인 차감.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("코인이 부족합니다.");
        }
    }

    // 방어구 강화
    private void UpgradeArmor(MyChar character)
    {
        const int upgradeCost = 50;
        if (inventory.Coins >= upgradeCost)
        {
            inventory.SpendCoins(upgradeCost);
            character.IncreaseDefense(10);
            Console.WriteLine($"방어구 업그레이드 완료! {upgradeCost}코인 차감.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("코인이 부족합니다.");
        }
    }

    // 포션 상점
    private void BuyPotion()
    {
        Console.WriteLine();
        Console.WriteLine("-물약 목록-");
        Thread.Sleep(500);
        for (int i = 0; i < potions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {potions[i].Name} {potions[i].Price}");
        }
        Console.WriteLine("9. 돌아가기");

        int choice = InputHelper.GetValidNumber("-입력-", 1, 9);
        if (choice == 9)
        {
            return; // 돌아가기
        }

        if (choice >= 1 && choice <= potions.Count)
        {
            if (inventory.Coins >= 30)
            {
                inventory.SpendCoins(30);
                inventory.AddItem(potions[choice - 1]);
                Console.WriteLine($"{potions[choice - 1].Name}을(를) 구매했습니다.");
            }
            else
            {
                Console.WriteLine("코인이 부족합니다.");
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

}