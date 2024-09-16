using System;
using System.Data;
using Npgsql;

namespace SQLApp
{
    public class SQLApp
    {
        public static void Main(string[] args)
        {
            Repository repository = new Repository();
            Console.WriteLine(repository.Create("books",
                 new []{"bookname", "author", "publisher", "genre", "bookcost"},
                new []{"Ulysses", "James Joyce", "Radcliffe Publishing", "Fiction", "19.99"}));
            repository.Read("books",
                "*");
            repository.Update("books",
                new string[]{"bookname"}, new []{"In Search of Lost Time"}, "bookname = 'The Great Gatsby'");
            repository.Delete("books", "id = 1");
        }
    }
}