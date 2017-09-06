using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailApp
{
    public partial class Form1 : Form
    {
        private string _mailFromAddress;
        private string _mailFromPassword;
        private string _mailToAddress;
        private int _countOfMessages;
        private string _subject;
        private string _mailFromName;
        private bool _isHtmlCode;
        private string _mailText;
        private bool _success;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckStrings() == false)
            {
                MessageBox.Show(@"Заполните, пожалуйста, все поля!", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }

            CheckHtml();
            SendMessage();

            if (_success) MessageBox.Show(@"Письма отправлены! Время доставки зависит от скорости работы сервиса.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            _success = false;
        }

        /// <summary>
        /// Проверка на заполнение всех полей
        /// </summary>
        /// <returns></returns>
        private bool CheckStrings()
        {
            _mailFromAddress = textBox1.Text;
            _mailFromPassword = textBox2.Text;
            _mailToAddress = textBox3.Text;
            _subject = textBox4.Text;
            _mailFromName = textBox5.Text;
            _mailText = textBox7.Text;

            if (String.IsNullOrWhiteSpace(_mailFromAddress) || String.IsNullOrWhiteSpace(_mailFromPassword) ||
                String.IsNullOrWhiteSpace(_mailToAddress) || String.IsNullOrWhiteSpace(_subject) ||
                String.IsNullOrWhiteSpace(_mailText) || String.IsNullOrWhiteSpace(_mailFromName))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка на HTML код в тексте письма
        /// </summary>
        private void CheckHtml()
        {
            if (checkBox1.Checked)
            {
                _isHtmlCode = true;
            }
            else
            {
                _isHtmlCode = false;
            }
        }

        private void SendMessage()
        {
            try
            {
                _countOfMessages = (int)numericUpDown1.Value;
                Task[] tasks = new Task[_countOfMessages];
                int i;
                for (i = 0; i < _countOfMessages; i++)
                {
                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);

                    smtp.Credentials = new NetworkCredential(_mailFromAddress, _mailFromPassword);
                    smtp.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(_mailFromAddress, _mailFromName);
                    mail.To.Add(_mailToAddress);
                    mail.Subject = string.Format(_subject);
                    mail.Body = string.Format(_mailText);
                    mail.IsBodyHtml = _isHtmlCode;
                    tasks[i] = Task.Run(() => smtp.Send(mail));
                }

                Task.WaitAll(tasks);
                _success = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mailFromAddress = null;
            _mailFromPassword = null;
            _mailToAddress = null;
            _subject = null;
            _mailFromName = null;
            _mailText = null;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox7.Text = string.Empty;
            numericUpDown1.Value = 1;
            _isHtmlCode = false;
            checkBox1.Checked = false;
            _success = false;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
    }
}
