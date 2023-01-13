using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOpenTK {

	internal class TexturedTriangle : Triangle {

		/*
		 テクスチャの座標は左下が(0, 0)、右上が(1, 1)となります
		 */

		string TextureFile = "tx-wall.jpg";

		/*
		各頂点に対して(x, y)の形で指定する
		順番はおそらく頂点データの順番になる(今回では 左下->右上->上中央)
		 */
		float[] texCoords = {
			0f, 0f,
			1f, 0f,
			0.5f, 1f
		};

		internal TexturedTriangle(Shader shader) : base(shader) {
			/*
			 テクスチャの折り返し
			 デフォルトでは範囲外の座標は画像を折り返すようになっている
			 Repeat: テクスチャ画像を繰り返す(デフォルト)
			 MirroredRepeat: GL_REPEATと同じ、ただし繰り返し部分は画像をミラー表示する
			 ClampToEdge: 画像が引き伸ばされる感じになる
			 ClampToBorder: 範囲外の座標を指定の色で塗りつぶす

			 TextureParameterNameごとに指定することが可能
			 (x, y, x) は (s, t, p) というふうになって
			 今回の場合では TextureTarget.Texture2D なので TextureWrapS と TextureWrapT に指定している

			 TextureWrapMode は enum を int にキャストして使用すること

			 2Dのテクスチャの場合はTextureTarget.Texture2D
			 3Dのテクスチャの場合はTextureTarget.Texture3Dという風になる
			 */
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

			/*
			 ClampToBorderにした場合は追加でTextureBorderColorで色を指定しないといけない
			 */
			float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);


		}
	}
}
