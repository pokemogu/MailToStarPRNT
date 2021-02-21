using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
using Topshelf;
using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using MailKit.Net;
using MailKit.Security;

class MailToStarPRNT
{
    public static readonly string AppName = "MailToStarPRNT";
    readonly System.Timers.Timer timer;
    string appdatadir;

    //StarMicronics.StarIO.IPort starprnt_port; // StarPRNTプリンターインスタンス
    StarMicronics.StarIOExtension.Emulation emulation; // StarPRNTエミュレーションモード
    string port_name;
    int print_timeout;

    // メール受信関連メンバー
    MailKit.MailService mailclient; // メール受信インスタンス
    string mail_server_name; // メールサーバ名(IPアドレス or FQDN)
    UInt16 mail_server_port_number; // メールサーバポート番号
    string mail_server_username;
    string mail_server_password;
    bool mail_server_ssl;

    bool print_on_error;
    bool print_on_start_stop;

    string mail_subject_condition;
    string mail_from_condition;
    string mail_body_condition;
    public MailToStarPRNT()
    {
        this.timer = new System.Timers.Timer() { AutoReset = true };
        this.timer.Elapsed += TimerElapsed;
        this.appdatadir = "";
        this.print_on_error = true;
        this.print_on_start_stop = true;
        this.port_name = "";
        this.print_timeout = 10000;
        this.mail_server_ssl = true;
        this.mail_from_condition = "";
        this.mail_subject_condition = "";
        this.mail_body_condition = "";
    }

    private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs ev)
    {
#if DEBUG
        Trace.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " メール受信処理を開始します");
#endif

        if (!this.mailclient.IsConnected)
        {
            // StarPRNTプリンターへの接続開始
            StarMicronics.StarIO.IPort starprnt_port = null;
            try
            {
                starprnt_port = StarMicronics.StarIO.Factory.I.GetPort(this.port_name, "", this.print_timeout);
            }
            catch (StarMicronics.StarIO.PortException e)
            {
                Trace.TraceError("StarPRNTプリンターに接続できませんでした。\n" + e.Message);
                return;
            }

            PrintMails(starprnt_port);

            // プリンター接続を終了
            StarMicronics.StarIO.Factory.I.ReleasePort(starprnt_port);
            starprnt_port = null;
        }

#if DEBUG
        Trace.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " メール受信処理を終了します");
