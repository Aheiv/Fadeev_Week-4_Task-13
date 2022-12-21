using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба13wf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Client> clientList = new List<Client>();
            StreamReader sr = new StreamReader("ClientList.txt", Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                string[] s = sr.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 5)
                {
                    clientList.Add(new Creditor(s[0], Convert.ToDateTime(s[1]), Convert.ToDecimal(s[2]),
                                                      Convert.ToDouble(s[3]), Convert.ToDecimal(s[4])));
                }
                if (s.Length == 4)
                {
                    int number;
                    if (Int32.TryParse(s[2], out number))
                    {
                        clientList.Add(new Organization(s[0], Convert.ToDateTime(s[1]),
                                                     number, Convert.ToDecimal(s[3])));
                    }
                    else
                    {
                        clientList.Add(new Investor(s[0], Convert.ToDateTime(s[1]),
                                  Convert.ToDecimal(s[2]), Convert.ToDouble(s[3])));
                    }
                }
            }
            sr.Close();

            foreach (Client client in clientList)
            {
                client.PrintInfo(richTextBox1);
                richTextBox1.Text += "\n\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Client> clientList = new List<Client>();
            StreamReader sr = new StreamReader("ClientList.txt", Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                string[] s = sr.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 5)
                {
                    clientList.Add(new Creditor(s[0], Convert.ToDateTime(s[1]), Convert.ToDecimal(s[2]),
                                                      Convert.ToDouble(s[3]), Convert.ToDecimal(s[4])));
                }
                if (s.Length == 4)
                {
                    int number;
                    if (Int32.TryParse(s[2], out number))
                    {
                        clientList.Add(new Organization(s[0], Convert.ToDateTime(s[1]),
                                                     number, Convert.ToDecimal(s[3])));
                    }
                    else
                    {
                        clientList.Add(new Investor(s[0], Convert.ToDateTime(s[1]),
                                  Convert.ToDecimal(s[2]), Convert.ToDouble(s[3])));
                    }
                }
            }
            sr.Close();
            DateTime askDate;
            if (!DateTime.TryParse(textBox1.Text, out askDate))
                MessageBox.Show("Некорректный ввод даты!");
            else
            {
                askDate = DateTime.Parse(textBox1.Text);
                int foundClients = 0;
                richTextBox1.Text = "";
                foreach (Client client in clientList)
                {
                    if (client.IsClientByDate(askDate))
                    {
                        client.PrintInfo(richTextBox1);
                        foundClients++;
                    }
                }
                if (foundClients == 0)
                {
                    richTextBox1.Text = "Совпадений нет...";
                }
            }
        }
       public abstract class Client
        {
            public abstract void PrintInfo(RichTextBox richTextBox);
            public abstract bool IsClientByDate(DateTime date);
        }

        public class Investor : Client
        {
            public string Surname { get; set; }
            public DateTime DepositDate { get; set; }
            public decimal DepositAmount { get; set; }
            public double DepositInterest { get; set; }

            public Investor(string surname, DateTime depositDate, decimal depositAmount, double depositInteres)
            {
                Surname = surname;
                DepositDate = depositDate;
                DepositAmount = depositAmount;
                DepositInterest = depositInteres;
            }


            public override void PrintInfo(RichTextBox richTextBox)
            {
                richTextBox.Text += $"Фамилия вкладчика: {Surname}\n";
                richTextBox.Text += $"Дата открытия вклада: {DepositDate.ToShortDateString()}\n";
                richTextBox.Text += $"Размер вклада: {DepositAmount}" + " рублей\n";
                richTextBox.Text += $"Процент по вкладу: {DepositInterest}" + "%\n";
            }

            public override bool IsClientByDate(DateTime date)
            {
                if (DepositDate == date)
                    return true;
                return false;
            }
        }

        public class Creditor : Client
        {
            public string Surname { get; set; }
            public DateTime CreditDate { get; set; }
            public decimal CreditAmount { get; set; }
            public double CreditInterest { get; set; }
            public decimal CreditBalance { get; set; }

            public Creditor(string surname, DateTime creditDate, decimal creditAmount, double creditInterest,
                            decimal creditBalance)
            {
                Surname = surname;
                CreditDate = creditDate;
                CreditAmount = creditAmount;
                CreditInterest = creditInterest;
                CreditBalance = creditBalance;
            }

            public override void PrintInfo(RichTextBox richTextBox)
            {
                richTextBox.Text += $"Фамилия кредитора: {Surname}\n";
                richTextBox.Text += $"Дата выдачи кредита: {CreditDate.ToShortDateString()}\n";
                richTextBox.Text += $"Размер кредита: {CreditAmount}" + " рублей\n";
                richTextBox.Text += $"Процент по кредиту: {CreditInterest}" + "%\n";
                richTextBox.Text += $"Остаток долга: {CreditBalance}" + " рублей\n";
            }

            public override bool IsClientByDate(DateTime date)
            {
                if (CreditDate == date)
                    return true;
                return false;
            }
        }

        public class Organization : Client
        {
            public string Name { get; set; }
            public DateTime AccountDate { get; set; }
            public int AccountNumber { get; set; }
            public decimal AccountAmount { get; set; }

            public Organization(string name, DateTime accountDate, int accountNumber, decimal accountAmount)
            {
                Name = name;
                AccountDate = accountDate;
                AccountNumber = accountNumber;
                AccountAmount = accountAmount;
            }

            public override void PrintInfo(RichTextBox richTextBox)
            {
                richTextBox.Text += $"Название организации: {Name}\n";
                richTextBox.Text += $"Дата открытия счета: {AccountDate.ToShortDateString()}\n";
                richTextBox.Text += $"Номер счета: {AccountNumber}\n";
                richTextBox.Text += $"Сумма на счету: {AccountAmount}" + " рублей\n";
            }

            public override bool IsClientByDate(DateTime date)
            {
                if (AccountDate == date)
                    return true;
                return false;
            }
        }
    }
}
