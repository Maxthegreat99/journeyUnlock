# JourneyUnlock
A TShock plugin allowing you to unlock every/specific items for your journey characters.

## How to Install
1. Put the .dll into the `\ServerPlugins\` folder.
2. Restart the server.
3. Give your desired group the the permissions defined in the configs folder.

## How to Use
### Commands and Usage
- `journeyunlock/junlock {item name/item id/*}` - unlocks the targeted item for the sender, if `*` is used a parameter it shall unlock every items, requires the `journeyunlock.unlock` permission.
- `unlockfor/unlockf {player name} {item name/item id/*}` - executes the `journeyunlock` command on the parametered player, the parameters entered on the second argument as the same as in the `journeyunlock` command, requires the `journeyunlock.unlockfor` permission.

## How to Build
1. Download the source code.
2. Open the `.sln` file.
3. Check to make sure the references are all correct and up to date.
4. Build.

## Notes
- if you find any bugs be sure to create an issue or report them to my [discord](https://discord.gg/e465y7Xeba), same goes if you have a sugestion.
