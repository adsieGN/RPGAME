using System;
using System.Threading;
using Faker;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGAME
{
    public enum WeaponsIndexes { AK_47, STECHKIN, DEAGLE, SNIPER_RIFLE, M4A4, AWP, USP_S, AXE, KNIFE };
    public enum WeaponsForces {
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
    public enum WeaponsPrices {
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
        public Person player;

        public Arena(Person _player)
        {
            player = _player;
            player.ConversionForce();
        }

        public void Breefing()
        {
            Console.Clear();
            Console.WriteLine("Какова сумма ставки?");
            bet = Convert.ToInt32(Console.ReadLine());

            if (player.money >= bet)
            {
                player.money -= bet;

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
            Person enemy1 = Person.RandomPerson("Боец", 2000);

            if(enemy1.strenght >= player.strenght)
            {
                Console.WriteLine("Вы проиграли...");
                player.Damage(-(rand1.Next(20, 100)));
                player.LevelUp(100);
                Thread.Sleep(2000);
                Console.Clear();
            }
            else if(enemy1.strenght < player.strenght)
            {
                player.Damage(-3);
                player.money += bet * 2;
                Console.WriteLine("Вы выйграли {0}", bet*2);
                player.LevelUp(800);
                Thread.Sleep(2000);
                Console.Clear();
            }
        }
    }

    static class Shop
    {
        static public Person seller = Person.RandomPerson("Продавец", 150000);
        static int Weaponscount = new Random().Next(20, 100);

        static public void Purchase(Person player, Entity weapon)
        {
            Console.Clear();

            if (player.money >= weapon.price)
            {
                player.inventory.Add(weapon);
                seller.money += weapon.price;
                player.money -= weapon.price;
                Weaponscount--;

                Console.Clear();
                Console.WriteLine("Поздравляю с покупкой {0} за {1}, приходи ещё!", weapon.name, weapon.price);
                player.LevelUp(200);
                player.ConversionForce();
                Thread.Sleep(2000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Подкопи денешек и приходи снова!");
                Thread.Sleep(2000);
                Console.Clear();
            }
        }

        static public void Breefing(Person player)
        {
            Console.Clear();
            Console.WriteLine("Информация магазина:");
            Console.WriteLine("Продавец: "+seller.name);
            Console.WriteLine("Оружия в наличии {0} штук", Weaponscount);

            Thread.Sleep(3000);
            Console.Clear();
            Console.WriteLine("Что покупаете?");

            for (int i = 0; i < Entity.Weapons.Length; i++)
            {
                Console.WriteLine((i+1) + ". " + Entity.Weapons[i].name + "(" + Entity.Weapons[i].price + ")");
            }

            int choise = Convert.ToInt32(Console.ReadLine());

            Purchase(player, Entity.Weapons[choise - 1]);
        }
    }

    class Entity
    {
        public int price, plusforce;
        public string name;

        public static Entity[] Weapons = {
            new Entity("AK_47", (int)WeaponsPrices.AK_47, (int)WeaponsForces.AK_47),
            new Entity("STECHKIN", (int)WeaponsPrices.STECHKIN, (int)WeaponsForces.STECHKIN),
            new Entity("DEAGLE", (int)WeaponsPrices.DEAGLE, (int)WeaponsForces.DEAGLE),
            new Entity("SNIPER_RIFLE", (int)WeaponsPrices.SNIPER_RIFLE, (int)WeaponsForces.SNIPER_RIFLE),
            new Entity("M4A4", (int)WeaponsPrices.M4A4, (int)WeaponsForces.M4A4),
            new Entity("AWP", (int)WeaponsPrices.AWP, (int)WeaponsForces.AWP),
            new Entity("USP_S", (int)WeaponsPrices.USP_S, (int)WeaponsForces.USP_S),
            new Entity("AXE", (int)WeaponsPrices.AXE, (int)WeaponsForces.AXE),
            new Entity("KNIFE", (int)WeaponsPrices.KNIFE, (int)WeaponsForces.KNIFE)
        };

        public Entity(string _name, int _price, int force)
        {
            name = _name;
            price = _price;
            plusforce = force;
        }

        public override string ToString()
        {
            return name+" --- "+price;
        }
    }

    class Person
    {
        static Random rand1 = new Random();
        private List<string> names = new List<string>();

        public int age, money, hp, force, level, exp, expToLevelUP, strenght;
        public string sex, job, name;
        public List<Entity> inventory = new List<Entity>();

        public Person(int _age, int _money, string _sex, string _job, int _force = 20, int _level = 1)
        {
            var rand1 = new Random();

            hp = 100; exp = 0; force = 20; expToLevelUP = (level * 6)*100;

            age = _age;
            sex = _sex;
            job = _job;
            money = _money;
            force = _force;
            level = _level;
            ConversionForce();

            for(int i = 1; i <= 20; i++)
            {
                names.Add(Name.FullName());
            }

            name = names[rand1.Next(0, names.Count-1)];

        }

        public static Person RandomPerson(string job_ = "Рабочий", int money_ = 3500)
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
            return new Person(rand1.Next(16, 60), money_, sex_, job_, rand1.Next(1, 120));
        }
        public static Person RandomPerson(int money_ = 3500, string job_ = "Рабочий")
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
            return new Person(rand1.Next(16, 60), money_, sex_, job_);
        }
        public void LevelUp(int sumexp)
        {
            exp += sumexp;

            if (exp > expToLevelUP)
            {
                level++;
                exp -= expToLevelUP;
                expToLevelUP = (level * 6) * 100;
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
            foreach (Entity str in inventory)
            {
                Console.WriteLine("\t\t\t" + str.ToString());
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    class Program
    {
        public static void Menu(Person user)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("-----------------------Выберите действие-----------------------");
            Console.WriteLine("1. Инвентарь");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Магазин");
            Console.WriteLine("4. Арена\n");
            Console.ResetColor();

            try
            {
                int i = Convert.ToInt32(Console.ReadLine());

                Arena arena = new Arena(user);

                switch (i)
                {
                    case 1:
                        user.ShowInventory();
                        Menu(user);
                        break;
                    case 2:
                        user.ShowStats();
                        Menu(user);
                        break;
                    case 3:
                        Shop.Breefing(user);
                        Menu(user);
                        break;
                    case 4:
                        arena.Breefing();
                        Menu(user);
                        break;
                    default:
                        Menu(user);
                        break;
                }
            }
            catch (Exception) { Console.Clear(); Menu(user); }
        }

        static void Main(string[] args)
        {
            Random rand1 = new Random();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Чтобы приступить к созданию персонажа нажмите клавишу...");
            Console.ReadLine();
            Console.WriteLine("Создание...");
            Console.ResetColor();

            string sex;
            int a = rand1.Next(1, 2);

            switch (a)
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

            int age = rand1.Next(20, 60);
            string job = "Странник";

            var user = new Person(age, 3500, sex, job);
            Thread.Sleep(2000);
            Console.Title = String.Format("{0}: HP - {1}, Strenght - {2}, Level - {3}, Exp - {4}/{5}, Money - {6}", user.name, user.hp, user.strenght, user.level, user.exp, user.expToLevelUP, user.money);

            Console.Clear();
            Menu(user);
        }
    }
}
