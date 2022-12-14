using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK {

	internal class Square {

		float[] vertices = new float[4 * 3] {
			0.5f, 0.5f, 0f,
			-0.5f, 0.5f, 0f,
			0.5f, -0.5f, 0f,
			-0.5f, -0.5f, 0f
		};

		// GLは三角形しか出力できないのでEBOを使って複数の三角形の頂点を組み合わせて作る
		// 頂点データでやるとリソースがもったいない?のでindicesを作って三角形のインデックスを指定してあげる
		uint[] indices = new uint[2 * 3] {
			0, 1, 2,
			1, 2, 3
		};

		int ElementBufferObject;
		int VertexBufferObject;
		int VertexArrayObject;

		Shader shader;

		internal Square(Shader shader) {
			this.shader = shader;

			VertexBufferObject = GL.GenBuffer();
			ElementBufferObject = GL.GenBuffer();

			VertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(VertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			var vertexAttribIndex = shader.GetAttribLocation("aPosition");

			// VBOを属性に登録
			GL.VertexAttribPointer(vertexAttribIndex, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(vertexAttribIndex); // 属性を有効化

			// EBOはこれを追加するだけ！
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
		}

		internal void Draw() {
			shader.Use();
			GL.BindVertexArray(VertexArrayObject);
			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
		}
	}
}
