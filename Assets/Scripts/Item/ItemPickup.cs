using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    
    public override void Interact()
    {
        base.Interact();

        Pickup();
    }

    void Pickup() {
        //bool wasPickedUp = Inventory.instance.Add(item);
        //if (wasPickedUp) {
        //    Destroy(gameObject);
        //}
    }
}
