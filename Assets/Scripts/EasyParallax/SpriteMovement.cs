using UnityEngine;

namespace EasyParallax
{
    /**
 * Moves a sprite along the X axes using a predefined speed
    * 翻訳 : 事前に定義された速度を使用して、スプライトをX軸に沿って移動します
 */
    public class SpriteMovement : MonoBehaviour
    {
        public MovementSpeedType movementSpeedType;

        [Tooltip("Used only if no movement speed type is specified")]
        public float speed = 1f;

        private void Awake()
        {
            if (movementSpeedType)
                speed = movementSpeedType.speed;
        }

        private void Update()
        {
            //Save the current position, so we can edit it
            //翻訳 : 現在の位置を保存して、編集できるようにします
            var newPosition = transform.position;
            //Move the position along the x axis by an amount that depends on the
            //翻訳 : 位置をx軸に沿って移動します。移動量は次のようになります
            //defined speed and the deltaTime, so we can get a framerate independent movement
            //翻訳 : 定義された速度とdeltaTimeに依存するため、フレームレートに依存しない移動が可能です
            newPosition.x -= speed * Time.deltaTime;
            //Update our position
            //翻訳 : 位置を更新します
            transform.position = newPosition;
        }
    }
}