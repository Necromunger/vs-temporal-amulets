using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace TemporalAmulets.Behaviors;

public class EntityBehaviorTemporalNecklace : EntityBehavior
{
    private double accum = 0;
    private const double CheckIntervalSec = 1;

    public EntityBehaviorTemporalNecklace(Entity entity) : base(entity) { }
    public override string PropertyName() => "EntityBehaviorTemporalNecklace";

    public override void OnGameTick(float dt)
    {
        accum += dt;
        if (accum < CheckIntervalSec) return;
        accum = 0;

        if (!(entity is EntityPlayer eplayer)) return;

        // get inventory
        var player = eplayer.World?.PlayerByUid(eplayer.PlayerUID);
        if (player == null) return;

        IInventory inv = player.InventoryManager?.GetOwnInventory("character");
        if (inv == null) return;

        ItemSlot neckSlot = inv[(int)EnumCharacterDressType.Neck];
        if (neckSlot == null || neckSlot.Empty) return;

        // get actual item from slot
        var stack = neckSlot.Itemstack;
        if (stack == null) return;

        var collectible = stack.Collectible;
        if (collectible == null) return;

        // check durability over 0
        var durability = collectible.GetRemainingDurability(stack);
        if (durability <= 0) return;

        float restore = collectible.Attributes?["sanityRestorationAmount"]?.AsFloat() ?? 0f;

        // check if stability behavior exists and its low enough
        var stab = entity.GetBehavior<EntityBehaviorTemporalStabilityAffected>();
        if (stab == null || stab.OwnStability + restore > 1) return;

        // restore sanity
        stab.OwnStability = GameMath.Clamp(stab.OwnStability + restore, 0f, 1f);

        // damage item
        collectible.DamageItem(entity.World, entity, neckSlot, 1);
    }
}