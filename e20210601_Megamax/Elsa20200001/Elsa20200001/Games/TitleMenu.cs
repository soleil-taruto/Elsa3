﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameProgressMasters;
using Charlotte.Novels;

namespace Charlotte.Games
{
	public class TitleMenu : IDisposable
	{
		public static TitleMenu I;

		public TitleMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		#region DrawWall

		private DrawWallTask DrawWall = new DrawWallTask();

		private class DrawWallTask : DDTask
		{
			public bool TopMenuLeaved = false;

			public override IEnumerable<bool> E_Task()
			{
				DDTaskList el = new DDTaskList();

				//el.Add(SCommon.Supplier(this.Effect_0001(1, 2, 3)));
				//el.Add(SCommon.Supplier(this.Effect_0001(4, 5, 6)));
				//el.Add(SCommon.Supplier(this.Effect_0001(7, 8, 9)));

				for (int frame = 0; ; frame++)
				{
					DDDraw.SetBright(new I3Color(32, 0, 0));
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();

					if (!this.TopMenuLeaved)
					{
						DDPrint.SetBorder(new I3Color(128, 0, 0));
						DDPrint.SetPrint(30, 30, 0, 60);
						DDPrint.Print("ドレミーロックマン(仮)");
						DDPrint.Reset();
					}

					el.ExecuteAllTask_Reverse();

					yield return true;
				}
			}

			private IEnumerable<bool> Effect_0001(int dummy_01, int dummy_02, int dummy_03)
			{
				for (; ; )
					yield return true;
			}
		}

		#endregion

		#region TopMenu

		private TopMenuTask TopMenu = new TopMenuTask();

		private class TopMenuTask : DDTask
		{
			public const int ITEM_NUM = 4;
			public int SelectIndex = 0;

			public override IEnumerable<bool> E_Task()
			{
				Func<bool>[] drawItems = new Func<bool>[ITEM_NUM];

				for (int index = 0; index < ITEM_NUM; index++)
					drawItems[index] = SCommon.Supplier(this.E_DrawItem(index));

				for (; ; )
				{
					for (int index = 0; index < ITEM_NUM; index++)
						drawItems[index]();

					yield return true;
				}
			}

			private IEnumerable<bool> E_DrawItem(int selfIndex)
			{
				DDPicture picture = Ground.I.Picture.TitleMenuItems[selfIndex];

				const double ITEM_UNSEL_X = 160.0;
				const double ITEM_UNSEL_A = 0.5;
				const double ITEM_SEL_X = 180.0;
				const double ITEM_SEL_A = 1.0;
				const double ITEM_Y = 320.0;
				const double ITEM_Y_STEP = 50.0;

				double x = ITEM_SEL_X;
				double y = ITEM_Y + selfIndex * ITEM_Y_STEP;
				double a = ITEM_UNSEL_A;
				double realX = ITEM_UNSEL_X;
				double realY = y;
				double realA = a;

				for (; ; )
				{
					x = this.SelectIndex == selfIndex ? ITEM_SEL_X : ITEM_UNSEL_X;
					a = this.SelectIndex == selfIndex ? ITEM_SEL_A : ITEM_UNSEL_A;

					DDUtils.Approach(ref realX, x, 0.93);
					DDUtils.Approach(ref realA, a, 0.93);

					DDDraw.SetAlpha(realA);
					DDDraw.DrawCenter(picture, realX, realY);
					DDDraw.Reset();

					yield return true;
				}
			}
		}

		#endregion

		private DDSimpleMenu SimpleMenu;

