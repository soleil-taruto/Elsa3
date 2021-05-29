using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourcePicture
	{
		public DDPicture Dummy = DDPictureLoaders.Standard(@"dat\General\Dummy.png");
		public DDPicture WhiteBox = DDPictureLoaders.Standard(@"dat\General\WhiteBox.png");
		public DDPicture WhiteCircle = DDPictureLoaders.Standard(@"dat\General\WhiteCircle.png");
		public DDPicture DummyScreen = DDPictureLoaders.Standard(@"dat\General\DummyScreen.png");

		public DDPicture Copyright = DDPictureLoaders.Standard(@"dat\Logo\Copyright.png");

		public DDPicture Title = DDPictureLoaders.Standard(@"dat\タイトル.png");

		public DDPicture[] TitleMenuItems = new DDPicture[]
		{
			DDPictureLoaders.Standard(@"dat\タイトルメニュー項目_スタート.png"),
			DDPictureLoaders.Standard(@"dat\タイトルメニュー項目_コンテニュー.png"),
			DDPictureLoaders.Standard(@"dat\タイトルメニュー項目_設定.png"),
			DDPictureLoaders.Standard(@"dat\タイトルメニュー項目_終了.png"),
		};

		//public DDPicture 箱から出る_背景 = DDPictureLoaders.Standard(@"dat\箱から出る\背景.png");
		public DDPicture 箱から出る_箱0001 = DDPictureLoaders.Standard(@"dat\箱から出る\箱0001.png").SetGlobally();
		public DDPicture 箱から出る_箱0002 = DDPictureLoaders.Standard(@"dat\箱から出る\箱0002.png").SetGlobally();

		//public DDPicture Novel_背景 = DDPictureLoaders.Standard(@"dat\Novel\背景.png");
		public DDPicture Novel_吹き出し = DDPictureLoaders.Standard(@"dat\Novel\フキダシデザイン\e0165_1.png").SetGlobally();
		public DDPicture Novel_吹き出しThink = DDPictureLoaders.Standard(@"dat\Novel\フキダシデザイン\e0347_1.png").SetGlobally();
		public DDPicture Novel_箱 = DDPictureLoaders.Standard(@"dat\Novel\箱.png").SetGlobally();
		public DDPicture Novel_少女_普 = DDPictureLoaders.Standard(@"dat\Novel\少女_普.png").SetGlobally();
		public DDPicture Novel_少女_怒 = DDPictureLoaders.Standard(@"dat\Novel\少女_怒.png").SetGlobally();

		public DDPicture WallPicture_0101 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0001.png").SetGlobally();
		public DDPicture WallPicture_0102 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0002.png").SetGlobally();
		public DDPicture WallPicture_0103 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0003.png").SetGlobally();
		public DDPicture WallPicture_0201 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0004.png").SetGlobally();
		public DDPicture WallPicture_0202 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0005.png").SetGlobally();
		public DDPicture WallPicture_0203 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0006.png").SetGlobally();
		public DDPicture WallPicture_0301 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0007.png").SetGlobally();
		public DDPicture WallPicture_0302 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0008.png").SetGlobally();
		public DDPicture WallPicture_0303 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0009.png").SetGlobally();
		public DDPicture WallPicture_0401 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0010.png").SetGlobally();
		public DDPicture WallPicture_0402 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0011.png").SetGlobally();
		public DDPicture WallPicture_0403 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0012.png").SetGlobally();
		public DDPicture WallPicture_0501 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0013.png").SetGlobally();
		public DDPicture WallPicture_0502 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0014.png").SetGlobally();
		public DDPicture WallPicture_0503 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0015.png").SetGlobally();
		public DDPicture WallPicture_0601 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0016.png").SetGlobally();
		public DDPicture WallPicture_0602 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0017.png").SetGlobally();
		public DDPicture WallPicture_0603 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0018.png").SetGlobally();
		public DDPicture WallPicture_0701 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0019.png").SetGlobally();
		public DDPicture WallPicture_0702 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0020.png").SetGlobally();
		public DDPicture WallPicture_0703 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0021.png").SetGlobally();
		public DDPicture WallPicture_0801 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0022.png").SetGlobally();
		public DDPicture WallPicture_0802 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0023.png").SetGlobally();
		public DDPicture WallPicture_0803 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0024.png").SetGlobally();
		public DDPicture WallPicture_0901 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0025.png").SetGlobally();
		public DDPicture WallPicture_0902 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0026.png").SetGlobally();
		public DDPicture WallPicture_0903 = DDPictureLoaders.Standard(@"dat\WallPicture\WallPicture_0027.png").SetGlobally();

		public DDPicture SnapshotIcon = DDPictureLoaders.Standard(@"dat\icoon-mono\カメラのアイコン素材 7.png").SetGlobally();

		public DDPicture WallFire = DDPictureLoaders.Standard(@"dat\WallFire.png").SetGlobally();

		public DDPicture Ending_生還_背景_01 = DDPictureLoaders.Standard(@"dat\なほ\nc171813.jpg").SetGlobally();
		public DDPicture Ending_生還_背景_02 = DDPictureLoaders.Standard(@"dat\ぱくたそ\BOB96P8110162.png").SetGlobally();
		public DDPicture Ending_復讐_背景_01 = DDPictureLoaders.Standard(@"dat\ぱくたそ\kazukiphotomon04.png").SetGlobally();

		public DDPicture WhiteWall = DDPictureLoaders.Standard(@"dat\WhiteWall.png").SetGlobally();
	}
}
