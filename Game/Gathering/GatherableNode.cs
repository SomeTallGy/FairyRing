using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FairyO.Object;

namespace FairyO.Game.Gathering
{
    public class GatherableNode : MonoBehaviour
    {
        // ----------- constants -----------
        private const float SHAKE_PUNCH_AMT = 0.05f;    // punch amount when you shake the node

        // ----------- inspector -----------
        public Node node;                               // Node Scriptable Object

        // -------- private fields ---------
        private GameObject nodeGO;                      // Node's gameObject;
        private GatherableItemFactory factory;          // gatherable item factory
        private ParticleSystem particleSys;             // Node's particleSystem
        private Quaternion originalRot;                 // Original: Node's rotation
        private Tween scaleTween, rotTween;             // Tweens: For the punch animation tween
        private bool shaken;                            // is this still being shook?
        private int itemYield;                          // how many items to drop before disappearing

        void Start()
        {
            // 1. Parse Node
            if (node != null)
                ParseNode();
            else
                Debug.LogError("No Node to Parse!");

            // 2. Init factory
            factory = new GatherableItemFactory(node);
        }

        void Update()
        {
            // [SpaceBar] - Shake the node!
            if (Input.GetKeyDown(KeyCode.Space) && GatheringGame.IsReady)
            {
                if (nodeGO != null && !shaken && itemYield > 0)
                {
                    StartCoroutine(Shake(nodeGO));
                    if (particleSys != null)
                    {
                        particleSys.Emit(Random.Range((int)0, (int)3));
                    }
                }
                else if(nodeGO != null && !shaken && itemYield <= 0)
                {
                    if (particleSys != null)
                    {
                        particleSys.Emit(Random.Range((int)0, (int)3));
                    }
                    
                    // set gathering game ready
                    GatheringGame.IsReady = false;

                    // dissappear node
                    this.transform.DOScale(Vector3.zero, 0.5f).OnComplete(()=>{
                        GameObject.Destroy(this.gameObject);
                    });
                }
            }
        }

        private void ParseNode()
        {
            // 1. Instantiate game object from Prefab in node.ScriptedObject
            nodeGO = GameObject.Instantiate(node.prefab, this.transform);
            nodeGO.transform.parent = this.transform;

            // 2. TEMP: Rotate object's X axis so it's more easily visible (Temporary for Bushes)
            nodeGO.transform.Rotate(new Vector3(-45, 0, 0));

            // 3. Store original transform.properties of GameObject
            originalRot = nodeGO.transform.rotation;

            // 4. Get particle system
            particleSys = nodeGO.GetComponent<ParticleSystem>();

            // 5. Set Item Yield
            itemYield = node.yield_max; // set it to max for now
            this.transform.parent.GetComponent<GatheringGame>().GatherableItemsTotal = node.yield_max;
        }

        public IEnumerator Shake(GameObject go)
        {
            // 1. Get ready to shake, by setting the node.transform.properties to their original state
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localRotation = originalRot;

            // 2. Complete any tweens that are already in progress
            if (scaleTween != null && rotTween != null)
            {
                scaleTween.Complete();
                rotTween.Complete();
            }

            // 3. Shake!
            scaleTween = go.transform.DOPunchScale(new Vector3(SHAKE_PUNCH_AMT, SHAKE_PUNCH_AMT, SHAKE_PUNCH_AMT), 2.0f, 7);
            rotTween = go.transform.DOPunchRotation(new Vector3(0, 10.0f, 0), 2.0f, 5);
            shaken = true;

            // 4. Emit the node's gatherable items
            yield return StartCoroutine(EmitNodeObject(Random.Range((int)3, (int)8)));

            // 5. All done!
            shaken = false;
            yield return this;
        }

        private IEnumerator EmitNodeObject(int num)
        {
            // deduct from itemYield
            if(itemYield < num)
            {
                num = itemYield;
            }
            itemYield -= num;

            // Emit itemprefabs
            int n = 1;
            while (n <= num)
            {
                // 1. Create a noisy position along the X and Y axis (not Z)
                float x = this.transform.position.x + Random.Range(-0.35f, 0.35f);
                float y = this.transform.position.y + Random.Range(-0.2f, 0.2f);
                float z = this.transform.position.z;
                Vector3 position = new Vector3(x, y, z);

                // 2. Create a noisy position in Euler(x,y,z)
                float a = Random.Range(0f, 360f);
                float b = Random.Range(0f, 360f);
                float c = Random.Range(0f, 360f);
                Quaternion rotation = Quaternion.Euler(a, b, c);

                // 3. Create a force based on the position
                Vector3 force = new Vector3(x, y, z) * 0.01f;

                // 4. Build random gatherable item
                factory.NewRandGatherableItem(this.transform.parent, position, rotation, force);

                // 5. Repeat for every n in num
                n++;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.25f)); // randomize time a bit
            }

            // Finished!
            yield return this;
        }
    }
}