		public void Perform()
		{
			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			DDEngine.FreezeInput();

			Ground.I.Music.Title.Play();

			this.SimpleMenu = new DDSimpleMenu()
			{
				BorderColor = new I3Color(64, 0, 0),
				WallDrawer = this.DrawWall.Execute,
			};

			this.TopMenu.SelectIndex = 0;

			for (; ; )
			{
				bool cheatFlag;

				{
					int bk_freezeInputFrame = DDEngine.FreezeInputFrame;
					DDEngine.FreezeInputFrame = 0;
					cheatFlag = 1 <= DDInput.DIR_6.GetInput();
					DDEngine.FreezeInputFrame = bk_freezeInputFrame;
				}

				if (DDInput.DIR_8.IsPound())
					this.TopMenu.SelectIndex--;

				if (DDInput.DIR_2.IsPound())
					this.TopMenu.SelectIndex++;

				this.TopMenu.SelectIndex += TopMenuTask.ITEM_NUM;
				this.TopMenu.SelectIndex %= TopMenuTask.ITEM_NUM;

				if (DDInput.A.GetInput() == 1) // ? 決定ボタン押下
				{
					switch (this.TopMenu.SelectIndex)
					{
						case 0:
							if (DDConfig.LOG_ENABLED && cheatFlag)
							{
								this.DrawWall.TopMenuLeaved = true;
								this.CheatMainMenu();
								this.DrawWall.TopMenuLeaved = false; // restore
							}
							else
							{
#if true // 暫定
								this.DrawWall.TopMenuLeaved = true;
								this.暫定スタートメニュー();
								this.DrawWall.TopMenuLeaved = false; // restore
#else
								this.LeaveTitleMenu();

								using (new GameProgressMaster())
								{
									GameProgressMaster.I.Perform();
								}
								this.ReturnTitleMenu();
#endif
							}
							break;

						case 1:
							{
#if true // 暫定
								this.DrawWall.TopMenuLeaved = true;
								this.暫定コンテニューメニュー();
								this.DrawWall.TopMenuLeaved = false; // restore
#else
								this.LeaveTitleMenu();

								using (new GameProgressMaster())
								{
									GameProgressMaster.I.Perform_コンテニュー();
								}
								this.ReturnTitleMenu();
#endif
							}
							break;

						case 2:
							{
								this.DrawWall.TopMenuLeaved = true;

								using (new SettingMenu())
								{
									SettingMenu.I.SimpleMenu = this.SimpleMenu;
									SettingMenu.I.Perform();
								}
								this.DrawWall.TopMenuLeaved = false; // restore
							}
							break;

						case 3:
							goto endMenu;

						default:
							throw new DDError();
					}
				}
				if (DDInput.B.GetInput() == 1) // ? キャンセルボタン押下
				{
					if (this.TopMenu.SelectIndex == TopMenuTask.ITEM_NUM - 1)
						break;

					this.TopMenu.SelectIndex = TopMenuTask.ITEM_NUM - 1;
				}

				this.DrawWall.Execute();
				this.TopMenu.Execute();

				DDEngine.EachFrame();
			}
		endMenu:
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.WallDrawer();
				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();
		}

		// 暫定メニュー_ここから

		private void 暫定スタートメニュー()
		{
			Action<string> a_gameStart = startMapName =>
			{
				this.LeaveTitleMenu();

				using (new WorldGameMaster())
				{
					WorldGameMaster.I.World = new World(startMapName);
					WorldGameMaster.I.Status = new GameStatus();
					WorldGameMaster.I.Perform();
				}
				this.ReturnTitleMenu();
			};

			for (; ; )
			{
				int selectIndex = this.SimpleMenu.Perform(40, 40, 40, 24, "ステージ選択(仮)", new string[]
				{
					"ステージ１",
					"ステージ２",
					"戻る",
				},
				0
				);

				switch (selectIndex)
				{
					case 0:
						a_gameStart("Stage_0001_v001\\t1001");
						break;

					case 1:
						a_gameStart("Stage_0002_v001\\t1001");
						break;

					case 2:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			;
		}

		private void 暫定コンテニューメニュー()
		{
			for (; ; )
			{
				int selectIndex = this.SimpleMenu.Perform(40, 40, 40, 24, "コンテニュー(仮)", new string[]
				{
					"未実装",
				},
				0
				);

				switch (selectIndex)
				{
					case 0:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			;
		}

		// 暫定メニュー_ここまで

		private void CheatMainMenu()
		{
			Action<string> a_gameStart = startMapName =>
			{
				this.LeaveTitleMenu();

				using (new WorldGameMaster())
				{
					WorldGameMaster.I.World = new World(startMapName);
					WorldGameMaster.I.Status = new GameStatus();
					WorldGameMaster.I.Perform();
				}
				this.ReturnTitleMenu();
			};

			for (; ; )
			{
				int selectIndex = this.SimpleMenu.Perform(40, 40, 40, 24, "開発デバッグ用メニュー", new string[]
				{
					"Stage_0001_v001",
					"Stage_0002_v001",
					"Stage_0003_v001",
					"ノベルパートテスト",
					"戻る",
				},
				0
				);

				switch (selectIndex)
				{
					case 0:
						a_gameStart("Stage_0001_v001\\t1001");
						break;

					case 1:
						a_gameStart("Stage_0002_v001\\t1001");
						break;

					case 2:
						a_gameStart("Stage_0002_v001\\t1001");
						break;

					case 3:
						{
							this.LeaveTitleMenu();

							using (new Novel())
							{
								Novel.I.Status.Scenario = new Scenario("テスト0001");
								Novel.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 4:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			;
		}

		private void LeaveTitleMenu()
		{
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.WallDrawer();
				DDEngine.EachFrame();
			}

			GC.Collect();
		}

		private void ReturnTitleMenu()
		{
			DDTouch.Touch(); // 曲再生の前に -- .Play() で Touch した曲を解放してしまわないように
			Ground.I.Music.Title.Play();

			//DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			GC.Collect();
		}
	}
}
