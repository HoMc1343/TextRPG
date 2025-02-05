using SPATextRPG;

public class Village
{
    private MyChar player;
    private Inventory inventory;

    public Village(MyChar player, Inventory inventory)
    {
        this.player = player;
        this.inventory = inventory;
    }

    public void EnterVillage()
    {
        while (!player.IsDead)
        {
            Console.WriteLine();
            Console.WriteLine("-마을-");
            Thread.Sleep(500);
            Console.WriteLine("° :.   • ○   ° ★  .   .");
            Thread.Sleep(50);
            Console.WriteLine("★ ° . .   . ☾ °☆ . * ●");
            Thread.Sleep(50);
            Console.WriteLine("∩ │◥███◣ ╱◥███◣");
            Thread.Sleep(50);
            Console.WriteLine("╱◥◣ ◥████◣▓∩▓│∩║");
            Thread.Sleep(50);
            Console.WriteLine("│╱◥█◣║∩∩∩ ║◥█▓ ▓█◣");
            Thread.Sleep(50);
            Console.WriteLine("││∩│▓ ║∩田│║▓ ▓ ▓∩║");
            Thread.Sleep(1000);
            Console.WriteLine("1. 상점");
            Console.WriteLine("2. 전투");
            Console.WriteLine("3. 인벤토리");
            Console.WriteLine("4. 상태창");
            Console.WriteLine("7. 저장하기");
            Console.WriteLine("8. 불러오기");
            Console.WriteLine("9. 종료");

            int choice = InputHelper.GetValidNumber("-입력-", 1, 9);
            switch (choice)
            {
                case 1: new Shop(inventory).EnterShop(player); break;
                case 2: StartBattle(); break;
                case 3: ShowInventory(); break;
                case 4: ShowStatus(); break;
                case 7: SaveLoadManager.SaveGame(player, inventory); break; // 저장 기능 호출
                case 8:
                    var (loadedPlayer, loadedInventory) = SaveLoadManager.LoadGame();
                    if (loadedPlayer != null && loadedInventory != null)
                    {
                        player = loadedPlayer;
                        inventory = loadedInventory;
                    }
                    break;
                case 9: return;
                default: Console.WriteLine("잘못된 입력입니다."); break;
            }
        }
        Console.WriteLine("플레이어가 사망했습니다.");
    }







    private void StartBattle()
    {
        Console.WriteLine();
        Thread.Sleep(500);
        Console.WriteLine("──────▄██▀▀▀▀▄");
        Thread.Sleep(50);
        Console.WriteLine("────▄███▀▀▀▀▀▀▀▄");
        Thread.Sleep(50);
        Console.WriteLine("──▄████▀▀DUNGEON▀▄");
        Thread.Sleep(50);
        Console.WriteLine("▄█████▀▀▀       ▀▀▀▄");
        Thread.Sleep(1000);
        Console.WriteLine("-STAGE-");
        Console.WriteLine("1. 고블린");
        Console.WriteLine("2. 오크");
        Console.WriteLine("3. 드래곤");

        int choice = InputHelper.GetValidNumber("-입력-", 1, 2);
        {
            MyChar monster = choice switch
            {
                1 => new Goblin(),
                2 => new Oak(),
                3 => new Dragon(),
                _ => null
            };

            if (monster != null)
            {
                List<Item> rewards = new List<Item> { new HealthPotion(), new StrengthPotion() };
                new Stage(player, monster, rewards, inventory).Start();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
    private void ShowInventory()
    {
        Console.WriteLine();
        Console.WriteLine("-인벤토리-");
        Thread.Sleep(500);
        inventory.ShowItems();
        Console.WriteLine($"보유 코인: {inventory.Coins}");
        Console.WriteLine();
        Console.WriteLine("1: 아이템 사용");
        Console.WriteLine("2: 뒤로가기");

        int choice = InputHelper.GetValidNumber("-입력-", 1, 2);
        if (choice == 1)
        {
            if (inventory.ItemsCount == 0)
            {
                Console.WriteLine();
                Console.WriteLine("아이템이 없습니다.");
                return;
            }
            Console.WriteLine();
            Console.WriteLine("사용할 아이템을 선택하세요");
            int itemIndex = InputHelper.GetValidNumber("-입력-", 1, inventory.ItemsCount) - 1;
            inventory.UseItem(itemIndex, player);
        }
    }
    private void ShowStatus()
    {
        Console.WriteLine($"[{player.Name}의 상태]");
        Console.WriteLine($"직 업: {player.Role}");
        Console.WriteLine($"체 력: {player.Health}");
        Console.WriteLine($"공격력: {player.Attack}");
        Console.WriteLine($"방어력: {player.Defense}");
        Console.WriteLine($"Coin: {inventory.Coins}");
    }
}