using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK {

	internal class Shader : IDisposable {

		//コンパイル終了時のシェーダープログラムの場所
		int VertexShader;
		int FragmentShader;

		int Handle;

		// シェーダーコンパイル用クラス
		public Shader(string vertexPath, string fragmentPath) {
			// ソースコード読み込み
			string VertexShaderSource = File.ReadAllText(vertexPath);
			string FragmentShaderSource = File.ReadAllText(fragmentPath);

			// シェーダーオブジェクトの作成 + ソースコードをバインド
			VertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(VertexShader, VertexShaderSource);
			FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(FragmentShader, FragmentShaderSource);


			// シェーダーにコンパイル
			GL.CompileShader(VertexShader);

			// コンパイルステータスを取得し、エラーがあったらログを出力
			GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
			if (success == 0) {
				string infoLog = GL.GetShaderInfoLog(VertexShader);
				Console.WriteLine(infoLog);
			}

			// 以下同文

			GL.CompileShader(FragmentShader);

			GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
			if (success == 0) {
				string infoLog = GL.GetShaderInfoLog(FragmentShader);
				Console.WriteLine(infoLog);
			}

			// ハンドルでシェーダーをリンクさせるためのオブジェクトを生成
			Handle = GL.CreateProgram();

			// 先程コンパイルしたシェーダーをアタッチ
			GL.AttachShader(Handle, VertexShader);
			GL.AttachShader(Handle, FragmentShader);

			// リンクする
			GL.LinkProgram(Handle);

			// リンカの状態を取得、エラーがあったら出力
			GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
			if (success == 0) {
				string infoLog = GL.GetProgramInfoLog(Handle);
				Console.WriteLine(infoLog);
			}

			// Handle以外はいらないので削除
			GL.DetachShader(Handle, VertexShader);
			GL.DetachShader(Handle, FragmentShader);
			GL.DeleteShader(FragmentShader);
			GL.DeleteShader(VertexShader);
		}

		// このシェーダーを使用するときに使う
		public void Use() {
			GL.UseProgram(Handle);
		}

		// Dispose処理

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				GL.DeleteProgram(Handle);

				disposedValue = true;
			}
		}

		~Shader() {
			GL.DeleteProgram(Handle);
		}


		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
