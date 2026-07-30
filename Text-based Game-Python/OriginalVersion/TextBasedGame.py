def showinstructions():
    print("-"*68)
    print("Dungeon Hero Text Based Adventure Game")
    print("Collect all 6 magical items to win the game or fall to the Sorcerer")
    print("Move commands: go North, go South, go East, go West")
    print("Add to inventory: get “item name”")
    print("To end game: quit")
    print("-"*68)


def showstatus(currentRoom, inventory, rooms):
    print("*"*35)
    print(f"You are in the {currentRoom}")
    print("Inventory:", inventory)
    if "Item" in rooms[currentRoom] and rooms[currentRoom]["Item"]:
        print(f"You see {rooms[currentRoom]['Item']}")
        print("*"*35)
    else:
        print("There are no items in this room")
        print("*"*35)

inventory=[]

rooms = {
        "Atrium": {"West": "Sacrificial Chamber", "Item": "no items"},
        "Sacrificial Chamber": {"East": "Atrium", "North": "Auditorium", "Item": "The Magicians Judge"},
        "Auditorium": {"South": "Sacrificial Chamber", "West": "Vault", "North": "Dungeons", "East": "Antechamber",
                       "Item": "Helm of Larethian Wit"},
        "Vault": {"East": "Auditorium", "Item": "Orbum Misticum"},
        "Dungeons": {"South": "Auditorium", "East": "Torture Room", "Item": "Armor of Agathys"},
        "Torture Room": {"West": "Dungeons", "Item": "Shield of the Morning"},
        "Antechamber": {"West": "Auditorium", "North": "Crypt", "Item": "Boots of Haste"},
        "Crypt": {"South": "Antechamber", "Item": "Sorcerer"}
    }
required_items = ["Boots of Haste", "The Magicians Judge", "Helm of Larethian Wit", "Orbum Misticum",
                      "Armor of Agathys", "Shield of the Morning"]

currentRoom = "Atrium"


def main():







    global currentRoom
    print("\n")
    while True:

        showinstructions()
        print("\n")
        showstatus(currentRoom, inventory, rooms)
        print("\n")
        user_action = input("What would you like to do?").lower()
        print(user_action)




        print("\n")
        if user_action=="quit":
            print("Thank you for playing")
            break
        elif user_action.startswith("go "):
            direction = user_action[3: ].capitalize()
            if direction in rooms[currentRoom]:
                currentRoom = rooms[currentRoom][direction]
                print(f"You move to {currentRoom}")
                print("\n")
                #line50

            else:
                print("With a Perception check that low you don't notice a door leading that way, try agian")
                print("\n")

        elif user_action.startswith("get "):
            Item = user_action[4:]
            if "Item" in rooms[currentRoom] and rooms[currentRoom]["Item"].lower() == Item and Item != "none":
                inventory.append(Item)
                rooms[currentRoom]["Item"] = ""
                print(f"{Item} is equipped, you feel its power coursing through you")
                print("\n")
            else:
                print("You rolled a Natural 1 for Investigation, try again")
                print("\n")
        else:
            print("Invalid command, try something else ")
            print("\n")

        if currentRoom == "Crypt":
            not_enough_magic = []
            for item in required_items:
                if item.lower() not in [i.lower() for i in inventory]:
                    not_enough_magic.append(item)
            if not_enough_magic:
                print("Here lies our intrepid adventurer, who thought they could slay Ferg-the-Licious without all the help they could get")
                print("\n")
                print("Game Over")
                break

            if all(item.lower() in [i.lower() for i in inventory] for item in required_items):
                print("You see the evil sorcerer. He cowers before the might of your magical items and is easily defeated.")
                print("Well done Hero, you have triumphed over a great evil this day")
                print("\n")
                print("Congratulations, You Win")
                break
        #
            #
            #moved to 80-82




if __name__ == "__main__":
    main()
    print("\n")
    input("Press ENTER to exit the game")
    