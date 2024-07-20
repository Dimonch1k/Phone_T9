using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Phone_T9
{
    public partial class Phone : Form
    {
        private int currentLine;
        private int currentPosition;

        private int clickCount = 0;
        private bool timerRunning = false;

        private Dictionary<int, List<char>> symbols = new Dictionary<int, List<char>>
        {
            { 2, new List<char> {'2', 'а', 'б', 'в', 'г', 'a', 'b', 'c'}},
            { 3, new List<char> {'3', 'д', 'е', 'ж', 'з', 'd', 'e', 'f'}},
            { 4, new List<char> {'4', 'и', 'й', 'к', 'л', 'g', 'h', 'i'}},
            { 5, new List<char> {'5', 'м', 'н', 'о', 'п', 'j', 'k', 'l'}},
            { 6, new List<char> {'6', 'р', 'с', 'т', 'у', 'ш', 'r', 's'}},
            { 7, new List<char> {'7', 'ф', 'х', 'с', 'ч', 'p', 'q', 'r'}},
            { 8, new List<char> {'8', 'ш', 'щ', 'ь', 's', 't', 'u', 'v'}},
            { 9, new List<char> {'9', 'є', 'ю', 'я', 'w', 'x', 'y', 'z'}},
            { 0, new List<char> {'0', '+'}}
        };
        private Dictionary<int, Timer> timers = new Dictionary<int, Timer>();


        public Phone()
        {
            InitializeComponent();
            InitializeElements();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void InitializeElements()
        {
            for (int i = 0; i <= 9; i++)
            {
                if (i == 1) continue;
                Timer timer = new Timer();
                timer.Interval = 1500;
                timer.Tick += ClickTimer_Tick;
                timers.Add(i, timer);
            }
        }

        // ==================== Buttons: UP, Down, Left, Right ========================== //
        private void btnUp_Click(object sender, EventArgs e)
        {
            currentLine = text.GetLineFromCharIndex(text.SelectionStart);
            if (currentLine == 0)
            {
                text.SelectionStart = 0;
                text.Focus();
                return;
            }
            Point pOld = text.GetPositionFromCharIndex(text.SelectionStart);
            Point pNew = new Point(pOld.X, pOld.Y - text.Font.Height);
            int charIndex = text.GetCharIndexFromPosition(pNew);
            text.SelectionStart = charIndex;
            text.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            currentLine = text.GetLineFromCharIndex(text.SelectionStart);
            if (currentLine == text.Lines.Length)
            {
                text.SelectionStart = text.Text.Length;
                text.Focus();
                return;
            }
            Point pOld = text.GetPositionFromCharIndex(text.SelectionStart);
            Point pNew = new Point(pOld.X, pOld.Y + text.Font.Height);
            int charIndex = text.GetCharIndexFromPosition(pNew);
            text.SelectionStart = charIndex;
            text.Focus();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (text.SelectionStart > 0)
            {
                text.SelectionStart--;
                text.SelectionLength = 0;
            }
            text.Focus();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (text.SelectionStart < text.Text.Length)
            {
                text.SelectionStart++;
                text.SelectionLength = 0;
            }
            text.Focus();
        }


        // ==================== Buttons remove sumbol 'C' ========================== //
        private void btnRemoveLast_Click(object sender, EventArgs e)
        {
            removeLast();
        }


        // ==================== IMPORTANT FUNCTIONS ================================ //
        private void removeLast()
        {
            int cursorPosition = text.SelectionStart;

            if (cursorPosition > 0 && cursorPosition <= text.Text.Length)
            {
                text.Text = text.Text.Remove(cursorPosition - 1, 1);

                text.SelectionStart = cursorPosition - 1;
                text.SelectionLength = 0;
            }

            text.Focus();
        }

        private void insertNewSymbol(int key)
        {
            List<char> btnSymbols= symbols[key];
            if (!timerRunning)
            {
                runTimer(key);
                addSymbolT9(btnSymbols);
            }
            else
            {
                removeLast();
                changeCount(btnSymbols);
                addSymbolT9(btnSymbols);
            }
        }

        private void addSymbol(char symbol)
        {
            currentPosition = text.SelectionStart;
            text.Text = text.Text.Insert(text.SelectionStart, symbol.ToString());
            text.SelectionStart = currentPosition + 1;
            text.SelectionLength = 0;
            text.Focus();
        }


        private void addSymbolT9(List<char> btnSymbols)
        {
            currentPosition = text.SelectionStart;
            text.Text = text.Text.Insert(currentPosition, btnSymbols[clickCount].ToString());
            text.SelectionStart = currentPosition + 1;
            text.SelectionLength = 0;
            text.Focus();
        }

        private void changeCount(List<char> btnSymbols)
        {
            clickCount++;
            if (clickCount >= btnSymbols.Count)
            {
                clickCount = 0;
            }
        }


        // ==================== Button Timer ====================================== //
        private void ClickTimer_Tick(object sender, EventArgs e)
        {
            Timer clickedTimer = (Timer)sender;
            clickedTimer.Stop();
            timerRunning = false;
        }

        private void runTimer(int key)
        {
            timers[key].Start();
            clickCount = 0;
            timerRunning = true;
        }


        // ==================== Number Pad ========================================= //

        private void btn1_Click(object sender, EventArgs e)
        {
            addSymbol('1');
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            insertNewSymbol(2);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            insertNewSymbol(3);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            insertNewSymbol(4);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            insertNewSymbol(5);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            insertNewSymbol(6);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            insertNewSymbol(7);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            insertNewSymbol(8);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            insertNewSymbol(9);
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            insertNewSymbol(0);
        }


        private void btnStar_Click(object sender, EventArgs e)
        {
            addSymbol('*');
        }

        private void btnSharp_Click(object sender, EventArgs e)
        {
            addSymbol('#');
        }
    }
}