#endif
    }

    public bool Start(Topshelf.HostControl hc)
    {
        // ログリスナーの設定 (コンソールアプリケーションとして起動した場合はコンソール、サービスとして起動した場合はイベントログ)
        if (hc is Topshelf.Hosts.ConsoleRunHost)
            Trace.Listeners.Add(new ConsoleTraceListener());
        else
            Trace.Listeners.Add(new EventLogTraceListener(MailToStarPRNT.AppName));

        Trace.TraceInformation("アプリケーションを開始します。\nStarPRNT SDK version " + StarMicronics.StarIO.Factory.I.GetStarIOVersion());

        // アプリケーションデータディレクトリ初期化
        /*
        string localappdir = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        this.appdatadir = System.IO.Path.Combine(localappdir, MailToStarPRNT.AppName);
        if (!System.IO.Directory.Exists(this.appdatadir))
        {
            try
            {
                System.IO.Directory.CreateDirectory(this.appdatadir);
            }
            catch(Exception e)
            {
                Trace.TraceInformation("アプリケーションディレクトリ " + this.appdatadir + " の作成に失敗しました。アプリケーションをエラー終了します。\n" + e.ToString());
                return false;
            }
        }
        */

        // 設定ファイル読み込み
        var iniprop = LoadIniFile("MailToStarPRNT.ini");

        // メール受信インスタンス初期化
        string mail_protocol;
        if (iniprop.TryGetValue("MailProtocol", out mail_protocol))
        {
            if (Regex.IsMatch(mail_protocol, "IMAP4?", RegexOptions.IgnoreCase))
                this.mailclient = new MailKit.Net.Imap.ImapClient();
            else if (Regex.IsMatch(mail_protocol, "POP3?", RegexOptions.IgnoreCase))
                this.mailclient = new MailKit.Net.Pop3.Pop3Client();
            else
            {
                Trace.TraceError("MailProtocolの設定値が正しくありません。IMAPかPOPを指定してください。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }
        else
            this.mailclient = new MailKit.Net.Pop3.Pop3Client();

        if (!iniprop.TryGetValue("MailServerName", out this.mail_server_name))
        {
            Trace.TraceError("必須設定であるMailServerNameが設定されていません。アプリケーションをエラー終了します。");
            hc.Stop();
            return false;
        }
        if (!iniprop.TryGetValue("MailServerUserName", out this.mail_server_username))
        {
            Trace.TraceError("必須設定であるMailServerUserNameが設定されていません。アプリケーションをエラー終了します。");
            hc.Stop();
            return false;
        }
        if (!iniprop.TryGetValue("MailServerPassword", out this.mail_server_password))
        {
            Trace.TraceError("必須設定であるMailServerPasswordが設定されていません。アプリケーションをエラー終了します。");
            hc.Stop();
            return false;
        }
        if (iniprop.TryGetValue("MailServerSSL", out string mail_server_ssl_str))
        {
            if (Regex.IsMatch(mail_server_ssl_str, @"^Yes$", RegexOptions.IgnoreCase))
                this.mail_server_ssl = true;
            else if (Regex.IsMatch(mail_server_ssl_str, @"^No$", RegexOptions.IgnoreCase))
                this.mail_server_ssl = false;
            else
            {
                Trace.TraceError("MailServerSSLの設定値はYesかNoである必要があります。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }
        if (iniprop.TryGetValue("MailServerPort", out string mail_server_port_number_str))
        {
            if(!UInt16.TryParse(mail_server_port_number_str,out this.mail_server_port_number))
            {
                Trace.TraceError("MailServerPortNumberの設定値は0以上65535以下の整数である必要があります。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }
        else
        {
            // 設定ファイルにポート番号が設定されていない場合は、デフォルトポート番号を使用する。
            if (this.mailclient is MailKit.Net.Imap.ImapClient && this.mail_server_ssl)
                this.mail_server_port_number = 993;
            else if (this.mailclient is MailKit.Net.Imap.ImapClient && !this.mail_server_ssl)
                this.mail_server_port_number = 143;
            else if (this.mailclient is MailKit.Net.Pop3.Pop3Client && this.mail_server_ssl)
                this.mail_server_port_number = 995;
            else if (this.mailclient is MailKit.Net.Pop3.Pop3Client && !this.mail_server_ssl)
                this.mail_server_port_number = 110;
        }
        iniprop.TryGetValue("MailPrintFromCondition", out this.mail_from_condition);
        iniprop.TryGetValue("MailPrintSubjectCondition", out this.mail_subject_condition);
        iniprop.TryGetValue("MailPrintBodyCondition", out this.mail_body_condition);

        if (iniprop.TryGetValue("PrintStartStopMessage", out string print_on_start_stop_str))
        {
            if (Regex.IsMatch(print_on_start_stop_str, @"^Yes$", RegexOptions.IgnoreCase))
                this.print_on_start_stop = true;
            else if (Regex.IsMatch(print_on_start_stop_str, @"^No$", RegexOptions.IgnoreCase))
                this.print_on_start_stop = false;
            else
            {
                Trace.TraceError("PrintStartStopMessageの設定値はYesかNoである必要があります。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }
        if (iniprop.TryGetValue("PrintErrors", out string print_on_error_str))
        {
            if (Regex.IsMatch(print_on_error_str, @"^Yes$", RegexOptions.IgnoreCase))
                this.print_on_error = true;
            else if (Regex.IsMatch(print_on_error_str, @"^No$", RegexOptions.IgnoreCase))
                this.print_on_error = false;
            else
            {
                Trace.TraceError("PrintErrorsの設定値はYesかNoである必要があります。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }

        // StarPRNTプリンターの初期化
        string model_name = "";
        string firmware_version = "";
        if (!iniprop.TryGetValue("StarPRNTPortName", out this.port_name))
        {
            List<StarMicronics.StarIO.PortInfo> printers = StarMicronics.StarIO.Factory.I.SearchPrinter();

            printers.ForEach(printer =>
            {
#if DEBUG
                Console.WriteLine("================================");
                Console.WriteLine("PortName: " + printer.PortName);
                Console.WriteLine("ModelName: " + printer.ModelName);
                Console.WriteLine("MacAddress: " + printer.MacAddress);
                Console.WriteLine("USBSerialNumber: " + printer.USBSerialNumber);
#endif
                if (printer.ModelName != "")
                {
                    this.port_name = printer.PortName;
                    return;
                }
            });
            if (this.port_name == "")
            {
                Trace.TraceError("StarPRNTプリンターが見つかりませんでした。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }

        // StarPRNTプリンターへの接続開始
        StarMicronics.StarIO.IPort starprnt_port;
        try
        {
            if(iniprop.TryGetValue("StarPRNTTimeOutSeconds",out string print_timeout_str)){
                if (int.TryParse(print_timeout_str, out this.print_timeout))
                    this.print_timeout *= 1000; // ミリ秒単位に修正
                else
                    this.print_timeout = 10000;
            }

            starprnt_port = StarMicronics.StarIO.Factory.I.GetPort(this.port_name, "", this.print_timeout);
            var devinfo = starprnt_port.GetFirmwareInformation();
            model_name = devinfo["ModelName"];
            firmware_version = devinfo["FirmwareVersion"];
            Trace.TraceInformation("次のStarPRNTプリンターを使用します。\nモデル名: " + model_name + ", ファームウェア: " + firmware_version + ", 通信ポート: " + this.port_name);
        }
        catch(StarMicronics.StarIO.PortException e)
        {
            Trace.TraceError("StarPRNTプリンターに接続できませんでした。アプリケーションをエラー終了します。\n" + e.Message);
            hc.Stop();
            return false;
        }

        // StarPRNTエミュレーションモードの設定
        if(iniprop.TryGetValue("StarPRNTEmulation", out string emulation_str))
        {
            switch (emulation_str)
            {
                case "StarGraphic":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.StarGraphic;
                    break;
                case "StarLine":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.StarLine;
                    break;
                case "StarPRNT":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.StarPRNT;
                    break;
                case "StarPRNTL":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.StarPRNTL;
                    break;
                case "StarDotImpact":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.StarDotImpact;
                    break;
                case "EscPos":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.EscPos;
                    break;
                case "EscPosMobile":
                    this.emulation = StarMicronics.StarIOExtension.Emulation.EscPosMobile;
                    break;
                default:
                    Trace.TraceError("StarPRNTEmulationの設定値が正しくありません。アプリケーションをエラー終了します。");
                    hc.Stop();
                    return false;
            }
        }
        else
            this.emulation = GuessStarPRNTEmulation(model_name);
#if DEBUG
        Trace.WriteLine("StarPRNT Emulation Mode: " + this.emulation.ToString());
#endif

        // 起動時メッセージを印刷
        if (this.print_on_start_stop)
        {
            if (!SendStringToStarPRNT(starprnt_port,"★" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 自動印刷を開始します★\n\n\n\n\n\n\n\n\n\n"))
            {
                Trace.TraceError("StarPRNTプリンターへの初期化メッセージの送信時にエラーが発生しました。アプリケーションをエラー終了します。");
                hc.Stop();
                return false;
            }
        }

        // プリンター接続を終了
        StarMicronics.StarIO.Factory.I.ReleasePort(starprnt_port);
        starprnt_port = null;

        // メールログイン試行
        PrintMails(null);

        // メールチェックタイマー設定・開始
        int mail_check_interval = 30000;
        if(iniprop.TryGetValue("MailCheckIntervalSeconds", out string mail_check_interval_str))
        {
            if (int.TryParse(mail_check_interval_str, out mail_check_interval))
                mail_check_interval *= 1000; // ミリ秒単位に修正
            else
                mail_check_interval = 30000;
        }
        timer.Interval = mail_check_interval;
        timer.Start();

        return true;
    }
    public void Stop()
    {
        if (this.print_on_start_stop)
        {
            // StarPRNTプリンターへの接続開始
            StarMicronics.StarIO.IPort starprnt_port = null;
            try
            {
                starprnt_port = StarMicronics.StarIO.Factory.I.GetPort(this.port_name, "", this.print_timeout);
            }
            catch (StarMicronics.StarIO.PortException ex)
            {
                Trace.TraceError("StarPRNTプリンターに接続できませんでした。\n" + ex.Message);
                return;
            }

            if (!SendStringToStarPRNT(starprnt_port,"★" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 自動印刷を停止します★\n\n\n\n\n\n\n\n\n\n"))
                Trace.TraceError("StarPRNTプリンターへの停止時メッセージの送信時にエラーが発生しました。");

            // プリンター接続を終了
            StarMicronics.StarIO.Factory.I.ReleasePort(starprnt_port);
            starprnt_port = null;
        }
        timer.Stop();
        Trace.TraceInformation("アプリケーションを終了します。");
    }
    private byte[] BuildCommandsFromString(string str)
    {
        var builder = StarMicronics.StarIOExtension.StarIoExt.CreateCommandBuilder(this.emulation);
        builder.BeginDocument();
        builder.AppendInternational(InternationalType.USA);
        builder.AppendFontStyle(FontStyleType.A);
        builder.AppendCodePage(CodePageType.UTF8);
        builder.AppendAlignment(AlignmentPosition.Left);
        builder.Append(Encoding.UTF8.GetBytes(str));
        builder.AppendCutPaper(CutPaperAction.PartialCutWithFeed);
        builder.EndDocument();

        return builder.Commands;
    }
    private bool SendStringToStarPRNT(StarMicronics.StarIO.IPort starprnt_port,string str)
    {
        try
        {
            if (this.BeginCheckStarPRNTPrinter(starprnt_port))
            {
                var commands = this.BuildCommandsFromString(str);
                uint sent_bytes = starprnt_port.WritePort(commands, 0, (uint)commands.Length);
                if(!this.EndCheckStarPRNTPrinter(starprnt_port))
                    return false;
                if (sent_bytes != (uint)commands.Length)
                {
                    Trace.TraceError("印刷データの一部がStarPRNTプリンターに送信されませんでした。");
                    return false;
                }
                return true;
            }
            else
                return false;
        }
        catch (StarMicronics.StarIO.PortException e)
        {
            string errdesc = "エラーが発生しました。";
            if (e.ErrorCode == StarMicronics.StarIO.StarResultCode.ErrorPaperPresent)
                errdesc = "用紙保持制御のタイムアウトが発生しました。";
            else if (e.ErrorCode == StarMicronics.StarIO.StarResultCode.ErrorFailed)
                errdesc = "何らかのエラーが発生しました。";

            Trace.TraceError("StarPRNTプリンターとの印刷中の通信で" + errdesc + "\n" + e.Message);
            return false;
        }
    }
    private bool CheckStarPRNTPrinterStatus(StarMicronics.StarIO.StarPrinterStatus status)
    {
        if (status.PaperDetectionError)
        {
            throw new WarningException(@"プリンターの用紙区切りを検出できませんでした。");
        }
        if (status.CompulsionSwitch)
        {
            throw new WarningException(@"ドロアーのコンパルジョンスイッチが押されています。");
        }
        if (status.CoverOpen)
        {
            throw new WarningException(@"プリンターカバーが開いています。");
        }
        if (status.CutterError)
        {
            throw new Exception(@"プリンターのオートカッターで問題が発生しています。");
        }
        if (status.HeadThermistorError)
        {
            throw new Exception(@"プリンターのヘッドサーミスターに異常が発生しています。");
        }
        if (status.Offline)
        {
            throw new WarningException(@"プリンターがオフラインになっています。");
        }
        if (status.OverTemp)
        {
            throw new WarningException(@"プリンターの印字ヘッドが高温により停止中です。");
        }
        if (status.ReceiptPaperEmpty)
        {
            throw new WarningException(@"プリンターの用紙切れです。");
        }
        if (status.ReceiptPaperNearEmptyInner)
        {
            throw new WarningException(@"プリンターの用紙残量が僅かです。");
        }
        if (status.ReceiveBufferOverflow)
        {
            throw new Exception(@"プリンターの受信バッファがフルになっています。");
        }
        if (status.VoltageError)
        {
            throw new Exception(@"プリンターの電源電圧で異常値を検出しました。");
        }
        if (status.UnrecoverableError)
        {
            throw new Exception(@"プリンターに復帰不可能エラーが発生しています。");
        }
        return true;
    }

    private bool BeginCheckStarPRNTPrinter(StarMicronics.StarIO.IPort starprnt_port)
    {
        try
        {
            var status = starprnt_port.BeginCheckedBlock();
            return CheckStarPRNTPrinterStatus(status);
        }
        catch (StarMicronics.StarIO.PortException e)
        {
            string errdesc = "エラーが発生しました。";
            if (e.ErrorCode == StarMicronics.StarIO.StarResultCode.ErrorPaperPresent)
                errdesc = "用紙保持制御のタイムアウトが発生しました。";
            else if (e.ErrorCode == StarMicronics.StarIO.StarResultCode.ErrorFailed)
                errdesc = "何らかのエラーが発生しました。";

            Trace.TraceError("StarPRNTプリンターとの印刷開始時の通信で" + errdesc + "\n" + e.Message);
            return false;
        }
        catch (WarningException e)
        {
            Trace.TraceWarning("StarPRNTプリンターとの印刷開始時に次の問題が検出されました。\n" + e.Message);
            return true;
        }
        catch (Exception e)
        {
            Trace.TraceError("StarPRNTプリンターとの印刷開始時に次の問題が検出されました。\n" + e.Message);
            return false;
        }
    }
    private bool EndCheckStarPRNTPrinter(StarMicronics.StarIO.IPort starprnt_port)
    {
        try
        {
            var status = starprnt_port.EndCheckedBlock();
            return CheckStarPRNTPrinterStatus(status);
        }
        catch (StarMicronics.StarIO.PortException e)
        {
            string errdesc = "エラーが発生しました。";
            if (e.ErrorCode == StarMicronics.StarIO.StarResultCode.ErrorPaperPresent)
                errdesc = "用紙保持制御のタイムアウトが発生しました。";
            else if (e.ErrorCode == StarMicronics.StarIO.StarResultCode.ErrorFailed)
                errdesc = "何らかのエラーが発生しました。";

            Trace.TraceError("StarPRNTプリンターとの印刷終了時の通信で" + errdesc + "\n" + e.Message);
            return false;
        }
        catch (WarningException e)
        {
            Trace.TraceWarning("StarPRNTプリンターとの印刷終了時に次の問題が検出されました。\n" + e.Message);
            return true;
        }
        catch (Exception e)
        {
            Trace.TraceError("StarPRNTプリンターとの印刷終了時に次の問題が検出されました。\n" + e.Message);
            return false;
        }
    }

    private StarMicronics.StarIOExtension.Emulation GuessStarPRNTEmulation(string modelname)
    {
        if (Regex.IsMatch(modelname, @"^TSP1\d\d[\D\s].*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^TSP1\d\d[\D\s].*', emulation = StarGraphic");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarGraphic;
        }
        if (Regex.IsMatch(modelname, @"^TSP[678]\d\d[\D\s].*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^TSP[678]\d\d[\D\s].*', emulation = StarLine");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarLine;
        }
        if (Regex.IsMatch(modelname, @"^FVP10.*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^FVP10.*', emulation = StarLine");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarLine;
        }
        if (Regex.IsMatch(modelname, @"^mC-Print.*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^mC-Print.*', emulation = StarPRNT");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarPRNT;
        }
        if (Regex.IsMatch(modelname, @"^mPOP.*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^mPOP.*', emulation = StarPRNT");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarPRNT;
        }
        if (Regex.IsMatch(modelname, @"^SM\-.*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^SM\-.*', emulation = StarPRNT");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarPRNT;
        }
        if (Regex.IsMatch(modelname, @"^SP\d\d\d.*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^SP\d\d\d.*', emulation = StarDotImpact");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarDotImpact;
        }
        if (Regex.IsMatch(modelname, @"^BSC10.*"))
        {
#if DEBUG
            Trace.WriteLine(@"Model name = " + modelname + @", pattern matched = '^BSC10.*', emulation = StarLine");
#endif
            return StarMicronics.StarIOExtension.Emulation.StarLine;
        }
        return StarMicronics.StarIOExtension.Emulation.StarPRNT;
    }

    private Dictionary<string,string> LoadIniFile(string inifile)
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
    private bool PrintMails(StarMicronics.StarIO.IPort starprnt_port = null)
    {
        try
        {
            this.mailclient.Connect(this.mail_server_name, this.mail_server_port_number, this.mail_server_ssl);
            this.mailclient.Authenticate(this.mail_server_username, this.mail_server_password);

            if (starprnt_port != null)
            {
                if(this.mailclient is MailKit.Net.Pop3.Pop3Client)
                {
                    MailKit.Net.Pop3.Pop3Client popclient = (MailKit.Net.Pop3.Pop3Client)this.mailclient;
                    for(int i = 0; i < popclient.Count; i++)
                    {
                        var message = popclient.GetMessage(i);

                        if (this.mail_from_condition != null && this.mail_from_condition != "")
                        {
                            bool contains_addr = false;
                            foreach(var addr in message.From)
                            {
                                if(addr is MimeKit.MailboxAddress)
                                {
                                    var mailbox_addr = (MimeKit.MailboxAddress)addr;
                                    if (mailbox_addr.Address == this.mail_from_condition)
                                    {
                                        contains_addr = true;
                                        break;
                                    }
                                }
                            }
                            if (!contains_addr)
                                continue;
                        }

                        if (this.mail_subject_condition != null && this.mail_subject_condition != "")
                            if (!message.Subject.Contains(this.mail_subject_condition))
                                continue;

                        if (this.mail_body_condition != null && this.mail_body_condition != "")
                            if (!message.TextBody.Contains(this.mail_body_condition))
                                continue;

                        if (SendStringToStarPRNT(starprnt_port,message.TextBody))
                            popclient.DeleteMessage(i);
                    }
                }
                else if(this.mailclient is MailKit.Net.Imap.ImapClient)
                {
                    MailKit.Net.Imap.ImapClient imapclient = (MailKit.Net.Imap.ImapClient)this.mailclient;

                    MailKit.Search.TextSearchQuery from_query = null, subject_query = null, body_query = null;

                    if (this.mail_from_condition != null && this.mail_from_condition != "")
                        from_query = MailKit.Search.SearchQuery.FromContains(this.mail_from_condition);

                    if (this.mail_subject_condition != null && this.mail_subject_condition != "")
                        subject_query = MailKit.Search.SearchQuery.SubjectContains(this.mail_subject_condition);

                    if (this.mail_body_condition != null && this.mail_body_condition != "")
                        body_query = MailKit.Search.SearchQuery.BodyContains(this.mail_body_condition);

                    MailKit.Search.SearchQuery query = MailKit.Search.SearchQuery.All;
                    if (from_query != null)
                    {
#if DEBUG
                        Trace.WriteLine(from_query.Term + " " + from_query.Text);
#endif
                        query = query.And(from_query);
                    }
                    if (subject_query != null)
                    {
#if DEBUG
                        Trace.WriteLine(subject_query.Term + " " + subject_query.Text);
#endif
                        query = query.And(subject_query);
                    }
                    if (body_query != null)
                    {
#if DEBUG
                        Trace.WriteLine(body_query.Term + " " + body_query.Text);
#endif
                        query = query.And(body_query);
                    }

                    imapclient.Inbox.Open(MailKit.FolderAccess.ReadWrite);
                    var uids = imapclient.Inbox.Search(query);
                    var deleted_uids = new List<MailKit.UniqueId>();
                    foreach (MailKit.UniqueId uid in uids)
                    {
                        var message = imapclient.Inbox.GetMessage(uid);
                        if (SendStringToStarPRNT(starprnt_port,message.TextBody))
                        {
                            imapclient.Inbox.AddFlags(uid, MailKit.MessageFlags.Deleted, true);
                            deleted_uids.Add(uid);
                        }
                    }
                    if (deleted_uids.Count > 0)
                        imapclient.Inbox.Expunge(deleted_uids);
                }
            }
        }
        catch (Exception e) when (e is System.IO.IOException || e is System.Net.Sockets.SocketException)
        {
            Trace.TraceError("メールサーバーとの通信に失敗しました。\n" + e.ToString());
            if (this.print_on_error)
            {
                SendStringToStarPRNT(starprnt_port,"【エラー】" +
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " +
                    "メールサーバーとの通信に失敗しました。自動印刷ができない状態です。ネットワークかメールサーバーに異常が発生している可能性があります。このエラーが出続ける場合は管理者にお問い合わせ下さい。\n" +
                    e.GetType().ToString() + ": " + e.Message + "\n\n\n\n\n\n\n\n\n\n");
            }
        }
        catch (MailKit.Security.AuthenticationException e)
        {
            Trace.TraceError("メールサーバーへのログインに失敗しました。ユーザー名かパスワードが間違っているか、サーバー側でアカウントロックされた可能性があります。\n" + e.ToString());
            if (this.print_on_error)
            {
                SendStringToStarPRNT(starprnt_port,"【エラー】" +
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " +
                "メールサーバーへのログインに失敗しました。自動印刷ができない状態です。管理者にお問い合わせ下さい。" +
                e.Message + "\n\n\n\n\n\n\n\n\n\n");
            }
        }
        catch (Exception e)
        {
            Trace.TraceError("メール処理でエラーが発生しました。\n" + e.ToString());
            if (this.print_on_error)
            {
                SendStringToStarPRNT(starprnt_port,"【エラー】" +
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " +
                "自動印刷ができない状態です。管理者にお問い合わせ下さい。\n" +
                e.GetType().ToString() + ": " + e.Message + "\n\n\n\n\n\n\n\n\n\n");
            }
        }
        finally
        {
            if (this.mailclient.IsConnected)
                this.mailclient.Disconnect(true);
        }

        return true;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var rc = Topshelf.HostFactory.Run(x =>
        {
            x.Service<MailToStarPRNT>(s =>
            {
                s.ConstructUsing(name => new MailToStarPRNT());
                s.WhenStarted((tc,hc) => { return tc.Start(hc); });
                s.WhenStopped(tc => tc.Stop());
            });
            x.EnableServiceRecovery(sr =>
            {
                sr.RestartService(1);
            });
            x.DependsOnEventLog();
            x.RunAsLocalSystem();
            x.StartAutomaticallyDelayed();
            x.SetDescription("Print e-mail to Star Micronics thermal printers");
            x.SetDisplayName(MailToStarPRNT.AppName);
            x.SetServiceName(MailToStarPRNT.AppName);
        });
        var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
        Environment.ExitCode = exitCode;
    }
}
