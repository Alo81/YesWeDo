# YesWeDo!
## Smash Ultimate Randomizer and Spirit Battle Editor

### Prerequisites:

  - Hacked Switch preconfigured for playing modded Super Smash Bros Ultimate
  - .Net Core 3.1 Runtime installed: 
    >https://dotnet.microsoft.com/download/dotnet/3.1/runtime
  - Ability to extract files from data.arc:  
    >Details on extracting files can be found here: https://gamebanana.com/tuts/12827)
  
__________________________________
  
### Requirements:

  - ui_spirits_battle_db.prc 
    >(Can be ripped from data.arc @ \ui\param\database\)
  
__________________________________

### Optional (but recommended):

- Arcadia 
  >(For toggling between Randomizers)

##### Rip files from data.arc and place in "/PreLoad" folder

  - ui_fighter_spirit_aw_db.prc
    >@\ui\param\database\  
  - spirits_battle_event.prc
    >@\ui\param\spirits\
  
__________________________________

### Set Up:

  - Place ui_spirits_battle_db.prc in the root of the application (".\YesWeDo\)  
  - Run YesWeDo.exe
__________________________________
  
### Usage:

  **Randomizer**
  Tools -> Randomize All
  ![image](https://user-images.githubusercontent.com/6089807/109874328-94b58600-7c34-11eb-8cf6-663e6702b890.png)
  
  This will randomize all spirit battles using the seed provided (or a random seed if none provided) and create multiple randomizers from the same seed.  
  
  ![image](https://user-images.githubusercontent.com/6089807/109873955-1527b700-7c34-11eb-91f9-0bcd0bdc4937.png)
  
  These results can be dragged into your Arcadia mod folder (msot likely /ultimate/mods) and enabled/disabled with Arcadia.
  
  Disclaimer: If due to the nature of Random, you run into a battle that cannot be beatenm, switch to one of the other Randomizers of the same seed, and Fighter Unlocks should be randomized in the same order.  
  
  **Editing Spirit Battles**  
  Select your battle from the top dropdown.  The first tab is for settings specific to the battle, the next tabs are for settings specific to the fighters.  

When done making changes, select: File -> Export
![image](https://user-images.githubusercontent.com/6089807/109875154-b19e8900-7c35-11eb-85ec-3155f02b685c.png)

You'll see three options: 

  Packaged: Saves a "ready to ship" version of your mod with modified spirit battles, modified spirit images, and other supported modified files (Custom events as example) which can be loaded straight into Arcadia.  
  
  Standalone:  Saves an unencrypted file containing your currently selected Battle and Fighters.  This can be shared with others who wish to import custom battles from multiple people.  
  
  PackagedAndStandalone: Do both!

**Importing Spirit Battles**
You can also import battles from a folder into an existing SpiritBattle DB.  If you wish to play others Spirit Battles, you can place them in the Custom Battles folder, and Import Battles from the folder.  

File -> Import... 

Theres other stuff I'm probably forgetting.  
