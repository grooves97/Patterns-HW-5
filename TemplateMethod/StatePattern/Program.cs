using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePattern
{
    interface IState
    {
        //включить
        void On();
        //выключить
        void Off();
        //печать
        void Print();
        //Добавить бумагу
        void AddPaper(int count);
    }

    /// <summary>
    /// Состояние ключения 
    /// </summary>
    class PowerOffState : IState
    {
        private readonly Printer _printer;

        public PowerOffState(Printer printer)
        {
            _printer = printer;
        }
        public void On()
        {
            Console.WriteLine("Принтер включен");
            _printer.SetState(_printer.WaitingState);
        }

        public void Off()
        {
            Console.WriteLine("Принтер и так выключен");
        }

        public void Print()
        {
            Console.WriteLine("Принтер отключен, печать невозможна");
        }

        public void AddPaper(int count)
        {
            _printer.AddPaper(count);
            Console.WriteLine("Бумага добавлена");
        }
    }

    /// <summary>
    /// Состояние ожидания
    /// </summary>
    class WaitingState : IState
    {
        private readonly Printer _printer;

        public WaitingState(Printer printer)
        {
            _printer = printer;
        }

        public void On()
        {
            Console.WriteLine("Принтер уже и так включен");
        }

        public void Off()
        {
            Console.WriteLine("Принтер выключен");
        }

        public void Print()
        {
            if (_printer.CountPaper > 0)
            {
                Console.WriteLine("Сейчас всё распечатаем");
                _printer.AddPaper(-1);
            }
            else
            {
                _printer.SetState(_printer.PaperOffState);
                _printer.PrintDocument();
            }
        }

        public void AddPaper(int count)
        {
            _printer.AddPaper(count);
            Console.WriteLine("Бумага добавлена");
        }
    }

    /// <summary>
    /// состояния проверки бумаги
    /// </summary>
    class PaperOffState : IState
    {
        private readonly Printer _printer;

        public PaperOffState(Printer printer)
        {
            _printer = printer;
        }

        public void On()
        {
            Console.WriteLine("Принтер уже и так включен");
        }

        public void Off()
        {
            Console.WriteLine("Принтер выключен");
            _printer.SetState(_printer.PowerOffState);
        }

        public void Print()
        {
            if (_printer.CountPaper > 0)
            {
                _printer.SetState(_printer.PrintState);
                _printer.PrintDocument();
            }
            else
            {
                Console.WriteLine("Бумаги нет, печатать не буду");
            }

        }

        public void AddPaper(int count)
        {
            Console.WriteLine("Добавляем бумагу");
            _printer.AddPaper(count);
            if (_printer.CountPaper > 0)
                _printer.SetState(_printer.WaitingState);
        }
    }

    /// <summary>
    /// состояние печати
    /// </summary>
    class PrintState : IState
    {
        private readonly Printer _printer;

        public PrintState(Printer printer)
        {
            _printer = printer;
        }
        public void On()
        {
            Console.WriteLine("Принтер уже и так включен");
        }

        public void Off()
        {
            Console.WriteLine("Принтер выключен");
        }

        public void Print()
        {
            if (_printer.CountPaper > 0)
            {
                Console.WriteLine("Идёт печать...");
                _printer.AddPaper(-1);
                _printer.SetState(_printer.WaitingState);
            }

            else
            {
                _printer.SetState(_printer.PaperOffState);
                _printer.PrintDocument();
            }

        }

        public void AddPaper(int count)
        {
            _printer.AddPaper(count);
            Console.WriteLine("Бумага добавлена");
        }
    }

    /// <summary>
    /// класс принтер
    /// </summary>
    class Printer
    {
        private IState _state;
        private int _countPaper;

        public PaperOffState PaperOffState { get; private set; }
        public PowerOffState PowerOffState { get; private set; }
        public PrintState PrintState { get; private set; }
        public WaitingState WaitingState { get; private set; }
        public int CountPaper
        {
            get { return _countPaper; }
        }

        public Printer()
        {
            PowerOffState = new PowerOffState(this);
            PaperOffState = new PaperOffState(this);
            PrintState = new PrintState(this);
            WaitingState = new WaitingState(this);
            _state = WaitingState;
        }

        public void SetState(IState state)
        {
            _state = state;
        }

        public void PrintDocument()
        {
            _state.Print();
        }

        public void PowerOff()
        {
            _state.Off();
        }
        public void PowerOn()
        {
            _state.On();
        }

        public void AddPaper(int count)
        {
            _countPaper += count;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var printer = new Printer();
            printer.PowerOn();
            printer.PrintDocument();
            printer.AddPaper(3);
            printer.PrintDocument();
            printer.PrintDocument();
            printer.PrintDocument();
            printer.PrintDocument();
            printer.PowerOff();

        }
    }
}
