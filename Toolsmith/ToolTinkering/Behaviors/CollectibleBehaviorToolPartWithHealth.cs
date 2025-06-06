﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolsmith.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Toolsmith.ToolTinkering.Behaviors {
    public class CollectibleBehaviorToolPartWithHealth : CollectibleBehavior {
        public CollectibleBehaviorToolPartWithHealth(CollectibleObject collObj) : base(collObj) {

        }

        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo) {
            if (TinkeringUtility.ShouldNotAccessStats(inSlot)) {
                return;
            }

            if (!inSlot.Itemstack.Collectible.HasBehavior<CollectibleBehaviorToolBlunt>()) {
                if (inSlot.Itemstack.Collectible.HasBehavior<CollectibleBehaviorToolHead>()) {
                    if (inSlot.Itemstack.HasTotalHoneValue() && inSlot.Itemstack.GetTotalHoneValue() > 0 && inSlot.Itemstack.GetTotalHoneValue() < 1) {
                        dsc.AppendLine(Lang.Get("toolheadhoninginprogress"));
                    } else if (!inSlot.Itemstack.HasTotalHoneValue()) {
                        dsc.AppendLine(Lang.Get("toolheadfreehone"));
                    }
                    if (inSlot.Itemstack.GetPartMaxSharpness() > 0) {
                        var remainingSharpPercent = inSlot.Itemstack.GetPartRemainingSharpnessPercent();
                        if (remainingSharpPercent < 0 || remainingSharpPercent >= 1.0f) {
                            dsc.AppendLine(Lang.Get("fullysharpened"));
                        } else {
                            var percent = Math.Floor(remainingSharpPercent * 100);
                            dsc.AppendLine(Lang.Get("partiallysharpened", percent));
                        }
                    }
                }
            }

            var remainingPercent = inSlot.Itemstack.GetPartRemainingHPPercent(); //Is there ANY way this could possibly have a null itemstack and still get called...? I'd be shocked honestly, hah!
            if (remainingPercent <= 0.0f || remainingPercent >= 1.0f) { //If this returns 0 or less, assume it has not been used or is at full durability! If it's 1.0 or more (somehow?), well, similarly :P
                dsc.AppendLine(Lang.Get("pristinecondition"));
            } else {
                var percent = Math.Floor(remainingPercent * 100);
                dsc.AppendLine(Lang.Get("partiallydamaged", percent));
            }
        }
    }
}
