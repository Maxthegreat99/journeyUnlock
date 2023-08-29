using System.Security.Policy;
using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;
using TerrariaApi.Server;
using TShockAPI;

namespace journeyUnlock
{
    [ApiVersion(2, 1)]
    public class journeyUnlock : TerrariaPlugin
    {
        public override string Author => "Maxthegreat99";

        public override string Description => "A TShock plugin allowing you to unlock every/specific items for your journey characters.";

        public override string Name => "journeyUnlock";

        public override Version Version => new(1, 0, 1, 0);

        public journeyUnlock(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(
                permissions: new List<string> { "journeyunlock.unlock" },
                cmd: this.unlockCommand,
                "journeyunlock", "junlock"));

            Commands.ChatCommands.Add(new Command(
                permissions: new List<string> { "journeyunlock.unlockfor" },
                cmd: this.unlockForCommand,
                "unlockfor", "unlockf"));
        }

        private void unlock(Player tplayer, TSPlayer sender , string parameter, bool isSelf)
        {

            //Case: unlock every item
            if (parameter == "*")
            {
                for (var i = 0; i < ItemID.Count; i++)
                {
                    tplayer.creativeTracker.ItemSacrifices.RegisterItemSacrifice(i, 999);
                    var _response = NetCreativeUnlocksModule.SerializeItemSacrifice(i, 999);
                    NetManager.Instance.SendToClient(_response, tplayer.whoAmI);
                }
                if (!isSelf)
                {
                    TSPlayer.FindByNameOrID(tplayer.name)[0].SendInfoMessage("[journeyUnlock] {0} has unlocked every items for you!", sender.Name);
                    sender.SendSuccessMessage("[journeyUnlock] successfully unlocked every items for {0}!", tplayer.name);
                }
                else
                    sender.SendSuccessMessage("[journeyUnlock] successfully unlocked every items!");

                return;
            }

            //Case: unlock a specific item via id
            int itemid;
            if (int.TryParse(parameter, out itemid) && TShock.Utils.GetItemById(itemid) != null)
            {
                tplayer.creativeTracker.ItemSacrifices.RegisterItemSacrifice(itemid, 999);
                var _response = NetCreativeUnlocksModule.SerializeItemSacrifice(itemid, 999);
                NetManager.Instance.SendToClient(_response, tplayer.whoAmI);

                if (!isSelf)
                {
                    TSPlayer.FindByNameOrID(tplayer.name)[0].SendInfoMessage("[journeyUnlock] {0} has unlocked the [i:{1}] for you!", sender.Name, itemid);
                    sender.SendSuccessMessage("[journeyUnlock] successfully unlocked the [i:{0}] for {1}!", itemid, tplayer.name);
                }
                else
                    sender.SendSuccessMessage("[journeyUnlock] successfully unlocked the [i:{0}]!",itemid);

                return;
            }
            else if (TShock.Utils.GetItemById(itemid) == null || itemid < 1 || itemid > ItemID.Count)
            {
                sender.SendErrorMessage("[journeyUnlock] Invalid item id!");
                return;
            }

            //Case: unlock a specific item via name
            string itemname = parameter;

            if (TShock.Utils.GetItemByName(itemname).Count == 0)
            {
                sender.SendErrorMessage("[journeyUnlock] No items found!");
                return;
            }

            if (TShock.Utils.GetItemByName(itemname).Count > 1)
            {
                sender.SendErrorMessage("[journeyUnlock] More than one item was found!");
                return;
            }

            itemid = TShock.Utils.GetItemByName(itemname)[0].netID;

            tplayer.creativeTracker.ItemSacrifices.RegisterItemSacrifice(itemid, 999);
            var response = NetCreativeUnlocksModule.SerializeItemSacrifice(itemid, 999);
            NetManager.Instance.SendToClient(response, tplayer.whoAmI);

            if (!isSelf)
            {
                TSPlayer.FindByNameOrID(tplayer.name)[0].SendInfoMessage("[journeyUnlock] {0} has unlocked the [i:{1}] for you!", sender.Name, itemid);
                sender.SendSuccessMessage("[journeyUnlock] successfully unlocked the [i:{0}] for {1}!", itemid, tplayer.name);
            }
            else
                sender.SendSuccessMessage("[journeyUnlock] successfully unlocked the [i:{0}]!", itemid);
        }

        private void unlockCommand(CommandArgs args)
        {
            //improper comamnd usuage
            if(args.Parameters.Count != 1)
            {
                args.Player.SendInfoMessage("[journeyUnlock] /junlock {item name/id} - unlocks the targeted item.");
                args.Player.SendInfoMessage("[journeyUnlock] /junlock * - unlocks every items.");
                return;
            }

            unlock(args.TPlayer, args.Player, args.Parameters[0], true);
        }

        private void unlockForCommand(CommandArgs args)
        {
            //improper comamnd usuage
            if (args.Parameters.Count != 2)
            {
                args.Player.SendInfoMessage("[journeyUnlock] /unlockFor {player name} {item name/id} - unlocks the targeted item for the parametered player.");
                args.Player.SendInfoMessage("[journeyUnlock] /unlockFor {player name} * - unlocks every items for the targeted player.");
                return;
            }

            if (TSPlayer.FindByNameOrID(args.Parameters[0]).Count == 0)
            {
                args.Player.SendErrorMessage("[journeyUnlock] No player found!");
                return;
            }
            if (TSPlayer.FindByNameOrID(args.Parameters[0]).Count > 1)
            {
                args.Player.SendErrorMessage("[journeyUnlock] More than one player was found!");
                return;
            }


            TSPlayer player = TSPlayer.FindByNameOrID(args.Parameters[0])[0];

            unlock(player.TPlayer, args.Player, args.Parameters[1], false);
        }

    }
}
