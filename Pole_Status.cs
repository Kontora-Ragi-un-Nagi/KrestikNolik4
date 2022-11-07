﻿using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;


public class Pole_Status
    // состояние ячейки игрового поля
    {
        private string poleTag = "";
        public string PoleTag { get; set; }

        private int poleStatus = 0;
        public int PoleStatus
        {
            get { return poleStatus; }
            set { poleStatus = value; }
        }
        private Button btn = new Button();
        public Button Btn
        {
            get { return btn; }
            set { btn = value; }
        }

        public Pole_Status(string pNum)
        {
            PoleTag = pNum;
            PoleStatus = 0;
            Btn = new Button();
        }
 }

