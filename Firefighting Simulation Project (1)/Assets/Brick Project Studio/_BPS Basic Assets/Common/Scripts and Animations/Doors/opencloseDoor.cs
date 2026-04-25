using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;

        void Start()
        {
            open = false;

            var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            if (interactable != null)
            {
                interactable.selectEntered.AddListener(OnXRSelect);
            }
        }

        void OnXRSelect(SelectEnterEventArgs args)
        {
            ToggleDoor();
        }

        public void ToggleDoor()
        {
            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

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
            print("you are opening the door");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            print("you are closing the door");
            openandclose.Play("Closing");
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
//	public class opencloseDoor : MonoBehaviour
//	{

//		public Animator openandclose;
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
//			openandclose.Play("Opening");
//			open = true;
//			yield return new WaitForSeconds(.5f);
//		}

//		IEnumerator closing()
//		{
//			print("you are closing the door");
//			openandclose.Play("Closing");
//			open = false;
//			yield return new WaitForSeconds(.5f);
//		}


//	}
//}