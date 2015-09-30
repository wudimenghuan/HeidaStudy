using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Threading;
using System.Net;

namespace HeidaStudy
{
    public partial class MainForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        Thread[] threads;
        int threadsCount;
        int targetTime;
        string loginUrl = "http://210.46.97.78/eol/homepage/common/login.jsp";
        string htmlTime;
        string stopUseTime;
        int totalMinutes;
        private delegate void Dele();
        int timeProcess;
        bool isStop = true;
        bool isFinished = false;
        Thread timeThread;

        public MainForm()
        {
            InitializeComponent();
        }

        private void lblLinkBlog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blog.stx8.com/");
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            CanStart();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            CanStart();
        }

        private void txtTargetTime_TextChanged(object sender, EventArgs e)
        {
            CanStart();
        }

        private void lblLinkBackup_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blog.sina.com.cn/u/2260172663");
        }

        private void lblLinkHome_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.stx8.com");
        }

        private void lblLinkBbs_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.stx8.com/");
        }

        private void setTip()
        {
            //balloonTip1.SetBalloonCaption(nameTxt, "��ʾ");
            balloonTip1.SetBalloonText(txtName, "������Ĵ����ɣ�210.46.97.78�����ѧƽ̨���û���");
            balloonTip1.SetBalloonText(txtPassword, "���������˾���������뿩��ѧУ�涨�����12λ��������Χ�Ļ�������ҳҲ�ϲ�ȥŶ��Ҫȥ��¥�޸������Ŷ");
            balloonTip1.SetBalloonText(btnSelectCourse, "�ڴ˴�ѡ��γ̣�����ѡ����ʱ������ʱ���޼��ٹ���Ŷ");
            balloonTip1.SetBalloonText(txtTargetTime, "����Ŀ��ʱ��ʱ����Զ�ֹͣŶ");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            setTip();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            User.userName = txtName.Text;
            User.password = txtPassword.Text;

            if (int.TryParse(txtTargetTime.Text, out targetTime) == false)
            {
                MessageBox.Show("����������");
                txtTargetTime.Focus();
                txtTargetTime.SelectAll();
            }
            else if (User.course == null || User.course.Equals(""))
            {
                MessageBox.Show("��ѡ��γ�");
                btnSelectCourse.Focus();
            }
            else if (User.Login())
            {
                System.Diagnostics.Process.Start("http://blog.stx8.com/");
                DisableButton();
                Ready();
            }
            else
            {
                MessageBox.Show("���������޷����ӷ�����");
            }
        }

        private void DisableButton()
        {
            btnStart.Enabled = false;
            txtName.Enabled = false;
            txtPassword.Enabled = false;
            btnSelectCourse.Enabled = false;
            txtTargetTime.Enabled = false;
        }

        private void btnSelectCourse_Click(object sender, EventArgs e)
        {
            using (FormCourse frmCourse = new FormCourse())
            {
                string[] strCourse = frmCourse.SelectCourse();
                if (!strCourse[0].Equals(""))
                {
                    lblSelectedCourse.Text = strCourse[1];
                    User.course = strCourse[0];
                }
                else
                {
                    lblSelectedCourse.Text = "δѡ��";
                    User.course = null;
                }
            }
            CanStart();
        }

        private void CanStart()
        {
            if (!txtName.Text.Equals("") && !txtPassword.Text.Equals("") && !txtTargetTime.Text.Equals("") && User.course != null)
                btnStart.Enabled = true;
            else
                btnStart.Enabled = false;
        }

        private void Ready()
        {
            isFinished = false;
            if (User.course == "all")
            {
                threadsCount = 1;
            }
            else
            {
                threadsCount = 4;
            }
            threads = new Thread[threadsCount];
            Thread startThread = new Thread(ControlThread);
            startThread.IsBackground = true;
            startThread.Name = "Start";
            startThread.Start();
        }

        private void ControlThread()
        {
            while (true)
            {
                if (isFinished)
                {
                    for (int i = 0; i < threadsCount; i++)
                    {
                        if (threads[i] != null && threads[i].IsAlive == true)
                            threads[i].Abort();
                    }
                    if (timeThread != null && timeThread.IsAlive == true)
                        timeThread.Abort();
                    Dele d = new Dele(Finished);
                    this.Invoke(d);
                    return;
                }
                isStop = true;
                for (int i = 0; i < threadsCount; i++)
                {
                    if (threads[i] != null && threads[i].IsAlive == true)
                        isStop = false;
                }
                if (isStop == true)
                {
                    if (lblTime.Text != "δ��½")
                    {
                        timeProcess = 100;
                        showTimeProcess();
                        ShowFinish();
                        Thread.Sleep(5000);
                    }
                    if (timeThread != null && timeThread.IsAlive == true)
                        timeThread.Abort();
                    Start();
                }
                Thread.Sleep(5000);
            }
        }

        private void Finished()
        {
            lblConnected.Text = "δ��½";
            lblStatus.Text = "�Ѿ����ѧϰ���ﵽ��ѧϰĿ�꣺" + txtTargetTime.Text;
            EnableButton();
        }

        private void EnableButton()
        {
            btnStart.Enabled = true;
            txtName.Enabled = true;
            txtPassword.Enabled = true;
            btnSelectCourse.Enabled = true;
            txtTargetTime.Enabled = true;
        }

        private void Start()
        {
            for (int i = 0; i < threadsCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(AutoStudy));
                threads[i].Name = "x" + i;
                threads[i].Start(User.course);
            }
        }

        private void AutoStudy(Object course)
        {
            string courseNumber;
            HttpHeader header;
            CookieContainer cookieContainer;
            courseNumber = (String)course;
            header = new HttpHeader();
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.12 (KHTML, like Gecko) Maxthon/3.0 Chrome/18.0.966.0 Safari/535.12";
            header.maxTry = 300;

            if (Thread.CurrentThread.Name == "x0")
            {
                timeProcess = 0;
                showTimeProcess();
            }

            while (true)
            {
                try
                {
                    cookieContainer = HTMLHelper.GetCooKie(loginUrl, "IPT_LOGINUSERNAME=" + User.userName + "&IPT_LOGINPASSWORD=" + User.password, header);
                    ShowConnectionSuccessful();
                    break;
                }
                catch (Exception)
                {
                    ShowCookieInfo();
                    continue;
                }
            }

            if (Thread.CurrentThread.Name == "x0")
            {
                timeProcess = 5;
                showTimeProcess();
                timeProcess = 10;
                showTimeProcess();
            }

            string html = null;
            if (courseNumber == "all")
            {
                string ts;
                TimeSpan span;
                TimeSpan span2 = new TimeSpan(8, 0, 0);
                span = DateTime.Now - Convert.ToDateTime("1970-01-01") - span2;
                ts = span.TotalMilliseconds.ToString("0");

                if (Thread.CurrentThread.Name == "x0")
                {
                    GetTotalTimeClass getTime = new GetTotalTimeClass(header, cookieContainer);
                    timeThread = new Thread(new ParameterizedThreadStart(GetTotalTime));
                    timeThread.Name = "time";
                    timeThread.IsBackground = true;
                    timeThread.Start(getTime);
                }

                for (int i = 0; i < 12; i++)
                {
                    Thread.Sleep(30000);
                    while (true)
                    {
                        try
                        {
                            if (i % 6 == 0)
                            {
                                span = DateTime.Now - Convert.ToDateTime("1970-01-01") - span2;
                                ts = span.TotalMilliseconds.ToString("0");
                                html = HTMLHelper.GetHtml("http://210.46.97.78/eol/refresh.jsp?_=" + ts, cookieContainer, header);
                            }
                            if (Thread.CurrentThread.Name == "x0")
                            {
                                timeProcess = 20 + 5 * i;
                                showTimeProcess();
                            }
                            ShowConnectionSuccessful();
                            break;
                        }
                        catch (Exception)
                        {
                            ShowConnectionFailed();
                            continue;
                        }
                    }
                }
                if (Thread.CurrentThread.Name == "x0")
                {
                    timeProcess = 85;
                    showTimeProcess();
                }
                Thread.Sleep(1000);
                for (; ; )
                {
                    try
                    {
                        html = Logout(header, cookieContainer, html);
                        if (Thread.CurrentThread.Name == "x0")
                        {
                            timeProcess = 90;
                            showTimeProcess();
                        }
                        ShowConnectionSuccessful();
                        break;
                    }
                    catch (Exception)
                    {
                        ShowConnectionFailed();
                        continue;
                    }
                }
            }
            else
            {
                //����ʱ��
                while (true)
                {
                    try
                    {
                        html = HTMLHelper.GetHtml("http://210.46.97.78/eol/lesson/onlinetime_listener.jsp?lessId=" + courseNumber + "&url=null", cookieContainer, header);
                        ShowConnectionSuccessful();
                        break;
                    }
                    catch (Exception)
                    {
                        ShowConnectionFailed();
                        continue;
                    }
                }

                if (Thread.CurrentThread.Name == "x0")
                {
                    timeProcess = 5;
                    showTimeProcess();
                }

                for (; ; )
                {
                    try
                    {
                        html = HTMLHelper.GetHtml("http://210.46.97.78/eol/lesson/onlinetime_listener.jsp?lessId=" + courseNumber + "&url=http://210.46.97.78/eol/welcomepage/course/index.jsp?lid=" + courseNumber, cookieContainer, header);
                        ShowConnectionSuccessful();
                        break;
                    }
                    catch (Exception)
                    {
                        ShowConnectionFailed();
                        continue;
                    }
                }

                if (Thread.CurrentThread.Name == "x0")
                {
                    timeProcess = 10;
                    showTimeProcess();

                    GetTimeClass getTime = new GetTimeClass(courseNumber, header, cookieContainer);
                    timeThread = new Thread(new ParameterizedThreadStart(GetTime));
                    timeThread.Name = "time";
                    timeThread.IsBackground = true;
                    timeThread.Start(getTime);
                }

                for (int i = 0; i < 7; i++)
                {
                    Thread.Sleep(60000);
                    for (; ; )
                    {
                        try
                        {
                            html = HTMLHelper.GetHtml("http://210.46.97.78/eol/lesson/onlinetime_listener.jsp?lessId=" + courseNumber + "&url=http://210.46.97.78/eol/welcomepage/course/index.jsp?lid=" + courseNumber, cookieContainer, header);
                            if (Thread.CurrentThread.Name == "x0")
                            {
                                timeProcess = 20 + 10 * i;
                                showTimeProcess();
                            }
                            ShowConnectionSuccessful();
                            break;
                        }
                        catch (Exception)
                        {
                            ShowConnectionFailed();
                            continue;
                        }
                    }
                }
                if (Thread.CurrentThread.Name == "x0")
                {
                    timeProcess = 85;
                    showTimeProcess();
                }
                Thread.Sleep(1000);
                while (true)
                {
                    try
                    {
                        html = Logout(header, cookieContainer, html);
                        if (Thread.CurrentThread.Name == "x0")
                        {
                            timeProcess = 90;
                            showTimeProcess();
                        }
                        ShowConnectionSuccessful();
                        break;
                    }
                    catch (Exception)
                    {
                        ShowConnectionFailed();
                        continue;
                    }
                }
            }
        }

        private string Logout(HttpHeader header, CookieContainer cookieContainer, string html)
        {
            for (; ; )
            {
                try
                {
                    html = HTMLHelper.GetHtml("http://210.46.97.78/eol/popups/logout.jsp", cookieContainer, header);
                    ShowConnectionSuccessful();
                    break;
                }
                catch (Exception)
                {
                    ShowConnectionFailed();
                    continue;
                }
            }
            return html;
        }

        private void GetTime(Object getTimeObject)
        {
            GetTimeClass getTimeClass = (GetTimeClass)getTimeObject;
            string html;
            while (true)
            {
                try
                {
                    html = HTMLHelper.GetHtml("http://210.46.97.78/eol/welcomepage/course/index.jsp?lid=" + getTimeClass.CourseNumber, getTimeClass.CookieContainer, getTimeClass.Header);
                    string first = "<li>��¼�γ���ʱ����";
                    string last = "</li>";
                    string yearFirst = "��¼�γ�ʱ�䣺";
                    string yearLast = "</li>";
                    int x = html.IndexOf(first);
                    int y = html.IndexOf(last, x);
                    x = x + first.Length;
                    htmlTime = html.Substring(x, y - x).Trim();
                    int x1 = html.IndexOf(yearFirst, y);
                    int x2 = html.IndexOf(yearLast, x1);
                    x1 = x1 + yearFirst.Length;
                    stopUseTime = html.Substring(x1, x2 - x1).Trim();
                    string[] times = stopUseTime.Substring(0, 10).Split('-');
                    int year = int.Parse(times[0]);
                    int month = int.Parse(times[1]);
                    int day = int.Parse(times[2]);
                    if (year > 2015)
                    {
                        Environment.Exit(0);
                    }
                    else if (month > 6)
                    {
                        Environment.Exit(0);
                    }
                    ShowTime();
                    ShowTotalProcess();
                    break;
                }
                catch (Exception)
                {
                    ShowTimeError();
                    continue;
                }
            }
        }

        private void GetTotalTime(Object getTimeObject)
        {
            GetTotalTimeClass getTimeClass = (GetTotalTimeClass)getTimeObject;
            string html;
            for (; ; )
            {
                try
                {
                    html = HTMLHelper.GetHtml("http://210.46.97.78/eol/welcomepage/student/index.jsp", getTimeClass.CookieContainer, getTimeClass.Header);
                    string first = "<li>������ʱ����";
                    string last = "</li>";
                    string yearFirst = "��¼ʱ�䣺<span class=\"loginlasttime\">";
                    string yearLast = "</li>";
                    int x = html.IndexOf(first);
                    int y = html.IndexOf(last, x);
                    x = x + first.Length;
                    htmlTime = html.Substring(x, y - x).Trim();
                    int x1 = html.IndexOf(yearFirst);
                    int x2 = html.IndexOf(yearLast, x1);
                    x1 = x1 + yearFirst.Length;
                    stopUseTime = html.Substring(x1, x2 - x1).Trim();
                    string[] times = stopUseTime.Substring(0, 10).Split('-');
                    int year = int.Parse(times[0]);
                    int month = int.Parse(times[1]);
                    int day = int.Parse(times[2]);
                    if (year > 2015)
                    {
                        Environment.Exit(0);
                    }
                    else if (month > 6)
                    {
                        Environment.Exit(0);
                    }
                    ShowTime();
                    ShowTotalProcess();
                    break;
                }
                catch (Exception)
                {
                    ShowTimeError();
                    continue;
                }
            }
        }

        private void ShowTime()
        {
            Dele d = new Dele(Time);
            this.Invoke(d);
        }

        private void Time()
        {
            lblStatus.Text = "�����ӣ�ʱ���Ѹ��£���Ҫ�˳�����...";
            lblTime.Text = htmlTime;
        }

        private void ShowTimeError()
        {
            Dele d = new Dele(TimeError);
            this.Invoke(d);
            Thread.Sleep(2000);
        }

        private void TimeError()
        {
            lblStatus.Text = "��ȡʱ������������»�ȡ";
        }

        private void ShowCookieInfo()
        {
            Dele d = new Dele(CookieInfo);
            this.Invoke(d);
            Thread.Sleep(2000);
        }

        private void CookieInfo()
        {
            lblConnected.Text = "����ʧ�ܣ�������ѹ��ɽ���������ٵ�ʱ�����У���ȴ��Զ�����";
            lblStatus.Text = "�����Զ�����";
        }

        private void ShowFinish()
        {
            Dele d = new Dele(Finish);
            this.Invoke(d);
            Thread.Sleep(2000);
        }

        private void Finish()
        {
            lblStatus.Text = "�ѳɹ����£����ڿ��԰�ȫ�˳���ȴ���һ���Զ�����";
        }

        private void ShowConnectionSuccessful()
        {
            Dele d = new Dele(ConnectionSuccessful);
            this.Invoke(d);
            Thread.Sleep(2000);
        }

        private void ConnectionSuccessful()
        {
            lblConnected.Text = "������";
            lblStatus.Text = "�����ӣ�����ѧϰ�У���Ҫ�˳�����";
        }

        private void ShowConnectionFailed()
        {
            Dele d = new Dele(ConnectionFailed);
            this.Invoke(d);
            Thread.Sleep(2000);
        }

        private void ConnectionFailed()
        {
            lblConnected.Text = "����ʧ��";
            lblStatus.Text = "������ѹ��ɽ�������Ŷ��У���ȴ�";
        }

        private void showTimeProcess()
        {
            Dele d = new Dele(TimeProcess);
            this.Invoke(d);
        }

        private void TimeProcess()
        {
            stbTime.Value = timeProcess;
        }

        private void ShowTotalProcess()
        {
            Dele d = new Dele(TotalProcess);
            this.Invoke(d);
        }

        private void TotalProcess()
        {
            int hourIndex = htmlTime.IndexOf("Сʱ");
            int minuteIndex = htmlTime.IndexOf("��");
            int minuteNumberIndex = hourIndex + 2;
            int hour = 0;
            int minute = 0;
            if (hourIndex == -1)
            {
                minute = Convert.ToInt32(htmlTime.Substring(0, minuteIndex));
            }
            else
            {
                hour = Convert.ToInt32(htmlTime.Substring(0, hourIndex));
                minute = Convert.ToInt32(htmlTime.Substring(minuteNumberIndex, minuteIndex - minuteNumberIndex));
            }
            totalMinutes = hour * 60 + minute;
            if (totalMinutes < targetTime)
            {
                stbTotalTime.Value = totalMinutes * 100 / targetTime;
            }
            else
            {
                stbTotalTime.Value = 100;
                isFinished = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            this.Close();
            Environment.Exit(0);
        }

        private void lblLinkSoftware_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://shenqi.heida.stx8.com");
        }

        private void lblQqGroup_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("tencent://groupwpa/?subcmd=all&param=7B2267726F757055696E223A3139383838393139322C2274696D655374616D70223A313432383233363139377D0A");
        }

        private void lblWeibo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://weibo.com/wudimenghuan");
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            using (FormDonate frmDonate = new FormDonate())
            {
                frmDonate.Donate();
            }
        }


    }

    class GetTimeClass
    {
        string courseNumber;

        public string CourseNumber
        {
            get { return courseNumber; }
            set { courseNumber = value; }
        }
        HttpHeader header;

        public HttpHeader Header
        {
            get { return header; }
            set { header = value; }
        }
        CookieContainer cookieContainer;

        public CookieContainer CookieContainer
        {
            get { return cookieContainer; }
            set { cookieContainer = value; }
        }

        public GetTimeClass(string courseNumber, HttpHeader header, CookieContainer cookieContainer)
        {
            this.courseNumber = courseNumber;
            this.header = header;
            this.cookieContainer = cookieContainer;
        }
    }

    class GetTotalTimeClass
    {
        HttpHeader header;

        public HttpHeader Header
        {
            get { return header; }
            set { header = value; }
        }
        CookieContainer cookieContainer;

        public CookieContainer CookieContainer
        {
            get { return cookieContainer; }
            set { cookieContainer = value; }
        }

        public GetTotalTimeClass(HttpHeader header, CookieContainer cookieContainer)
        {
            this.header = header;
            this.cookieContainer = cookieContainer;
        }
    }
}