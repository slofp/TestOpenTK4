using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace TestOpenTK {
	internal class Game : GameWindow {

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

		// 頂点バッファオブジェクトのGPU用ポインタ？ID？
		int VertexBufferObject;


		public Game(int width, int height, string title)
		: base(GameWindowSettings.Default, new NativeWindowSettings {
			Size = new Vector2i(width, height),
			Title = title
		}) {
		}

		// windowロード時に1度だけ呼び出される
		protected override void OnLoad() {
			base.OnLoad();

			// windowの色
			GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);


			// 頂点バッファオブジェクトを生成
			VertexBufferObject = GL.GenBuffer();


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
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			// <補足>
			// メモリは(プログラムが終了すれば)自動開放する
			// ただ、手動で開放したい場合は以下のようにする
			// GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			// GL.DeleteBuffer(VertexBufferObject);

			// 0にバインドすると基本nullになる。
			// バインドせずにバッファーを変更してはならない。
		}

		// レンダーする作業
		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			// OnLoadで指定されたClearColorを使用して
			// windowの色をクリアする
			// ※レンダリング時に最初に実行しないといけない
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// ダブルバッファのスワップ関数
			// レンダーバッファ <-> 描画バッファ
			SwapBuffers();
		}

		// windowのサイズを変更するたびに実行される
		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			// Normalized Device Coordinate(正規化デバイス座標系)(略: NDC)
			// をwindowにマッピング
			// NDCは、仮想的な描画デバイス上の位置を記述する座標系を構成している
			// 左下が(0, 0) -> 右上が(1, 1)
			// 使い道としては、描画物を描画デバイス上の任意座標に配置したいときに使う
			GL.Viewport(0, 0, e.Width, e.Height);
		}

		// 様々な入力を受け取り、変数やら(ゲーム自体)を変更する作業
		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
				Close();
		}
	}
}
