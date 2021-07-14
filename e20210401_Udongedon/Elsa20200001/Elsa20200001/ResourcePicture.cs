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

		//public DDPicture Udonge = DDPictureLoaders.Standard(@"dat\dairi\67252239_p0.png");

		public DDPicture 立ち絵_小悪魔_00 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p0.png");
		public DDPicture 立ち絵_小悪魔_01 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p1.png");
		public DDPicture 立ち絵_小悪魔_02 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p2.png");
		public DDPicture 立ち絵_小悪魔_03 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p3.png");
		public DDPicture 立ち絵_小悪魔_04 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p4.png");
		public DDPicture 立ち絵_小悪魔_05 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p5.png");
		public DDPicture 立ち絵_小悪魔_06 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p6.png");
		public DDPicture 立ち絵_小悪魔_07 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p7.png");
		public DDPicture 立ち絵_小悪魔_08 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p8.png");
		public DDPicture 立ち絵_小悪魔_09 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p9.png");
		public DDPicture 立ち絵_小悪魔_10 = DDPictureLoaders.Standard(@"dat\dairi\66051062_p10.png");
		public DDPicture 立ち絵_鍵山雛_00 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p0.png");
		public DDPicture 立ち絵_鍵山雛_01 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p1.png");
		public DDPicture 立ち絵_鍵山雛_02 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p2.png");
		public DDPicture 立ち絵_鍵山雛_03 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p3.png");
		public DDPicture 立ち絵_鍵山雛_04 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p4.png");
		public DDPicture 立ち絵_鍵山雛_05 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p5.png");
		public DDPicture 立ち絵_鍵山雛_06 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p6.png");
		public DDPicture 立ち絵_鍵山雛_07 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p7.png");
		public DDPicture 立ち絵_鍵山雛_08 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p8.png");
		public DDPicture 立ち絵_鍵山雛_09 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p9.png");
		public DDPicture 立ち絵_鍵山雛_10 = DDPictureLoaders.Standard(@"dat\dairi\67049351_p10.png");
		public DDPicture 立ち絵_メディスン_00 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p0.png");
		public DDPicture 立ち絵_メディスン_01 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p1.png");
		public DDPicture 立ち絵_メディスン_02 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p2.png");
		public DDPicture 立ち絵_メディスン_03 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p3.png");
		public DDPicture 立ち絵_メディスン_04 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p4.png");
		public DDPicture 立ち絵_メディスン_05 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p5.png");
		public DDPicture 立ち絵_メディスン_06 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p6.png");
		public DDPicture 立ち絵_メディスン_07 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p7.png");
		public DDPicture 立ち絵_メディスン_08 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p8.png");
		public DDPicture 立ち絵_メディスン_09 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p9.png");
		public DDPicture 立ち絵_メディスン_10 = DDPictureLoaders.Standard(@"dat\dairi\66537404_p10.png");
		public DDPicture 立ち絵_ルーミア_00 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p0.png");
		public DDPicture 立ち絵_ルーミア_01 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p1.png");
		public DDPicture 立ち絵_ルーミア_02 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p2.png");
		public DDPicture 立ち絵_ルーミア_03 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p3.png");
		public DDPicture 立ち絵_ルーミア_04 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p4.png");
		public DDPicture 立ち絵_ルーミア_05 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p5.png");
		public DDPicture 立ち絵_ルーミア_06 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p6.png");
		public DDPicture 立ち絵_ルーミア_07 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p7.png");
		public DDPicture 立ち絵_ルーミア_08 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p8.png");
		public DDPicture 立ち絵_ルーミア_09 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p9.png");
		public DDPicture 立ち絵_ルーミア_10 = DDPictureLoaders.Standard(@"dat\dairi\65997323_p10.png");
		public DDPicture 立ち絵_チルノ_00 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p0.png");
		public DDPicture 立ち絵_チルノ_01 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p1.png");
		public DDPicture 立ち絵_チルノ_02 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p2.png");
		public DDPicture 立ち絵_チルノ_03 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p3.png");
		public DDPicture 立ち絵_チルノ_04 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p4.png");
		public DDPicture 立ち絵_チルノ_05 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p5.png");
		public DDPicture 立ち絵_チルノ_06 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p6.png");
		public DDPicture 立ち絵_チルノ_07 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p7.png");
		public DDPicture 立ち絵_チルノ_08 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p8.png");
		public DDPicture 立ち絵_チルノ_09 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p9.png");
		public DDPicture 立ち絵_チルノ_10 = DDPictureLoaders.Standard(@"dat\dairi\66022662_p10.png");

		public DDPicture MessageWindow = DDPictureLoaders.Standard(@"dat\フキダシデザイン\e0165_1.png");

		public DDPicture メディスン = DDPictureLoaders.Standard(@"dat\メディスン\medicine.png");
		public DDPicture DotOther = DDPictureLoaders.RGBToTrans(@"dat\Shoot_old_Resource\th_fuuchi-sozai\img\dot-Other.png", new I3Color(0, 0, 0));
		public DDPicture チルノ = DDPictureLoaders.Standard(@"dat\チルノ\dot_cirno.png");

		public DDPicture P_KOAKUMA_P1 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p1.png");
		public DDPicture P_KOAKUMA_P3 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p3.png");
		public DDPicture P_KOAKUMA_TACHIE_00 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p8.png");
		public DDPicture P_KOAKUMA_TACHIE_01 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p9.png");
		public DDPicture P_KOAKUMA_TACHIE_02 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p10.png");
		public DDPicture P_KOAKUMA_TACHIE_03 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p11.png");
		public DDPicture P_KOAKUMA_TACHIE_04 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p12.png");
		public DDPicture P_KOAKUMA_TACHIE_05 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p13.png");
		public DDPicture P_KOAKUMA_TACHIE_06 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p14.png");
		public DDPicture P_KOAKUMA_TACHIE_07 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p15.png");
		public DDPicture P_KOAKUMA_TACHIE_08 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p16.png");
		public DDPicture P_KOAKUMA_WALL_TEXT = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p17.png");
		public DDPicture P_KOAKUMA_WALL = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\jiki-koakuma-sozai\19740345_big_p18.png");
		public DDPicture P_ENEMYDIE = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\bhmqm120\explosionA-color_10F_xp.png");
		public DDPicture P_ENEMYDIE_ABGR = DDPictureLoaders.SelectARGB(@"dat\Shoot_old_Resource\bhmqm120\explosionA-color_10F_xp.png", "ABGR");
		public DDPicture P_ENEMYSHOTDIE = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\sentou-effect-anime9\320x240\pipo-btleffect072.png");
		public DDPicture P_PLAYERDIE = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\sentou-effect-anime4\640x480\pipo-btleffect049.png");
		public DDPicture P_PUMPKIN = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\DVDM\Enemy11.png");
		public DDPicture P_PUMPKIN_AGRB = DDPictureLoaders.SelectARGB(@"dat\Shoot_old_Resource\DVDM\Enemy11.png", "AGRB");
		public DDPicture P_PUMPKIN_ARBG = DDPictureLoaders.SelectARGB(@"dat\Shoot_old_Resource\DVDM\Enemy11.png", "ARBG");
		public DDPicture P_TAMA = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\kuushot\kuushot.png");
		public DDPicture P_TAMA_B = DDPictureLoaders.RGBToTrans(@"dat\Shoot_old_Resource\kuushot\kuushot.png", new I3Color(0, 0, 0));
		public DDPicture P_BLUETILE_01 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\989-tileable-midnight-blue-grunge-patterns\tileable_midnight_blue_grunge_pattern_1.jpg");
		public DDPicture P_BLUETILE_02 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\989-tileable-midnight-blue-grunge-patterns\tileable_midnight_blue_grunge_pattern_2.jpg");
		public DDPicture P_BLUETILE_02_REDUCT4 = DDPictureLoaders.Reduct(@"dat\Shoot_old_Resource\speckboy\989-tileable-midnight-blue-grunge-patterns\tileable_midnight_blue_grunge_pattern_2.jpg", 4);
		public DDPicture P_BLUETILE_03 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\989-tileable-midnight-blue-grunge-patterns\tileable_midnight_blue_grunge_pattern_3.jpg");
		public DDPicture P_BW_ARMY = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-camo-1a.png");
		public DDPicture P_BW_NAVY = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-camo-1b.png");
		public DDPicture P_BW_PUMPKIN = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-55b.png");
		public DDPicture P_WALL_31001_前面 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-zebra-1a.png");
		public DDPicture P_WALL_31001_背面 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-35a.png");
		public DDPicture P_WALL_32001_前面 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern70c.png");
		public DDPicture P_WALL_32001_背面 = DDPictureLoaders.SelectARGB(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-24c.png", "ABGR");
		public DDPicture P_WALL_32011_背面 = DDPictureLoaders.SelectARGB(@"dat\Shoot_old_Resource\speckboy\pattern8-pattern-2\pattern8-pattern-24c.png", "ABRG");
		public DDPicture P_DIGITS_W = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\suuji-font\suuji16x32_06.png");
		public DDPicture P_DIGITS_DDY = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\suuji-font\suuji16x32_01.png");
		public DDPicture P_DIGITS_DY = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\suuji-font\suuji16x32_02.png");
		public DDPicture P_DIGITS_Y = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\suuji-font\suuji16x32_03.png");
		//public DDPicture P_WALL = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\run\22350006_big_p26.jpg");
		//public DDPicture P_FAIRYETC = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\th_fuuchi-sozai\img\dot-kaze-ti.png"); // del
		public DDPicture P_FAIRYETC = DDPictureLoaders.RGBToTrans(@"dat\Shoot_old_Resource\th_fuuchi-sozai\img\dot-kaze-ti.png", new I3Color(0, 0, 0));
		public DDPicture P_MAHOJIN_HAJIKE = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\sentou-effect-anime1\640x480\pipo-btleffect007.png");
		//public DDPicture P_TITLE_WALL = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\run\22350006_big_p20.jpg");
		public DDPicture P_FUJINBOSS = DDPictureLoaders.RGBToTrans(@"dat\Shoot_old_Resource\th_fuuchi-sozai\img\dot-kaze-ti2.png", new I3Color(0, 0, 0));
		public DDPicture P_HENYORI_LASER = DDPictureLoaders.Standard(@"dat\lazer.png");
		public DDPicture P_DVDM_BULLET_03 = DDPictureLoaders.Standard(@"dat\Shoot_old_Resource\DVDM\Bullet03_64x64.png");
	}
}
