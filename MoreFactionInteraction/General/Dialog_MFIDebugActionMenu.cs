﻿using RimWorld;
using Verse;
using RimWorld.Planet;

namespace MoreFactionInteraction
{
    using System.Linq;

    internal class Dialog_MFIDebugActionMenu : Dialog_DebugActionsMenu
    {
        protected override void DoListingItems()
        {
            base.DoListingItems();
            if (WorldRendererUtility.WorldRenderedNow)
            {
                DoGap();
                DoLabel("Tools - MFI");

                base.DebugToolWorld("Spawn pirate base", delegate
                     {
                         int tile = GenWorld.MouseTile(false);

                         if (tile < 0 || Find.World.Impassable(tile))
                            Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);

                         else
                         {
                             Faction faction = (from x in Find.FactionManager.AllFactions
                                                where !x.def.hidden
                                                    && !x.defeated
                                                    && !x.IsPlayer
                                                    && x.HostileTo(other: Faction.OfPlayer)
                                                    && x.def.permanentEnemy
                                                select x).First();

                             Settlement factionBase = (Settlement) WorldObjectMaker.MakeWorldObject(def: WorldObjectDefOf.Settlement);
                             factionBase.SetFaction(newFaction: faction);
                             factionBase.Tile = tile;
                             factionBase.Name = SettlementNameGenerator.GenerateSettlementName(factionBase: factionBase);
                             Find.WorldObjects.Add(o: factionBase);
                         }
                     }
                );
            }
        }
    }
}