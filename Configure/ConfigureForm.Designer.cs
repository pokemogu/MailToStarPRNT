
namespace Configure
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.label1 = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxMailSSL = new System.Windows.Forms.CheckBox();
            this.comboBoxMailProtocol = new System.Windows.Forms.ComboBox();
            this.comboBoxStarPRNTPrinter = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxStarPRNTEmulation = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownStarPRNTTimeout = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxMail = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownMailCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxMailPassword = new System.Windows.Forms.TextBox();
            this.textBoxMailUserName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownMailServerPort = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMailServerName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBoxCondition = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxConditionBody = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxConditionFrom = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxConditionSubject = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBoxStarPRNT = new System.Windows.Forms.GroupBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.groupBoxOther = new System.Windows.Forms.GroupBox();
            this.checkBoxPrintErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintStartStopMessage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStarPRNTTimeout)).BeginInit();
            this.groupBoxMail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMailCheckInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMailServerPort)).BeginInit();
            this.groupBoxCondition.SuspendLayout();
            this.groupBoxStarPRNT.SuspendLayout();
            this.groupBoxOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "使用するプリンター";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.helpProvider.SetHelpKeyword(this.label5, "");
            this.helpProvider.SetHelpString(this.label5, "");
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Name = "label5";
            this.helpProvider.SetShowHelp(this.label5, false);
            this.label5.Size = new System.Drawing.Size(49, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "プロトコル";
            // 
            // checkBoxMailSSL
            // 
            this.checkBoxMailSSL.AutoSize = true;
            this.helpProvider.SetHelpString(this.checkBoxMailSSL, "メールを受信する為のプロトコルで暗号化SSLを使用するかどうかを設定してください。");
            this.checkBoxMailSSL.Location = new System.Drawing.Point(148, 23);
            this.checkBoxMailSSL.Name = "checkBoxMailSSL";
            this.helpProvider.SetShowHelp(this.checkBoxMailSSL, true);
            this.checkBoxMailSSL.Size = new System.Drawing.Size(44, 16);
            this.checkBoxMailSSL.TabIndex = 2;
            this.checkBoxMailSSL.Text = "SSL";
            this.checkBoxMailSSL.UseVisualStyleBackColor = true;
            this.checkBoxMailSSL.CheckedChanged += new System.EventHandler(this.checkBoxMailSSL_CheckedChanged);
            // 
            // comboBoxMailProtocol
            // 
            this.comboBoxMailProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMailProtocol.FormattingEnabled = true;
            this.helpProvider.SetHelpKeyword(this.comboBoxMailProtocol, "");
            this.helpProvider.SetHelpString(this.comboBoxMailProtocol, "メールを受信する為のプロトコルを選択してください。どのプロトコルかはメールサーバーの担当に確認してください。");
            this.comboBoxMailProtocol.Items.AddRange(new object[] {
            "POP",
            "IMAP"});
            this.comboBoxMailProtocol.Location = new System.Drawing.Point(69, 21);
            this.comboBoxMailProtocol.Name = "comboBoxMailProtocol";
            this.helpProvider.SetShowHelp(this.comboBoxMailProtocol, true);
            this.comboBoxMailProtocol.Size = new System.Drawing.Size(73, 20);
            this.comboBoxMailProtocol.TabIndex = 1;
            this.comboBoxMailProtocol.SelectedIndexChanged += new System.EventHandler(this.comboBoxMailProtocol_SelectedIndexChanged);
            // 
            // comboBoxStarPRNTPrinter
            // 
            this.comboBoxStarPRNTPrinter.FormattingEnabled = true;
            this.comboBoxStarPRNTPrinter.Items.AddRange(new object[] {
            "自動"});
            this.comboBoxStarPRNTPrinter.Location = new System.Drawing.Point(114, 18);
            this.comboBoxStarPRNTPrinter.Name = "comboBoxStarPRNTPrinter";
            this.comboBoxStarPRNTPrinter.Size = new System.Drawing.Size(168, 20);
            this.comboBoxStarPRNTPrinter.TabIndex = 1;
            this.comboBoxStarPRNTPrinter.DropDown += new System.EventHandler(this.comboBoxStarPRNTPrinter_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "エミュレーションモード";
            // 
            // comboBoxStarPRNTEmulation
            // 
            this.comboBoxStarPRNTEmulation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStarPRNTEmulation.FormattingEnabled = true;
            this.comboBoxStarPRNTEmulation.Items.AddRange(new object[] {
            "自動",
            "StarPRNT",
            "StarPRNTL",
            "StarLine",
            "StarGraphic",
            "EscPos",
            "EscPosMobile",
            "StarDotImpact"});
            this.comboBoxStarPRNTEmulation.Location = new System.Drawing.Point(114, 44);
            this.comboBoxStarPRNTEmulation.Name = "comboBoxStarPRNTEmulation";
            this.comboBoxStarPRNTEmulation.Size = new System.Drawing.Size(168, 20);
            this.comboBoxStarPRNTEmulation.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "通信タイムアウト";
            // 
            // numericUpDownStarPRNTTimeout
            // 
            this.numericUpDownStarPRNTTimeout.Location = new System.Drawing.Point(114, 70);
            this.numericUpDownStarPRNTTimeout.Name = "numericUpDownStarPRNTTimeout";
            this.numericUpDownStarPRNTTimeout.Size = new System.Drawing.Size(57, 19);
            this.numericUpDownStarPRNTTimeout.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "秒";
            // 
            // groupBoxMail
            // 
            this.groupBoxMail.Controls.Add(this.label11);
            this.groupBoxMail.Controls.Add(this.numericUpDownMailCheckInterval);
            this.groupBoxMail.Controls.Add(this.label10);
            this.groupBoxMail.Controls.Add(this.label9);
            this.groupBoxMail.Controls.Add(this.textBoxMailPassword);
            this.groupBoxMail.Controls.Add(this.textBoxMailUserName);
            this.groupBoxMail.Controls.Add(this.label8);
            this.groupBoxMail.Controls.Add(this.numericUpDownMailServerPort);
            this.groupBoxMail.Controls.Add(this.label7);
            this.groupBoxMail.Controls.Add(this.textBoxMailServerName);
            this.groupBoxMail.Controls.Add(this.label6);
            this.groupBoxMail.Controls.Add(this.checkBoxMailSSL);
            this.groupBoxMail.Controls.Add(this.comboBoxMailProtocol);
            this.groupBoxMail.Controls.Add(this.label5);
            this.groupBoxMail.Location = new System.Drawing.Point(12, 12);
            this.groupBoxMail.Name = "groupBoxMail";
            this.groupBoxMail.Size = new System.Drawing.Size(329, 150);
            this.groupBoxMail.TabIndex = 0;
            this.groupBoxMail.TabStop = false;
            this.groupBoxMail.Text = "メール受信設定";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(132, 124);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 12);
            this.label11.TabIndex = 13;
            this.label11.Text = "秒に1回";
            // 
            // numericUpDownMailCheckInterval
            // 
            this.numericUpDownMailCheckInterval.Location = new System.Drawing.Point(69, 122);
            this.numericUpDownMailCheckInterval.Maximum = new decimal(new int[] {
            604800,
            0,
            0,
            0});
            this.numericUpDownMailCheckInterval.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownMailCheckInterval.Name = "numericUpDownMailCheckInterval";
            this.numericUpDownMailCheckInterval.Size = new System.Drawing.Size(57, 19);
            this.numericUpDownMailCheckInterval.TabIndex = 12;
            this.numericUpDownMailCheckInterval.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "受信頻度";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "パスワード";
            // 
            // textBoxMailPassword
            // 
            this.textBoxMailPassword.Location = new System.Drawing.Point(69, 97);
            this.textBoxMailPassword.Name = "textBoxMailPassword";
            this.textBoxMailPassword.Size = new System.Drawing.Size(123, 19);
            this.textBoxMailPassword.TabIndex = 10;
            this.textBoxMailPassword.UseSystemPasswordChar = true;
            this.textBoxMailPassword.TextChanged += new System.EventHandler(this.textBoxMailPassword_TextChanged);
            // 
            // textBoxMailUserName
            // 
            this.textBoxMailUserName.Location = new System.Drawing.Point(69, 72);
            this.textBoxMailUserName.Name = "textBoxMailUserName";
            this.textBoxMailUserName.Size = new System.Drawing.Size(123, 19);
            this.textBoxMailUserName.TabIndex = 8;
            this.textBoxMailUserName.TextChanged += new System.EventHandler(this.textBoxMailUserName_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "ユーザー名";
            // 
            // numericUpDownMailServerPort
            // 
            this.numericUpDownMailServerPort.Location = new System.Drawing.Point(261, 47);
            this.numericUpDownMailServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownMailServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMailServerPort.Name = "numericUpDownMailServerPort";
            this.numericUpDownMailServerPort.Size = new System.Drawing.Size(57, 19);
            this.numericUpDownMailServerPort.TabIndex = 6;
            this.numericUpDownMailServerPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(198, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "ポート番号";
            // 
            // textBoxMailServerName
            // 
            this.textBoxMailServerName.Location = new System.Drawing.Point(69, 47);
            this.textBoxMailServerName.Name = "textBoxMailServerName";
            this.textBoxMailServerName.Size = new System.Drawing.Size(123, 19);
            this.textBoxMailServerName.TabIndex = 4;
            this.textBoxMailServerName.TextChanged += new System.EventHandler(this.textBoxMailServerName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "サーバー名";
            // 
            // groupBoxCondition
            // 
            this.groupBoxCondition.Controls.Add(this.label17);
            this.groupBoxCondition.Controls.Add(this.label16);
            this.groupBoxCondition.Controls.Add(this.textBoxConditionBody);
            this.groupBoxCondition.Controls.Add(this.label15);
            this.groupBoxCondition.Controls.Add(this.textBoxConditionFrom);
            this.groupBoxCondition.Controls.Add(this.label14);
            this.groupBoxCondition.Controls.Add(this.label13);
            this.groupBoxCondition.Controls.Add(this.textBoxConditionSubject);
            this.groupBoxCondition.Controls.Add(this.label12);
            this.groupBoxCondition.Location = new System.Drawing.Point(347, 12);
            this.groupBoxCondition.Name = "groupBoxCondition";
            this.groupBoxCondition.Size = new System.Drawing.Size(384, 150);
            this.groupBoxCondition.TabIndex = 1;
            this.groupBoxCondition.TabStop = false;
            this.groupBoxCondition.Text = "印刷条件設定";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(300, 71);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 12);
            this.label17.TabIndex = 8;
            this.label17.Text = "が含まれる場合";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(300, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 12);
            this.label16.TabIndex = 5;
            this.label16.Text = "が含まれる場合";
            // 
            // textBoxConditionBody
            // 
            this.textBoxConditionBody.Location = new System.Drawing.Point(100, 68);
            this.textBoxConditionBody.Name = "textBoxConditionBody";
            this.textBoxConditionBody.Size = new System.Drawing.Size(194, 19);
            this.textBoxConditionBody.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 71);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 12);
            this.label15.TabIndex = 6;
            this.label15.Text = "メールの本文に";
            // 
            // textBoxConditionFrom
            // 
            this.textBoxConditionFrom.Location = new System.Drawing.Point(100, 43);
            this.textBoxConditionFrom.Name = "textBoxConditionFrom";
            this.textBoxConditionFrom.Size = new System.Drawing.Size(194, 19);
            this.textBoxConditionFrom.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 12);
            this.label14.TabIndex = 3;
            this.label14.Text = "メールの送信元に";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(300, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "が含まれる場合";
            // 
            // textBoxConditionSubject
            // 
            this.textBoxConditionSubject.Location = new System.Drawing.Point(100, 18);
            this.textBoxConditionSubject.Name = "textBoxConditionSubject";
            this.textBoxConditionSubject.Size = new System.Drawing.Size(194, 19);
            this.textBoxConditionSubject.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "メールの件名に";
            // 
            // groupBoxStarPRNT
            // 
            this.groupBoxStarPRNT.Controls.Add(this.label1);
            this.groupBoxStarPRNT.Controls.Add(this.label2);
            this.groupBoxStarPRNT.Controls.Add(this.label3);
            this.groupBoxStarPRNT.Controls.Add(this.label4);
            this.groupBoxStarPRNT.Controls.Add(this.comboBoxStarPRNTPrinter);
            this.groupBoxStarPRNT.Controls.Add(this.numericUpDownStarPRNTTimeout);
            this.groupBoxStarPRNT.Controls.Add(this.comboBoxStarPRNTEmulation);
            this.groupBoxStarPRNT.Location = new System.Drawing.Point(12, 168);
            this.groupBoxStarPRNT.Name = "groupBoxStarPRNT";
            this.groupBoxStarPRNT.Size = new System.Drawing.Size(329, 100);
            this.groupBoxStarPRNT.TabIndex = 2;
            this.groupBoxStarPRNT.TabStop = false;
            this.groupBoxStarPRNT.Text = "StarPRNTプリンター設定";
            // 
            // ButtonOK
            // 
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonOK.Location = new System.Drawing.Point(12, 274);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 4;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(93, 274);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "キャンセル";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // groupBoxOther
            // 
            this.groupBoxOther.Controls.Add(this.checkBoxPrintErrors);
            this.groupBoxOther.Controls.Add(this.checkBoxPrintStartStopMessage);
            this.groupBoxOther.Location = new System.Drawing.Point(347, 168);
            this.groupBoxOther.Name = "groupBoxOther";
            this.groupBoxOther.Size = new System.Drawing.Size(384, 100);
            this.groupBoxOther.TabIndex = 3;
            this.groupBoxOther.TabStop = false;
            this.groupBoxOther.Text = "その他の設定";
            // 
            // checkBoxPrintErrors
            // 
            this.checkBoxPrintErrors.AutoSize = true;
            this.checkBoxPrintErrors.Location = new System.Drawing.Point(6, 40);
            this.checkBoxPrintErrors.Name = "checkBoxPrintErrors";
            this.checkBoxPrintErrors.Size = new System.Drawing.Size(296, 16);
            this.checkBoxPrintErrors.TabIndex = 1;
            this.checkBoxPrintErrors.Text = "StarPRNTプリンターに異常を知らせるメッセージを印刷する";
            this.checkBoxPrintErrors.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintStartStopMessage
            // 
            this.checkBoxPrintStartStopMessage.AutoSize = true;
            this.checkBoxPrintStartStopMessage.Location = new System.Drawing.Point(6, 18);
            this.checkBoxPrintStartStopMessage.Name = "checkBoxPrintStartStopMessage";
            this.checkBoxPrintStartStopMessage.Size = new System.Drawing.Size(367, 16);
            this.checkBoxPrintStartStopMessage.TabIndex = 0;
            this.checkBoxPrintStartStopMessage.Text = "起動時と停止時にStarPRNTプリンターに起動・停止メッセージを印刷する";
            this.checkBoxPrintStartStopMessage.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 306);
            this.Controls.Add(this.groupBoxOther);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.groupBoxStarPRNT);
            this.Controls.Add(this.groupBoxCondition);
            this.Controls.Add(this.groupBoxMail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigurationForm";
            this.Text = "設定";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStarPRNTTimeout)).EndInit();
            this.groupBoxMail.ResumeLayout(false);
            this.groupBoxMail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMailCheckInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMailServerPort)).EndInit();
            this.groupBoxCondition.ResumeLayout(false);
            this.groupBoxCondition.PerformLayout();
            this.groupBoxStarPRNT.ResumeLayout(false);
            this.groupBoxStarPRNT.PerformLayout();
            this.groupBoxOther.ResumeLayout(false);
            this.groupBoxOther.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ComboBox comboBoxStarPRNTPrinter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStarPRNTEmulation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownStarPRNTTimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBoxMail;
        private System.Windows.Forms.CheckBox checkBoxMailSSL;
        private System.Windows.Forms.ComboBox comboBoxMailProtocol;
        private System.Windows.Forms.TextBox textBoxMailServerName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownMailServerPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxMailPassword;
        private System.Windows.Forms.TextBox textBoxMailUserName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownMailCheckInterval;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBoxCondition;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxConditionBody;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxConditionFrom;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxConditionSubject;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBoxStarPRNT;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox groupBoxOther;
        private System.Windows.Forms.CheckBox checkBoxPrintErrors;
        private System.Windows.Forms.CheckBox checkBoxPrintStartStopMessage;
    }
}

