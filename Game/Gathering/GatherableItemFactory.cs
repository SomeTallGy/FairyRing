﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyO.Object;
using FairyO.Game.Gathering;

public class GatherableItemFactory
{
    private Node node;  // node scriptable object

    public GatherableItemFactory(Node node)
    {
        this.node = node;
    }

    public GameObject NewRandGatherableItem(Transform parent, Vector3 position, Quaternion rotation, Vector3 force)
    {
        // 1. Get a random scriptable object
        Item item = GetRandItemFromNode(node);

        // 2. Instantiate a new GameObject
        GameObject go = GameObject.Instantiate(item.prefab, position, rotation);
        go.transform.parent = parent;

        // 3. Set Components
        go.AddComponent<GatherableItem>();
        go.AddComponent<SelfDestruct>();

        // 2. Add data to GatherableItem
        GatherableItem gatherableItem = go.GetComponent<GatherableItem>();
        if (gatherableItem != null)
            gatherableItem.Item = (Item)item;

        // 5. Scale up to the game's scale
        Vector3 gameScale = parent.localScale;
        Vector3 goScale = go.transform.localScale;
        go.transform.localScale = Vector3.Scale(goScale, gameScale);

        // 6. Apply force to GameObject
        go.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        // 7. Finished!
        return go;
    }

    private Item GetRandItemFromNode(Node node)
    {
        float r = Random.Range(0.0f, 1.0f); // random weight between 0 and 1
        float s = 0.0f; // incremented sum of weights as we traverse node.gatherableCollectibles

        foreach (var item in node.itemsYielded)
        {
            if (r >= s && r <= item.weight + s) // return if gatherableCollectible's weight is within span
            {
                return item.item;
            }

            s += item.weight;
        }

        if (r >= s && r <= 1) // account for tiny fraction (artifact from float) between s and 1;
        {
            return node.itemsYielded[node.itemsYielded.Count - 1].item;
        }

        return null;
    }

}
