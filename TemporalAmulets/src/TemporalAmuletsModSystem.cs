using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using TemporalAmulets.Behaviors;
using TemporalAmulets.Items;

namespace TemporalAmulets;

public class TemporalAmuletsModSystem : ModSystem
{
    public ICoreAPI api;

    public override void Start(ICoreAPI api)
    {
        this.api = api;

        api.RegisterItemClass("ItemWearableTemporalNecklace", typeof(ItemWearableTemporalNecklace));
        api.RegisterCollectibleBehaviorClass("BehaviorTemporalNecklace", typeof(BehaviorTemporalNecklace));
        api.RegisterEntityBehaviorClass("EntityBehaviorTemporalNecklace", typeof(EntityBehaviorTemporalNecklace));
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        AddStabilityCommand();
    }

    private void AddStabilityCommand()
    {
        api.ChatCommands
            .GetOrCreate("player")
            .BeginSubCommands("sanity")
            .WithDescription("Updates player sanity")
            .RequiresPrivilege(Privilege.controlserver)
            .WithArgs(api.ChatCommands.Parsers.OnlinePlayer("target"))
            .WithArgs(api.ChatCommands.Parsers.Double("amount"))
            .HandleWith(args =>
            {
                var playerArg = args.Parsers[0] as PlayersArgParser;
                var players = args.Parsers[0].GetValue() as PlayerUidName[];
                if (players.Length == 0)
                    return TextCommandResult.Error("Target player not found or not online.");

                var playerUidName = players[0];

                var player = api.World.PlayerByUid(playerUidName.Uid);
                if (player == null)
                    return TextCommandResult.Error("Target player not found or not online.");

                var amount = (double)args.Parsers[1].GetValue();

                var be = player.Entity.GetBehavior<EntityBehaviorTemporalStabilityAffected>();
                if (be == null) return TextCommandResult.Error("Player has no temporal-stability behavior.");

                be.OwnStability = GameMath.Clamp(amount, 0.0, 1.0);
                player.Entity.WatchedAttributes.SetDouble("temporalStability", be.OwnStability);
                player.Entity.WatchedAttributes.MarkPathDirty("temporalStability");

                return TextCommandResult.Success($"Set {player.PlayerName}'s stability to {be.OwnStability:P0}.");
            });
    }
}
