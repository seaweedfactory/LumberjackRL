# LumberjackRL
A Lumberjackesque Roguelike originally created for the 2011 Anual Roguelike Release Party

# The Story So Far
Your brother is missing and you need to find him! Explore the area to find him, while avoiding monsters and starvation yourself.

# Installation
1. Get the most recent release from http://github.com/seaweedfactory/LumberjackRL/releases.
2. You must have the Java Runtime Environment installed to run this program. You can get it at http://java.com/.
3. Unzip the file to it's own folder. A .JAR file, this guide, and a folder full of graphics should appear.
4. Run the following command:

java -jar LumberjackRLv0.6.26.jar

Or double-click on the file if it is properly associated.
5. That's it, the program will only produce files in this folder.

# Starting a New Game
Run the program and press the n key to create a new game world. It is produced randomly for each new game and this procedure can take some time.
Be patient and give it a chance. The help screen will appear when it is complete. This screen lists all the key commands in the game.

# Key Controls
The a,w,s,and d keys move through menus and on the map

SPACE selects items, actions, and places

h key to get a list of commands.

x cancels or exits screens

e equips or unequips items

p picks up items on the map, or drops items from the inventory

i shows the inventory, where items can be equipped and unequipped

c show the character screen, where points can be distributed

m shows the past two dozen messages

1,2,3 and 4 use items equipped to slots

v saves the game (this process can take a short while)

l loads the saved game (this process can take a short while)

z allows the use of extended actions like activating, talking, looking, sleeping and trading


# Concepts and Hints
Use the extended command Activate to open and close doors.

The world is fully changeable. Try different tools on different types of land and objects.

For example, a shovel can dig grass patches and change them to dirt.

Ash can be used to turn dirt into fertile land.

Pumpkin seeds can be planted in fertile land.

Pickaxes dig through rock and can mine minerals.

Sleep by moving over a tent or bed and using the extended command Sleep.

Sleeping takes a while, as the simulation continues while you sleep. The program will appear frozen, but it is running.

Sleeping heals life slowly. Beds sleep for 4 hours, tents sleep for 2.

Things that grow only grow when you are in the same area. Try planting something and sleeping over night in the same area.

Crops generally grow in fertile land. Pumpkins grow with a spreading vine.

Water is fully dynamic and will flow toward areas of lower density.

Drains can be created with a pick, springs with a shovel.

Water likes to move in streambeds best, create them by digging away dirt to make stone, then use a pickaxe.

Floodgates can be opened and closed by activating them.

Fire can be created with the lantern. Fire spreads, so be careful. Different materials fuel fire better.

Animals and monsters are no very clever right now. Attack them by bumping into them.

Ghosts appear from open graves. Keep this in mind if you are grave robbing.

Chefs only sell food during the day.

Caves are not always connected; you may have to use the pickaxe to make a path.

Caves will let you know when you have reached the bottom level. Otherwise, there is always a stair down.

Your brother is out there, probably in a cave, and not much happens when you find him.

# About .REGION Files
The program uses files with a .region extension to save data about the game world. At present, these files are not properly removed when creating new games. The program will run just fine, but you may accumulate an excess of unused .region files as you play more games. Simply close the program and delete all the .region files. They will be recreated from the save_game file when it is loaded.
