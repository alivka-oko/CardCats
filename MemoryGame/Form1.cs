using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PictureBox prev;
        byte flag = 0;
        int remain = 8;
        byte hint = 3;
        byte timeLeft = 40;
        int ppt = 15;
        private void Form1_Load(object sender, EventArgs e)
        {
            newgame();
            panel1.Hide();

        }

        void resetImages()
        {
            foreach (Control x in this.Controls) if (x is PictureBox) 
                    (x as PictureBox).Image = Properties.Resources._0;

            // Вставляем все изображения PictureBox с вопросительным знаком (?)
            // "Переворачиваем" карточки
        }
            

        void showImage(PictureBox box) 
        {
            switch(Convert.ToInt32(box.Tag)) 
            {
                case 1:
                    box.Image = Properties.Resources._1;
                    break;
                case 2:
                    box.Image = Properties.Resources._2;
                    break;
                case 3:
                    box.Image = Properties.Resources._3;
                    break;
                case 4:
                    box.Image = Properties.Resources._4;
                    break;
                case 5:
                    box.Image = Properties.Resources._5;
                    break;
                case 6:
                    box.Image = Properties.Resources._6;
                    break;
                case 7:
                    box.Image = Properties.Resources._7;
                    break;
                case 8:
                    box.Image = Properties.Resources._8;
                    break;
                default:
                    box.Image = Properties.Resources._0;
                    break;
            }            

        }


        void setTagRandom() //заполнение поля
        {
            int[] arr = new int[16];
            int index = 0;
            Random rand = new Random();
            int r;
            while (index < 16) //1.	Создает массив и заполняет его внутри случайными числами от 1 до 16.
            {
                r = rand.Next(1, 17);
                if(Array.IndexOf(arr,r)==-1) /* Заполнение без повторений
                                              (Метод indexOf возвращает первый индекс,
                                              по которому данный элемент может быть найден в массиве или -1,
                                              если такого индекса нет.)*/
                {
                    arr[index] = r;
                    index++;
                }  
            }
            for(index =0; index < 16; index++) //2.	Выполняет процесс вычитания для чисел больше 8.
                                               //(потому что у нас есть 8 разных изображений для этой игры)
            {
                if (arr[index] > 8) 
                    arr[index] -= 8;
            }
            index = 0;
            foreach(Control x in this.Controls) //3.Устанавливает номера массива в теги PictureBoxes.
                                                //(Подставляет картинки)
            {
                if(x is PictureBox)
                {
                    (x as PictureBox).Tag = arr[index].ToString();
                    index++;
                }
            }

        }
        void compare(PictureBox previous, PictureBox current) // Сравнение
        {
            if(previous.Tag.ToString()==current.Tag.ToString()) //Если выбранные карточки совпали по тегу
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                previous.Visible = false; //Убрать карточки с поля
                current.Visible = false;
                if(--remain==0) //Если карточек не осталось - конец игры
                {
                    timer1.Enabled = false;
                    remaining.Text = "Поздравляю!";
                    panel1.Show();
                    MessageBox.Show("Ура, Вы победили!", "Конец игры");
                    Hint.Enabled = false;
                    
                }
               else remaining.Text = "Осталось найти: " + remain;
            }
            else // Не совпали по тегу
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                previous.Image = Properties.Resources._0; //Скрыть обратно карточки
                current.Image = Properties.Resources._0;
            }
        }

        void allvisibleTrue() //Включить видимость картинок
        {
            foreach (Control x in this.Controls) if (x is PictureBox) (x as PictureBox).Visible = true;
        }
        void activeAll() //Активировать карточки
        {
            foreach (Control x in this.Controls) if (x is PictureBox) (x as PictureBox).Enabled = true;
        }
        void deActiveAll()//Выключить карточки
        {
            foreach (Control x in this.Controls) if (x is PictureBox) (x as PictureBox).Enabled = false;
        }

        
        void newgame() // Новая игра
        {
            panel1.Hide(); //Спрятать "конец игры"
            remain = 8;//Осталось пар
            hint = 3;//Попыток подсмотреть
            ppt = 15;//Кол-во ходов

            remaining.Text = "Осталось найти: " + remain;
            Hint.Text = "Показать (" + hint + ")";
            timeLeft = 40;//Таймер
            time.Text = "Осталось времени: " + timeLeft + " сек.";
            label1.Text = "Осталось ходов: " + ppt;


            allvisibleTrue();//Включить видимость картинок
            setTagRandom();//рандомные значения полей

            foreach (Control x in this.Controls) if (x is PictureBox) showImage(x as PictureBox); //Показываем карточки
                                                                                                  //для запоминания на 3 сек.
            Application.DoEvents();
            System.Threading.Thread.Sleep(3000);

            resetImages(); // Скрываем картинки через 3 сек.

            Hint.Enabled = true;//Включить кнопку "Показать"
            flag = 0;

            timer1.Enabled = true;//Вкл. таймер
            activeAll();// сделать карточки активными (кликабельными)                 
            
        }

        private void pictureBox1_Click(object sender, EventArgs e) //Для нажатия на две картинки
        {
            PictureBox current = (sender as PictureBox); //Назначаем активным PictureBox на котором кликнули.
            showImage(sender as PictureBox);
            if (flag == 0) //Выбор первой картинки
            {
                prev = current;
                flag = 1;
                //label4.Text = "" + flag;
            }
            else if(prev!=current) //Выбор второй картинки
            {
                compare(prev, current);
                flag = 0; //Обнуление для нового выбора

                ppt--; //Минус ход
                label1.Text = "Осталось ходов: " + ppt;
               // label4.Text = "" + flag;

            }
           
        }

        private void Hint_Click(object sender, EventArgs e) //При нажатии на кнопку показываются все картинки
        {
            foreach(Control x in this.Controls) if(x is PictureBox) showImage(x as PictureBox); //"Открытие карточек"
            Application.DoEvents();
            System.Threading.Thread.Sleep(1500); //Показать на 1,5 сек.
            resetImages(); // Перевернуть обратно
            if (--hint == 0) Hint.Enabled = false; //Вычесть попытку

            Hint.Text = "Показать (" + hint + ")";
        }

        private void timer1_Tick(object sender, EventArgs e) //Счётчик времени
        {
            
            if (--timeLeft == 0) //Время закончилось
            {
                timer1.Enabled = !timer1.Enabled; //Выкл. таймер
                time.Text = "Время истекло.";
                panel1.Show(); // "Конец игры"
                MessageBox.Show("Время закончилось", "Конец игры!");
                deActiveAll(); //деактивровать карточки (нельзя нажать)
                Hint.Enabled = false; //Выключить кнопку "Показать"
                
            }
            else
            time.Text = "Осталось времени: " + timeLeft + " сек.";
            
            if (ppt == 0) // Закончились попытки раньше, чем закончилось время
            {
                timer1.Enabled = !timer1.Enabled; 
                panel1.Show(); //Показать "Конец игры!"
                MessageBox.Show("Вы проиграли! Ходы закончились.", "Конец игры!");
                deActiveAll(); //деактивировать карточки
                Hint.Enabled = false;
            }
        }

        private void newGameButton_Click(object sender, EventArgs e) //Новая игра
        {
            newgame();
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка начать игру в самом начале
        {
            panel2.Hide();
            newgame();

        }
    }
}
