/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn.Unity.Example;

namespace YarnSpinner
{
    public class Movement : MonoBehaviour
    {
        public List<int> items = new List<int>();
        public bool canmove = true;
        public CharacterController charController;
        public float moveSpeed = 1f;
        public float interactionRadius = 2.0f;
        public GameObject itemimages;
        public GameObject publicobject;
        public ItemLib itemlibrary;

        private readonly List<GameObject> buttons = new List<GameObject>();
        private List<Inventory> itemlibs;

        public Image currentequippeddisp;
        public int activeitem = -1;

        public Sprite unequipitem;

        /// Update is called once per frame
        private void Start()
        {
            itemlibs = itemlibrary.items;
        }

        private void Update()
        {
            if (FindObjectOfType<DialogueRunner>().IsDialogueRunning || canmove == false) return;

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0f;
            forward = forward.normalized;

            var right = new Vector3(forward.z, 0f, -forward.x);

            var localMoveDir = new Vector3(horizontal, 0f, vertical);

            if (localMoveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(localMoveDir),
                    10f * Time.smoothDeltaTime);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            }

            if (localMoveDir.sqrMagnitude > 1f)
                localMoveDir = localMoveDir.normalized;

            charController.Move(localMoveDir * moveSpeed * Time.deltaTime);

            //inventory code
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                foreach (var item in items)
                {
                    var button = Instantiate(publicobject);
                    button.name = item.ToString();
                    button.GetComponent<Image>().sprite = itemlibs[item].icon;
                    button.transform.parent = itemimages.transform;
                    buttons.Add(button);
                }

                var ubutton = Instantiate(publicobject);
                ubutton.name = "Unequip";
                ubutton.GetComponent<Image>().sprite = unequipitem;
                ubutton.transform.parent = itemimages.transform;
                buttons.Add(ubutton);
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                itemimages.SetActive(true);
                itemimages.gameObject.transform.position = new Vector3(50,
                    itemimages.transform.position.y + Input.mouseScrollDelta.y * 10, 0);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                foreach (var item in buttons) Destroy(item);

                itemimages.SetActive(false);
                itemimages.gameObject.transform.localPosition = new Vector3(50, 0, 0);
            }

            if (activeitem == -1)
                currentequippeddisp.gameObject.SetActive(false);
            else
                currentequippeddisp.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)) CheckForNearbyNPC();
        }

        // Draw the range at which we'll start talking to people.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            // Flatten the sphere into a disk, which looks nicer in 2D games
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1, 1, 1));

            // Need to draw at position zero because we set position in the line above
            Gizmos.DrawWireSphere(Vector3.zero, interactionRadius);
        }

        /**
         * Find all DialogueParticipants
         * Filter them to those that have a Yarn start node and are in range; 
         * then start a conversation with the first one
         */
        public void CheckForNearbyNPC()
        {
            var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
            var target = allParticipants.Find(delegate(NPC p)
            {
                return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
                       (p.transform.position - transform.position) // is in range?
                       .magnitude <= interactionRadius;
            });
            if (target != null)
                // Kick off the dialogue at this node.
                FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
        }

        public void equipiten(int id)
        {
            Debug.Log(itemlibs[id].name + " Equipped!");
            currentequippeddisp.sprite = itemlibs[id].icon;
            activeitem = id;
        }
    }
}