﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using Toolsmith.Utils;
using Vintagestory.GameContent;

namespace Toolsmith.ToolTinkering.Drawbacks {
    public static class DrawbackUtility {

        /*
         * Things that each Drawback can be expected to have, make this into an Interface and Class eventually!
         *  - Call that returns a list of applicable Tool Tags this Drawback can be applied on
         *  - Method that recieves a specific rolled tag and returns if it can be applied or not - will help filter applicable tags to choose from
         *  - Stored ID
         *  - Method that returns it's effects on the tool's performance
         *  - If it is Minor or Major
         *  - If Minor, what possible Major Drawbacks can this turn into?
         *  - Weight value to actually roll this Drawback, some might want to be rarer if they are nasty.
         *  
         */
        
        // -- Drawback Attribute Tree Example --
        //
        //  Drawback Tree Base
        //      |
        //      |-- (Drawback ID/Name) <-- Use this ID as both it's lang key and an identifier tag
        //      |
        //      |--
        //
        //
        //
        //
        //
        //
        //

        //Check and see if a drawback is rolled and then have it applied. Returns true if drawback is applied, false if not!
        public static bool TryChanceForDrawback(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, float sharpnessPercent) {
            if (sharpnessPercent >= 0.8) {
                return false;
            }

            if (sharpnessPercent <= 0) {
                ApplyRandomDrawback(world, byEntity, itemslot, sharpnessPercent);
                return true;
            }

            bool shouldApplyDrawback = MathUtility.ShouldChanceForDefectCurve(world, sharpnessPercent, itemslot.Itemstack.GetToolMaxSharpness());
            if (shouldApplyDrawback) {
                ApplyRandomDrawback(world, byEntity, itemslot, sharpnessPercent);
            }
            return shouldApplyDrawback;
        }

        //Check for valid drawbacks for this tool type given, then try rolling for one to apply it.
        public static void ApplyRandomDrawback(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, float sharpnessPercent) {
            if (HasDrawback(itemslot.Itemstack)) {
                if (ToolsmithModSystem.Config.DebugMessages) {
                    ToolsmithModSystem.Logger.Warning("A Tool should have had a Drawback Worsened!!!");
                }
            } else {
                if (ToolsmithModSystem.Config.DebugMessages) {
                    ToolsmithModSystem.Logger.Warning("A Tool should have had a Drawback applied!");
                }
            }
        }

        public static bool HasDrawback(ItemStack itemStack) {
            return itemStack.Attributes.HasAttribute(ToolsmithAttributes.Drawback);
        }
    }
}
