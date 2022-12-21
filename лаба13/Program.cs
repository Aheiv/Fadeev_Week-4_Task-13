using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace лаба13
{
    public abstract class Client
    {
        public abstract void PrintInfo();
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


        public override void PrintInfo()
        {
            Console.WriteLine("Фамилия вкладчика: {0}", Surname);
            Console.WriteLine("Дата открытия вклада: {0}", DepositDate.ToShortDateString());
            Console.WriteLine("Размер вклада: {0}", DepositAmount + " рублей");
            Console.WriteLine("Процент по вкладу: {0}", DepositInterest + "%");
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

        public override void PrintInfo()
        {
            Console.WriteLine("Фамилия кредитора: {0}", Surname);
            Console.WriteLine("Дата выдачи кредита: {0}", CreditDate.ToShortDateString());
            Console.WriteLine("Размер кредита: {0}", CreditAmount + " рублей");
            Console.WriteLine("Процент по кредиту: {0}", CreditInterest + "%");
            Console.WriteLine("Остаток долга: {0}", CreditBalance + " рублей");
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

        public override void PrintInfo()
        {
            Console.WriteLine("Название организации: {0}", Name);
            Console.WriteLine("Дата открытия счета: {0}", AccountDate.ToShortDateString());
            Console.WriteLine("Номер счета: {0}", AccountNumber);
            Console.WriteLine("Сумма на счету: {0}", AccountAmount + " рублей");
        }

        public override bool IsClientByDate(DateTime date)
        {
            if (AccountDate == date)
                return true;
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Client> clientList = new List<Client>();
            StreamReader sr = new StreamReader("ClientList.txt", Encoding.Default);
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
                client.PrintInfo();
                Console.WriteLine();
            }

            Console.Write("Введите дату начала сотрудничества клиента: ");
            DateTime askDate = DateTime.Parse(Console.ReadLine());
            int foundClients = 0;

            foreach (Client client in clientList)
            {
                if (client.IsClientByDate(askDate))
                {
                    client.PrintInfo();
                    foundClients++;
                    Console.WriteLine();
                }
            }
            if (foundClients == 0)
            {
                Console.WriteLine("Клиенты по данной дате не найдены");
            }
        }
    }
}
