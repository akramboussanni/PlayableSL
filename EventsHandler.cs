using System.Collections.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp914Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;

namespace PlayableSL;

public class MyCustomEventsHandler : CustomEventsHandler
{
    private Dictionary<RoleTypeId, RoleTypeId> redirectRoles = new()
    {
        { RoleTypeId.Scp096, RoleTypeId.ClassD }
    };

    private List<ItemType> redirectItems = new()
    {
         ItemType.SCP207, ItemType.MicroHID
    };
    
    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        if (!redirectRoles.TryGetValue(ev.NewRole, out var redirect)) return;
        ev.NewRole = ev.ChangeReason == RoleChangeReason.RemoteAdmin ? ev.OldRole.RoleTypeId : redirect;
    }

    public override void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev)
    {
        if (ev.Pickup.Type == ItemType.SCP1344)
            ev.Player.Ban("Never try using this stupid ass shit again.", long.MaxValue);
    }

    public override void OnScp914ProcessedInventoryItem(Scp914ProcessedInventoryItemEventArgs ev)
    {
        if (!redirectItems.Contains(ev.OldItemType)) return;
        ev.Player.RemoveItem(ev.Item);
        ev.Player.AddItem(ItemType.Coal);
    }

    public override void OnScp914ProcessedPickup(Scp914ProcessedPickupEventArgs ev)
    {
        if (!redirectItems.Contains(ev.OldItemType)) return;
        ev.Pickup?.Destroy();
        Pickup.Create(ItemType.Coal, ev.NewPosition);
    }

    public override void OnPlayerChangedItem(PlayerChangedItemEventArgs ev)
    {
        if (ev.NewItem?.Type == ItemType.KeycardChaosInsurgency)
        {
            var newComponent = ev.Player.GameObject.AddComponent<SnakeComponent>();
            newComponent.Initialize(ev.Player);
            return;
        }

        if (ev.OldItem?.Type != ItemType.KeycardChaosInsurgency && ev.Player.GameObject.TryGetComponent(out SnakeComponent existingComponent))
            Object.DestroyImmediate(existingComponent);
    }
}