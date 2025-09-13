using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

using TemporalAmulets.Behaviors;
using TemporalAmulets.Items;

namespace TemporalAmulets;

public class TemporalAmuletsModSystem : ModSystem
{
    public override void Start(ICoreAPI api)
    {
        api.RegisterCollectibleBehaviorClass("BehaviorTemporalNecklace", typeof(BehaviorTemporalNecklace));
        api.RegisterItemClass("ItemWearableTemporalNecklace", typeof(ItemWearableTemporalNecklace));
        api.RegisterEntityBehaviorClass("EntityBehaviorTemporalNecklace", typeof(EntityBehaviorTemporalNecklace));
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("temporalamulets:hello"));
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("temporalamulets:hello"));
    }
}
