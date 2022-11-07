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

namespace KrestikNolik4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int HodCount = 0;
        public int X_Count = 0;
        public int O_Count = 0;

        private Brush _FonPobedi = new SolidColorBrush(Colors.Gold);
        private Brush _FonIgri = new SolidColorBrush(Colors.LightBlue);
        private Brush _FonProstoi = new SolidColorBrush(Colors.LightCyan);
        private Brush _FonNichja = new SolidColorBrush(Colors.Blue);
        private Brush _FonSnacala = new SolidColorBrush(Colors.LightCoral);
        private Brush _pusto = new SolidColorBrush(Colors.LightGray);
        string PathImage = "Resources\\";


        public ImageBrush _nolik = new ImageBrush();
        public ImageBrush _krestik = new ImageBrush();

        public bool GameOver = false;
        public bool DatorX = false;
        public bool DatorO = false;

        public Pole_Status[,] Field = new Pole_Status[4, 4];

        #region Main window initialization
        public MainWindow()
        {
            InitializeComponent();

            HodCount = 0;
            X_Count = 0;    O_Count = 0;
            string PathImage = "Resources\\";
            _nolik.ImageSource = new BitmapImage(new Uri(PathImage + "Nolik.png", UriKind.RelativeOrAbsolute));
            _krestik.ImageSource = new BitmapImage(new Uri(PathImage + "Krestik.png", UriKind.RelativeOrAbsolute));

            GameOver = false;
            DatorX = false;
            DatorO = false;
            CvsC.IsChecked = true;

            int i = 0;
            int j = 0;
            string neuTag = "";
            for (i = 0; i <= 3; i++)
            {
                for (j = 0; j <= 3; j++)
                {
                    neuTag = j.ToString() + i.ToString();
                    Field[i, j] = new Pole_Status(neuTag);
                }
            }

            Field[0, 0].Btn = btn00; Field[0, 1].Btn = btn10; Field[0, 2].Btn = btn20;   Field[0, 3].Btn = btn30;
            Field[1, 0].Btn = btn01; Field[1, 1].Btn = btn11; Field[1, 2].Btn = btn21;   Field[1, 3].Btn = btn31;
            Field[2, 0].Btn = btn02; Field[2, 1].Btn = btn12; Field[2, 2].Btn = btn22;   Field[2, 3].Btn = btn32;
            Field[3, 0].Btn = btn03; Field[3, 1].Btn = btn13; Field[3, 2].Btn = btn23;   Field[3, 3].Btn = btn33;
        }

        #endregion Main window initialization

        #region Uzvaras linija
        private void UzvarasLinija()
        {

            ImageBrush _Bingo = new ImageBrush();
            if (X_Count > O_Count)
            {
                _Bingo = new ImageBrush(new BitmapImage(new Uri(PathImage + "krestik_U.png", UriKind.Relative)));
                for (int i = 0; i <= 3; i++)
                    for (int j = 0; j <= 3; j++)
                    { if (Field[i, j].PoleStatus==10) { Field[i, j].Btn.Background = _Bingo; } }
            }
            else 
            { _Bingo = new ImageBrush(new BitmapImage(new Uri(PathImage + "Nolik_U.png", UriKind.Relative)));
                for (int i = 0; i <= 3; i++)
                    for (int j = 0; j <= 3; j++)
                    { if (Field[i, j].PoleStatus == 1) { Field[i, j].Btn.Background = _Bingo; } }
            }
           
        } 
        #endregion // Uzvaras linija


        #region Punktu_Skaititajs
        private void Punktu_Skaititajs( int Status, ref bool Str_X, ref bool Str_O, ref int TekSum_X, ref int TekSum_O,
                                                                               ref int GlobalSum_X, ref int GlobalSum_O)
        {
            if (Status == 1) // это 0
            {
                if (!Str_O) { Str_O = true; TekSum_O = 0; } // пошли 0
                if (Str_O) { TekSum_O++; } // считаем длину 0
                if (Str_X) { Str_X = false; if (TekSum_X > 1) { GlobalSum_X = GlobalSum_X + TekSum_X * 10; } TekSum_X = 0; } // проверяем длину Х
            }
            if (Status == 10) // это X
            {
                if (!Str_X) { Str_X = true; TekSum_X = 0; } // пошли X
                if (Str_X) { TekSum_X++; } // считаем длину X
                if (Str_O) { Str_O = false; if (TekSum_O > 1) { GlobalSum_O = GlobalSum_O + TekSum_O * 10; } TekSum_O = 0; } // проверяем длину 0
            }

        } // Punktu_Skaititajs

        #endregion // Punktu_Skaititajs

        #region Speles skaits
        private void Speles_skaits()
        {
            X_Count = 0;
            O_Count = 0;

            int Sum_X = 0; int Sum_O = 0;
            bool Polosa_X = false; bool Polosa_O = false;

            // по строкам
            for (int i = 0; i < 3; i++)
            {
                Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки

                for (int j = 0; j <= 3; j++)
                {
                  Punktu_Skaititajs(Field[i, j].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
                }
                if ((Polosa_X)&&(Sum_X > 1))   { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
                if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0

            }
            // по столбцам
            for (int i = 0; i < 3; i++)
            {
                Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки
                for (int j = 0; j <= 3; j++)
                {
                  Punktu_Skaititajs(Field[j, i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
                }
                if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
                if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0

            }
            // по диагонали слева направо

            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки

            for (int i = 0; i <= 3; i++) // это большая диагональ слева направо 
            {
              Punktu_Skaititajs(Field[i, i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


                Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки

            for (int i = 0; i <= 2; i++) // это средняя диагональ слева направо снизу
            {
                 Punktu_Skaititajs(Field[i+1, i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки

            for (int i = 0; i <= 2; i++) // это средняя диагональ слева направо сверху
            {
              Punktu_Skaititajs(Field[i, i+1].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки

            for (int i = 0; i < 2; i++) // это маленькая диагональ слева направо снизу
            {
               Punktu_Skaititajs(Field[i+2, i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки

            for (int i = 0; i <2; i++) // это маленькая диагональ слева направо сверху
            {
                Punktu_Skaititajs(Field[i, i+2].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            // по диагонали справа налево 

            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки
            for (int i = 0; i <= 3; i++) // это большая диагональ справа налево 
            {
              Punktu_Skaititajs(Field[i, 3-i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки
            for (int i = 1; i <= 3; i++) // это средняя диагональ снизу справа налево 
            {
              Punktu_Skaititajs(Field[i, 4 - i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки
            for (int i = 0; i <= 2; i++) // это средняя диагональ сверху справа налево 
            {
              Punktu_Skaititajs(Field[i, 2 - i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0

            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки
            for (int i = 2; i <= 3; i++) // это маленькая диагональ снизу справа налево 
            {
             Punktu_Skaititajs(Field[i, 5 - i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            Sum_X = 0; Sum_O = 0; Polosa_X = false; Polosa_O = false; // поиск цепочки
            for (int i = 0; i <2; i++) // это маленькая диагональ сверху справа налево 
            {
              Punktu_Skaititajs(Field[i, 1 - i].PoleStatus, ref Polosa_X, ref Polosa_O, ref Sum_X, ref Sum_O, ref X_Count, ref O_Count);
            }
            if ((Polosa_X) && (Sum_X > 1)) { X_Count = X_Count + Sum_X * 10; }  // дописываем баллы Х
            if ((Polosa_O) && (Sum_O > 1)) { O_Count = O_Count + Sum_O * 10; }  // дописываем баллы 0


            X_Tablo.Text = X_Count.ToString();
            O_Tablo.Text = O_Count.ToString();

        }
        #endregion Speles skaits

        #region Ir uzvara
        private bool IrUzvara()
        {
            int i = 0;
            int j = 0;
            int Summa = 0;
            // по строкам 
            for (i = 0; i <= 2; i++)
            {
                Summa = 0;

                for (j = 0; j <= 2; j++)
                { Summa = Summa + Field[i, j].PoleStatus; }

                if ((Summa == 30) || (Summa == 3))
                {
                    UzvarasLinija();
                    return ((Summa == 30) || (Summa == 3));
                }
            }
            // по колонкам 
            Summa = 0;
            for (j = 0; j <= 2; j++)
            {
                Summa = 0;

                for (i = 0; i <= 2; i++)
                { Summa = Summa + Field[i, j].PoleStatus; }

                if ((Summa == 30) || (Summa == 3))
                {
                    UzvarasLinija();
                    return ((Summa == 30) || (Summa == 3));
                }
            }
            // по диагоналям
            Summa = 0;
            for (j = 0; j <= 2; j++)
            { Summa = Summa + Field[j, 2 - j].PoleStatus; }
            if ((Summa == 30) || (Summa == 3))
            {
                UzvarasLinija();
                return ((Summa == 30) || (Summa == 3));
            }

            Summa = 0;
            for (j = 0; j <= 2; j++)
            { Summa = Summa + Field[j, j].PoleStatus; }
            if ((Summa == 30) || (Summa == 3))
            {
                UzvarasLinija();
                return ((Summa == 30) || (Summa == 3));
            }

            return ((Summa == 30) || (Summa == 3));
        } //bool IrUzvara()

        #endregion Ir uzvara

        #region // человек сделал ход - нажал кнопку
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button)
            {
                Button btn = (Button)sender;

                if ((btn != null) && (HodCount < 16) && (GameOver == false))
                {
                    int BTag = int.Parse(btn.Tag.ToString());

                    int i = BTag % 10;
                    int j = BTag / 10;

                    if (Field[i, j].PoleStatus == 0)  //zaciklit hodcount
                    {
                        HodCount++;
                        if ((HodCount % 2) == 0) // четный ход
                        { Field[i, j].Btn.Background = _nolik; Field[i, j].PoleStatus = 1; }
                        else // нечетный ход
                        { Field[i, j].Btn.Background = _krestik; Field[i, j].PoleStatus = 10; }
                       

                        if (HodCount==16)
                        {
                            GameOver = true;
                            
                            if (X_Count!=O_Count)
                            {
                                tbTablo.Background = _FonPobedi;
                                if (X_Count > O_Count) { tbTablo.Text = "UZVARA! Krustiņi uzvarēja!"; }
                                else { tbTablo.Text = "UZVARA! Nulles uzvarēja!"; }
                                UzvarasLinija();
                            }
                            else 
                            {
                                tbTablo.Background = _FonNichja;
                                tbTablo.Text = "NEIZŠĶIRTS ! Vairāk gājienu nav!";
                            }
                        }

                        // проверка с кем играем
                        if ((DatorX | DatorO) && (HodCount < 16))
                        {
                            Dator_hod();
                            Speles_skaits();
                            if (HodCount == 16)
                            {
                                GameOver = true;

                                if (X_Count != O_Count)
                                {
                                    tbTablo.Background = _FonPobedi;
                                    if (X_Count > O_Count) { tbTablo.Text = "UZVARA! Krustiņi uzvarēja!"; }
                                    else { tbTablo.Text = "UZVARA! Nulles uzvarēja!"; }
                                    UzvarasLinija();
                                }
                                else
                                {
                                    tbTablo.Background = _FonNichja;
                                    tbTablo.Text = "NEIZŠĶIRTS ! Vairāk gājienu nav!";
                                }
                            }
                        }
                        else if ((DatorX | DatorO) && (HodCount < 16) && (GameOver == true))
                        {
                            tbTablo.Background = _FonSnacala;
                            tbTablo.Text = "Ierosinu sākt spēli no jauna !";
                        }

                    }
                    else if (GameOver == true)
                    {
                        tbTablo.Background = _FonSnacala;
                        tbTablo.Text = "Ierosinu sākt spēli no jauna !";
                    }
                    Speles_skaits();
                }
               // btn.IsHitTestVisible = false;
            }
        }
        #endregion // человек сделал ход - нажал кнопку

        #region //надежный ход компьютера

        private bool Drosibas_hod() // нужна защита
        {
            int i = 0;
            int j = 0;
            int Summa = 0; int CheckSum = 0; int DatorSum = 0;
            int SumPobedi = 0; int CheckPobedi = 0;
            ImageBrush _Bingo = new ImageBrush();

            if (DatorX) { _Bingo = _krestik; } else { _Bingo = _nolik; }

            if (DatorX) { CheckSum = 1; DatorSum = 10; CheckPobedi = 10; }
            else { CheckSum = 10; DatorSum = 1; CheckPobedi = 1; } // какое значение ищем
            // по строкам 
            for (i = 0; i <= 2; i++)
            {
                Summa = 0; SumPobedi = 0;

                for (j = 0; j <= 2; j++)
                {
                    if (Field[i, j].PoleStatus == CheckSum) { Summa = Summa + Field[i, j].PoleStatus; }
                    if (Field[i, j].PoleStatus == CheckPobedi) { SumPobedi = SumPobedi + Field[i, j].PoleStatus; }
                }

                // есть быстрая победа

                // по строкам
                if ((((Summa == 20) & (DatorO)) || ((Summa == 2) & (DatorX))) ||   // защитился
                   (((SumPobedi == 2) & (DatorO)) || ((SumPobedi == 20) & (DatorX))))    // есть быстрая победа
                {

                    for (j = 0; j <= 2; j++)
                    {
                        if (Field[i, j].PoleStatus == 0)      // нужное поле
                        {
                            Field[i, j].PoleStatus = DatorSum;
                            Field[i, j].Btn.Background = _Bingo;
                            HodCount++;
                            return true;
                        }
                    };
                }
            }

            // по колонкам 
            for (j = 0; j <= 2; j++)
            {
                Summa = 0; SumPobedi = 0;

                for (i = 0; i <= 2; i++)
                {
                    if (Field[i, j].PoleStatus == CheckSum) { Summa = Summa + Field[i, j].PoleStatus; }
                    if (Field[i, j].PoleStatus == CheckPobedi) { SumPobedi = SumPobedi + Field[i, j].PoleStatus; }
                }

                if ((((Summa == 20) & (DatorO)) || ((Summa == 2) & (DatorX))) ||   // защитился
                   (((SumPobedi == 2) & (DatorO)) || ((SumPobedi == 20) & (DatorX))))    // есть быстрая победа
                {

                    for (i = 0; i <= 2; i++)
                    {
                        if (Field[i, j].PoleStatus == 0) // нужное поле
                        {
                            Field[i, j].PoleStatus = DatorSum;
                            Field[i, j].Btn.Background = _Bingo;
                            HodCount++;
                            return true;
                        }
                    };
                }
            }
            // по диагоналям
            Summa = 0; SumPobedi = 0;
            for (j = 0; j <= 2; j++)
            {
                if (Field[2 - j, j].PoleStatus == CheckSum) { Summa = Summa + Field[2 - j, j].PoleStatus; }
                if (Field[2 - j, j].PoleStatus == CheckPobedi) { SumPobedi = SumPobedi + Field[2 - j, j].PoleStatus; }
            }

            if ((((Summa == 20) & (DatorO)) || ((Summa == 2) & (DatorX))) ||   // защитился
               (((SumPobedi == 2) & (DatorO)) || ((SumPobedi == 20) & (DatorX))))    // есть быстрая победа
            {
                for (j = 0; j <= 2; j++)
                {
                    if (Field[2 - j, j].PoleStatus == 0)
                    {
                        Field[2 - j, j].PoleStatus = DatorSum;
                        Field[2 - j, j].Btn.Background = _Bingo;
                        HodCount++;
                        return true;
                    }
                }

            } // защитился

            Summa = 0; SumPobedi = 0;
            for (j = 0; j <= 2; j++)
            {
                Summa = Summa + Field[j, j].PoleStatus;
                SumPobedi = SumPobedi + Field[j, j].PoleStatus;
            }

            if ((((Summa == 20) & (DatorO)) || ((Summa == 2) & (DatorX))) ||   // защитился
               (((SumPobedi == 2) & (DatorO)) || ((SumPobedi == 20) & (DatorX))))    // есть быстрая победа
            {
                for (j = 0; j <= 2; j++)
                {
                    if (Field[j, j].PoleStatus == 0)
                    {
                        Field[j, j].PoleStatus = DatorSum;
                        Field[j, j].Btn.Background = _Bingo;
                        HodCount++;
                        return true;
                    }
                }

            }

            return false;

        } // Drosibas_hod()

        #endregion  // надежный ход компьютера

        #region //компьютер играет  
        private void Dator_hod()
        {
            Button btn = new Button();
            ImageBrush _DBingo = new ImageBrush();
            if (DatorX) { _DBingo = _krestik; } else { _DBingo = _nolik; }
            bool Vairogs = false;

            if (HodCount == 0) //компьютер начинает
            { Field[1, 1].Btn.Background = _krestik; Field[1, 1].PoleStatus = 10; HodCount++; }
            else //компьютер продолжает
            {
                Vairogs = Drosibas_hod();
                if (!Vairogs)  // не нужно было защищаться
                { //Ищем свободное место
                    int i = -1; int j = -1; // координаты свободного места
                    if (Field[1, 1].PoleStatus == 0) { i = 1; j = 1; } // занимаем центр
                    else //  центр занят
                    {
                        if (Field[0, 0].PoleStatus == 0) { i = 0; j = 0; } // занимаем верхний левый угол
                        else // верхний левый угол занят
                        {
                            if (Field[2, 0].PoleStatus == 0) { i = 2; j = 0; } // занимаем нижний левый угол
                            else  // нижний левый угол занят
                            {
                                if (Field[0, 2].PoleStatus == 0) { i = 0; j = 2; }// занимаем верхний правый угол
                                else // верхний правый угол занят
                                {
                                    if (Field[2, 2].PoleStatus == 0) { i = 2; j = 2; }// занимаем нижний правый угол
                                    else // нижний правый угол занят
                                    {
                                        if (Field[0, 1].PoleStatus == 0) { i = 0; j = 1; }// занимаем центр верхней строки
                                        else // центр верхней строки занят
                                        {
                                            if (Field[2, 1].PoleStatus == 0) { i = 2; j = 1; }// занимаем центр нижней строки
                                            else // центр нижней строки занят
                                            {
                                                if (Field[1, 0].PoleStatus == 0) { i = 1; j = 0; }// занимаем центр левой колонки
                                                else // центр левой колонки занят
                                                {
                                                    if (Field[1, 2].PoleStatus == 0) { i = 1; j = 2; }// занимаем центр правой колонки
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if ((i > -1) & (j > -1))
                    {
                        if (DatorX) { Field[i, j].PoleStatus = 10; }
                        else Field[i, j].PoleStatus = 1;
                        Field[i, j].Btn.Background = _DBingo;
                        HodCount++;
                    }

                }  // не нужно было защищаться

            } //компьютер продолжает (HodCount != 0)


        }

        #endregion //компьютер играет

        #region // запустить новую игру


        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            tbTablo.Background = _FonProstoi;
            tbTablo.Text = "Gatavs spēlei";

            int i = 0;
            int j = 0;
            for (i = 0; i <= 3; i++)
            {
                for (j = 0; j <= 3; j++)
                {
                    Field[i, j].PoleStatus = 0;
                    Field[i, j].Btn.Background = _pusto;
                    Field[i, j].Btn.IsHitTestVisible = true;
                }
            }

            GameOver = false;
            HodCount = 0;
            X_Count = 0; O_Count = 0;

            X_Tablo.Text = X_Count.ToString();
            O_Tablo.Text = O_Count.ToString();


            DatorX = (DvsC.IsChecked == true);
            DatorO = (CvsD.IsChecked == true);

            if (DatorX) { Dator_hod(); }
        }
        #endregion // запустить новую игру
    }
}
