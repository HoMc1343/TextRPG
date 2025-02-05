using SPATextRPG;

public class Stage
{
    private MyChar player;
    private MyChar monster;
    private List<Item> rewards;
    private Inventory inventory;

    public Stage(MyChar player, MyChar monster, List<Item> rewards, Inventory inventory)
    {
        this.player = player;
        this.monster = monster;
        this.rewards = rewards;
        this.inventory = inventory;
    }

    public bool Start()
    {
        Console.WriteLine();
        Console.WriteLine($"{monster.Name}과(와)의 전투 시작!");
        Console.WriteLine($"[체력: {monster.Health}][공격력: {monster.Attack}]");

        while (!player.IsDead && !monster.IsDead)
        {
            PlayerTurn();
            if (monster.IsDead) break;
            MonsterTurn();
        }

        if (player.IsDead)
        {
            Console.WriteLine("▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
            Console.WriteLine("████████▌▄▌▄▐▐▌█████████");
            Console.WriteLine("████████▌▄▌▄▐▐▌▀████████");
            Console.WriteLine("▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");

            Console.WriteLine("플레이어 패배! 게임 오버.");
            return true;
        }

        Console.WriteLine();
        Console.WriteLine("＿人人人人人＿");
        Console.WriteLine(">  승  리  <");
        Console.WriteLine("￣Y^Y^Y^Y￣");
        GrantRewards();
        return false;

    }

    private void PlayerTurn()
    {
        Console.WriteLine();
        Console.WriteLine($"{player.Name}의 턴 (공격하려면 Enter)");
        Console.ReadLine();
        Console.WriteLine($"{monster.Name}에게 {player.Attack} 데미지!");
        monster.TakeDamage(player.Attack);
        Thread.Sleep(1000);
    }

    private void MonsterTurn()
    {
        Console.WriteLine();
        Console.WriteLine($"{monster.Name}의 턴");
        Console.WriteLine($"{player.Name}에게 {monster.Attack} 피해");
        player.TakeDamage(monster.Attack);
        Thread.Sleep(1000);
    }

    private void GrantRewards()
    {
        Random random = new Random();
        int coins = (monster is Goblin) ? random.Next(10, 16) : random.Next(15, 31);

        inventory.AddCoins(coins);
        Console.WriteLine($"{coins} 코인 획득");

        Console.WriteLine("보상 아이템을 선택하세요:");
        for (int i = 0; i < rewards.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {rewards[i].Name}");
        }

        int choice = InputHelper.GetValidNumber("-입력-", 1, rewards.Count);

        // 선택한 아이템을 인벤토리에 추가
        Item selectedItem = rewards[choice - 1];
        inventory.AddItem(selectedItem);
    }
}