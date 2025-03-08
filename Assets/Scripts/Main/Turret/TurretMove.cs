using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMove : MonoBehaviour
{
    [SerializeField] float torqueForce = 0.5f; // トルク（回転力）の強さ
    [SerializeField] float maxAngularSpeed = 1.0f; // 最大回転速度
    [SerializeField] float maxRotationAngle = 80.0f; // 最大回転角度
    [SerializeField] float moveForce = 0.5f; // 移動の力の強さ
    [SerializeField] float maxMoveSpeed = 3.0f; // 最大移動速度
    [SerializeField] float stopThreshold = 0.05f; // 速度がこの値以下なら完全停止
    [SerializeField] float topLimit = 4f; // 上側の限界値
    [SerializeField] float bottomLimit = -4f; // 下側の限界値
    [SerializeField] float dampingFactor = 0.99f;  // 減速の割合(回転)
    [SerializeField] float moveDamping = 0.85f;  // 減速の割合(移動)

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        // トルクの初期化
        float torque = 0.0f;
        // 移動の初期化
        float move = 0.0f;

        if (Input.GetKey("a"))
        {
            torque = torqueForce; // 右回転
        }
        else if (Input.GetKey("d"))
        {
            torque = -torqueForce; // 左回転
        }
        else if (Input.GetKey("w"))
        {
            move = moveForce; // 上
        }
        else if (Input.GetKey("s"))
        {
            move = -moveForce; // 下
        }

        // 減速処理: 入力がない場合、徐々に回転を止める
        if (torque == 0.0f)
        {
            rb.angularVelocity *= dampingFactor;
        }
        else
        {
            // トルクを加える (Z軸方向にトルクを加える)
            rb.AddTorque(Vector3.forward * torque, ForceMode.Force);


            // 最大回転速度の制限
            if (rb.angularVelocity.magnitude > maxAngularSpeed)
            {
                rb.angularVelocity = rb.angularVelocity.normalized * maxAngularSpeed;
            }

            ClampRotation();
        }

        // 移動処理
        Vector3 newPosition = transform.position;

        // 上下移動を適用
        if (move != 0.0f)
        {
            newPosition.y += move * Time.deltaTime; // Z軸の移動を適用
        }

        // 新しい位置に移動
        transform.position = newPosition;

        // 減速処理: 入力がない場合、上下移動を止める
        if (move == 0.0f)
        {
            newPosition.y *= moveDamping; // 減速
            if (Mathf.Abs(newPosition.y) < stopThreshold)
            {
                newPosition.y = 0f; // 完全停止
            }
        }

        // 上下の移動範囲制限
        ClampPosition();
    }

    // 最大回転角度の制限
    private void ClampRotation()
    {
        // 現在の回転角度を取得
        Vector3 currentRotation = transform.eulerAngles;

        // -180～180に変換
        if (currentRotation.z > 180.0f)
        {
            currentRotation.z -= 360.0f; // -180～180に変換
        }

        currentRotation.z = Mathf.Clamp(currentRotation.z, -maxRotationAngle, maxRotationAngle);

        // 回転を適用
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }

    // 上下の移動範囲制限
    private void ClampPosition()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, bottomLimit, topLimit);
        transform.position = position;
    }
}
