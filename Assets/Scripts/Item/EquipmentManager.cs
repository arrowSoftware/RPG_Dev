using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged; 

    private void Awake() {
        instance = this;
    }

    public Equipment[] defaultEquipment;
    public SkinnedMeshRenderer targetMesh;  
    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;

    Inventory inventory;

    private void Start() {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        inventory = Inventory.instance;
        currentMeshes = new SkinnedMeshRenderer[numSlots];
        EquipDefaultItems();
    }

    public void Equip(Equipment newItem) {
        int slotIndex = (int)newItem.equipmentSlot;

        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChanged != null) {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        SetEquipmentBlendShape(newItem, 100);
        currentEquipment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;
    }

    public Equipment Unequip(int slotIndex) {
        if (currentEquipment[slotIndex] != null) {
            if (currentMeshes[slotIndex] != null) {
                Destroy(currentMeshes[slotIndex].gameObject); 
            }
            Equipment oldItem = currentEquipment[slotIndex];
            SetEquipmentBlendShape(oldItem, 0);

            inventory.Add(oldItem);
            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null) {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll() {
        for (int i = 0; i < currentEquipment.Length; i++) {
            Unequip(i);
        }
        EquipDefaultItems();
    }

    void SetEquipmentBlendShape(Equipment item, int weight) {
        foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions) {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }

    void EquipDefaultItems() {
        foreach (Equipment item in defaultEquipment) {
            Equip(item);
        }
    }
}
