using System.Security.Cryptography;

Dictionary<string, Dictionary<string, string>> rooms = new()
{
    ["Atrium"] = new()
    {
        ["West"] = "Sacrificial Chamber", ["Item"] = "no items"
    },
    ["Sacrificial Chamber"] = new()
    {
        ["East"] = "Atrium", ["North"] = "Auditorium", ["Item"] = "Magicians Judge"
    },
    ["Auditorium"] = new()
    {
        ["South"] = "Sacrificial Chamber", ["West"] = "Vault", ["North"] = "Dungeons", ["East"] = "Antechamber", ["Item"] = "Helm of Larethian Wit"
    },
    ["Vault"] = new()
    {
        ["East"] = "Auditorium", ["Item"] = "Orbum Misticum"
    },
    ["Dungeons"] = new()
    {
        ["South"] = "Auditorium", ["East"] = "Torture Room", ["Item"] = "Armor of Agathys"
    },
    ["Torture Room"]  = new()
    {
        ["West"] = "Dungeons", ["Item"] = "Shield of the Morning"
    },
    ["Antechamber"] = new()
    {
        ["West"] = "Auditorium", ["North"] = "Crypt", ["Item"] = "Boots of Haste"
    },
    ["Crypt"] = new()
    {
        ["South"] = "Antechamber", ["Item"] = "Sorcerer"
    },
};


Dictionary<string, string> highTier = new()
{
    ["Magicians Judge"] = "Belt of Hill Giant Strength", ["Helm of Larethian Wit"] = "Dawnbringer",
    ["Orbum Misticum"] = "Efreeti Chain",
    ["Armor of Agathys"] = "Pyxis of Pandemonium", ["Shield of the Morning"] = "Shard of Solitaire",
    ["Boots of Haste"] = "Witchlight Watch"
};


List<string> possibleMagicItemsLowTier = new()
{
    "Boots of Haste", "Magicians Judge", "Helm of Larethian Wit", "Orbum Misticum", "Armor of Agathys", "Shield of the Morning"
};


List<string> possibleMagicItemsHighTier = new()
{
    "Belt of Hill Giant Strength", "DawnBringer", "Efreeti Chain", "Pyxis of Pandemonium", "Shard of Solitaire", "Witchlight Watch"
};

List<string> inventory = new();


string currentRoom = "Atrium";
int currentRoomRoll = 0;


int diceRoll()
{
    int d20 = RandomNumberGenerator.GetInt32(1, 21);
    bool isNatural20 = false;
    bool isNatural1 = false;
    

    if (d20 == 20)
    {
        isNatural20 = true;
        Console.WriteLine($"Your d20 roll is {d20}: Critical Success!");
        return d20;
    }
    else if (d20 == 1)
    {
        isNatural1 = true;
        Console.WriteLine($"Your d20 roll is {d20}: Critical Failure!");
        return d20;
    }
    else
    {
        Console.WriteLine($"Your d20 roll is {d20}");
        return d20;
    }
}




static void ShowInstructions()
{
    Console.WriteLine(new string('-', 68));
    Console.WriteLine("Dungeon Hero Text-Based Adventure Game");
    Console.WriteLine("Collect all 6 magical items to win the game or fall to the sorcerer");
    Console.WriteLine("Move commands: go North, go South, go East, go West");
    Console.WriteLine("Add to inventory: get \"item name\"");
    Console.WriteLine("To end game: \"quit\"");
    Console.WriteLine(new string('-', 68));
}

static void ShowStatus(string currentRoom, List<string> inventory, Dictionary<string, Dictionary<string, string>> rooms, Dictionary<string, string> highTier, int currentRoomRoll)
{
    Console.WriteLine(new string('*', 35));
    Console.WriteLine($"You are in the {currentRoom}");
    Console.WriteLine("Inventory: " + string.Join(", ", inventory));
    if (rooms[currentRoom].ContainsKey("Item") && !string.IsNullOrEmpty(rooms[currentRoom]["Item"]) && rooms[currentRoom]["Item"] != "no items")
    {
        string item = rooms[currentRoom]["Item"];
        highTier.TryGetValue(item, out string highTierItem);
        if (currentRoomRoll >= 15)
        {
            Console.WriteLine($"Your investigation was high! You see the {highTierItem}");
        }
        else
        {
            Console.WriteLine($"You see the {item}");
        }
    }
    else
    {
        Console.WriteLine("There are no items in this room");
    }

    Console.WriteLine(new string('*', 35));
}


