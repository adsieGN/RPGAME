using System;
using System.IO;
using System.Xml;
using System.Threading;
using Faker;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RPGAME
{
    public enum DefWeaponsIndexes { AK_47, STECHKIN, DEAGLE, SNIPER_RIFLE, M4A4, AWP, USP_S, AXE, KNIFE };
    public enum DefWeaponsForces {
        AK_47 = 120,
        STECHKIN = 40,
        DEAGLE = 60,
        SNIPER_RIFLE = 140,
        M4A4 = 100,
        AWP = 560,
        USP_S = 40,
        AXE = 25,
        KNIFE = 15
    };
    public enum DefWeaponsPrices {
        AK_47 = 12700,
        STECHKIN = 6400,
        DEAGLE = 7300,
        SNIPER_RIFLE = 14500,
        M4A4 = 10200,
        AWP = 15600,
        USP_S = 5200,
        AXE = 2000,
        KNIFE = 1000
    };

    class Arena
    {
        Random rand1 = new Random();
        public int bet;

        public Arena()
        {
            Program.player.ConversionForce();
        }

        public void Breefing()
        {
            Console.Clear();
            Console.WriteLine("Какова сумма ставки?");
            bet = Convert.ToInt32(Console.ReadLine());

            if (Program.player.money >= bet)
            {
                Program.player.money -= bet;
                StartBattle();
            }
            else
            {
                Console.WriteLine("Подкопи денешек и подходи снова...");
                Thread.Sleep(2000);
                Console.Clear();
            }
        }

        public void StartBattle()
        {
            Person enemy1 = Person.Randomplayer("Боец", 2000);

            if (enemy1.strenght >= Program.player.strenght)
            {
                Console.WriteLine("Вы проиграли...");
                Program.player.Damage(Program.player.strenght - enemy1.strenght);
                Program.player.LevelUp(100);
                Thread.Sleep(2000);
                Console.Clear();
            }
            else if (enemy1.strenght < Program.player.strenght)
            {
                Program.player.Damage(rand1.Next(0, 100));
                Program.player.money += bet * 2;
                Console.WriteLine("Вы выйграли {0}", bet);
                Program.player.LevelUp(800);
                Thread.Sleep(2000);
                Console.Clear();
            }
        }
    }

    [Serializable]
    public class Shop
    {
        public Person seller = Person.Randomplayer("Продавец", 150000);
        public int Weaponscount = new Random().Next(20, 100);

        public Shop() { }

        public void Purchase(Entity weapon, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                if (Program.player.money >= weapon.price)
                {
                    Program.player.inventory.Add(weapon);
                    seller.money += weapon.price;
                    Program.player.money -= weapon.price;
                    Weaponscount--;

                    Console.WriteLine(i+1 + ". Покупка: Поздравляю с приобретением {0} за {1}, приходи ещё!", weapon.name, weapon.price);

                    Program.player.LevelUp(200);
                    Program.player.ConversionForce();
                }
                else
                {
                    Console.WriteLine("Подкопи денешек и приходи снова!");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
                }
            }

            Thread.Sleep(2000);
            Console.Clear();
        }

        public void Breefing()
        {
            Console.Clear();
            Console.WriteLine("Информация магазина:");
            Console.WriteLine("Продавец: " + seller.name);
            Console.WriteLine("Оружия в наличии {0} штук", Weaponscount);

            Thread.Sleep(3000);
            Console.Clear();
            Console.WriteLine("Что покупаете?");

            for (int i = 0; i < Entity.Weapons.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + Entity.Weapons[i].name + "(" + Entity.Weapons[i].price + ")");
            }

            int Wchoise = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Сколько штук?");

            int countChoise = Convert.ToInt32(Console.ReadLine());

            if(countChoise <= 0) { countChoise = 1; }

            Console.Clear();

            Purchase(Entity.Weapons[Wchoise - 1], countChoise);

        }
    }

    [Serializable]
    public class Entity
    {
        public int price, plusforce;
        public string name;

        public static Entity[] Weapons = {
            new Entity("AK_47", (int)DefWeaponsPrices.AK_47, (int)DefWeaponsForces.AK_47),
            new Entity("STECHKIN", (int)DefWeaponsPrices.STECHKIN, (int)DefWeaponsForces.STECHKIN),
            new Entity("DEAGLE", (int)DefWeaponsPrices.DEAGLE, (int)DefWeaponsForces.DEAGLE),
            new Entity("SNIPER_RIFLE", (int)DefWeaponsPrices.SNIPER_RIFLE, (int)DefWeaponsForces.SNIPER_RIFLE),
            new Entity("M4A4", (int)DefWeaponsPrices.M4A4, (int)DefWeaponsForces.M4A4),
            new Entity("AWP", (int)DefWeaponsPrices.AWP, (int)DefWeaponsForces.AWP),
            new Entity("USP_S", (int)DefWeaponsPrices.USP_S, (int)DefWeaponsForces.USP_S),
            new Entity("AXE", (int)DefWeaponsPrices.AXE, (int)DefWeaponsForces.AXE),
            new Entity("KNIFE", (int)DefWeaponsPrices.KNIFE, (int)DefWeaponsForces.KNIFE)
        };

        public Entity() { }
        public Entity(string _name, int _price, int force)
        {
            name = _name;
            price = _price;
            plusforce = force;
        }

        public override string ToString()
        {
            return name+" --- "+plusforce+" {DAMAGE}";
        }
    }

    [Serializable]
    public class Person
    {
        static Random rand1 = new Random();

        public int age, money, hp, force, level, exp, expToLevelUP, strenght;
        public string sex, job, name;
        internal List<Entity> inventory = new List<Entity>();

        public Person() { }
        public Person(int _age, int _money, string _sex, string _job, int _force = 20, int _level = 1)
        {
            var rand1 = new Random();

            hp = 1000; exp = 0; force = 20; expToLevelUP = (level * 6)*100+100;

            age = _age;
            sex = _sex;
            job = _job;
            money = _money;
            force = _force;
            level = _level;
            ConversionForce();
            name = Name.FullName();

        }

        public static Person Randomplayer(string job_ = "Рабочий", int money_ = 3500, bool withWeapon = true)
        {
            string sex_;

            switch (rand1.Next(1, 2))
            {
                case 1:
                    sex_ = "Мужской";
                    break;
                case 2:
                    sex_ = "Женский";
                    break;
                default:
                    sex_ = "Мужской";
                    break;
            }
            Person person = new Person(rand1.Next(16, 60), money_, sex_, job_, 20, rand1.Next(1, 5));

            if (withWeapon)
            {
                for(int i = 1; i <= rand1.Next(1, 5); i++)
                {
                    person.inventory.Add(Entity.Weapons[rand1.Next(0, Entity.Weapons.Length)]);
                }
            }
            person.ConversionForce();

            return person;
        }
        public static Person Randomplayer(int money_ = 3500, string job_ = "Рабочий", bool withWeapon = true)
        {
            Random rand1 = new Random();
            string sex_;

            switch (rand1.Next(1, 2))
            {
                case 1:
                    sex_ = "Мужской";
                    break;
                case 2:
                    sex_ = "Женский";
                    break;
                default:
                    sex_ = "Мужской";
                    break;
            }
            Person person = new Person(rand1.Next(16, 60), money_, sex_, job_, 20, rand1.Next(1, 5));

            if (withWeapon)
            {
                for (int i = 1; i <= rand1.Next(1, 5); i++)
                {
                    person.inventory.Add(Entity.Weapons[rand1.Next(0, Entity.Weapons.Length)]);
                }
            }
            person.ConversionForce();

            return person;
        }
        public void LevelUp(int sumexp)
        {
            exp += sumexp;

            if (exp > expToLevelUP)
            {
                level++;
                exp -= expToLevelUP;
                expToLevelUP = (level * 20) * 100;
                Console.Title = String.Format("{0}: HP - {1}, Strenght - {2}, Level - {3}, Exp - {4}/{5}, Money - {6}", name, hp, strenght, level, exp, expToLevelUP, money);
                Console.Beep();
            }
        }
        public int ConversionForce()
        {
            strenght = force+level*2;
            foreach (Entity ent in inventory)
            {
                strenght += ent.plusforce;
            }
            Console.Title = String.Format("{0}: HP - {1}, Strenght - {2}, Level - {3}, Exp - {4}/{5}, Money - {6}", name, hp, strenght, level, exp, expToLevelUP, money);
            return strenght;
        }
        public void Damage(int dmg)
        {
            hp += dmg;
            Console.Title = String.Format("{0}: HP - {1}, Strenght - {2}, Level - {3}, Exp - {4}/{5}, Money - {6}", name, hp, strenght, level, exp, expToLevelUP, money);

            if (hp <= 0)
            {
                Kill();
            }
        }
        public void Kill()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Вы были убиты. Через 3с игра выключится");
            Console.ResetColor();
            Thread.Sleep(3000);
            Environment.Exit(0);
        }
        public void ShowStats()
        {
            Console.Clear();
            Console.WriteLine("----------------------------STATS------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Имя: "+name);
            Console.WriteLine("Пол: "+sex);
            Console.WriteLine("Уровень: " + level);
            Console.WriteLine("EXP: " + exp);
            Console.WriteLine("Общая сила: "+strenght);
            Console.WriteLine("Работа: "+job);
            Console.WriteLine("Деньги: "+money);
            Console.WriteLine("Возраст: "+age);
            Console.WriteLine();
            Console.ResetColor();
        }
        public void ShowInventory()
        {
            Console.Clear();
            Console.WriteLine("---------------------------INVENTORY---------------------------");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (Entity weapon in inventory)
            {
                Console.WriteLine("\t\t\t" + weapon.ToString());
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    class Program
    {
        public static Arena arena;
        public static Person player;
        public static Shop shop;

        public static void NewGame()
        {
            Console.Clear();
            Console.WriteLine("Чтобы приступить к созданию персонажа нажмите клавишу...");
            Console.ReadLine();
            Console.WriteLine("Создание...");
            Console.ResetColor();


            string sex;
            int b = new Random().Next(1, 2);

            switch (b)
            {
                case 1:
                    sex = "Муской";
                    break;
                case 2:
                    sex = "Женский";
                    break;
                default:
                    sex = "Мужской";
                    break;
            }

            int age = new Random().Next(20, 60);
            string job = "Странник";

            player = new Person(age, 4000, sex, job);
            arena = new Arena();
            shop = new Shop();

            SaveGame();
        }

        public static void InitGame()
        {
            if (Directory.Exists(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg"))
            {
                foreach (string file in Directory.GetFiles(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg"))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg");
            }

            if (Directory.Exists(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\weapons"))
            {
                foreach (string file in Directory.GetFiles(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\weapons"))
                {
                    File.Delete(file);
                }
            }

            FileStream stream;

            foreach(Entity ent in Entity.Weapons)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Entity));
                stream = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\weapons\"+ent.name+".cfg", FileMode.OpenOrCreate);
                serializer.Serialize(stream, ent);
                stream.Close();
            }

            NewGame();
            Program.Start();
        }

        public static void SaveGame()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Person));
                FileStream writter = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\player.cfg", FileMode.OpenOrCreate);
                serializer.Serialize(writter, player);
                writter.Close();

                serializer = new XmlSerializer(typeof(List<Entity>));
                writter = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\inventory.cfg", FileMode.OpenOrCreate);
                serializer.Serialize(writter, player.inventory);
                writter.Close();

                serializer = new XmlSerializer(typeof(Shop));
                writter = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\shop.cfg", FileMode.OpenOrCreate); ;
                serializer.Serialize(writter, shop);
                writter.Close();
            }
            catch (Exception ex)
            {
                Console.Title = ex.Message;
            }

        }

        public static void LoadGame()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Person));
                FileStream stream = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\player.cfg", FileMode.OpenOrCreate);
                player = (Person)serializer.Deserialize(stream);
                stream.Close();

                serializer = new XmlSerializer(typeof(List<Entity>));
                stream = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\inventory.cfg", FileMode.OpenOrCreate);
                player.inventory = (List<Entity>)serializer.Deserialize(stream);
                stream.Close();

                serializer = new XmlSerializer(typeof(Shop));
                stream = new FileStream(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\shop.cfg", FileMode.OpenOrCreate);
                shop = (Shop)serializer.Deserialize(stream);
                stream.Close();

                string[] WeaponFiles = Directory.GetFiles(@"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg\weapons");

                for(int i = 0; i < WeaponFiles.Length; i++)
                {
                    XmlSerializer _serializer = new XmlSerializer(typeof(Entity));
                    FileStream _stream = new FileStream(WeaponFiles[i], FileMode.Open);

                    Entity.Weapons.SetValue(_serializer.Deserialize(_stream), i);
                    _stream.Close();
                }

                arena = new Arena();
            }
            catch (Exception ex)
            {
                Console.Title = ex.Message;
            }
        }

        public static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("-----------------------Выберите действие-----------------------");
            Console.WriteLine("1. Инвентарь");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Магазин");
            Console.WriteLine("4. Арена\n");
            Console.WriteLine("5. Сохранить игру");
            Console.WriteLine("6. Начать новую игру");
            Console.ResetColor();

            try
            {
                int i = Convert.ToInt32(Console.ReadLine());

                switch (i)
                {
                    case 1:
                        player.ShowInventory();
                        Menu();
                        break;
                    case 2:
                        player.ShowStats();
                        Menu();
                        break;
                    case 3:
                        shop.Breefing();
                        Menu();
                        break;
                    case 4:
                        arena.Breefing();
                        Menu();
                        break;
                    case 5:
                        SaveGame();
                        Console.Clear();
                        Menu();
                        break;
                    case 6:
                        InitGame();
                        break;
                    default:
                        Menu();
                        break;
                }
            }
            catch (Exception) { Console.Clear(); Menu(); }
        }

        public static void Start()
        {
            Random rand1 = new Random();

            string cfgDir = @"C:\Users\ADSie\source\repos\RPGAME\RPGAME\bin\Debug\cfg";

            if (Directory.Exists(cfgDir) && File.Exists(cfgDir + @"\player.cfg") && File.Exists(cfgDir + @"\shop.cfg") && File.Exists(cfgDir + @"\inventory.cfg") && Directory.GetFiles(cfgDir + @"\weapons").Length > 3)
            {
                LoadGame();
            }
            else
            {
                InitGame();
            }
            Thread.Sleep(2000);

            Console.Title = String.Format("{0}: HP - {1}, Strenght - {2}, Level - {3}, Exp - {4}/{5}, Money - {6}", player.name, player.hp, player.strenght, player.level, player.exp, player.expToLevelUP, player.money);
            Console.Clear();
            Menu();
        }

        static void Main(string[] args)
        {
            Start();
        }
    }
}
