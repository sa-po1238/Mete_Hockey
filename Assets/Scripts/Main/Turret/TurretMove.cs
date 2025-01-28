using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMove : MonoBehaviour
{
    [SerializeField] float torqueForce = 0.5f; // トルク（回転力）の強さ
    [SerializeField] float maxAngularSpeed = 1.0f; // 最大回転速度
    [SerializeField] float dampingFactor = 0.99f;  // 減速の割合
    [SerializeField] float maxRotationAngle = 80.0f; // 最大回転角度
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        // トルクの初期化
        float torque = 0.0f;


        if (Input.GetKey("w") || Input.GetKey("a"))
        {
            torque = -torqueForce; // 左回転
        }
        else if (Input.GetKey("s") || Input.GetKey("d"))
        {
            torque = torqueForce; // 右回転
        }

        // 入力がない場合、回転速度をゼロに設定
        if (torque == 0.0f)
        {
            // 減速処理: 入力がない場合、徐々に回転を止める
            rb.angularVelocity *= dampingFactor;
        }
        else
        {
            // トルクを加える
            rb.AddTorque(Vector3.up * torque, ForceMode.Force);

            // 最大回転速度の制限
            if (rb.angularVelocity.magnitude > maxAngularSpeed)
            {
                rb.angularVelocity = rb.angularVelocity.normalized * maxAngularSpeed;
            }

            ClampRotation();
        }
    }

    // 最大回転角度の制限
    private void ClampRotation()
    {
        // 現在の回転角度を取得
        Vector3 currentRotation = transform.eulerAngles;

        // -180～180に変換
        if (currentRotation.y > 180.0f)
        {
            currentRotation.y -= 360.0f; // -180～180に変換
        }

        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxRotationAngle, maxRotationAngle);

        // 回転を適用
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }
}
