﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    /// <summary>
    /// プレイヤーの情報を表示するコントロール
    /// </summary>
    public partial class PlayerInfoControl : UserControl {
        /// <summary>
        /// 持ち時間[s]
        /// </summary>
        public int TimeASeconds { get; set; }

        /// <summary>
        /// 秒読み[s]
        /// </summary>
        public int TimeBSeconds { get; set; }

        /// <summary>
        /// 残り持ち時間
        /// </summary>
        public int RemainSeconds { get; set; }
        /// <summary>
        /// 秒読み時間
        /// </summary>
        public int ByoyomiSeconds { get; set; }

        bool tickCalled;

        public PlayerInfoControl() {
            InitializeComponent();
        }

        private void PlayerInfoControl_Load(object sender, EventArgs e) {
            PlayerName = "";
        }

        /// <summary>
        /// 手番(0:先手、1:後手)
        /// </summary>
        [Description("手番(0:先手、1:後手)")]
        public int Turn { get; set; }

        /// <summary>
        /// プレイヤー名
        /// </summary>
        public string PlayerName {
            get {
                return label1.Text.TrimStart('▲', '△');
            }
            set {
                label1.Text = "▲△"[Turn].ToString() + value;
            }
        }

        /// <summary>
        /// リセット
        /// </summary>
        public void Reset() {
            RemainSeconds = TimeASeconds;
            ByoyomiSeconds = TimeBSeconds;
            UpdateTimeDisplay();
        }

        /// <summary>
        /// 時間消費開始
        /// </summary>
        public void StartTurn() {
            tickCalled = false;
            timer1.Enabled = true;
        }

        /// <summary>
        /// 時間消費終了
        /// </summary>
        public void EndTurn() {
            timer1.Enabled = false;
            // CSAルールでは最低1秒消費なのでそのように。(適当)
            if (!tickCalled) {
                timer1_Tick(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 時間消費(1秒に1回呼ばれる想定)
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e) {
            // windowsのイベント処理が滞るとずれるけど手抜き
            tickCalled = true;

            // 残り時間算出
            if (0 < RemainSeconds) {
                RemainSeconds--;
            } else if (0 < ByoyomiSeconds) {
                ByoyomiSeconds--;
            } else {
                // 時間切れてる場合(適当にマイナスに進む)
                if (TimeBSeconds <= 0) {
                    RemainSeconds--;
                } else {
                    ByoyomiSeconds--;
                }
            }
            // 表示の更新
            UpdateTimeDisplay();
        }

        /// <summary>
        /// 表示の更新
        /// </summary>
        private void UpdateTimeDisplay() {
            int h = RemainSeconds / 3600;
            int m = (RemainSeconds - h * 3600) / 60;
            int s = RemainSeconds % 60;
            label2.Text = h.ToString() + ":" + m.ToString("00") + ":" + s.ToString("00") +
                "　" + ByoyomiSeconds.ToString("00") + "/" + TimeBSeconds.ToString("00");
        }
    }
}
