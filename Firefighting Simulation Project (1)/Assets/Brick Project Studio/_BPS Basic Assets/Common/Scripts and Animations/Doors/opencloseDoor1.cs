using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SojaExiles
{
    public class opencloseDoor1 : MonoBehaviour
    {
        public Animator openandclose1;
        public bool open;
        public Transform Player;

        void Start()
        {
            open = false;

            // Auto-wire XRI if the component exists on this object
            var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            if (interactable != null)
            {
                interactable.selectEntered.AddListener(OnXRSelect);
            }
        }

        // Called by XRI when a controller selects this object
        void OnXRSelect(SelectEnterEventArgs args)
        {
            ToggleDoor();
        }

        // Can also be called from MiddleVR, UI buttons, etc.
        public void ToggleDoor()
        {
            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

        // Existing mouse interaction — still works in editor/desktop
        void OnMouseOver()
        {
            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 15)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        ToggleDoor();
                    }
                }
            }
        }

        IEnumerator opening()
        {
            openandclose1.Play("Opening 1");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            openandclose1.Play("Closing 1");
            open = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SojaExiles

//{
//	public class opencloseDoor1 : MonoBehaviour
//	{

//		public Animator openandclose1;
//		public bool open;
//		public Transform Player;

//		void Start()
//		{
//			open = false;
//		}

//		void OnMouseOver()
//		{
//			{
//				if (Player)
//				{
//					float dist = Vector3.Distance(Player.position, transform.position);
//					if (dist < 15)
//					{
//						if (open == false)
//						{
//							if (Input.GetMouseButtonDown(0))
//							{
//								StartCoroutine(opening());
//							}
//						}
//						else
//						{
//							if (open == true)
//							{
//								if (Input.GetMouseButtonDown(0))
//								{
//									StartCoroutine(closing());
//								}
//							}

//						}

//					}
//				}

//			}

//		}

//		IEnumerator opening()
//		{
//			print("you are opening the door");
//			openandclose1.Play("Opening 1");
//			open = true;
//			yield return new WaitForSeconds(.5f);
//		}

//		IEnumerator closing()
//		{
//			print("you are closing the door");
//			openandclose1.Play("Closing 1");
//			open = false;
//			yield return new WaitForSeconds(.5f);
//		}


//	}
//}