using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mikhailov.Nsudotnet.NumberGuesser
{
    class Program
    {
        private static string _login;
        private static int _myNumber;
        private static int _userNumber;
        private static int _effortCount = 0;
        private static int[] _history = new int[1000]; 
        private static string[] _myExpression = {", you are loser", ", dno", ", get out here", ", go home", ", bezdar"};
        static void Main(string[] args)
        {
            DateTime _sTime, _eTime;
            string str;
            
            Console.WriteLine("Say me your login");
            _login = Console.ReadLine();
            Random rnd = new Random();
            _myNumber = rnd.Next(0, 100);
            Console.WriteLine("I invented a number [0; 100]. You must guess it. I watch time. The game starts.");
            
            _sTime = DateTime.Now;
            do
            {
                str = Console.ReadLine();
                if (str == "q")
                {
                    Console.WriteLine(":(");
                    return;
                }
                if (str == "")
                        continue;
                   
                try
                {
                    _userNumber = Int32.Parse(str);
                    _history[2*_effortCount] = _userNumber; 
                    if (_userNumber < _myNumber)
                    {
                        _history[2*_effortCount + 1] = 0;
                        Console.WriteLine("Your number is less than mine");
                    }
                    else if (_userNumber > _myNumber)
                    {
                        _history[2*_effortCount + 1] = 1;
                        Console.WriteLine("Your number is bigger than mine");
                    }
                    else break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("You need 0 <= number <= 100");
                    continue;
                }
                _effortCount++;
                if (_effortCount % 4 == 0)
                    Console.WriteLine(_login + _myExpression[rnd.Next(0, 4)]);
            } while (true);
            _eTime = DateTime.Now;
            
            Console.WriteLine("Krasav4ik");
            Console.WriteLine("Count efforts: {0}", _effortCount + 1);
            for (int i = 0; i < _effortCount * 2; i += 2)
            {
                Console.Write(_history[i]);
                if (_history[i+1] == 0)
                    Console.WriteLine(" less");
                else   
                    Console.WriteLine(" bigger");
            }
            Console.WriteLine((_eTime - _sTime).Minutes + " minutes");
        }
    }
}
