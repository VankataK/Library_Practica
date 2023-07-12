using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Library
{
    internal class Book
    {
        public string Isbn;
        public string Title;
        public string Author;
        public int Year;
        public double Price;
        public bool Availability;
        public string Borrower;
    }
    internal class Library
    {
        private List<Book> books;

        public Library()
        {
            books = new List<Book>();
            StreamReader reader = new StreamReader("library.txt", Encoding.GetEncoding("UTF-8"));
            string line = "";

            while ((line = reader.ReadLine())!=null)  
            {
                
                string[] input = line.Split('_').ToArray();
                if (input.Length>=6)
                {
                    Book book = new Book();
                    book.Isbn = input[0];
                    book.Title = input[1];
                    book.Author = input[2];
                    book.Year = int.Parse(input[3]);
                    book.Price = double.Parse(input[4]);
                    book.Availability = bool.Parse(input[5]);
                    if(input.Length==7) book.Borrower = input[6];
                    else book.Borrower = "";
                    books.Add(book);
                }
            }
            reader.Close();
        }
        public void Menu()
        {
            while (true)
            {
                Console.WriteLine("Библиотека");
                Console.WriteLine("Меню:");
                Console.WriteLine("1) Добавяне на книга");
                Console.WriteLine("2) Заемане на книга");
                Console.WriteLine("3) Връщане на книга");
                Console.WriteLine("4) Справка за всички налични книги");
                Console.WriteLine("5) Справка за всички заети книги");
                Console.WriteLine("6) Изход");
                Console.Write("Изберете опция: ");
                string input = Console.ReadLine();
                Console.WriteLine("============");
                if (input=="1")
                {
                    AddBook();
                }
                else if (input == "2")
                {
                    BorrowBook();
                }
                else if (input == "3")
                {
                    ReturnBook();
                }
                else if (input == "4")
                {
                    ShowAvailableBooks();
                }
                else if (input == "5")
                {
                    ShowBorrowedBooks();
                }
                else if (input == "6")
                {
                    SaveBooksToLibrary();
                    break;
                }
                else
                {
                    Console.WriteLine("Невалидна опция.");
                }
                Console.WriteLine("============");
            }
            
        }

        private void SaveBooksToLibrary()
        {
            StreamWriter writer = new StreamWriter("library.txt");
            foreach (Book book in books)
            {
                writer.WriteLine($"{book.Isbn}_{book.Title}_{book.Author}_{book.Year}_{book.Price}_{book.Availability}_{book.Borrower}");
            }
            writer.Close();
        }
        private void AddBook()
        {
            Console.WriteLine("Въведете данните на книгата:");
            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();
            Console.WriteLine("Заглавие: ");
            string title = Console.ReadLine();
            Console.Write("Автор: ");
            string author = Console.ReadLine();
            Console.Write("Година на издаване: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Цена: ");
            double price = double.Parse(Console.ReadLine());
            
            Book book = new Book();
            book.Isbn = isbn;
            book.Title = title;
            book.Author = author;
            book.Year = year;
            book.Price = price;
            book.Availability = true;
            book.Borrower = "";

            books.Add(book);
            SaveBooksToLibrary();
            Console.WriteLine("Книгата е добавена успешно.");
        }
        private void BorrowBook()
        {
            ShowAvailableBooks();
            List<Book> availibleBooks = books.FindAll(x=>x.Availability);
            if (availibleBooks.Count == 0) return;
            else
            {
                Console.Write("Напишете номерът на книгата, която искате да заемете: ");
                int index = int.Parse(Console.ReadLine())-1;
                if (index < 0 || index>=availibleBooks.Count)
                {
                    Console.WriteLine("Номерът е невалиден.");
                    return;
                }
                Book selectedBook = availibleBooks[index];
                Console.Write("Въведете име на заемателя: ");
                string borrower = Console.ReadLine();
                selectedBook.Availability = false;
                selectedBook.Borrower = borrower;
                SaveBooksToLibrary();
                Console.WriteLine("Книгата е заета успешно.");
            }
            
        }
        private void ReturnBook()
        {
            ShowBorrowedBooks();
            List<Book> borrowedBooks = books.FindAll(x => !x.Availability);
            if(borrowedBooks.Count == 0) return;
            else
            {
                Console.Write("Напишете номерът на книгата, която искате да върнете: ");
                int index = int.Parse(Console.ReadLine()) - 1;

                if (index < 0 || index >= borrowedBooks.Count)
                {
                    Console.WriteLine("Номерът е невалиден.");
                    return;
                }
                Book selectedBook = borrowedBooks[index];
                selectedBook.Availability = true;
                selectedBook.Borrower = "";
                SaveBooksToLibrary();
                Console.WriteLine("Книгата е върната успешно.");
            }
        }
        private void ShowAvailableBooks()
        {
            List<Book> availiblyBooks = books.FindAll(x=>x.Availability);
            if (availiblyBooks.Count==0)
            {
                Console.WriteLine("Няма налични книги.");
                return;
            }
            else
            {
                Console.WriteLine("Списък с всички налични книги:");
                for (int i = 0; i < availiblyBooks.Count; i++)
                {
                    Console.WriteLine($"{i+1}) {availiblyBooks[i].Isbn} {availiblyBooks[i].Title} - {availiblyBooks[i].Author}, Издадена: {availiblyBooks[i].Year} година, Цена: {availiblyBooks[i].Price} лв.");
                }

            }
        }
        private void ShowBorrowedBooks()
        {
            List<Book> borrowedBooks = books.FindAll(x => !x.Availability);
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("Няма заети книги.");
                return;
            }
            else
            {
                Console.WriteLine("Списък с всички заети книги:");
                for (int i = 0; i < borrowedBooks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {borrowedBooks[i].Isbn} {borrowedBooks[i].Title} - {borrowedBooks[i].Author}, Издадена: {borrowedBooks[i].Year} година, Цена: {borrowedBooks[i].Price} лв., Наемател: {borrowedBooks[i].Borrower}");
                }

            }
        }
    }
}
