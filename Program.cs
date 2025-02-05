namespace SPATextRPG;
using System.Collections.Generic;
using System.Threading;
using System.Text.Json;
using System.IO;
using System;

public static class SaveLoadManager
{
    private const string SaveFilePath = "savegame.json";

    public static void SaveGame(MyChar player, Inventory inventory)
    {
        var data = new GameData
        {
            PlayerName = player.Name,
            PlayerRole = player.Role,
            Health = player.Health,
            Attack = player.Attack,
            Defense = player.Defense,
            Coins = inventory.Coins,
            InventoryItems = new Dictionary<string, int>()
        };

        foreach (var item in inventory.GetItems())
        {
            data.InventoryItems[item.Key.Name] = item.Value;
        }

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SaveFilePath, json);

        Console.WriteLine("게임이 저장되었습니다!");
    }

    // ✅ LoadGame을 클래스 내부로 옮김
    public static (MyChar, Inventory) LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Console.WriteLine("저장된 게임이 없습니다.");
            return (null, null);
        }

        string json = File.ReadAllText(SaveFilePath);
        var data = JsonSerializer.Deserialize<GameData>(json);

        MyChar player = data.PlayerRole switch
        {
            "전사" => new Warrior(data.PlayerName),
            "기사" => new Knight(data.PlayerName),
            "탱커" => new Defenser(data.PlayerName),
            _ => null
        };

        player.Heal(data.Health - player.Health);
        player.AttackPower(data.Attack - player.Attack);
        player.IncreaseDefense(data.Defense - player.Defense);

        Inventory inventory = new Inventory(data.Coins);
        foreach (var item in data.InventoryItems)
        {
            Item newItem = item.Key switch
            {
                "체력 물약" => new HealthPotion(),
                "공격력 물약" => new StrengthPotion(),
                _ => null
            };

            for (int i = 0; i < item.Value; i++)
                inventory.AddItem(newItem);
        }

        Console.WriteLine("게임을 불러왔습니다");
        return (player, inventory);
    }
}

public class GameData // 플레이어 정보
{
    public string PlayerName { get; set; }
    public string PlayerRole { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Coins { get; set; }
    public Dictionary<string, int> InventoryItems { get; set; }
}


public interface MyChar // MyChar 인터페이스
{
    string Name { get; }
    string Role { get; }
    int Health { get; }
    int Attack { get; }
    int Defense { get; }
    bool IsDead { get; }
    void TakeDamage(int damage);
    void Heal(int amount);
    void AttackPower(int amount);
    void IncreaseDefense(int amount);
}

// MyChar를 상속한 Character 클래스
public abstract class Character : MyChar
{
    public string Name { get; protected set; }
    public string Role { get; protected set; }
    public int Health { get; protected set; }
    public int Attack { get; protected set; }
    public int Defense { get; protected set; }
    public bool IsDead => Health <= 0;

    protected Character(string name, string role, int health, int attack, int defense)
    {
        Name = name;
        Role = role;
        Health = health;
        Attack = attack;
        Defense = defense;
    }

    public void TakeDamage(int damage) // 피해를 입었을 때
    {
        int actualDamage = Math.Max(0, damage - Defense);
        Health = Math.Max(0, Health - actualDamage);
        Console.WriteLine($"{Name}이(가) {actualDamage}의 피해를 입었습니다.");
        Console.WriteLine($"남은 체력: {Health}");
    }

    public void Heal(int amount) // 회복을 했을 때
    {
        Health += amount;
        Console.WriteLine();
        Console.WriteLine($"{Name}의 체력이 {amount}만큼 회복되었습니다. 현재 체력: {Health}");
    }

    public void AttackPower(int amount) // 공격력 증가
    {
        Attack += amount;
        Console.WriteLine();
        Console.WriteLine($"{Name}의 공격력이 {amount}만큼 증가했습니다. 현재 공격력: {Attack}");
    }
    public void IncreaseDefense(int amount) // 방어력 증가
    {
        Defense += amount;
        Console.WriteLine();
        Console.WriteLine($"{Name}의 방어력이 {amount}만큼 증가했습니다. 현재 방어력: {Defense}");
    }
}

// Character를 상속한 직업 클래스
public class Warrior : Character
{
    public Warrior(string name) : base(name, "전사", 100, 20, 10) { }
}

public class Knight : Character
{
    public Knight(string name) : base(name, "기사", 120, 15, 10) { }
}

public class Defenser : Character
{
    public Defenser(string name) : base(name, "탱커", 150, 10, 20) { }
}

// MyChar 상속한 Monster 클래스 
public abstract class Monster : MyChar
{
    public string Name { get; protected set; }
    public string Role { get; protected set; }
    public int Health { get; protected set; }
    public int Attack { get; protected set; }
    public int Defense { get; protected set; }
    public bool IsDead => Health <= 0;

    public void TakeDamage(int damage)
    {
        Health = Math.Max(0, Health - damage);
        Console.WriteLine($"{Name}이(가) {damage}의 피해를 입었습니다. 남은 체력: {Health}");
    }

