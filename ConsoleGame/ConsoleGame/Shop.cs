using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame
{
    public class Shop
    {
        public static void ShowShop(Character player)
        {
            while (true)
            {
                DisplayShopMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowItemsForSale(player, Item.ItemType.Weapon);
                        break;
                    case "2":
                        ShowItemsForSale(player, Item.ItemType.Armor);
                        break;
                    case "3":
                        ShowConsumables();            // 소모품 목록 출력
                        BuyConsumable(player);      // 소모품 구매
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                Console.WriteLine("아무 키나 누르면 계속...");
                Console.ReadKey();
            }
        }

        private static void DisplayShopMenu()
        {
            Console.Clear();
            Console.WriteLine("===== 상점 메뉴 =====");
            Console.WriteLine("1. 무기 상점");
            Console.WriteLine("2. 방어구 상점");
            Console.WriteLine("3. 소모품 상점");
            Console.WriteLine("6. 상점 나가기");
            Console.WriteLine("===================");
            Console.Write("원하시는 행동을 입력해주세요: ");
        }

        private static void ShowItemsForSale(Character player, Item.ItemType itemType)
        {
            List<Item> itemsForSale = ItemManager.ItemInfos
                                                .Where(item => item.Type == itemType && !item.IsBound)
                                                .ToList();

            ShowItems(player, itemType, itemsForSale);
        }

        private static void ShowItems(Character player, Item.ItemType itemType, List<Item> itemsForSale)
        {
            string itemTypeString = itemType switch
            {
                Item.ItemType.Weapon => "Weapon",
                Item.ItemType.Armor => "Armor",
                Item.ItemType.Potion => "Potion",
                Item.ItemType.Scroll => "Scroll",
                Item.ItemType.Consumable => "Consumable",
                Item.ItemType.Defense => "Defense"
            };

            Console.WriteLine($"상점 - {itemTypeString} 상점");
            Console.WriteLine();

            // 보유한 골드 출력
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();

            // 상점 아이템 목록 출력
            Console.WriteLine("[아이템 목록]");

            int index = 1;
            foreach (var item in itemsForSale)
            {
                Console.WriteLine($"- {index}. {item.Name} : {item.Price} G");
                index++;
            }

            Console.WriteLine();

            // 아이템 구매 및 판매 기능
            ShowItemActions(player, itemType);
        }

        private static void ShowItemActions(Character player, Item.ItemType itemType)
        {
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 선택해주세요.\n>> ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    BuyItem(player, itemType);
                    break;
                case "2":
                    SellItem(player);
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }

        private static void BuyItem(Character player, Item.ItemType itemType)
        {
            Console.WriteLine("구매할 아이템 번호를 선택해주세요. (취소: 0)");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice) || choice < 0)
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            if (choice == 0)
            {
                Console.WriteLine("구매가 취소되었습니다.");
                return;
            }

            var itemsToBuy = ItemManager.ItemInfos.Where(i => i.Type == itemType && !i.Purchased).ToList();

            if (choice > itemsToBuy.Count || choice < 1)
            {
                Console.WriteLine("잘못된 선택입니다.");
                return;
            }

            var selectedToBuy = itemsToBuy[choice - 1];

            if (player.Gold < selectedToBuy.Price)
            {
                Console.WriteLine("골드가 부족합니다.");
                return;
            }

            player.Gold -= selectedToBuy.Price;
            ItemManager.UpdateItemPurchasedStatus(selectedToBuy);

            // 인벤토리에 아이템 추가
            player.InventoryManager.AddItem(new Item(selectedToBuy.Name, selectedToBuy.Type, selectedToBuy.Price, selectedToBuy.StatBonus, selectedToBuy.Description, false));

            Console.WriteLine($"{selectedToBuy.Name}을(를) 구매했습니다.");
        }


        private static void SellItem(Character player)
        {
            var inventoryManager = player.InventoryManager;

            if (inventoryManager.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                return;
            }

            Console.WriteLine("판매할 아이템을 선택하세요. (취소: 0)");

            int index = 1;
            foreach (var item in inventoryManager.Inventory)
            {
                Console.WriteLine($"{index}. {item.Name} ({item.Type}) : {item.Price} G");
                index++;
            }

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > index - 1)
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            if (choice == 0)
            {
                Console.WriteLine("판매가 취소되었습니다.");
                return;
            }

            var selectedItem = inventoryManager.Inventory[choice - 1];

            if (selectedItem.Equipped)
            {
                Console.WriteLine($"{selectedItem.Name}은(는) 현재 장착 중입니다. 장착을 해제하고 판매하시겠습니까? (Y/N)");
                string confirm = Console.ReadLine().ToUpper();

                if (confirm != "Y")
                {
                    Console.WriteLine("판매가 취소되었습니다.");
                    return;
                }

                player.InventoryManager.UnequipItem(choice);
            }

            int sellPrice = (int)(selectedItem.Price * 0.85);
            player.Gold += sellPrice;
            inventoryManager.RemoveItem(selectedItem);

            Console.WriteLine($"{selectedItem.Name}을(를) {sellPrice} G에 판매했습니다.");
        }

        private static void ShowConsumables()
        {
            Console.WriteLine("[소모품 목록]");

            int index = 1;
            foreach (var item in ItemManager.ItemInfos.Where(item => item.Type == Item.ItemType.Consumable && !item.Purchased))
            {
                Console.WriteLine($"- {index}. {item.Name} : {item.Price} G");
                index++;
            }

            if (index == 1)
            {
                Console.WriteLine("소모품이 없습니다.");
            }

            Console.WriteLine();
        }

        private static void BuyConsumable(Character player)
        {
            Console.Write("구매할 아이템 번호를 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex))
            {
                var consumables = ItemManager.ItemInfos.Where(item => item.Type == Item.ItemType.Consumable && !item.Purchased).ToList();

                if (itemIndex >= 1 && itemIndex <= consumables.Count)
                {
                    var selectedConsumable = consumables[itemIndex - 1];

                    Console.Write($"구매할 {selectedConsumable.Name}의 수량을 입력하세요: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        int totalPrice = selectedConsumable.Price * quantity;

                        if (player.Gold >= totalPrice)
                        {
                            player.Gold -= totalPrice;
                            ItemManager.UpdateItemPurchasedStatus(selectedConsumable);

                            for (int i = 0; i < quantity; i++)
                            {
                                player.InventoryManager.AddItem(new Item(selectedConsumable.Name, selectedConsumable.Type, selectedConsumable.Price, selectedConsumable.StatBonus, selectedConsumable.Description, false));
                            }

                            Console.WriteLine($"{selectedConsumable.Name} {quantity}개를 구매했습니다.");
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 수량입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 아이템 번호입니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

    }
}