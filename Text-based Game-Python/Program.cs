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


List<string> requiredItems = new()
{
    "Boots of Haste", "Magicians Judge", "Helm of Larethian Wit", "Orbum Misticum", "Armor of Agathys", "Shield of the Morning"
};


List<string> inventory = new();





string currentRoom = "Atrium";


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

static void ShowStatus(string currentRoom, List<string> inventory, Dictionary<string, Dictionary<string, string>> rooms)
{
    Console.WriteLine(new string('*', 35));
    Console.WriteLine($"You are in the {currentRoom}");
    Console.WriteLine("Inventory: " + string.Join(", ", inventory));
    if (rooms[currentRoom].ContainsKey("Item") && !string.IsNullOrEmpty(rooms[currentRoom]["Item"]) && rooms[currentRoom]["Item"] != "no items")
    {
        string item = rooms[currentRoom]["Item"];
        Console.WriteLine($"You see the {item}");
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
    ShowStatus(currentRoom, inventory, rooms);
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
                string actualItem = rooms[currentRoom]["Item"];
                if (string.Equals(requestedItem, actualItem, StringComparison.OrdinalIgnoreCase))
                {
                    inventory.Add(actualItem);
                    rooms[currentRoom]["Item"] = "";
                    Console.WriteLine($"the {actualItem} is equipped, you feel its power coursing through you");
                }
                else
                {
                    Console.WriteLine("You rolled a Natural 1 for investigation, try again");
                    continue;
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
        bool hasAllItems = true;

        foreach (string needed in requiredItems)
        {
            if (!inventory.Contains(needed))
            {
                hasAllItems = false;
                break;
            }
        }

        if (!hasAllItems)
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
 * -item in room will depend on both success of dice roll, and type of dice roll
 */

