using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace TemporalAmulets.Behaviors;

public class EntityBehaviorTemporalNecklace : EntityBehavior
{
    private const double stability_interval = 1;
    private double stability_timer = 0;
    private IPlayer player;
    private IInventory playerInventory;
    private EntityPlayer eplayer;
    private EntityBehaviorTemporalStabilityAffected stabilityAffected;

    public EntityBehaviorTemporalNecklace(Entity entity) : base(entity)
    {
        eplayer = entity as EntityPlayer;
    }

    public override void AfterInitialized(bool onFirstSpawn)
    {
        player = eplayer.World?.PlayerByUid(eplayer.PlayerUID);
        stabilityAffected = entity.GetBehavior<EntityBehaviorTemporalStabilityAffected>();
    }

    public override string PropertyName() => "EntityBehaviorTemporalNecklace";

    public override void OnGameTick(float dt)
    {
        if (player == null) return;

        stability_timer += dt;
        if (stability_timer < stability_interval) return;
        stability_timer = 0;

        // get player character inventory
        if (playerInventory == null)
            playerInventory = player.InventoryManager?.GetOwnInventory("character");

        if (playerInventory == null) return;

        // get neck slot
        ItemSlot neckSlot = playerInventory[(int)EnumCharacterDressType.Neck];
        if (neckSlot == null || neckSlot.Empty) return;

        // get item in neck slot
        var stack = neckSlot.Itemstack;
        if (stack == null) return;

        var collectible = stack.Collectible;
        if (collectible == null) return;

        // check durability over 0
        var durability = collectible.GetRemainingDurability(stack);
        if (durability <= 0) return;

        float restore = collectible.Attributes?["sanityRestorationAmount"]?.AsFloat() ?? 0f;

        // check if stability behavior exists and its low enough
        if (stabilityAffected == null || stabilityAffected.OwnStability + restore > 1) return;

        // restore sanity
        stabilityAffected.OwnStability = GameMath.Clamp(stabilityAffected.OwnStability + restore, 0f, 1f);

        // damage item
        collectible.DamageItem(entity.World, entity, neckSlot);
        //collectible.SetDurability(stack, durability - 1);
    }
}