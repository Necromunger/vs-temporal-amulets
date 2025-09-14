using Vintagestory.API.Common;
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
}
