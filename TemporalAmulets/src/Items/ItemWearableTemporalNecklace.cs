using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace TemporalAmulets.Items;

public class ItemWearableTemporalNecklace : Item
{
    public override byte[] GetLightHsv(IBlockAccessor blockAccessor, BlockPos pos, ItemStack stack = null)
    {
        if (stack != null && GetRemainingDurability(stack) <= 0)
            return [0, 0, 0]; // off when broken

        return base.GetLightHsv(blockAccessor, pos, stack); // default
    }
}