while (true)
{
    ShowInstructions();
    ShowStatus(currentRoom, inventory, rooms, highTier, currentRoomRoll);
    Console.Write("What would you like to do?\n>");
    string? userAction = Console.ReadLine()?.Trim().ToLower();
    Console.WriteLine(userAction);
    if (userAction == null)
    {
        Console.WriteLine("Empty input, try again.");
        continue;
    }
    if (userAction == "quit")
    {
        Console.WriteLine("Thank you for playing");
        break;
    }
    else if (userAction.StartsWith("go "))
    {
        if (userAction.Length >= 3)
        {
            string sliced = userAction.Substring(3);
            string direction = char.ToUpper(sliced[0]) + sliced.Substring(1).ToLower();

            if (rooms[currentRoom].ContainsKey(direction))
            {
                currentRoomRoll = diceRoll();
                Console.WriteLine($"Your investigation check was {currentRoomRoll}");
                currentRoom = rooms[currentRoom][direction];
                Console.WriteLine($"You move to {currentRoom}");
            }
            else
            {
                Console.WriteLine("With a perception check that low you don't notice a door leading that way, try again");
                continue;
            }
        }
        else
        {
            Console.WriteLine("Invalid input, try again.");
            continue;
        }
        
    }
    else if (userAction.StartsWith("get "))
    {
        if (userAction.Length >= 4)
        {
            string requestedItem = userAction.Substring(4).Trim();

            if (rooms[currentRoom].ContainsKey("Item") && !string.IsNullOrEmpty(rooms[currentRoom]["Item"]) &&
                rooms[currentRoom]["Item"] != "no items")
            {
                string baseLineItem = rooms[currentRoom]["Item"];
                
                if (currentRoomRoll < 15)
                {
                    
                    if (string.Equals(requestedItem, baseLineItem, StringComparison.CurrentCultureIgnoreCase))
                    {

                        inventory.Add(baseLineItem);
                        rooms[currentRoom]["Item"] = "";
                        Console.WriteLine(
                            $"the {baseLineItem} is equipped, you feel a slight surge of power coursing through you");
                    }
                    else
                    {
                        Console.WriteLine("You didn't type the item correctly, try again");
                    }
                }
                else if (currentRoomRoll >= 15)
                {
                    highTier.TryGetValue(baseLineItem, out string highTierItem);
                    if (string.Equals(requestedItem, highTierItem, StringComparison.CurrentCultureIgnoreCase))
                    {
                        inventory.Add(highTierItem);
                        rooms[currentRoom]["Item"] = "";
                        Console.WriteLine(
                            $"the {highTierItem} is equipped, you feel an overwhelming rush of power coursing through you");
                        
                    }
                    else
                    {
                        Console.WriteLine("You didn't type the item correctly, try again");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("You rolled a Natural 1 for investigation, try again.");
                }
            }
            else
            {
                Console.WriteLine("You rolled a Natural 1 for investigation, try again");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid input, try again.");
        continue;
    }

    if (currentRoom == "Crypt")
    {
        int lowTierCount = 0;
        int highTierCount = 0;
        
        
        bool hasAllItems = true;

        foreach (string needed in inventory)
        {
            if (possibleMagicItemsLowTier.Contains(needed))
            {
                lowTierCount++;
                
            }
            else if (possibleMagicItemsHighTier.Contains(needed))
            {
                highTierCount++;
            }
        }

        if ((highTierCount < 4 && lowTierCount < 6))
        {
            Console.WriteLine("Here lies our intrepid adventurer, who thought they could slay Ferg-the-licious without all the help they could get");
            Console.WriteLine("Game Over");
            break;
        }
        else
        {
            Console.WriteLine("You see the evil sorcerer. He cowers before the might of your magical items and is easily defeated.");
            Console.WriteLine("Well done Hero, you have triumphed over a great evil this day");
            Console.WriteLine("Congratulations, You Win");
            break;
        }
    }
}



/*
 * Plans for udpates:
 * -cleaner printing to console
 * -equip items with get item not get "item name"
 * -add mini bosses in rooms with items
 * -either gives items a multiplier for attack or each mini boss would need a +
 *    smaller amount of items required to beat
 * -add and track hit points
 * -add health potions
 * -add more details for room desriptions
 * -add option for perception or investigation check with an associated 'dice roll'
 * -item in room will depend on both success of dice roll, and type of dice roll **implemented partially 07/31/26
 */

