﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Novels;

namespace Charlotte.Games
{
	public class SettingMenu : IDisposable
	{
		public DDSimpleMenu SimpleMenu;

		// <---- prm

		public static SettingMenu I;

		public SettingMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			DDSE[] seSamples = Ground.I.SE.テスト用s;

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"ゲームパッドのボタン設定",
					"キーボードのキー設定",
					"ウィンドウサイズ変更",
					"ＢＧＭ音量",
					"ＳＥ音量",
					"ノベルパートのメッセージ表示速度",
					"ノベルパートのスキップ設定 [ " +
						(Ground.I.スキップ設定 == Ground.スキップ設定_e.既読のみ ? "既読のみ" : "未読も含む") +
						" ]",
					"戻る",
				};

				selectIndex = this.SimpleMenu.Perform(40, 40, 40, 24, "設定", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
						this.SimpleMenu.PadConfig();
						break;

					case 1:
						this.SimpleMenu.PadConfig(true);
						break;

					case 2:
						this.SimpleMenu.WindowSizeConfig();
						break;

					case 3:
						this.SimpleMenu.VolumeConfig("ＢＧＭ音量", DDGround.MusicVolume, 0, 100, 1, 10, volume =>
						{
							DDGround.MusicVolume = volume;
							DDMusicUtils.UpdateVolume();
						},
						() => { }
						);
						break;

					case 4:
						this.SimpleMenu.VolumeConfig("ＳＥ音量", DDGround.SEVolume, 0, 100, 1, 10, volume =>
						{
							DDGround.SEVolume = volume;
							//DDSEUtils.UpdateVolume(); // old

							foreach (DDSE se in seSamples) // サンプルのみ音量更新
								se.UpdateVolume();
						},
						() =>
						{
							DDUtils.Random.ChooseOne(seSamples).Play();
						}
						);
						DDSEUtils.UpdateVolume(); // 全音量更新
						break;

					case 5:
						this.SimpleMenu.IntVolumeConfig(
							"ノベルパートのメッセージ表示速度",
							Ground.I.NovelMessageSpeed,
							NovelConsts.MESSAGE_SPEED_MIN,
							NovelConsts.MESSAGE_SPEED_MAX,
							1,
							2,
							speed => Ground.I.NovelMessageSpeed = speed,
							() => { }
							);
						break;

					case 6:
						Ground.I.スキップ設定 =
							Ground.I.スキップ設定 == Ground.スキップ設定_e.既読のみ ?
							Ground.スキップ設定_e.未読も含む :
							Ground.スキップ設定_e.既読のみ;
						break;

					case 7:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			DDEngine.FreezeInput();
		}
	}
}
