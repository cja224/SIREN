using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisherInteraction : MonoBehaviour
{
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;
    public XRRayInteractor rayInteractor;
    public Transform parentObject;

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    private GameObject fireObject;
    public GameObject firespreadObject; // "fire" 오브젝트를 참조하기 위한 변수
    private XRGrabInteractable grabInteractable; // XR Grab Interactable 컴포넌트 참조

    private bool isRightHandLocked = false;

    private void Start()
    {
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.AddListener(OnSelectEntered);
        }

        fireObject = FindInactiveChildByName(parentObject, "Fireextinguisher(fire)");

        // XR Grab Interactable 컴포넌트 초기화 및 비활성화
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false; // 처음에는 비활성화
        }

        else
        {
            fireObject.SetActive(false);
        }

    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (firespreadObject != null && firespreadObject.activeSelf) // "fire" 오브젝트가 활성화 상태일 때만 실행
        {
            if (args.interactableObject.transform.CompareTag("fireextinguisher"))
            {
                DeactivateFireExtinguisher(args.interactableObject.transform.gameObject); // 소화기 비활성화         
		        ActivateFireObject(); // "Fireextinguisher(fire)" 오브젝트 활성화       
		        PlayHandAnimations(); // 손 애니메이션 실행       
		        SetRightHandAsChildOfLeftHand(); // 오른손을 왼손의 자식으로 설정하고 움직임 잠금    
		        LockRightHandMovement(); // 오른손 움직임 잠금 활성화
            }
        }
    }

    private GameObject FindInactiveChildByName(Transform parent, string childName)
    {
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName && !child.gameObject.activeSelf)
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }

    // 소화기 오브젝트를 비활성화하는 함수
    private void DeactivateFireExtinguisher(GameObject extinguisher)
    {
        if (extinguisher != null && extinguisher.activeSelf)
        {
            extinguisher.SetActive(false);
        }
    }

    // "Fireextinguisher(fire)" 오브젝트를 활성화하는 함수
    private void ActivateFireObject()
    {
        if (fireObject != null && !fireObject.activeSelf)
        {
            fireObject.SetActive(true);
        }
    }

    private void PlayHandAnimations()
    {
        // 왼손 애니메이션 실행
        if (leftHandAnimator != null)
        {
            leftHandAnimator.SetTrigger("GrabFireExtinguisherLeft");
        }

        // 오른손 애니메이션 실행
        if (rightHandAnimator != null)
        {
            rightHandAnimator.SetTrigger("GrabFireExtinguisherRight");
        }
    }

	// 오른손을 왼손의 자식으로 설정하고 움직임을 잠그는 함수
    private void SetRightHandAsChildOfLeftHand()
    {
        if (leftHandTransform != null && rightHandTransform != null)
        {
            // 오른손을 왼손의 자식으로 설정
            rightHandTransform.SetParent(leftHandTransform);
        }
    }

    // 오른손의 움직임을 잠그는 함수
    private void LockRightHandMovement()
    {
        if (rightHandTransform != null)
        {

            // 오른손의 위치와 회전을 원하는 값으로 설정
            rightHandTransform.localPosition = new Vector3(0.200000003f, -0.0700000003f, -0.0299999993f);
            rightHandTransform.localRotation = Quaternion.Euler(346.300018f, 358.279999f, 334.600006f);

            // 오른손 모델 오브젝트 비활성화
            GameObject parentObject = GameObject.Find("RightHand");
            if (parentObject != null)
            {
                // 부모 오브젝트의 자식 중 이름을 가진 오브젝트 찾기
                Transform hand = parentObject.transform.Find("Right Hand Model");
                if (hand != null)
                {
                    hand.gameObject.SetActive(false); // 자식 오브젝트 비활성화
                }
            }
            isRightHandLocked = true; // 오른손 움직임 잠금 플래그 설정

        }
    }

    private void Update()
    {
        // firespreadObject가 활성화된 경우 XR Grab Interactable 컴포넌트를 활성화
        if (firespreadObject != null && firespreadObject.activeSelf)
        {
            if (grabInteractable != null)
            {
                grabInteractable.enabled = true;
            }
        }

        if (isRightHandLocked && rightHandTransform != null)
        {
            rightHandTransform.localPosition = new Vector3(0.200000003f, -0.0700000003f, -0.0299999993f);
            rightHandTransform.localRotation = Quaternion.Euler(346.300018f, 358.279999f, 334.600006f);
        }
    }

    private void OnDestroy()
    {
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.RemoveListener(OnSelectEntered);
        }
    }
}



