<p align="center">
  <img src="https://user-images.githubusercontent.com/6089807/109901215-4a48ff00-7c5e-11eb-8895-14f9f4628b35.png">
</p>

Changelog:
v1.2
- Change Spirit Titles shown in UI
> Instructions: Add msg_spirits to preload
  - msg_spirits.msbt
    >@\ui\message\
v1.1
- Add config option for minimum randomizer timer
- Add config option for fields to be excluded from Randomization
   

## Smash Ultimate Randomizer and Spirit Battle Editor

### Prerequisites:

  - Hacked Switch preconfigured for playing modded Super Smash Bros Ultimate
  - [.Net Core 3.1 Runtime installed](https://dotnet.microsoft.com/download/dotnet/3.1/runtime)
  - Ability to extract files from data.arc:  
    >Details on extracting files can be found here: https://gamebanana.com/tuts/12827)
  
__________________________________
  
### Requirements:

  - ui_spirits_battle_db.prc 
    >(Can be ripped from data.arc @ \ui\param\database\)
  
__________________________________

### Optional (but recommended):

- [Arcadia](https://github.com/Coolsonickirby/ARCadia)
  >(For drag and drop support toggling between Randomizers)

##### Extract files from data.arc and place in "/PreLoad" folder

  - ui_fighter_spirit_aw_db.prc
    >@\ui\param\database\  
  - spirits_battle_event.prc
    >@\ui\param\spirits\  
  - msg_spirits.msbt
    >@\ui\message\
  
__________________________________

### Set Up:

  - Place **ui_spirits_battle_db.prc** in the root of the application (".\YesWeDo\)  
  - Run **YesWeDo.exe**
__________________________________
  
### Usage:

### Randomizer
  
  ![image](https://user-images.githubusercontent.com/6089807/109874328-94b58600-7c34-11eb-8cf6-663e6702b890.png)
    
**Tools -> Randomize All**
  
  This will randomize all spirit battles using the seed provided (or a random seed if none provided) and create multiple randomizers rooted in the same seed.  
  
  ![image](https://user-images.githubusercontent.com/6089807/109873955-1527b700-7c34-11eb-91f9-0bcd0bdc4937.png)
  
  These results can be dragged into your Arcadia mod folder (most likely /ultimate/mods) and enabled/disabled with Arcadia.
  
##  Disclaimer: If due to the nature of Random, you run into a battle that cannot be beaten which halts your progress, switch to one of the other Randomizers from the same seed.  Fighter Unlocks should be randomized in the same order, and you should hopefully be able to beat at least one of the 3 randomized battles.  
  
### Editing Spirit Battles
  Select your battle from the top dropdown.  The first tab is for settings specific to the battle, the next tabs are for settings specific to the fighters.  

When done making changes, select: 

**File -> Export**

![image](https://user-images.githubusercontent.com/6089807/109875154-b19e8900-7c35-11eb-85ec-3155f02b685c.png)

You'll see three options: 

  - **Packaged**: Saves a "ready to ship" version of your mod with modified spirit battles, modified spirit images, and other supported modified files (Custom events as example) which can be loaded straight into Arcadia.  
  
  - **Standalone**:  Saves an unencrypted file containing your currently selected Battle and Fighters.  This can be shared with others who wish to import custom battles from multiple people.  
  
  - **PackagedAndStandalone**: Do both!

![image](https://user-images.githubusercontent.com/6089807/109911479-c8fa6800-7c6f-11eb-935e-82f856f68e95.png)


### Importing Spirit Battles
You can also import battles from a folder into an existing SpiritBattle DB.  If you wish to play others Spirit Battles, you can place them in the **Custom Battles** folder, and Import Battles from the folder.  

**File -> Import... **

### Additional details: 
All options for dropdowns are pulled from the loaded ui_spirits_battle_db.prc.  This means that as new Smash updates come out, you should be able to replace the file and get new parameters loaded automatically.  

If spirits_battle_event.prc is in the Preload folder, it will be used to populate Event types and labels.  This means you should be able to (if done correctly) generate new events that will load organically into the program and into the randomizer.  

You can modify the **YesWeDo.dll.config** file to change some settings and modify directory locations.  
