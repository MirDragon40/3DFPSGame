using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform Target;   // Ÿ���� �÷��̾�
    public float YDistance = 20f;  // ������ ���� .
    private Vector3 _initialEulerAngles;  // ���� ȸ���Ҷ� �̿�, �÷��̾� ��ġ�� �°� ȸ��

    private void Start()
    {
        _initialEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = Target.position;
        targetPosition.y += YDistance;

        transform.position = targetPosition;

        Vector3 targetEulerAngles = Target.eulerAngles;
        targetEulerAngles.x = _initialEulerAngles.x;
        targetEulerAngles.z = _initialEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;
    }
}
