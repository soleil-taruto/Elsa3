using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Designs;
using Charlotte.Games.Enemies;
using Charlotte.Games.Enemies.Events;
using Charlotte.GameProgressMasters;
using Charlotte.LevelEditors;

namespace Charlotte.Games
{
	public class Game : IDisposable
	{
		public Map Map;

		// <---- prm

		public enum EndStatus_e
		{
			NextStage = 1,
			ReturnToTitleMenu,
			死亡エンド,
			生還エンド,
			復讐エンド,
		}

		public EndStatus_e EndStatus = EndStatus_e.NextStage;

		// <---- ret

		public static Game I;

		private DDSubScreen SnapshotCountScreen;
		private double SnapshotCountScreen_A = 1.0;

		public Game()
		{
			I = this;

			this.SnapshotCountScreen = new DDSubScreen(130, 60, true);
		}

		public void Dispose()
		{
			this.SnapshotCountScreen.Dispose();
			this.SnapshotCountScreen = null;

			I = null;
		}

		public Player Player = new Player();

		private bool CamSlideMode; // ? モード中
		private int CamSlideCount;
		private int CamSlideX; // -1 ～ 1
		private int CamSlideY; // -1 ～ 1

		public int Frame;

		/// <summary>
		/// 残りスナップショット回数
		/// </summary>
		public int SnapshotCount = Ground.I.StartSnapshotCount;

		/// <summary>
		/// スナップショット
		/// null == リスポーン地点_未設定
		/// </summary>
		public Snapshot Snapshot = null;

		/// <summary>
		/// 敵が接近しているタイルはアルファ値を下げて描画する。
		/// その際、敵の接近を判定するために使用する。
		/// </summary>
		public DDList<D2Point> タイル接近_敵描画_Points = new DDList<D2Point>();

		/// <summary>
		/// 最終ゾーン情報
		/// </summary>
		public class FinalZoneInfo
		{
			public double Rate = 0.0;
			public Common.OnceHandler OH_Event9003 = new Common.OnceHandler();
			public Common.OnceHandler OH_Event9003B = new Common.OnceHandler();
			public Common.OnceHandler OH_Event9004 = new Common.OnceHandler();
			public bool Ending_復讐_突入 = false;
		}

		/// <summary>
		/// 最終ゾーン情報
		/// null == 最終ゾーン未侵入
		/// </summary>
		public FinalZoneInfo FinalZone = null;

		public bool Last行き先案内_Crashed_Start方面;
		public bool Last行き先案内_Crashed_Goal方面;
		public bool 行き先案内_Crashed_Start方面;
		public bool 行き先案内_Crashed_Goal方面;
		public double LastExec行き先案内_X = -SCommon.IMAX;
		public double LastExec行き先案内_Y = -SCommon.IMAX;

		public void Perform()
		{
			this.ReloadEnemies();

			// プレイヤー・ステータス反映(マップ入場時)
			{
				// none
			}

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain(10);

			DDEngine.FreezeInput();

			// 音楽 --> GameProgressMaster.Perform() で再生・停止している。

			Func<bool> f_reorderEnemies = SCommon.Supplier(this.E_ReorderEnemies());
			Func<bool> f_ゴミ回収 = SCommon.Supplier(this.E_ゴミ回収());

			bool goalFlag = false; // ? ゴールした。
			bool jumpLock = false; // ? ジャンプ・ロック // ジャンプしたらボタンを離すまでロックする。

			this.Respawn();

			for (this.Frame = 0; ; this.Frame++)
			{
				f_reorderEnemies();

				this.Last行き先案内_Crashed_Start方面 = this.行き先案内_Crashed_Start方面;
				this.Last行き先案内_Crashed_Goal方面 = this.行き先案内_Crashed_Goal方面;
				this.行き先案内_Crashed_Start方面 = false;
				this.行き先案内_Crashed_Goal方面 = false;

				if (DDInput.PAUSE.GetInput() == 1 || this.Dead_Pause)
				{
					this.Dead_Pause = false; // clear

					this.Pause();

					if (this.Pause_ReturnToTitleMenu)
					{
						this.EndStatus = EndStatus_e.ReturnToTitleMenu;
						break;
					}
					if (this.Pause_Respawn)
					{
						this.Snapshot = null;
						this.Respawn();
					}
				}
				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_RETURN) == 1)
				{
					this.DebugPause();
				}

				if (this.Player.DeadFrame == 0)
					this.カメラ位置調整(false);

