using System;
using System.Configuration;
using System.Windows.Forms;

namespace Notificator
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            FillSettings();
        }

        private void FillSettings()
        {
            try
            {
                textBoxUrl.Text = (string)Properties.Settings.Default["Url"];
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                textBoxInterval.Text = (string)Properties.Settings.Default["Interval"];
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void TextBoxUrl_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Url"] = textBoxUrl.Text;
            Properties.Settings.Default.Save();
        }

        private void TextBoxInterval_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Interval"] = textBoxInterval.Text;
            Properties.Settings.Default.Save();
        }
    }
}
