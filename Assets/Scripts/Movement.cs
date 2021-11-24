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

using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public Image currentequippeddisp;
        public int activeitem = -1;

        public Sprite unequipitem;
        public AudioSource footsteps;
        public bool oncarpet = false;
        public AudioClip wood;
        public AudioClip carpet;
        private readonly List<GameObject> buttons = new List<GameObject>();
        private List<Inventory> itemlibs;

        
        public AudioSource openmenu;
        public AudioSource closemenu;
        public AudioSource selectitem;

        public Animator PlayerAnim;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private float coyotetime;

        private bool deleteonquit = false;
        private float turnsmoothtime = 0.1f;
        private float turnsmoothvelocity;

        public bool mouselocked;
        public bool ininventory = false;
        public CinemachineFreeLook cfl;
        /// Update is called once per frame
        private void Start()
        {

            DontDestroyOnLoad(gameObject);
            itemlibs = itemlibrary.items;
            loadinventory(); 
        }

        private void Update()
        {



            //DELETE THIS IN PROD 
            if (Input.GetKeyDown(KeyCode.P))
            {
                deleteonquit = true;
                Debug.Log("Purged the Variables for reset");
            }

            if (FindObjectOfType<DialogueRunner>().IsDialogueRunning || canmove == false)
            {
                footsteps.mute = true;
                cfl.m_XAxis.m_MaxSpeed = 0.0f;
                cfl.m_YAxis.m_MaxSpeed = 0.0f;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                PlayerAnim.SetFloat(Speed, 0);
                return;
            }
            
            PlayerAnim.SetBool("Bulb?",  PlayerPrefs.GetInt("HasBulb") == 1);
            cfl.m_XAxis.m_MaxSpeed = 300.0f;
            cfl.m_YAxis.m_MaxSpeed = 2.0f;
            if (Input.GetMouseButtonDown(0))
                mouselocked = true;

            if (Input.GetKeyDown(KeyCode.I))
                ininventory = !ininventory;


            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            PlayerAnim.SetFloat(Speed, Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)));

            if (direction.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                    Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothvelocity,
                    turnsmoothtime);
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                Vector3 movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                charController.Move(movedir.normalized * (moveSpeed * Time.deltaTime));
            }


            //Gravity
            if (!grounded())
                coyotetime += Time.deltaTime;
            else
                coyotetime = 0;
            charController.Move(Vector3.down * coyotetime * 9.8f * Time.deltaTime);

            footsteps.mute = (Mathf.Abs(vertical) < .25 && Mathf.Abs(horizontal) < .25);

            //inventory code
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                foreach (var item in items)
                {
                    var button = Instantiate(publicobject, itemimages.transform, true);
                    button.name = item.ToString();
                    button.GetComponent<Image>().sprite = itemlibs[item].icon;
                    buttons.Add(button);
                }

                var ubutton = Instantiate(publicobject, itemimages.transform, true);
                ubutton.name = "Unequip";
                ubutton.GetComponent<Image>().sprite = unequipitem;
                buttons.Add(ubutton);
                openmenu.Play();
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                itemimages.SetActive(true);
                mouselocked = false;
                itemimages.gameObject.transform.position = new Vector3(75,
                    itemimages.transform.position.y + Input.mouseScrollDelta.y * 10, 0);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                foreach (var item in buttons) Destroy(item);
                mouselocked = true;
                itemimages.SetActive(false);
                itemimages.gameObject.transform.localPosition = new Vector3(75, 0, 0);
                closemenu.Play();
            }

            if (activeitem == -1)
                currentequippeddisp.gameObject.SetActive(false);
            else
                currentequippeddisp.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)) CheckForNearbyNPC();
            if (ininventory)
                mouselocked = false;
            if (mouselocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }

        }

        private void OnApplicationQuit()
        {
           saveinventory();
        }

        // Draw the range at which we'll start talking to people.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            // Flatten the sphere into a disk, which looks nicer in 2D games
            Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward*1, Quaternion.identity, new Vector3(1, 1, 1));

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
                       (p.transform.position - (transform.position+ transform.forward*1)) // is in range?
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
            selectitem.Play();
        }

        public void saveinventory()
        {
            for (var i = 0; i < items.Count; i++) PlayerPrefs.SetInt("Slot_" + i, items[i]);
            PlayerPrefs.SetInt("INV_Size", items.Count);
            PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
            var position = this.transform.position;
            PlayerPrefs.SetFloat("PlayerPosX", position.x);
            PlayerPrefs.SetFloat("PlayerPosY", position.y);
            PlayerPrefs.SetFloat("PlayerPosZ", position.z);
            var eulerAngles = this.transform.eulerAngles;
            PlayerPrefs.SetFloat("PlayerRotX", eulerAngles.x);
            PlayerPrefs.SetFloat("PlayerRotY", eulerAngles.y);
            PlayerPrefs.SetFloat("PlayerRotZ", eulerAngles.z);
            if(deleteonquit)
                PlayerPrefs.DeleteAll();
        }

        public void loadinventory()
        {
            for (var i = 0; i < PlayerPrefs.GetInt("INV_Size"); i++) items.Add(PlayerPrefs.GetInt("Slot_" + i));
        }

        bool grounded()
        {
            if(Physics.Raycast(this.transform.position + Vector3.down * 1.01f, Vector3.down, .1f))
                return true;
            return false;
        }
    }
}