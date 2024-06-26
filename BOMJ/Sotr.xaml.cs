﻿using BOMJ.Models;
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

namespace BOMJ
{
    /// <summary>
    /// Логика взаимодействия для Sotr.xaml
    /// </summary>
    public partial class Sotr : Window
    {
        public Sotr()
        {
            InitializeComponent();
            // Использование бд
            using (SpravkaContext db = new SpravkaContext())
            {
                List<string> sortList = new List<string>() { "По возрастанию ЗП", "По убыванию ЗП" };
                sortUserComboBox.ItemsSource = sortList.ToList();

                productlistView.ItemsSource = db.Sotruds.ToList();

                countProducts.Text = $"Количество: {db.Sotruds.Count()}";


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void sortUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }
        //Обнавление записей
        private void UpdateProducts()
        {
            using (SpravkaContext db = new SpravkaContext())
            {

                var currentProducts = db.Sotruds.ToList();
                productlistView.ItemsSource = currentProducts;
                countProducts.Text = $"Количество: {currentProducts.Count} из {db.Sotruds.ToList().Count}";

                //Сортировка
                if (sortUserComboBox.SelectedIndex != -1)
                {
                    if (sortUserComboBox.SelectedValue == "По убыванию ЗП")
                    {
                        currentProducts = currentProducts.OrderByDescending(u => u.Money).ToList();

                    }

                    if (sortUserComboBox.SelectedValue == "По возрастанию Зп")
                    {
                        currentProducts = currentProducts.OrderBy(u => u.Money).ToList();

                    }
                }
                //Поиск
                if (searchBox.Text.Length > 0)
                {

                    currentProducts = currentProducts.Where(u => u.Name.Contains(searchBox.Text) || u.Surname.Contains(searchBox.Text)).ToList();

                }

                productlistView.ItemsSource = currentProducts;
            }
        }

        private void сlearButton_Click(object sender, RoutedEventArgs e)
        {
            searchBox.Text = "";
            sortUserComboBox.SelectedIndex = -1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new AddSotrudWindow().ShowDialog();
        }
        // Удаление данных
        private void delUserButton(object sender, RoutedEventArgs e)
        {
            using (SpravkaContext db = new SpravkaContext())
            {
                var product = (productlistView.SelectedItem) as Sotrud;

                if (product != null)
                {

                    if (MessageBox.Show($"Вы точно хотите удалить {product.Name}", "Внимание!",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        db.Sotruds.Remove(product);
                        db.SaveChanges();
                        MessageBox.Show($"Сотрудник {product.Name} удален!");
                        productlistView.ItemsSource = db.Sotruds.ToList();
                        countProducts.Text = $"Количество: {db.Sotruds.Count()}";
                    }

                }
            }
        }
        // Очистка сортировки данных
        private void сlearButton_Restart(object sender, RoutedEventArgs e)
        {
            new Sotr().ShowDialog();
            this.Close();
        }
    }
}

