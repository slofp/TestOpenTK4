#version 330 core
// 各シェーダーは上のようなバージョン指定で始まる
// ここのバージョン番号はOpenGLのバージョン(3.3以降)と一致する(420 -> 4.2)

// 頂点データをinキーワードで宣言。
// ベクトルデータ型(1～4)の座標データを格納
// location = 0は入力変数の具体的な位置
layout (location = 0) in vec3 aPosition;

// 頂点シェーダ
void main() {
	// vec3で入力されるが、期待してるのはvec4なので変換をする
	gl_Position = vec4(aPosition, 1.0);
}

// コメントアウトを消さないとだめ