				if (this.FinalZone != null)
				{
					DDUtils.Approach(ref this.FinalZone.Rate, 1.0, 0.997);

					if (this.FinalZone.Ending_復讐_突入)
					{
						this.EndStatus = EndStatus_e.復讐エンド;
						break;
					}
				}

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1) // エディットモード(デバッグ用)
				{
					this.Edit();
					this.ReloadEnemies();
					this.Frame = 0;
				}
				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_Q) == 1) // 強制クリア(デバッグ用)
				{
					if (Ground.I.CurrStageIndex == GameProgressMaster.FINAL_STAGE_INDEX) // ? 最終ステージ -> クリアせずに最終ゾーン手前へワープする。
					{
						if (this.Player.Y < 2900)
						{
							波紋効果.Clear();

							this.Player.X = 3150;
							this.Player.Y = 3000;
						}
					}
					else
					{
						goalFlag = true;
						break;
					}
				}
				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_W) == 1) // 色調整(デバッグ用)
				{
					this.色調整();
				}

				if (DDInput.R.GetInput() == 1) // リスポーン
				{
					if (this.IsRespawnable())
						this.Respawn();
					else
						this.リスポーンを阻止した();
				}
				if (DDInput.C.GetInput() == 1) // リスポーン地点_設置
				{
					if (
						this.FinalZone == null &&
						this.Player.DeadFrame == 0 && // ? プレイヤー死亡中ではない。
						this.Player.RebornFrame == 0 && // ? プレイヤー登場中ではない。
						(DDUtils.CountDown(ref this.SnapshotCount) || DDConfig.LOG_ENABLED) // デバッグ中は無制限
						)
					{
						this.TakeSnapshot();

						Ground.I.SE.Snapshot.Play();
						//波紋効果.Add(this.Player.X, this.Player.Y); // old
						SnapshotEffects.Perform();
					}
					else
					{
						this.スナップショットを阻止した();
					}
				}

				int plMove = 0; // プレイヤー移動 { -1, 0, 1 } == { 左, 移動ナシ, 右 }

				// プレイヤー入力
				{
					bool deadOrReborn = 1 <= this.Player.DeadFrame || 1 <= this.Player.RebornFrame;
					bool slow = false;
					bool camSlide = false;
					int jump = 0;

					// 入力抑止中であるか否かに関わらず左右の入力は受け付ける様にする。
					int freezeInputFrameBackup = DDEngine.FreezeInputFrame;
					DDEngine.FreezeInputFrame = 0;

					if (!deadOrReborn && 1 <= DDInput.DIR_4.GetInput())
					{
						plMove = -1;
					}
					if (!deadOrReborn && 1 <= DDInput.DIR_6.GetInput())
					{
						plMove = 1;
					}

					DDEngine.FreezeInputFrame = freezeInputFrameBackup; // restore

					if (1 <= DDInput.L.GetInput())
					{
						plMove = 0;
						camSlide = true;
					}
					if (!deadOrReborn && 1 <= DDInput.B.GetInput())
					{
						slow = true;
					}
					if (!deadOrReborn && 1 <= DDInput.A.GetInput())
					{
						jump = DDInput.A.GetInput();
					}

					if (plMove != 0)
						this.Player.MoveFrame++;
					else
						this.Player.MoveFrame = 0;

					this.Player.MoveSlow = plMove != 0 && slow;

					if (jump == 0)
						jumpLock = false;

					if (1 <= this.Player.JumpFrame)
					{
						const int JUMP_FRAME_MAX = 22;

						if (1 <= jump && this.Player.JumpFrame < JUMP_FRAME_MAX)
							this.Player.JumpFrame++;
						else
							this.Player.JumpFrame = 0;
					}
					else
					{
						// 事前入力 == 着地前の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。
						// 入力猶予 == 落下(地面から離れた)直後の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。

						const int 事前入力時間 = 10;
						const int 入力猶予時間 = 5;

						if (1 <= jump && jump < 事前入力時間 && this.Player.AirborneFrame < 入力猶予時間 && this.Player.JumpCount == 0 && !jumpLock)
						{
							this.Player.JumpCount = 1;
							this.Player.JumpFrame = 1;
							jumpLock = true;
						}
					}

					if (this.Player.JumpFrame == 1) // ? ジャンプ開始
					{
						Ground.I.SE.Jump.Play();
					}

					if (camSlide)
					{
						if (DDInput.DIR_4.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideX--;
						}
						if (DDInput.DIR_6.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideX++;
						}
						if (DDInput.DIR_8.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideY--;
						}
						if (DDInput.DIR_2.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideY++;
						}
						DDUtils.ToRange(ref this.CamSlideX, -1, 1);
						DDUtils.ToRange(ref this.CamSlideY, -1, 1);
					}
					else
					{
						if (this.CamSlideMode && this.CamSlideCount == 0)
						{
							this.CamSlideX = 0;
							this.CamSlideY = 0;
						}
						this.CamSlideCount = 0;
					}
					this.CamSlideMode = camSlide;
				}

				//startDead:
				if (1 <= this.Player.DeadFrame) // プレイヤー死亡中の処理
				{
					int frame = this.Player.DeadFrame - 1;

					if (GameConsts.PLAYER_DEAD_FRAME_MAX < frame)
					{
						if (this.FinalZone != null)
						{
							this.EndStatus = EndStatus_e.死亡エンド;
							break;
						}
						this.Respawn();
						goto endDead;
					}
					this.Player.DeadFrame++;

					// この時点でとりうる this.Player.DeadFrame の最大値は Consts.PLAYER_DEAD_FRAME_MAX + 2

					// ----

					if (frame == 0) // init
					{
						this.Dead();

						if (this.Dead_Respawn)
						{
							this.Respawn();
							goto endDead;
						}
					}
				}
			endDead:

				//startReborn:
				if (1 <= this.Player.RebornFrame) // プレイヤー再登場中の処理
				{
					int frame = this.Player.RebornFrame - 1;

					if (GameConsts.PLAYER_REBORN_FRAME_MAX < frame)
					{
						this.Player.RebornFrame = 0;
						DDGround.SystemTasks.Once(() => this.FreezeEnemy = false);
						goto endReborn;
					}
					this.Player.RebornFrame++;

					// この時点でとりうる this.Player.RebornFrame の最大値は Consts.PLAYER_REBORN_FRAME_MAX + 2

					// ----

					// none
				}
			endReborn:

				// プレイヤー移動
				if (
					this.Player.DeadFrame == 0 &&
					this.Player.RebornFrame == 0
					)
				{
					if (1 <= this.Player.MoveFrame)
					{
						double speed = 0.0;

						if (this.Player.MoveSlow)
						{
							speed = this.Player.MoveFrame * 0.2;
							DDUtils.Minim(ref speed, 3.0);
						}
						else
							speed = 7.0;

						speed *= plMove;

						DDUtils.Approach(ref this.Player.XSpeed, speed, 0.333);
					}
					else
						this.Player.XSpeed /= 2.0;

					this.Player.X += this.Player.XSpeed;

					const double 重力加速度 = 0.5;
					//const double 重力加速度 = 0.45; // del @ 2021.1.4
					const double 落下最高速度 = 7.0;
					const double ジャンプによる上昇_YSpeed = -7.0;

					if (1 <= this.Player.JumpFrame)
						this.Player.YSpeed = ジャンプによる上昇_YSpeed;
					else
						this.Player.YSpeed += 重力加速度;

					DDUtils.Minim(ref this.Player.YSpeed, 落下最高速度);

					this.Player.Y += this.Player.YSpeed; // 自由落下
				}

				// プレイヤー位置矯正
				if (
					this.Player.DeadFrame == 0 &&
					this.Player.RebornFrame == 0
					)
				{
					自機位置調整.Perform(ref this.Player.X, ref this.Player.Y, v => v.IsWall());

					if (自機位置調整.Touch == 自機位置調整.Touch_e.GROUND)
					{
						this.Player.JumpCount = 0;
						this.Player.AirborneFrame = 0;
						this.Player.YSpeed = Math.Min(0.0, this.Player.YSpeed);
					}
					else
					{
						this.Player.AirborneFrame++;

						if (自機位置調整.Touch == 自機位置調整.Touch_e.ROOF)
						{
							this.Player.JumpFrame = 0;
							//this.Player.YSpeed = Math.Max(0.0, this.Player.YSpeed);
							this.Player.YSpeed = Math.Abs(this.Player.YSpeed);
						}
					}
				}
				//endPlayer:

				// マップ外に出た。-> 適当に内側に押し戻しておく。
				{
					const double SPAN = 100.0;

					if (this.Player.X < 0.0) // ? マップの左側に出た。
					{
						this.Player.X += SPAN;
					}
					if (this.Map.W * GameConsts.TILE_W < this.Player.X) // ? マップの右側に出た。
					{
						this.Player.X -= SPAN;
					}
					if (this.Player.Y < 0.0) // ? マップの上側に出た。
					{
						this.Player.Y += SPAN;
					}
					if (this.Map.H * GameConsts.TILE_H < this.Player.Y) // ? マップの下側に出た。
					{
						this.Player.Y -= SPAN;
					}
				}

				// 画面遷移時の微妙なカメラ位置ズレ解消
				// -- スタート地点(入場地点)が地面と接していると、最初のフレームでプレイヤーは上に押し出されて(ゲームによっては)カメラの初期位置とズレてしまう。
				// ---- なのでこの場所で処理する。
				if (this.Frame == 0)
				{
					this.カメラ位置調整(true);
				}

				const double PL_CRASH_W = GameConsts.TILE_W - 1;
				const double PL_CRASH_H = GameConsts.TILE_H - 1;

				//DDCrash plCrash = DDCrashUtils.Point(new D2Point(this.Player.X, this.Player.Y)); // 点
				//DDCrash plCrash = DDCrashUtils.Rect_CenterSize(new D2Point(this.Player.X, this.Player.Y), new D2Size(Consts.TILE_W, Consts.TILE_H));
				DDCrash plCrash = DDCrashUtils.Rect_CenterSize(new D2Point(this.Player.X, this.Player.Y), new D2Size(PL_CRASH_W, PL_CRASH_H));

				// ====
				// 描画ここから
				// ====

				this.DrawWall();
				this.DrawTiles();
				this.Player.Draw();

				Game.I.タイル接近_敵描画_Points.Clear();

				// memo: DeadFlag をチェックするのは「当たり判定」から

				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					enemy.Crash = DDCrashUtils.None(); // reset
					enemy.Draw();
				}

				if (1 <= Ground.I.StartSnapshotCount) // 右下の残りスナップショット回数 -- 設定 0 なら常に表示しない
				{
					Respawnable_e respawnable = this.GetRespawnableStatus();

					Action<I2Size> a_draw = drawScreenSize =>
					{
						int dispSnapshotCount = this.SnapshotCount;
						I3Color dispColor = new I3Color(255, 255, 255);

						switch (respawnable)
						{
							case Respawnable_e.普通に可能:
								break;

							case Respawnable_e.絶対に不可ゾーン突入:
								return;

							case Respawnable_e.エンディング見たので可能:
								dispSnapshotCount = 0;
								break;

							case Respawnable_e.エンディング見てないので不可:
								dispSnapshotCount = 0;
								dispColor = new I3Color(255, 0, 0);
								break;
						}

						DDDraw.SetBright(dispColor);
						DDDraw.DrawCenter(Ground.I.Picture.SnapshotIcon, drawScreenSize.W - 92, drawScreenSize.H - 25);
						DDDraw.Reset();

						DDFontUtils.DrawString_XCenter(
							drawScreenSize.W - 35,
							drawScreenSize.H - 50,
							dispSnapshotCount.ToString("D2"),
							DDFontUtils.GetFont("03焚火-Regular", 50),
							false,
							dispColor
							);
					};

					if (this.CamSlideMode || this.CamSlideX != 0 || this.CamSlideY != 0)
					{
						DDUtils.Approach(ref this.SnapshotCountScreen_A, 0.1, 0.9);

						I2Size DRAW_SCREEN_SIZE = this.SnapshotCountScreen.GetSize();

						using (this.SnapshotCountScreen.Section())
						{
							DX.ClearDrawScreen();
							a_draw(DRAW_SCREEN_SIZE);
						}

						DDDraw.SetAlpha(this.SnapshotCountScreen_A);
						DDDraw.DrawSimple(
							this.SnapshotCountScreen.ToPicture(),
							DDConsts.Screen_W - DRAW_SCREEN_SIZE.W,
							DDConsts.Screen_H - DRAW_SCREEN_SIZE.H
							);
						DDDraw.Reset();
					}
					else
					{
						this.SnapshotCountScreen_A = 1.0;

						a_draw(new I2Size(DDConsts.Screen_W, DDConsts.Screen_H));
					}
				}

				if (this.当たり判定表示)
				{
					// 最後に描画されるように DDGround.EL.Add() する。

					DDGround.EL.Add(() =>
					{
						DDCurtain.DrawCurtain(-0.7);

						const double A = 0.7;

						DDCrashView.Draw(new DDCrash[] { plCrash }, new I3Color(255, 0, 0), 1.0);
						DDCrashView.Draw(this.Enemies.Iterate().Select(v => v.Crash), new I3Color(255, 255, 255), A);

						return false;
					});
				}

				// ====
				// 描画ここまで
				// ====

				// ====
				// 当たり判定ここから
				// ====

				// ゴール
				{
					const double R = 15.9;

					if (
						this.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.Player.X - R, this.Player.Y - R))).IsGoal() ||
						this.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.Player.X + R, this.Player.Y - R))).IsGoal() ||
						this.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.Player.X - R, this.Player.Y + R))).IsGoal() ||
						this.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.Player.X + R, this.Player.Y + R))).IsGoal()
						)
					{
						goalFlag = true;

						if (this.FinalZone != null) // ? 最終ステージ -> 曲を止める。
							DDMusicUtils.Stop();

						Ground.I.SE.Goal.Play();
						DDEngine.EachFrame(); // ゴールした瞬間を描画する。
						break; // 当たり判定まで進んでしまうと死亡判定と競合しそうなので、ここで break してしまう。
					}
				}

				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					// 衝突判定：敵 x 自機
					if (
						this.Player.DeadFrame == 0 && // ? プレイヤー死亡中ではない。
						this.Player.RebornFrame == 0 && // ? プレイヤー登場中ではない。
						!enemy.DeadFlag && // ? 敵：生存
						DDCrashUtils.IsCrashed(enemy.Crash, plCrash) // ? 衝突
						)
					{
						// ★ 自機_被弾ここから

#if true
						this.Player.DeadFrame = 1;
#else // test test test test test
						DDGround.EL.Add(SCommon.Supplier(Effects.中爆発(this.Player.X, this.Player.Y))); // 被弾が分かるように
#endif

						// ★ 自機_被弾ここまで
					}
				}

				// ====
				// 当たり判定ここまで
				// ====

				{
					// ? ゴール側からスタート側へ移動した。
					bool g2s =
						this.Last行き先案内_Crashed_Goal方面 &&
						!this.Last行き先案内_Crashed_Start方面 &&
						this.行き先案内_Crashed_Start方面;

					// ? スタート側からゴール側へ移動した。
					bool s2g =
						this.Last行き先案内_Crashed_Start方面 &&
						!this.Last行き先案内_Crashed_Goal方面 &&
						this.行き先案内_Crashed_Goal方面;

					if (g2s || s2g)
					{
						double x = Game.I.Player.X;
						double y = Game.I.Player.Y;

						// ? 前回と同じ場所
						if (
							Math.Abs(x - this.LastExec行き先案内_X) < 100 &&
							Math.Abs(y - this.LastExec行き先案内_Y) < 100
							)
						{
							// noop
						}
						else
						{
							this.LastExec行き先案内_X = x;
							this.LastExec行き先案内_Y = y;

							行き先案内_Effect(x, y, s2g);
						}
					}
				}

				for (int c = 0; c < 100; c++) // 回数_適当
				{
					MapCell cell = this.Map.GetCell(
						DDUtils.Random.GetInt(this.Map.W),
						DDUtils.Random.GetInt(this.Map.H)
						);

					//if (cell.IsDefault)
					//    throw null; // never

					cell.ColorPhaseShift *= 0.99;
				}

				f_ゴミ回収();

				this.Enemies.RemoveAll(v => v.DeadFlag);

				DDEngine.EachFrame();

				// ★★★ ゲームループの終点 ★★★
			}
			DDEngine.FreezeInput();

			// プレイヤー・ステータス反映(マップ退場時)
			{
				// noop
			}

			DDMain.KeepMainScreen();

			if (goalFlag) // ? ゴールした。
			{
				波紋効果.Add(this.Player.X, this.Player.Y);

				foreach (DDScene scene in DDSceneUtils.Create(30))
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
					DDEngine.EachFrame();
				}
				DDCurtain.SetCurtain(50, -1.0);

				foreach (DDScene scene in DDSceneUtils.Create(60))
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
					DDEngine.EachFrame();
				}

				if (this.FinalZone != null)
					this.EndStatus = EndStatus_e.生還エンド;
			}
			else // ? タイトルへ戻るを選択した。など
			{
				DDCurtain.SetCurtain(30, -1.0);

				foreach (DDScene scene in DDSceneUtils.Create(40))
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
					DDEngine.EachFrame();
				}
			}

			波紋効果.Clear();

			// ★★★ end of Perform() ★★★
		}

		private void 行き先案内_Effect(double x, double y, bool 正しいルート)
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.行き先案内(50, 0.3, 100, 正しいルート)));
			DDGround.EL.Add(SCommon.Supplier(Effects.行き先案内(60, 0.4, 200, 正しいルート)));
			DDGround.EL.Add(SCommon.Supplier(Effects.行き先案内(70, 0.5, 300, 正しいルート)));
			DDGround.EL.Add(SCommon.Supplier(Effects.行き先案内(80, 0.6, 400, 正しいルート)));
			DDGround.EL.Add(SCommon.Supplier(Effects.行き先案内(90, 0.7, 500, 正しいルート)));
		}

		public void TakeSnapshot()
		{
			this.Snapshot = new Snapshot();
			this.Snapshot.PlayerPosition = new D2Point(this.Player.X, this.Player.Y);
			this.Snapshot.PlayerVelocity = new D2Point(this.Player.XSpeed, this.Player.YSpeed);
			this.Snapshot.PlayerOtherStatus = new int[]
			{
				this.Player.MoveFrame,
				this.Player.MoveSlow ? 1 : 0,
				this.Player.JumpCount,
				this.Player.JumpFrame,
				this.Player.AirborneFrame
			};
			this.Snapshot.Enemies = this.Enemies.Iterate().Select(enemy => enemy.GetClone()).ToList();
		}

		#region 開発デバッグ用

		private void 色調整()
		{
			DDEngine.FreezeInput();

			Design_0001 design = (Design_0001)this.Map.Design; // 違う型なら落ちる。

			Common.ValueWrapper<int>[] items = new Common.ValueWrapper<int>[]
			{
				new Common.ValueWrapper<int>(() => design.Color_01.R, value => design.Color_01.R = value),
				new Common.ValueWrapper<int>(() => design.Color_01.G, value => design.Color_01.G = value),
				new Common.ValueWrapper<int>(() => design.Color_01.B, value => design.Color_01.B = value),
				new Common.ValueWrapper<int>(() => design.Color_02.R, value => design.Color_02.R = value),
				new Common.ValueWrapper<int>(() => design.Color_02.G, value => design.Color_02.G = value),
				new Common.ValueWrapper<int>(() => design.Color_02.B, value => design.Color_02.B = value),
				new Common.ValueWrapper<int>(() => design.Color_03.R, value => design.Color_03.R = value),
				new Common.ValueWrapper<int>(() => design.Color_03.G, value => design.Color_03.G = value),
				new Common.ValueWrapper<int>(() => design.Color_03.B, value => design.Color_03.B = value),
				new Common.ValueWrapper<int>(() => design.Color_A.R, value => design.Color_A.R = value),
				new Common.ValueWrapper<int>(() => design.Color_A.G, value => design.Color_A.G = value),
				new Common.ValueWrapper<int>(() => design.Color_A.B, value => design.Color_A.B = value),
				new Common.ValueWrapper<int>(() => design.Color_B.R, value => design.Color_B.R = value),
				new Common.ValueWrapper<int>(() => design.Color_B.G, value => design.Color_B.G = value),
				new Common.ValueWrapper<int>(() => design.Color_B.B, value => design.Color_B.B = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Arkanoid.R, value => design.EnemyColor_Arkanoid.R = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Arkanoid.G, value => design.EnemyColor_Arkanoid.G = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Arkanoid.B, value => design.EnemyColor_Arkanoid.B = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Cookie.R, value => design.EnemyColor_Cookie.R = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Cookie.G, value => design.EnemyColor_Cookie.G = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Cookie.B, value => design.EnemyColor_Cookie.B = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Death_A.R, value => design.EnemyColor_Death_A.R = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Death_A.G, value => design.EnemyColor_Death_A.G = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Death_A.B, value => design.EnemyColor_Death_A.B = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Death_B.R, value => design.EnemyColor_Death_B.R = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Death_B.G, value => design.EnemyColor_Death_B.G = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Death_B.B, value => design.EnemyColor_Death_B.B = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Pata.R, value => design.EnemyColor_Pata.R = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Pata.G, value => design.EnemyColor_Pata.G = value),
				new Common.ValueWrapper<int>(() => design.EnemyColor_Pata.B, value => design.EnemyColor_Pata.B = value),
			};

			string[] ITEM_NAMES = "1R:1G:1B:2R:2G:2B:3R:3G:3B:AR:AG:AB:BR:BG:BB:aR:aG:aB:cR:cG:cB:d1R:d1G:d1B:d2R:d2G:d2B:pR:pG:pB".Split(':');

			int selectIndex = 0;

			for (; ; )
			{
				if (DDInput.A.GetInput() == 1)
				{
					break;
				}

				if (DDInput.DIR_8.IsPound())
				{
					selectIndex--;
				}
				if (DDInput.DIR_2.IsPound())
				{
					selectIndex++;
				}

				selectIndex += items.Length;
				selectIndex %= items.Length;

				const int VAL_ADD_FAST = 15;

				bool valueChanged = false;

				if (DDInput.DIR_4.IsPound())
				{
					if (DDInput.B.GetInput() == 0)
						items[selectIndex].Value--;
					else
						items[selectIndex].Value -= VAL_ADD_FAST;

					valueChanged = true;
				}
				if (DDInput.DIR_6.IsPound())
				{
					if (DDInput.B.GetInput() == 0)
						items[selectIndex].Value++;
					else
						items[selectIndex].Value += VAL_ADD_FAST;

					valueChanged = true;
				}

				const int VALUE_MAX = 255;

				items[selectIndex].Value += VALUE_MAX + 1;
				items[selectIndex].Value %= VALUE_MAX + 1;

				if (valueChanged)
				{
					List<string> dest = new List<string>();

					for (int colorIndex = 0; colorIndex * 3 < items.Length; colorIndex++)
					{
						dest.Add(
							"\t\t\t\t" +
							"new I3Color(" +
							items[colorIndex * 3 + 0].Value + ", " +
							items[colorIndex * 3 + 1].Value + ", " +
							items[colorIndex * 3 + 2].Value +
							"),"
							);
					}
					File.WriteAllLines(@"C:\temp\Colors.txt", dest, Encoding.ASCII);
				}

				this.DrawWall();
				this.DrawTiles();

				DDPrint.SetPrint(20, 20, 17);
				DDPrint.SetBorder(new I3Color(0, 0, 0));

				for (int index = 0; index < items.Length; index++)
					DDPrint.PrintLine("[" + (index == selectIndex ? ">" : " ") + "] " + ITEM_NAMES[index] + " = " + items[index].Value);

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
		}

		#endregion

		private IEnumerable<bool> E_ReorderEnemies()
		{
			for (; ; )
			{
				for (int index = 1; index < this.Enemies.Count; index++)
				{
					if (!(this.Enemies[index - 1] is Enemy_Death) && this.Enemies[index] is Enemy_Death)
						this.Enemies.Swap(index - 1, index);

					if (index % 10 == 0) // 間隔は適当
						yield return true;
				}
				yield return true; // ループ内で1度も実行されない場合を想定
			}
		}

		/// <summary>
		/// あまりにもマップから離れすぎている敵・自弾の死亡フラグを立てる。
		/// </summary>
		/// <returns></returns>
		private IEnumerable<bool> E_ゴミ回収()
		{
			for (; ; )
			{
				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					if (this.IsProbablyEvacuated(enemy.X, enemy.Y))
						enemy.DeadFlag = true;

					yield return true;
				}
				yield return true; // ループ内で1度も実行されない場合を想定
			}
		}

		private bool Dead_Pause = false; // ? 死亡モーション中にポーズボタンを押した。
		private bool Dead_Respawn = false; // ? 死亡モーション中にリスポーンボタンを押した。

		/// <summary>
		/// 死亡モーション
		/// </summary>
		private void Dead()
		{
			// reset
			{
				this.Dead_Pause = false;
				this.Dead_Respawn = false;
			}

			Ground.I.SE.Miss.Play();

			DDMain.KeepMainScreen();

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);

				DDDraw.SetAlpha(0.3);
				DDDraw.SetBright(1.0, 0.0, 0.0);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
				DDDraw.Reset();

				if (DDInput.PAUSE.GetInput() == 1) // ? ポーズボタンが押された。-> 戻ってポーズ押下を発生させる。
				{
					this.Dead_Pause = true;
					return;
				}
				if (DDInput.R.GetInput() == 1) // ? リスポーン_ボタンが押された。
				{
					if (this.IsRespawnable())
					{
						if (this.FinalZone == null)
						{
							波紋効果.Add(this.Player.X, this.Player.Y); // リスポーン押下時も出す。
						}
						else
						{
							// noop
						}

						this.Dead_Respawn = true;
						return;
					}
					else
						this.リスポーンを阻止した();
				}
				DDEngine.EachFrame();
			}

			if (this.FinalZone == null)
			{
				波紋効果.Add(this.Player.X, this.Player.Y);
				Ground.I.SE.Death.Play();
			}
			else
			{
				// noop
			}
		}

		private bool IsRespawnable()
		{
			return
				this.FinalZone == null ||
				(
					10 <= Ground.I.ReachedStageIndex && // ? 1度はエンディングに到達している。
					!this.FinalZone.OH_Event9003.Entered
				);
		}

		private enum Respawnable_e
		{
			普通に可能 = 1,
			絶対に不可ゾーン突入,
			エンディング見たので可能,
			エンディング見てないので不可,
		}

		/// <summary>
		/// リスポーン可能状態を返す。
		/// IsRespawnableの詳細ver
		/// </summary>
		/// <returns>リスポーン可能状態</returns>
		private Respawnable_e GetRespawnableStatus()
		{
			if (this.FinalZone == null)
				return Respawnable_e.普通に可能;

			if (this.FinalZone.OH_Event9003.Entered)
				return Respawnable_e.絶対に不可ゾーン突入;

			if (10 <= Ground.I.ReachedStageIndex) // ? 1度はエンディングに到達している。
				return Respawnable_e.エンディング見たので可能;

			return Respawnable_e.エンディング見てないので不可;
		}

		private void リスポーンを阻止した()
		{
			Ground.I.SE.拒否.Play();
		}

		private void スナップショットを阻止した()
		{
			Ground.I.SE.拒否.Play();
		}

		private void Respawn()
		{
			if (this.Snapshot == null)
				this.RespawnRestart();
			else
				this.RespawnFromSnapshot(this.Snapshot);
		}

		/// <summary>
		/// スタート地点からのリスポーン
		/// </summary>
		private void RespawnRestart()
		{
			this.RespawnCommon();

			// デフォルトの「プレイヤーのスタート地点」
			// -- マップの中央
			this.Player.X = this.Map.W * GameConsts.TILE_W / 2.0;
			this.Player.Y = this.Map.H * GameConsts.TILE_H / 2.0;

			{
				int x;
				int y;

				if (this.Map.FindCell(out x, out y, v => v.Kind == MapCell.Kind_e.START))
				{
					this.Player.X = x * GameConsts.TILE_W + GameConsts.TILE_W / 2;
					this.Player.Y = y * GameConsts.TILE_H + GameConsts.TILE_H / 2;
				}
			}

			for (int x = 0; x < this.Map.W; x++)
				for (int y = 0; y < this.Map.H; y++)
					this.Map.GetCell(x, y).ColorPhaseShift = 0.0;

			this.Player.XSpeed = 0.0;
			this.Player.YSpeed = 0.0;

			this.Player.DeadFrame = 0; // 死亡時の処理から呼び出された場合用
			this.Player.RebornFrame = 1;
			this.Player.AirborneFrame = SCommon.IMAX / 2; // リスポーン＆ジャンプ_抑止のため

			this.CamSlideX = 0;
			this.CamSlideY = 0;

			this.SnapshotCount = Ground.I.StartSnapshotCount; // スナップショット_残り回数_復活

			this.ReloadEnemies();

			波紋効果.Add(this.Player.X, this.Player.Y);
			Ground.I.SE.Reborn.Play();
		}

		/// <summary>
		/// リスポーン地点からのリスポーン
		/// </summary>
		/// <param name="ss"></param>
		private void RespawnFromSnapshot(Snapshot ss)
		{
			this.RespawnCommon();

			this.Player.X = ss.PlayerPosition.X;
			this.Player.Y = ss.PlayerPosition.Y;

			this.Player.XSpeed = ss.PlayerVelocity.X;
			this.Player.YSpeed = ss.PlayerVelocity.Y;

			{
				int c = 0;

				this.Player.MoveFrame = ss.PlayerOtherStatus[c++];
				this.Player.MoveSlow = ss.PlayerOtherStatus[c++] != 0;
				this.Player.JumpCount = ss.PlayerOtherStatus[c++];
				this.Player.JumpFrame = ss.PlayerOtherStatus[c++];
				this.Player.AirborneFrame = ss.PlayerOtherStatus[c++];
			}

			this.Player.DeadFrame = 0; // 死亡時の処理から呼び出された場合用
			this.Player.RebornFrame = 1;
			this.Player.AirborneFrame = SCommon.IMAX / 2; // リスポーン＆ジャンプ_抑止のため

			this.CamSlideX = 0;
			this.CamSlideY = 0;

			this.Enemies = new DDList<Enemy>(ss.Enemies.Select(enemy => enemy.GetClone()).ToList());
			this.FreezeEnemy = true;

			波紋効果.Add(this.Player.X, this.Player.Y);
			Ground.I.SE.Reborn.Play();
		}

		private void RespawnCommon()
		{
			// Enemy_Meteor による地形変更を戻す。
			{
				for (int x = 0; x < this.Map.W; x++)
				{
					for (int y = 0; y < this.Map.H; y++)
					{
						MapCell cell = this.Map.GetCell(x, y);

						if (cell.KindOrig != null)
						{
							cell.Kind = cell.KindOrig.Value;
							cell.KindOrig = null;
						}
					}
				}
			}

			if (this.FinalZone != null) // ? 最終ゾーンからのリスポーン
			{
				this.FinalZone = new FinalZoneInfo(); // reset
				//this.Map.Design = GameProgressMaster.I.GetFinalMap().Design; // デザイン変更後はリスポーンできないので、不要なはず

				DDGround.EL.Add(SCommon.Supplier(Effects.Liteフラッシュ()));
			}

			this.LastExec行き先案内_X = -SCommon.IMAX;
			this.LastExec行き先案内_Y = -SCommon.IMAX;
		}

		private void カメラ位置調整(bool 一瞬で)
		{
			double targCamX = this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3);
			double targCamY = this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3);

			DDUtils.ToRange(ref targCamX, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
			DDUtils.ToRange(ref targCamY, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

			if (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H < GameConsts.TILE_H) // ? カメラの縦の可動域が1タイルより狭い場合
				targCamY = (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H) / 2; // 中心に合わせる。

			DDUtils.Approach(ref DDGround.Camera.X, targCamX, 一瞬で ? 0.0 : 0.8);
			DDUtils.Approach(ref DDGround.Camera.Y, targCamY, 一瞬で ? 0.0 : 0.8);

			//DDUtils.ToRange(ref DDGround.Camera.X, 0.0, this.Map.W * Consts.TILE_W - DDConsts.Screen_W);
			//DDUtils.ToRange(ref DDGround.Camera.Y, 0.0, this.Map.H * Consts.TILE_H - DDConsts.Screen_H);

			DDGround.ICamera.X = SCommon.ToInt(DDGround.Camera.X);
			DDGround.ICamera.Y = SCommon.ToInt(DDGround.Camera.Y);
		}

		#region Edit

		private void Edit()
		{
			this.Map.Load(); // ゲーム中にマップを書き換える場合があるので、再ロードする。

			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(true);
			LevelEditor.ShowDialog();

			int lastMouseX = DDMouse.X;
			int lastMouseY = DDMouse.Y;

			for (; ; )
			{
				if (LevelEditor.Dlg.XPressed)
					break;

				// 廃止
				//if (DDKey.GetInput(DX.KEY_INPUT_E) == 1)
				//    break;

				I2Point cellPos = GameCommon.ToTablePoint(
					DDGround.Camera.X + DDMouse.X,
					DDGround.Camera.Y + DDMouse.Y
					);

				MapCell cell = Game.I.Map.GetCell(cellPos);

				if (cell.IsDefault)
				{
					// noop
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT) && 1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL)) // 左シフト・コントロール押下 -> 塗り潰し / none
				{
					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
						this.Map.Save(); // 失敗を想定して、セーブしておく

						MapCell.Kind_e kind = LevelEditor.Dlg.GetKind();

						if (kind != cell.Kind)
						{
							MapCell.Kind_e targetKind = cell.Kind; // cell.Kind は this.EditFill で変更される。

							this.EditFill(
								cellPos,
								v => v.Kind == targetKind,
								v => v.Kind = kind
								);
						}
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT)) // 左シフト押下 -> 移動 / none
				{
					if (1 <= DDMouse.L.GetInput())
					{
						DDGround.Camera.X -= DDMouse.X - lastMouseX;
						DDGround.Camera.Y -= DDMouse.Y - lastMouseY;

						DDUtils.ToRange(ref DDGround.Camera.X, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
						DDUtils.ToRange(ref DDGround.Camera.Y, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

						DDGround.ICamera.X = SCommon.ToInt(DDGround.Camera.X);
						DDGround.ICamera.Y = SCommon.ToInt(DDGround.Camera.Y);
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL)) // 左コントロール押下 -> スポイト / none
				{
					if (1 <= DDMouse.L.GetInput())
					{
						LevelEditor.Dlg.SetKind(cell.Kind);
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LALT)) // 左 ALT 押下 -> 自機ワープ / none
				{
					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
						this.Player.X = cellPos.X * GameConsts.TILE_W + GameConsts.TILE_W / 2;
						this.Player.Y = cellPos.Y * GameConsts.TILE_H + GameConsts.TILE_H / 2;

						DDGround.EL.Add(SCommon.Supplier(Effects.中爆発(this.Player.X, this.Player.Y))); // アクションが分かるように
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else // 編集モード
				{
					if (1 <= DDMouse.L.GetInput())
					{
						cell.Kind = LevelEditor.Dlg.GetKind();
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						cell.Kind = MapCell.Kind_e.EMPTY;
					}
				}

				if (DDKey.GetInput(DX.KEY_INPUT_S) == 1) // S キー --> Save
				{
					this.Map.Save();

					// 表示
					{
						int endFrame = DDEngine.ProcFrame + 60;

						DDGround.EL.Add(() =>
						{
							DDPrint.SetPrint(0, 16);
							DDPrint.SetBorder(new I3Color(0, 0, 0));
							DDPrint.Print("セーブしました...");
							DDPrint.Reset();

							return DDEngine.ProcFrame < endFrame;
						});
					}
				}
				if (DDKey.GetInput(DX.KEY_INPUT_L) == 1) // L キー --> Load
				{
					this.Map.Load();

					// 表示
					{
						int endFrame = DDEngine.ProcFrame + 60;

						DDGround.EL.Add(() =>
						{
							DDPrint.SetPrint(0, 16);
							DDPrint.SetBorder(new I3Color(0, 0, 0));
							DDPrint.Print("ロードしました...");
							DDPrint.Reset();

							return DDEngine.ProcFrame < endFrame;
						});
					}
				}

				DDCurtain.DrawCurtain();

				LevelEditor.DrawTiles();

				lastMouseX = DDMouse.X;
				lastMouseY = DDMouse.Y;

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(false);
			LevelEditor.CloseDialog();

			this.Map.Save(); // ★★★ マップをセーブする ★★★
		}

		private void EditFill(I2Point targetPos, Predicate<MapCell> isFillable, Action<MapCell> fill)
		{
			Queue<I2Point> q = new Queue<I2Point>();

			q.Enqueue(targetPos);

			while (1 <= q.Count)
			{
				I2Point cellPos = q.Dequeue();
				MapCell cell = this.Map.GetCell(cellPos);

				if (!cell.IsDefault && isFillable(cell))
				{
					fill(cell);

					q.Enqueue(new I2Point(cellPos.X - 1, cellPos.Y));
					q.Enqueue(new I2Point(cellPos.X + 1, cellPos.Y));
					q.Enqueue(new I2Point(cellPos.X, cellPos.Y - 1));
					q.Enqueue(new I2Point(cellPos.X, cellPos.Y + 1));
				}
			}
		}

		#endregion

		private void DrawWall()
		{
			double cam_xRate;
			double cam_yRate;

#if true // 移動量とレートの増分が XY 同じになるようにする。
			{
				double cam_x = DDGround.Camera.X;
				double cam_y = DDGround.Camera.Y;

				double cam_w = this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W;
				double cam_h = this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H;

				double cam_wh = Math.Max(cam_w, cam_h);

				cam_x += (cam_wh - cam_w) / 2.0;
				cam_y += (cam_wh - cam_h) / 2.0;

				cam_xRate = cam_x / cam_wh;
				cam_yRate = cam_y / cam_wh;
			}
#else
			cam_xRate = DDGround.Camera.X / (this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
			cam_yRate = DDGround.Camera.Y / (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);
#endif

			this.Map.Design.DrawWall(
				DDGround.Camera.X,
				DDGround.Camera.Y,
				cam_xRate,
				cam_yRate
				);
		}

		private void DrawTiles()
		{
			int w = this.Map.W;
			int h = this.Map.H;

			int cam_l = DDGround.ICamera.X;
			int cam_t = DDGround.ICamera.Y;
			int cam_r = cam_l + DDConsts.Screen_W;
			int cam_b = cam_t + DDConsts.Screen_H;

			I2Point lt = GameCommon.ToTablePoint(cam_l, cam_t);
			I2Point rb = GameCommon.ToTablePoint(cam_r, cam_b);

			// マージン付与
			// -- マップセルの範囲をはみ出て描画されるタイルのためにマージンを付加する。
			{
				const int MARGIN = 2;

				lt.X -= MARGIN;
				lt.Y -= MARGIN;
				rb.X += MARGIN;
				rb.Y += MARGIN;
			}

			lt.X = SCommon.ToRange(lt.X, 0, w - 1);
			lt.Y = SCommon.ToRange(lt.Y, 0, h - 1);
			rb.X = SCommon.ToRange(rb.X, 0, w - 1);
			rb.Y = SCommon.ToRange(rb.Y, 0, h - 1);

			for (int x = lt.X; x <= rb.X; x++)
			{
				for (int y = lt.Y; y <= rb.Y; y++)
				{
					MapCell cell = this.Map.Table[x, y];

					int tileX = x * GameConsts.TILE_W + GameConsts.TILE_W / 2;
					int tileY = y * GameConsts.TILE_H + GameConsts.TILE_H / 2;

					this.Map.Design.DrawTile(cell, x, y, tileX - cam_l, tileY - cam_t);
				}
			}
		}

		public DDList<Enemy> Enemies = new DDList<Enemy>();
		public bool FreezeEnemy = false;

		private void ReloadEnemies()
		{
			this.Enemies.Clear();

			for (int x = 0; x < this.Map.W; x++)
			{
				for (int y = 0; y < this.Map.H; y++)
				{
					MapCell.Kind_e kind = this.Map.Table[x, y].Kind;

					D2Point pos = new D2Point(
						x * GameConsts.TILE_W + GameConsts.TILE_W / 2,
						y * GameConsts.TILE_H + GameConsts.TILE_H / 2
						);

					switch (kind)
					{
						case MapCell.Kind_e.DEATH: this.Enemies.Add(new Enemy_Death(pos)); break;

						case MapCell.Kind_e.ARKANOID_1: this.Enemies.Add(new Enemy_Arkanoid(pos, 1)); break;
						case MapCell.Kind_e.ARKANOID_2: this.Enemies.Add(new Enemy_Arkanoid(pos, 2)); break;
						case MapCell.Kind_e.ARKANOID_3: this.Enemies.Add(new Enemy_Arkanoid(pos, 3)); break;
						case MapCell.Kind_e.ARKANOID_4: this.Enemies.Add(new Enemy_Arkanoid(pos, 4)); break;
						case MapCell.Kind_e.ARKANOID_6: this.Enemies.Add(new Enemy_Arkanoid(pos, 6)); break;
						case MapCell.Kind_e.ARKANOID_7: this.Enemies.Add(new Enemy_Arkanoid(pos, 7)); break;
						case MapCell.Kind_e.ARKANOID_8: this.Enemies.Add(new Enemy_Arkanoid(pos, 8)); break;
						case MapCell.Kind_e.ARKANOID_9: this.Enemies.Add(new Enemy_Arkanoid(pos, 9)); break;

						case MapCell.Kind_e.COOKIE_時計回り_1: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 3)); break;
						case MapCell.Kind_e.COOKIE_時計回り_2: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 2)); break;
						case MapCell.Kind_e.COOKIE_時計回り_3: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 1)); break;
						case MapCell.Kind_e.COOKIE_時計回り_4: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 4)); break;
						case MapCell.Kind_e.COOKIE_時計回り_6: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 0)); break;
						case MapCell.Kind_e.COOKIE_時計回り_7: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 5)); break;
						case MapCell.Kind_e.COOKIE_時計回り_8: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 6)); break;
						case MapCell.Kind_e.COOKIE_時計回り_9: this.Enemies.Add(new Enemy_Cookie(pos, Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 7)); break;

						case MapCell.Kind_e.COOKIE_反時計回り_1: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 3)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_2: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 2)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_3: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 1)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_4: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 4)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_6: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 0)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_7: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 5)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_8: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 6)); break;
						case MapCell.Kind_e.COOKIE_反時計回り_9: this.Enemies.Add(new Enemy_Cookie(pos, -Enemy_Cookie.IROT_SPEED, Enemy_Cookie.IROT_360 / 8 * 7)); break;

						case MapCell.Kind_e.PATA_L: this.Enemies.Add(new Enemy_Pata(pos, -Enemy_Pata.SPEED_NORMAL)); break;
						case MapCell.Kind_e.PATA_L_SLOW: this.Enemies.Add(new Enemy_Pata(pos, -Enemy_Pata.SPEED_SLOW)); break;
						case MapCell.Kind_e.PATA_L_FAST: this.Enemies.Add(new Enemy_Pata(pos, -Enemy_Pata.SPEED_FAST)); break;
						case MapCell.Kind_e.PATA_R: this.Enemies.Add(new Enemy_Pata(pos, Enemy_Pata.SPEED_NORMAL)); break;
						case MapCell.Kind_e.PATA_R_SLOW: this.Enemies.Add(new Enemy_Pata(pos, Enemy_Pata.SPEED_SLOW)); break;
						case MapCell.Kind_e.PATA_R_FAST: this.Enemies.Add(new Enemy_Pata(pos, Enemy_Pata.SPEED_FAST)); break;

						case MapCell.Kind_e.EVENT_9001: this.Enemies.Add(new Enemy_Event9001(pos)); break;
						case MapCell.Kind_e.EVENT_9002_2: this.Enemies.Add(new Enemy_Event9002(pos, 2)); break;
						case MapCell.Kind_e.EVENT_9002_4: this.Enemies.Add(new Enemy_Event9002(pos, 4)); break;
						case MapCell.Kind_e.EVENT_9002_6: this.Enemies.Add(new Enemy_Event9002(pos, 6)); break;
						case MapCell.Kind_e.EVENT_9002_8: this.Enemies.Add(new Enemy_Event9002(pos, 8)); break;
						case MapCell.Kind_e.EVENT_9003: this.Enemies.Add(new Enemy_Event9003(pos)); break;
						case MapCell.Kind_e.EVENT_9003B: this.Enemies.Add(new Enemy_Event9003B(pos)); break;
						case MapCell.Kind_e.EVENT_9004: this.Enemies.Add(new Enemy_Event9004(pos)); break;
						case MapCell.Kind_e.EVENT_9005: this.Enemies.Add(new Enemy_Event9005(pos)); break;

						case MapCell.Kind_e.行き先案内_Start方面: this.Enemies.Add(new Enemy_行き先案内(pos, false)); break;
						case MapCell.Kind_e.行き先案内_Goal方面: this.Enemies.Add(new Enemy_行き先案内(pos, true)); break;

						default:
							break;
					}
				}
			}
		}

		/// <summary>
		/// マップから離れすぎているか
		/// 退場と見なして良いか
		/// </summary>
		/// <param name="x">X_座標</param>
		/// <param name="y">Y_座標</param>
		/// <returns></returns>
		private bool IsProbablyEvacuated(double x, double y)
		{
			const int MGN_SCREEN_NUM = 3;

			return
				x < -DDConsts.Screen_W * MGN_SCREEN_NUM || this.Map.W * GameConsts.TILE_W + DDConsts.Screen_W * MGN_SCREEN_NUM < x ||
				y < -DDConsts.Screen_H * MGN_SCREEN_NUM || this.Map.H * GameConsts.TILE_H + DDConsts.Screen_H * MGN_SCREEN_NUM < y;
		}

		private bool Pause_ReturnToTitleMenu = false;
		private bool Pause_Respawn = false;

		/// <summary>
		/// ポーズメニュー
		/// </summary>
		private void Pause()
		{
			波紋効果.抑止 = true;

			DDMain.KeepMainScreen();

			DDSimpleMenu simpleMenu = new DDSimpleMenu()
			{
				Color = new I3Color(255, 255, 255),
				BorderColor = new I3Color(0, 64, 128),
				WallDrawer = () =>
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);

					DDDraw.SetAlpha(0.8);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 4, DDConsts.Screen_W, DDConsts.Screen_H / 2);
					DDDraw.Reset();
				},
			};

			DDEngine.FreezeInput();

			Ground.I.SE.EnterPause.Play();

			// reset
			this.Pause_ReturnToTitleMenu = false;
			this.Pause_Respawn = false;

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(
					() => "ＰＡＵＳＥ",
					() => new string[]
					{
						DisturbString("このステージの最初からやり直す", Game.I.FinalZone != null),
						"タイトルに戻る",
						"ゲームに戻る",
					},
					selectIndex,
					true,
					250,
					180,
					50
					);

				switch (selectIndex)
				{
					case 0:
						if (Game.I.FinalZone == null)
						{
							this.Pause_Respawn = true;
							goto endLoop;
						}
						else
							Ground.I.SE.拒否.Play();

						break;

					case 1:
						this.Pause_ReturnToTitleMenu = true;
						Ground.I.SE.LeavePause_Title.Play();
						goto endLoop;

					case 2:
						Ground.I.SE.LeavePause.Play();
						goto endLoop;

					default:
						throw null; // never
				}
				//DDEngine.EachFrame(); // 不要
			}
		endLoop:
			DDEngine.FreezeInput();

			DDInput.A.FreezeInputUntilRelease = true;
			DDInput.B.FreezeInputUntilRelease = true;

			波紋効果.抑止 = false; // restore
		}

		private string DisturbString(string str, bool enabled)
		{
			if (enabled)
				str = new string(str.Select(chr => SCommon.CRandom.ChooseOne((chr + "／-｜＼").ToArray())).ToArray());

			return str;
		}

		private bool 当たり判定表示 = false;

		/// <summary>
		/// ポーズメニュー(デバッグ用)
		/// </summary>
		private void DebugPause()
		{
			DDMain.KeepMainScreen();

			DDSimpleMenu simpleMenu = new DDSimpleMenu()
			{
				Color = new I3Color(255, 255, 255),
				BorderColor = new I3Color(0, 128, 64),
				WallDrawer = () =>
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
					DDCurtain.DrawCurtain(-0.5);
				},
			};

			DDEngine.FreezeInput();

			// reset
			this.Pause_ReturnToTitleMenu = false;
			this.Pause_Respawn = false;

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(
					"デバッグ用メニュー",
					new string[]
					{
						"----",
						"強制遅延 [ 現在の設定：" + DDEngine.SlowdownLevel + " ]",
						"当たり判定表示 [ 現在の設定：" + this.当たり判定表示 + " ]",
						"このステージの最初からやり直す",
						"タイトルに戻る",
						"ゲームに戻る",
					},
					selectIndex,
					true,
					40,
					40,
					40
					);

				switch (selectIndex)
				{
					case 0:
						// none
						break;

					case 1:
						if (DDEngine.SlowdownLevel == 0)
							DDEngine.SlowdownLevel = 1;
						else
							DDEngine.SlowdownLevel *= 2;

						DDEngine.SlowdownLevel %= 16;
						break;

					case 2:
						this.当たり判定表示 = !this.当たり判定表示;
						break;

					case 3:
						this.Pause_Respawn = true;
						goto endLoop;

					case 4:
						this.Pause_ReturnToTitleMenu = true;
						goto endLoop;

					case 5:
						goto endLoop;

					default:
						throw null; // never
				}
				//DDEngine.EachFrame(); // 不要
			}
		endLoop:
			DDEngine.FreezeInput();

			DDInput.A.FreezeInputUntilRelease = true;
			DDInput.B.FreezeInputUntilRelease = true;
		}
	}
}
