Easy Parallax

This package allows you to easily add a parallax effect to your 2d game. It is most suitable for infinite runners, 
where your backgrounds move side to side infinitely.
Not a single line of code is needed to implement this asset!

How to implement:

1. Add the SpriteMovement component to all of your sprites that will move
2. Optionally add the SpriteDuplicator component to allow seamless sprite duplication using a pool, so that you never run out of objects
3. Create different MovementSpeedType Scriptable objects for your sprites. E.g. BackgroundFar, BackgroundClose, Foreground, etc.
4. Apply the MovementSpeedType to your objects (set the parameter in the SpriteMovement component)
5. Adjust the actual speed in the MovementSpeedType scriptable object. This allows for a centralized change in game speed without going over all objects.

翻訳
簡単な視差

このパッケージを使えば、2Dゲームに簡単に視差効果を加えることができます。無限ランナーに最適です、 
背景が左右に無限に動くようなゲームに最適です。
このアセットを実装するのに必要なコードは一行もありません！

実装方法

1. 移動するすべてのスプライトに、SpriteMovementコンポーネントを追加します。
2. オプションでSpriteDuplicatorコンポーネントを追加し、プールを使ってスプライトをシームレスに複製できるようにします。
3. スプライトに異なるMovementSpeedType Scriptableオブジェクトを作成します。例：BackgroundFar、BackgroundClose、Foregroundなど。
4. オブジェクトにMovementSpeedTypeを適用する（SpriteMovementコンポーネントでパラメータを設定する）。
5. スクリプト可能なMovementSpeedTypeオブジェクトで実際のスピードを調整する。これにより、すべてのオブジェクトを跨ぐことなく、ゲームスピードを集中的に変更することができます。

DeepL.com（無料版）で翻訳しました。