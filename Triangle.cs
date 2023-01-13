using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK {

	internal class Triangle : IDrawable {

		// OpenGLは必ず3Dで描画される。(2Dにしたかったらz座標は0.0fにすべき)
		// パラメーターは点のようになっており
		// それぞれ、x -> y -> z で記述される。
		// 値は-1.0から1.0までのfloatである必要があり、
		// それ以外は描画されない(つまり規定に沿っていない座標点は無理)

		// これらはNDCに従って、描画される。
		// 座標はグラフのようになっていて、(0, 0)が中央に来るようになる。
		// 座標 -> NDC(ViewPoint) -> 実際の描画
		float[] vertices = {
			-0.5f, -0.5f, 0.0f, // 左下
			0.5f, -0.5f, 0.0f, // 右下
			0.0f,  0.5f, 0.0f  // 頂点
		};

		float[] verticesWithColor = {
			-0.5f, -0.5f, 0.0f, 1f, 0f, 0f,
			0.5f, -0.5f, 0.0f, 0f, 1f, 0f,
			0.0f,  0.5f, 0.0f, 0f, 0f, 1f
		};

		// 頂点バッファオブジェクトのGPU用ポインタ？ID？
		int VertexBufferObject;

		// 頂点配列のポインタ？
		int VertexArrayObject;

		Shader shader;

		/*
			基本的なOpenGLの描画は三角形
		 */
		internal Triangle(Shader shader) {
			this.shader = shader;

			// 頂点バッファオブジェクトを生成
			VertexBufferObject = GL.GenBuffer();

			// ここから↓

			// こいつを導入するだけで格段に楽になるらしい
			// 頂点配列オブジェクトの生成 -> バインド
			VertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(VertexArrayObject);


			// バインド
			// 頂点バッファオブジェクトはBufferTarget.ArrayBufferとなる
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

			// GPUへのmalloc的な？(BindBufferは単純な変数宣言に近い？)
			// とりあえず頂点データをバッファーに転送する作業っぽい
			// バッファタイプ -> データの大きさ(bytes) -> データ -> データをどのように管理するか(*1)
			// *1:
			// StaticDraw: データを全く変化しない。ただ、もしかしたら変更する可能性がある。
			// DynamicDraw: データが変わる可能性が高い。
			// StreamDraw: データがリアルタイムで変更される。
			GL.BufferData(BufferTarget.ArrayBuffer, verticesWithColor.Length * sizeof(float), verticesWithColor, BufferUsageHint.StaticDraw);

			// <補足>
			// メモリは(プログラムが終了すれば)自動開放する
			// ただ、手動で開放したい場合は以下のようにする
			// GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			// GL.DeleteBuffer(VertexBufferObject);

			// 0にバインドすると基本nullになる。
			// バインドせずにバッファーを変更してはならない。

			// 1, シェーダーのlocate = 0と指定した通りの設定？どの頂点属性を設定したいか
			// 2, 頂点属性の大きさ、今回はvec3なので3
			// 3, データ型の指定
			// 4, データを正規化するか(整数型のときにtrueにすると、0またはsignedで-1、floatに変換できたら1になる)
			// 5, ストライド、連続する頂点属性の間のスペースのこと。
			//    今回の位置データは float のちょうど 3 倍の大きさで離れているので
			//    その値をストライドとして指定。
			//    ※ 配列が密に詰まっている（次の頂点属性値の間にスペースがない）ことがわかっているので、
			//    OpenGLがストライドを決定するようにストライドを0に指定することもできた
			//    (これは値が密に詰まっているときのみ機能する)。
			//    頂点属性が増えるたびに各頂点属性の間隔を注意深く定義しなければならない。
			// 6, 位置データがどこから始まるかのオフセット
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// シェーダー色指定layout
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);


			// ↑ここまでの動作をすれば基本的に描画される
			// データが変更されない限りはロード時に行ったほうが良い?
		}

		public void Draw() {
			//GL.UseProgram();
			shader.Use();
			GL.BindVertexArray(VertexArrayObject);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
		}
	}
}
