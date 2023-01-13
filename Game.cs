using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace TestOpenTK {
	internal class Game : GameWindow {

		// シェーダーデータ
		Shader shader;

		IDrawable drawObject;

		public Game(int width, int height, string title)
		: base(GameWindowSettings.Default, new NativeWindowSettings {
			Size = new Vector2i(width, height),
			Title = title
		}) {
		}

		// windowロード時に1度だけ呼び出される
		protected override void OnLoad() {
			base.OnLoad();

			shader = new Shader("shader.vert", "shader.flag");

			// windowの色
			GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);

			drawObject = new Triangle(shader);
		}

		protected override void OnUnload() {
			base.OnUnload();

			// シェーダーの開放を忘れずに
			shader.Dispose();
		}

		// レンダーする作業
		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			// OnLoadで指定されたClearColorを使用して
			// windowの色をクリアする
			// ※レンダリング時に最初に実行しないといけない
			GL.Clear(ClearBufferMask.ColorBufferBit);

			drawObject.Draw();

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
