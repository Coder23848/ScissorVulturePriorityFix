using BepInEx;

namespace ScissorVulturePriorityFix
{
    [BepInPlugin("com.coder23848.scissorvulturepriorityfix", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
#pragma warning disable IDE0051 // Visual Studio is whiny
        private void OnEnable()
#pragma warning restore IDE0051
        {
            On.VultureAI.IUseARelationshipTracker_UpdateDynamicRelationship += VultureAI_IUseARelationshipTracker_UpdateDynamicRelationship;
        }

        // This function applies a 0.1x multiplier to all non-slugcat relationships under conditions that scissor vultures always meet. This hook should undo that.
        private CreatureTemplate.Relationship VultureAI_IUseARelationshipTracker_UpdateDynamicRelationship(On.VultureAI.orig_IUseARelationshipTracker_UpdateDynamicRelationship orig, VultureAI self, RelationshipTracker.DynamicRelationship dRelation)
        {
            CreatureTemplate.Relationship ret = orig(self, dRelation);

            // Scissor vultures are excluded from all other parts of the function.

            if (self.vulture.State.socialMemory.GetLike(dRelation.trackerRep.representedCreature.ID) >= -0.25f && // The function returns something entirely different if the vulture hates the creature, and this hook shouldn't mess with that.
                self.IsMiros && dRelation.trackerRep.representedCreature.creatureTemplate.type != CreatureTemplate.Type.Slugcat) // Slugcats are unaffected by the multiplier, that's the problem.
            {
                ret.intensity = self.StaticRelationship(dRelation.trackerRep.representedCreature).intensity;
            }

            //self.relationshipTracker.visualize = true;

            return ret;
        }
    }
}