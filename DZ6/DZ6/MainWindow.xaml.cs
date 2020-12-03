using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace DZ6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// Program Includes: 
    ///                   1.Input amount of currency in 3 types Dollar, Euro, Yuan 
    ///                   2.The system will check the format of the input data. If the condition is met (only accept numbers), system will allow to enter data.
    ///                   3.After that, system will convert amount of currency to RUB 
    ///                   4.Each time user enter the new data, system will make the new object (Details about the objects are listed below).
    ///                   5.Show all the objects in the Listview of program
    ///                   6.User can remove objects when click on the object which want to remove and the button "Remove" will be enabled, then click the button to remove
    ///                   7.When click on the button "Compare Objects", system will drop the Message Box show the comparision bettew each object with each other(Details are below)  

    
    //Base Class Abstract Currency
    //1.Property "Result": result after converted from every derived class below
    //2.Abstract Properties "ObjType","Input" and normal Properties "NumberObjOutput","GetResult": Just can read data from it, cannot write. They also were used for display Objects on Listview
    abstract class Currency
    {
        protected string Result;
        private int NumberObj;
        public Currency(int numberObj) //NumberObj will be added each new object created
        {
            NumberObj = numberObj;
        }
        public int NumberObjOutput { get { return NumberObj; } }
        public abstract string ObjType { get; }
        public abstract string Input { get; }
        public string GetResult { get { return Result; } }    
    }

    //These Derived classes Dollar, Euro, Yuan were inherited from class Currency
    //Each Class was overrided function Equals(). 
    //  Override the Equals () function in each class to ensure the correct comparison of the value of each element in each created object, not a reference.
    //  Equals function is used for the function "compare objects"
    class Dollar : Currency
    {
        private double DollarInput;
        private string DollarInputStr;
        public Dollar(int numberObj,double input)
            : base(numberObj)
        {
            DollarInput = input;
            Result = (input * 75.40).ToString();
            DollarInputStr = input.ToString();
        }
        public override string Input
        {
            get 
            {
                return DollarInputStr;
            }
        }
        public override string ObjType
        {
            get 
            {
                return "Dollar Class";
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var R = (Dollar)obj;
            return Result.Equals(R.Result) && DollarInput.Equals(R.DollarInput);
        }
    }
    class Euro : Currency
    {
        private double EuroInput;
        private string EuroInputStr;
        public Euro(int numberObj, double input)
            : base(numberObj)
        {
            EuroInput = input;
            Result = (input * 91.12).ToString();
            EuroInputStr = input.ToString();
        }
        public override string Input
        {
            get 
            {
                return EuroInputStr;
            }
        }
        public override string ObjType
        {
            get
            {
                return "Euro Class";
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var R = (Euro)obj;
            return Result.Equals(R.Result) && EuroInput.Equals(R.EuroInput);
        }
    }
    class Yuan : Currency
    {
        private double YuanInput;
        private string YuanInputStr;
        public Yuan(int numberObj, double input)
            : base(numberObj)
        {
            YuanInput = input;
            Result = (input * 11.49).ToString();
            YuanInputStr = input.ToString();
        }
        public override string Input
        {
            get
            {
                return YuanInputStr;
            }
        }
        public override string ObjType
        {
            get
            {
                return "Yuan Class";
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var R = (Yuan)obj;
            return Result.Equals(R.Result) && YuanInput.Equals(R.YuanInput);
        }
    }

    public partial class MainWindow : Window
    {
        static List<Currency> DB = new List<Currency>(); // Global List, use to stored all objects have been created
        public MainWindow()
        {
            InitializeComponent();
        }

        //Check the Format of input data: just allow a number entered and combo box has to be choosen. Then will allow to enter input data
        void CheckInputFormat(object sender, System.EventArgs e)
        {
            int AcceptInputBlank = 0;
            int AcceptExchangeComboBox = 0;
            int AcceptIndex = 0;
            double Input;
            try
            {
                Input = Double.Parse(input.Text);
                AcceptInputBlank=1;
            }
            catch (FormatException m)
            {
                AcceptInputBlank = 0;
            }

            if (AcceptInputBlank == 0)
            {
                Warning.Visibility = Visibility.Visible;
            }
            else
            {
                Warning.Visibility = Visibility.Collapsed;
            }

            if (Currency.SelectedIndex == -1)
            { 
                AcceptExchangeComboBox = 0;
            }
            else
            {
                AcceptExchangeComboBox =1 ;
            }

            if (AcceptExchangeComboBox == 0)
            {
                WarningExchange.Visibility = Visibility.Visible;
            }
            else 
            {
                WarningExchange.Visibility = Visibility.Collapsed;
            }
            AcceptIndex = AcceptInputBlank + AcceptExchangeComboBox;

            if (AcceptIndex == 2)
            {
                EnterButton.IsEnabled = true;
            }
            else 
            {
                EnterButton.IsEnabled = false;
            }
        }

        //Enter Data when they met the condition, create new object and save into DB 
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            int DBSize = DB.Count+1;
            double Input = double.Parse(input.Text);
            int CurrencyIndex = Currency.SelectedIndex;
            Currency Obj = null;
            switch (CurrencyIndex)
            {
                case 0:
                    Obj = new Dollar(DBSize,Input);
                    break;
                case 1:
                    Obj = new Euro(DBSize, Input);
                    break;
                case 2:
                    Obj = new Yuan(DBSize, Input);
                    break;
            }
            DB.Add(Obj);
            Result.Text = Obj.GetResult;
            ListObj.ItemsSource = DB;
            ListObj.Items.Refresh();
            EnableCompareBtnCondition();
        }

        //When choose a object in listview, the button "Remove" will be enabled and allow to remove something
        int Click = 0;
        private void ListObj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int ClickTest = ListObj.SelectedIndex;
            if (ClickTest < DB.Count && ClickTest >= 0)
            {
                Click = ClickTest;
                RemoveBtn.IsEnabled = true;
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("DB Size : {0}", DB.Count);
            //When a object was removed. The Number of the next object will be deduce depend on the removed object. 
            //That why system have to be created a new object and add to position of last removed object in DB
            for (int i = Click; i < DB.Count; i++)
            {
                if (i < DB.Count - 1)
                {
                    Currency Obj=null;
                    if (DB[i + 1].ObjType == "Dollar Class")
                    {
                        Console.WriteLine("Dollar Changed");
                        Obj = new Dollar(i+1, Double.Parse(DB[i + 1].Input));
                    }
                    else if (DB[i + 1].ObjType == "Euro Class")
                    {
                        Console.WriteLine("Euro Changed");
                        Obj = new Euro(i+1, Double.Parse(DB[i + 1].Input));
                    }
                    else if (DB[i + 1].ObjType == "Yuan Class")
                    {
                        Console.WriteLine("Yuan Changed");
                        Obj = new Yuan(i+1, Double.Parse(DB[i + 1].Input));
                    }
                    DB[i]=null;
                    DB[i]=Obj;
                }
            }
            DB.RemoveAt(DB.Count - 1);
            ListObj.ItemsSource = DB;
            ListObj.Items.Refresh();
            RemoveBtn.IsEnabled = false;
            EnableCompareBtnCondition();
        }

        void EnableCompareBtnCondition()
        {
            if (DB.Count >= 2)
            {
                CompareObjBtn.IsEnabled = true;
            }
            else 
            {
                CompareObjBtn.IsEnabled = false;
            }
        }

        //Compare objects function use dunction Equals() 
        private void CompareObjBtn_Click(object sender, RoutedEventArgs e)
        {
            int Pointer = 0;
            string MBoxText = "";
            for (int i = 0; i < 2; i=i) //infinity loop just finish when the last element in DB was called
            {
                if (Pointer == DB.Count - 1)
                {
                    i = 3;
                }
                else 
                {
                    for (int j = Pointer + 1; j < DB.Count; j++)
                    {
                        string MBoxAdd = "";
                        if (DB[Pointer].Equals(DB[j]) == true)
                        {
                            MBoxAdd = "Obj " + (Pointer+1) + " == Obj " + (j+1) + "\n";
                        }
                        else if (DB[Pointer].Equals(DB[j]) == false)
                        {
                            MBoxAdd = "Obj " + (Pointer+1) + " != Obj " + (j+1) + "\n";
                        }
                        MBoxText = MBoxText + MBoxAdd;
                    }
                }
                Pointer++;
            }
            string MBoxCap = "Compare Objects";
            MessageBox.Show(MBoxText, MBoxCap);
        }
    }
}
