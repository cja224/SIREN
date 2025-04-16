//�����ձ��� ������ �ڵ�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisherInteraction : MonoBehaviour
{
    public Animator leftHandAnimator;   // �޼� �ִϸ�����
    public Animator rightHandAnimator;  // ������ �ִϸ�����
    public XRRayInteractor rayInteractor; // XR Ray Interactor�� ���� ����
    public Transform parentObject;  // �θ� ������Ʈ�� �̸� �Ҵ� (�̸� �ʿ� ����)

    // �޼հ� ������ Transform�� �����ϵ��� ����
    public Transform leftHandTransform;  // �޼� Transform
    public Transform rightHandTransform; // ������ Transform

    private GameObject fireObject; // ��Ȱ��ȭ�� �ڽ� ������Ʈ�� ã�� ���� ����
    public GameObject firespreadObject; // "fire" 오브젝트를 참조하기 위한 변수
    private XRGrabInteractable grabInteractable; // XR Grab Interactable 컴포넌트 참조

    // �߰��� ����
    private bool isRightHandLocked = false; // ������ �������� ��״� �÷���

    private void Start()
    {
        // XRRayInteractor���� selectEntered �̺�Ʈ�� �����մϴ�.
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.AddListener(OnSelectEntered);
        }

        // �θ� ������Ʈ�� ���� ��Ȱ��ȭ�� �ڽ� ������Ʈ ã��
        fireObject = FindInactiveChildByName(parentObject, "Fireextinguisher(fire)");

        // XR Grab Interactable 컴포넌트 초기화 및 비활성화
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false; // 처음에는 비활성화
        }

        if (fireObject == null)
        {
            Debug.LogWarning("'Fireextinguisher(fire)' ������Ʈ�� �θ�κ��� ã�� �� �����ϴ�.");
        }
        else
        {
            Debug.Log("'Fireextinguisher(fire)' ������Ʈ�� ���������� �θ�κ��� �����Ǿ����ϴ�: " + fireObject.name);
            fireObject.SetActive(false); // ���� �� ��Ȱ��ȭ ���·� ����
        }

    }

    // selectEntered �̺�Ʈ�� �߻��� �� ȣ��Ǵ� �Լ�
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (firespreadObject != null && firespreadObject.activeSelf) // "fire" 오브젝트가 활성화 상태일 때만 실행
        {
            Debug.Log("OnSelectEntered 호출됨");

            if (args.interactableObject.transform.CompareTag("fireextinguisher"))
            {
                Debug.Log("소화기 선택됨: " + args.interactableObject.transform.name);

                DeactivateFireExtinguisher(args.interactableObject.transform.gameObject);
                ActivateFireObject();
                PlayHandAnimations();
                SetRightHandAsChildOfLeftHand();
                LockRightHandMovement();
            }
        }
    }

    // �θ� ������Ʈ���� ��Ȱ��ȭ�� �ڽ� ������Ʈ ã�� (�̸����� ã��)
    private GameObject FindInactiveChildByName(Transform parent, string childName)
    {
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName && !child.gameObject.activeSelf)
                {
                    return child.gameObject;  // ��Ȱ��ȭ�� �ڽ��� ��ȯ
                }
            }
        }
        return null;
    }

    // ��ȭ�� ������Ʈ�� ��Ȱ��ȭ�ϴ� �Լ�
    private void DeactivateFireExtinguisher(GameObject extinguisher)
    {
        if (extinguisher != null && extinguisher.activeSelf)
        {
            extinguisher.SetActive(false);
            Debug.Log("��ȭ�� ��Ȱ��ȭ��: " + extinguisher.name);
        }
    }

    // "Fireextinguisher(fire)" ������Ʈ�� Ȱ��ȭ�ϴ� �Լ�
    private void ActivateFireObject()
    {
        if (fireObject != null && !fireObject.activeSelf)
        {
            fireObject.SetActive(true);
            Debug.Log("'Fireextinguisher(fire)' ������Ʈ Ȱ��ȭ��: " + fireObject.name);
        }
        else
        {
            Debug.LogWarning("'Fireextinguisher(fire)' ������Ʈ�� null�̰ų� �̹� Ȱ��ȭ�� �����Դϴ�.");
        }
    }

    private void PlayHandAnimations()
    {
        // �޼� �ִϸ��̼� ����
        if (leftHandAnimator != null)
        {
            leftHandAnimator.SetTrigger("GrabFireExtinguisherLeft");
        }

        // ������ �ִϸ��̼� ����
        if (rightHandAnimator != null)
        {
            rightHandAnimator.SetTrigger("GrabFireExtinguisherRight");
        }
    }

    // �������� �޼��� �ڽ����� �����ϰ� �������� ��״� �Լ�
    private void SetRightHandAsChildOfLeftHand()
    {
        if (leftHandTransform != null && rightHandTransform != null)
        {
            // �������� �޼��� �ڽ����� ����
            rightHandTransform.SetParent(leftHandTransform);
            Debug.Log("�������� �޼��� �ڽ����� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("�޼� �Ǵ� ������ Transform�� �������� �ʾҽ��ϴ�.");
        }
    }

    
    // �������� �������� ��״� �Լ�
    private void LockRightHandMovement()
    {
        if (rightHandTransform != null)
        {
            
            // �������� ��ġ�� ȸ���� ���ϴ� ������ ����
            rightHandTransform.localPosition = new Vector3(0.200000003f, -0.0700000003f, -0.0299999993f);
            rightHandTransform.localRotation = Quaternion.Euler(346.300018f, 358.279999f, 334.600006f);

            // ������ �� ������Ʈ ��Ȱ��ȭ
            GameObject parentObject = GameObject.Find("RightHand");
            if (parentObject != null)
            {
                // �θ� ������Ʈ�� �ڽ� �� �̸��� ���� ������Ʈ ã��
                Transform hand = parentObject.transform.Find("Right Hand Model");
                if (hand != null)
                {
                    hand.gameObject.SetActive(false); // �ڽ� ������Ʈ ��Ȱ��ȭ
                }
            }

                // ������ ������ ��� �÷��� ����
                isRightHandLocked = true;

        }
        else
        {
            Debug.LogWarning("������ Transform�� �������� �ʾҽ��ϴ�.");
        }
    }

    // Update �Լ����� �������� ��ġ�� ����
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
