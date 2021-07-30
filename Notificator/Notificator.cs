using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Notificator
{
    public partial class Notificator : Form
    {
        string userName = ConfigurationManager.AppSettings["username"];
        string url = ConfigurationManager.AppSettings["url"];
        string interval = ConfigurationManager.AppSettings["interval"] + "000";
        string toastUrl = "";

        public Notificator()
        {
            InitializeComponent();
            TrayMenuContext();
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            Thread checkMessage = new Thread(new ThreadStart(CheckMessage))
            {
                IsBackground = true
            };
            checkMessage.Start();
        }

        private void TrayMenuContext()
        {
            notifyIcon1.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, MenuExit_Click);
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void CheckMessage()
        {
            while (true)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + userName);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    if (result != "")
                    {
                        Notification notification = JsonConvert.DeserializeObject<Notification>(result);

                        if (notification.Success)
                        {
                            toastUrl = notification.Data.Url;

                            ToastContent toastContent = new ToastContent()
                            {
                                Launch = "bodyTapped",
                                Visual = new ToastVisual()
                                {
                                    BindingGeneric = new ToastBindingGeneric()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = notification.Data.Message
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = UnixToDate(notification.Data.Date, "dd-MM-yyyy HH:mm")
                                            }
                                        }
                                    }
                                },
                                Actions = new ToastActionsCustom()
                                {
                                    Buttons = { new ToastButton("Open", "go") }
                                },
                                Header = new ToastHeader("header", notification.Data.Title, "header")
                            };

                            var doc = new XmlDocument();
                            doc.LoadXml(toastContent.GetContent());

                            var promptNotification = new ToastNotification(doc);
                            promptNotification.Activated += PromptNotificationOnActivated;

                            ToastNotificationManagerCompat.CreateToastNotifier().Show(promptNotification);
                        }
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg.ToString());
                }

                Thread.Sleep(int.Parse(interval));
            }
        }

        private void PromptNotificationOnActivated(ToastNotification sender, object args)
        {
            ToastActivatedEventArgs strArgs = args as ToastActivatedEventArgs;

            switch (strArgs.Arguments)
            {
                case "go":
                    System.Diagnostics.Process.Start(toastUrl);
                    toastUrl = "";
                    break;
                case "bodyTapped":
                    //System.Diagnostics.Process.Start(toastUrl);
                    //toastUrl = "";
                    break;
            }
        }

        private static string UnixToDate(int Timestamp, string ConvertFormat)
        {
            DateTime ConvertedUnixTime = DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
            return ConvertedUnixTime.ToString(ConvertFormat);
        }
    }
}
