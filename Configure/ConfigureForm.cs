using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.ServiceProcess;
using StarMicronics.StarIO;

namespace Configure
{

    public partial class ConfigurationForm : Form
    {
        bool is_starprnt_list_initialized = false;
        private ushort GetDefaultPort(string protocol,bool ssl)
        {
            if (protocol == "IMAP" && ssl)
                return 993;
            else if (protocol == "IMAP" && !ssl)
                return 143;
            else if (protocol == "POP" && ssl)
                return 995;
            else if (protocol == "POP" && !ssl)
                return 110;

            throw new ArgumentException("Invalid protocol " + protocol + " specified.");
        }
        private void SetTextboxValue(Dictionary<string,string> iniprop,string key,TextBox textbox,string default_str = "")
        {
            if (iniprop.TryGetValue(key, out string val))
                textbox.Text = val;
            else
                textbox.Text = default_str;
        }
        private void SetNumericUpDownValue(Dictionary<string, string> iniprop, string key, NumericUpDown numericUpDown,uint default_num = 0)
        {
            uint val;
            if (!iniprop.TryGetValue(key, out string val_str))
                val = default_num;
            else
            {
                if (!UInt32.TryParse(val_str, out val))
                {
                    val = default_num;
                    MessageBox.Show(key + "の値が不正です。デフォルト値の" + val.ToString() + "に設定されます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            numericUpDown.Value = val;

        }
        private void SetCheckboxValue(Dictionary<string, string> iniprop, string key, CheckBox checkbox,bool default_bool = true)
        {
            bool val;
            if (!iniprop.TryGetValue(key, out string val_str))
                val = default_bool;
            else
            {
                if (Regex.IsMatch(val_str, @"^Yes$", RegexOptions.IgnoreCase))
                    val = true;
                else if (Regex.IsMatch(val_str, @"^No$", RegexOptions.IgnoreCase))
                    val = false;
                else
                {
                    val = default_bool;
                    MessageBox.Show(key + "の値が不正です。デフォルト値の" + (val ? "Yes" : "No") + "に設定されます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            checkbox.Checked = val;
        }
        private void SetComboboxValue(Dictionary<string, string> iniprop, string key, ComboBox combobox, int default_index = 0)
        {
            int index;
            if (!iniprop.TryGetValue(key, out string val))
                index = default_index;
            else
            {
                index = combobox.FindStringExact(val);
                if (index < 0)
                {
                    index = default_index;
                    MessageBox.Show(key + "の値が不正です。デフォルト値の" + combobox.Items[index].ToString() + "に設定されます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            combobox.SelectedIndex = index;
        }

        public ConfigurationForm()
        {
            InitializeComponent();

            Dictionary<string, string> iniprop;
            try
            {
                iniprop = LoadIniFile(@"MailToStarPRNT.ini");
            }
            catch(FileNotFoundException e)
            {
                iniprop = new Dictionary<string, string>();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString(),"エラー",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.ExitCode = 1;
                Application.Exit();
                return;
            }

            SetComboboxValue(iniprop, "MailProtocol", this.comboBoxMailProtocol);
            SetCheckboxValue(iniprop, "MailProtocolSSL", this.checkBoxMailSSL, true);
            SetNumericUpDownValue(iniprop, "MailServerPort", this.numericUpDownMailServerPort, GetDefaultPort(this.comboBoxMailProtocol.SelectedItem.ToString(), checkBoxMailSSL.Checked));
            SetTextboxValue(iniprop, "MailServerName", this.textBoxMailServerName);
            SetTextboxValue(iniprop, "MailServerUserName", this.textBoxMailUserName);
            SetTextboxValue(iniprop, "MailServerPassword", this.textBoxMailPassword);
            SetNumericUpDownValue(iniprop, "MailCheckIntervalSeconds", this.numericUpDownMailCheckInterval, 30);
            SetTextboxValue(iniprop, "MailPrintSubjectCondition", this.textBoxConditionSubject);
            SetTextboxValue(iniprop, "MailPrintFromCondition", this.textBoxConditionFrom);
            SetTextboxValue(iniprop, "MailPrintBodyCondition", this.textBoxConditionBody);
            SetComboboxValue(iniprop, "StarPRNTEmulation", this.comboBoxStarPRNTEmulation);
            SetNumericUpDownValue(iniprop, "StarPRNTTimeOutSeconds", this.numericUpDownStarPRNTTimeout, 10);

            if (iniprop.TryGetValue("StarPRNTPortName", out string starprnt_printer))
                this.comboBoxStarPRNTPrinter.SelectedIndex = this.comboBoxStarPRNTPrinter.Items.Add(starprnt_printer);
            else
                this.comboBoxStarPRNTPrinter.SelectedIndex = 0;

            SetCheckboxValue(iniprop, "PrintStartStopMessage", this.checkBoxPrintStartStopMessage,true);
            SetCheckboxValue(iniprop, "PrintErrors", this.checkBoxPrintErrors,false);

            if (this.textBoxMailServerName.Text == "")
                this.textBoxMailServerName.BackColor = Color.Red;
            else
                this.textBoxMailServerName.BackColor = SystemColors.Window;

            if (this.textBoxMailUserName.Text == "")
                this.textBoxMailUserName.BackColor = Color.Red;
            else
                this.textBoxMailUserName.BackColor = SystemColors.Window;

            if (this.textBoxMailPassword.Text == "")
                this.textBoxMailPassword.BackColor = Color.Red;
            else
                this.textBoxMailPassword.BackColor = SystemColors.Window;
        }


        private void ConfigurationForm_Load(object sender, EventArgs e)
        {

        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if(this.textBoxMailServerName.Text == "")
            {
                MessageBox.Show("メール受信設定のサーバー名が設定されていません。サーバー名の設定は必須です。", "設定変更", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (this.textBoxMailUserName.Text == "")
            {
                MessageBox.Show("メール受信設定のユーザー名が設定されていません。ユーザー名の設定は必須です。", "設定変更", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (this.textBoxMailUserName.Text == "")
            {
                MessageBox.Show("メール受信設定のパスワードが設定されていません。パスワードの設定は必須です。", "設定変更", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult result = MessageBox.Show("設定変更を反映する為に、印刷アプリケーションを再起動しますか?", "設定変更",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
                return;

            // iniファイルへの設定保存
            StreamWriter inifile;
            try
            {
                inifile = new System.IO.StreamWriter(@"MailToStarPRNT.ini",false,Encoding.UTF8);

                inifile.WriteLine("MailProtocol=" + this.comboBoxMailProtocol.SelectedItem.ToString());
                if(this.checkBoxMailSSL.Checked == false)
                    inifile.WriteLine("MailProtocolSSL=No");
                else
                    inifile.WriteLine("MailProtocolSSL=Yes");
                inifile.WriteLine("MailServerName=" + this.textBoxMailServerName.Text);
                inifile.WriteLine("MailServerPort=" + this.numericUpDownMailServerPort.Value.ToString());
                inifile.WriteLine("MailServerUserName=" + this.textBoxMailUserName.Text);
                inifile.WriteLine("MailServerPassword=" + this.textBoxMailPassword.Text);
                inifile.WriteLine("MailCheckIntervalSeconds=" + this.numericUpDownMailCheckInterval.Value.ToString());

                if (this.textBoxConditionSubject.Text != "")
                    inifile.WriteLine("MailPrintSubjectCondition=" + this.textBoxConditionSubject.Text);
                if (this.textBoxConditionFrom.Text != "")
                    inifile.WriteLine("MailPrintFromCondition=" + this.textBoxConditionFrom.Text);
                if (this.textBoxConditionBody.Text != "")
                    inifile.WriteLine("MailPrintBodyCondition=" + this.textBoxConditionBody.Text);

                if (this.comboBoxStarPRNTPrinter.SelectedItem.ToString() != "自動")
                    inifile.WriteLine("StarPRNTPortName=" + this.comboBoxStarPRNTPrinter.SelectedItem.ToString());
                if (this.comboBoxStarPRNTEmulation.SelectedItem.ToString() != "自動")
                    inifile.WriteLine("StarPRNTEmulation=" + this.comboBoxStarPRNTEmulation.SelectedItem.ToString());
                inifile.WriteLine("StarPRNTTimeOutSeconds=" + this.numericUpDownStarPRNTTimeout.Value.ToString());

                if (this.checkBoxPrintStartStopMessage.Checked == false)
                    inifile.WriteLine("PrintStartStopMessage=No");
                else
                    inifile.WriteLine("PrintStartStopMessage=Yes");
                if (this.checkBoxPrintErrors.Checked == false)
                    inifile.WriteLine("PrintErrors=No");
                else
                    inifile.WriteLine("PrintErrors=Yes");

                inifile.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("設定を保存できませんでした。\n\n" + ex.ToString(), "設定変更", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (result == DialogResult.No)
            {
                MessageBox.Show("設定を保存しましたが印刷アプリケーションを再起動するまでは設定は反映されません。","設定変更",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
                return;
            }

            // 印刷アプリケーションのサービス再起動
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ServiceController sc = new ServiceController("MailToStarPRNT");
                if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    sc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                }
                else if (sc.CanStop)
                {
                    sc.Stop();
                    sc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
                    if (sc.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        sc.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("サービス再起動に失敗しました。印刷アプリケーションが正常にインストールされていない可能性があります。\n\n" + ex.ToString(), "設定変更", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Win32Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("サービス再起動に失敗しました。システム権限が不足している可能性があります。\n\n" + ex.ToString(), "設定変更", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxMailServerName_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxMailServerName.Text == "")
                this.textBoxMailServerName.BackColor = Color.Red;
            else
                this.textBoxMailServerName.BackColor = SystemColors.Window;
        }

        private void textBoxMailUserName_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxMailUserName.Text == "")
                this.textBoxMailUserName.BackColor = Color.Red;
            else
                this.textBoxMailUserName.BackColor = SystemColors.Window;
        }

        private void textBoxMailPassword_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxMailPassword.Text == "")
                this.textBoxMailPassword.BackColor = Color.Red;
            else
                this.textBoxMailPassword.BackColor = SystemColors.Window;
        }
        private void comboBoxStarPRNTPrinter_DropDown(object sender, EventArgs e)
        {
            ComboBox combobox = (ComboBox)sender;
            if (!this.is_starprnt_list_initialized) {
                this.Cursor = Cursors.WaitCursor;
                List<StarMicronics.StarIO.PortInfo> printers = StarMicronics.StarIO.Factory.I.SearchPrinter();
                printers.ForEach(printer =>
                {
                    if (printer.ModelName != "")
                    {
                        if(combobox.FindStringExact(printer.PortName) < 0)
                            combobox.Items.Add(printer.PortName);
                    }
                });
                this.Cursor = Cursors.Default;
                this.is_starprnt_list_initialized = true;
            }
        }
        private Dictionary<string, string> LoadIniFile(string inifile)
        {
            Dictionary<string, string> iniprop = new Dictionary<string, string>();

            string[] lines = System.IO.File.ReadAllLines(inifile);
            foreach (string line in lines)
            {
                if (Regex.IsMatch(line, @"^\s*[;#].*"))
                    continue;
                var match = Regex.Match(line, @"^\s*(\S+)\s*=\s*(.*)$");
                if (match.Success)
                {
                    string key = match.Groups[1].Value.Trim();
                    string value = match.Groups[2].Value.Trim();
                    iniprop.Add(key, value);
                }
            }

            return iniprop;
        }

        private void comboBoxMailProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.numericUpDownMailServerPort.Value = GetDefaultPort(this.comboBoxMailProtocol.SelectedItem.ToString(), checkBoxMailSSL.Checked);
        }

        private void checkBoxMailSSL_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDownMailServerPort.Value = GetDefaultPort(this.comboBoxMailProtocol.SelectedItem.ToString(), checkBoxMailSSL.Checked);
        }
    }
}