    public void Heal(int amount) { } // 몬스터 회복 불가
    public void AttackPower(int amount) { } // 몬스터 공격력 강화 불가 (추후에 시도 가능)
    public void IncreaseDefense(int amount) { } // 몬스터 방어력 강화 불가 (추후에 시도 가능)
}

// Moster를 상속
public class Goblin : Monster
{
    public Goblin() { Name = "고블린"; Health = 100; Attack = 20; }
}

public class Oak : Monster
{
    public Oak() { Name = "오크"; Health = 200; Attack = 40; }
}

public class Dragon : Monster
{
    public Dragon() { Name = "드래곤"; Health = 1000; Attack = 70; }
}

// Item 인터페이스
public interface Item
{
    string Name { get; }
    int Price { get; }
    int AttackBonus { get; }
    int DefenseBonus { get; }
    void Use(MyChar character);
    bool IsEquipped { get; set; }
}

// Item을 상속한 물약 클래스
public class HealthPotion : Item
{
    public string Name => "체력 물약";
    public int Price => 50;
    public int AttackBonus => 0;
    public int DefenseBonus => 0;
    public bool IsEquipped { get; set; } = false;
    public void Use(MyChar character) => character.Heal(20);
}

public class StrengthPotion : Item
{
    public string Name => "공격력 물약";
    public int Price => 50;
    public int AttackBonus => 0;
    public int DefenseBonus => 0;
    public bool IsEquipped { get; set; } = false;
    public void Use(MyChar character) => character.AttackPower(10);
}

// Itemdmf 상송한 장비 클래스
public abstract class Equipment : Item
{
    public string Name { get; protected set; }
    public int Price { get; protected set; }
    public int AttackBonus { get; protected set; }
    public int DefenseBonus { get; protected set; }
    public bool IsEquipped { get; set; } = false;

    public virtual void Use(MyChar character)
    {
        if (IsEquipped)
        {
            character.AttackPower(-AttackBonus);
            character.IncreaseDefense(-DefenseBonus);
            Console.WriteLine($"{Name}을(를) 해제했습니다.");
        }
        else
        {
            character.AttackPower(AttackBonus);
            character.IncreaseDefense(DefenseBonus);
        }
        IsEquipped = !IsEquipped;
    }
}

public class Dagger : Equipment
{
    public Dagger() { Name = "단검 \t\t cxxx|;:;:;:;:;:;>"; Price = 200; AttackBonus = 5; DefenseBonus = 0; }
}

public class IronSword : Equipment
{
    public IronSword() { Name = "투박한 철검 \t\t @xxxx[{:::::::::::::::::::>"; Price = 230; AttackBonus = 10; DefenseBonus = 0; }
}

public class LongSword : Equipment
{
    public LongSword() { Name = "장검 \t\t o()xxxx[{:::::::::::::::::::::>"; Price = 500; AttackBonus = 20; DefenseBonus = 0; }
}

public class Excalibur : Equipment
{
    public Excalibur() { Name = "엑스칼리버 \t\t ס₪₪₪₪§|(Ξ≥≤≥≤≥≤ΞΞΞΞΞΞΞΞΞΞΞΞΞΞΞΞ>"; Price = 1000; AttackBonus = 50; DefenseBonus = 10; }
}

public class Shield : Equipment
{
    public Shield() { Name = "방패 \t\t "; Price = 450; AttackBonus = 0; DefenseBonus = 10; }
}

public class Armor : Equipment
{
    public Armor() { Name = "갑옷"; Price = 500; AttackBonus = 0; DefenseBonus = 15; }
}

// 모든 입력을 받는 Input 클래스
public static class InputHelper
{
    // 숫자 입력
    public static int GetValidNumber(string prompt, int min, int max)
    {
        int choice;
        while (true)
        {
            Console.WriteLine(prompt);
            Console.Write(">> ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
            {
                return choice;
            }
            Console.WriteLine("잘못된 입력입니다");
        }
    }

    // 문자열 입력
    public static string GetValidString(string prompt)
    {
        Console.WriteLine(prompt);
        Console.Write(">> ");
        string input = Console.ReadLine()?.Trim();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("잘못된 입력입니다");
            Console.Write(">> ");
            input = Console.ReadLine()?.Trim();
        }
        return input;
    }
}

// Main
class Program
{
    static void Main(string[] args)
    {
        string playerName = InputHelper.GetValidString("-플레이어 이름을 입력하세요-");

        MyChar player = null;
        while (player == null)
        {
            Console.WriteLine();
            Console.WriteLine("-직업 선택-");
            Thread.Sleep(500);
            Console.WriteLine("1. 𓀛 전사 [체력: 100][공격력: 20][방어력: 10]");
            Thread.Sleep(50);
            Console.WriteLine("2. 𓀧 기사 [체력: 120][공격력: 15][방어력: 10]");
            Thread.Sleep(50);
            Console.WriteLine("3. 𓀨 탱커 [체력: 150][공격력: 10][방어력: 20]");
            Thread.Sleep(50);

            int choice = InputHelper.GetValidNumber("-입력-", 1, 3);
            player = choice switch
            {
                1 => new Warrior(playerName),
                2 => new Knight(playerName),
                3 => new Defenser(playerName),
                _ => null
            };
        }
        Thread.Sleep(1000);
        Console.WriteLine("✨:*、✨");
        Thread.Sleep(500);
        Console.WriteLine("  .'*");
        Thread.Sleep(50);
        Console.WriteLine("   \"。");
        Thread.Sleep(50);
        Console.WriteLine("    :、");
        Thread.Sleep(50);
        Console.WriteLine("     *\"+");
        Thread.Sleep(50);
        Console.WriteLine("      ':、");
        Thread.Sleep(50);
        Console.WriteLine("       ＼");
        Thread.Sleep(50);
        Console.WriteLine("       ˗ˋˏዽˎˊ˗");
        Thread.Sleep(1500);

        Console.WriteLine($"{player.Name}이(가) 생성되었습니다.");
        Thread.Sleep(1500);

        new Village(player, new Inventory(1000)).EnterVillage();
    }
}